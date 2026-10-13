using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    public partial class FormFiltroPesquisa : Form
    {
        private readonly Action<List<FiltroPesquisa>> _aplicar;
        private bool _habilitadoAoAdicionar = true;

        public FormFiltroPesquisa()
        {
            InitializeComponent();
            ConfigurarControles();
        }

        internal FormFiltroPesquisa(IEnumerable<FiltroPesquisa> filtros, Action<List<FiltroPesquisa>> aplicar) : this()
        {
            _aplicar = aplicar;
            foreach (var filtro in filtros)
                AdicionarLinha(filtro.Copiar());
        }

        private void ConfigurarControles()
        {
            comboBoxColuna.DropDownStyle = comboBoxRelacao.DropDownStyle = comboBoxAcao.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxColuna.Items.AddRange(PesquisaItensService.Colunas);
            comboBoxColuna.SelectedIndexChanged += (s, e) => ExecutarSeguro(() =>
            {
                comboBoxRelacao.Items.Clear();
                comboBoxRelacao.Items.AddRange(comboBoxColuna.SelectedIndex >= 8
                    ? PesquisaItensService.RelacoesData : PesquisaItensService.RelacoesTexto);
                comboBoxRelacao.SelectedIndex = 0;
            });
            comboBoxColuna.SelectedIndex = 1;
            comboBoxAcao.SelectedIndex = 0;
            var grid = DataGridViewFilter;
            grid.Columns.Clear();
            grid.AllowUserToAddRows = grid.AllowUserToDeleteRows = false;
            grid.ReadOnly = false;
            grid.MultiSelect = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Habilitado", HeaderText = "Habilitado", Width = 65 });
            foreach (var coluna in new[] { "Coluna", "Relação", "Valor", "Ação" })
                grid.Columns.Add(new DataGridViewTextBoxColumn { Name = coluna, HeaderText = coluna, ReadOnly = true });
            grid.CurrentCellDirtyStateChanged += (s, e) => ExecutarSeguro(() =>
            {
                if (grid.IsCurrentCellDirty)
                    grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            });
            grid.SelectionChanged += (s, e) => buttonRemover.Enabled = grid.SelectedRows.Count > 0;
            grid.CellDoubleClick += Filtro_DoubleClick;
            buttonRemover.Enabled = false;
            buttonAdicionar.Click += (s, e) => ExecutarSeguro(Adicionar);
            buttonRemover.Click += (s, e) => ExecutarSeguro(() =>
            {
                if (grid.SelectedRows.Count > 0)
                    grid.Rows.Remove(grid.SelectedRows[0]);
            });
            buttonAplicar.Click += (s, e) => ExecutarSeguro(Aplicar);
            buttonOK.Click += (s, e) => ExecutarSeguro(() => { Aplicar(); DialogResult = DialogResult.OK; Close(); });
            buttonCancelar.DialogResult = DialogResult.Cancel;
            CancelButton = buttonCancelar;
            // O controle Reset existente não possui comportamento definido no PRD.
            buttonReset.Visible = false;
        }

        private void AdicionarLinha(FiltroPesquisa filtro)
        {
            int indice = DataGridViewFilter.Rows.Add(filtro.Habilitado, PesquisaItensService.Colunas[filtro.Coluna],
                filtro.Relacao, filtro.Valor, filtro.Excluir ? "Excluir" : "Incluir");
            DataGridViewFilter.Rows[indice].Tag = filtro;
        }

        private void Filtro_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= DataGridViewFilter.Rows.Count)
                return;

            ExecutarSeguro(() =>
            {
                DataGridViewFilter.EndEdit();
                DataGridViewRow linha = DataGridViewFilter.Rows[e.RowIndex];
                var filtro = (FiltroPesquisa)linha.Tag;
                comboBoxColuna.SelectedIndex = filtro.Coluna;
                comboBoxRelacao.SelectedItem = filtro.Relacao;
                textBoxValor.Text = filtro.Valor;
                comboBoxAcao.SelectedIndex = filtro.Excluir ? 1 : 0;
                _habilitadoAoAdicionar = Convert.ToBoolean(linha.Cells[0].Value);
                DataGridViewFilter.Rows.Remove(linha);
                textBoxValor.Focus();
                textBoxValor.SelectAll();
            });
        }

        private void Adicionar()
        {
            if (comboBoxAcao.SelectedIndex < 0)
                throw new ArgumentException("Selecione uma ação.");
            var filtro = new FiltroPesquisa
            {
                Habilitado = _habilitadoAoAdicionar, Coluna = comboBoxColuna.SelectedIndex,
                Relacao = comboBoxRelacao.SelectedItem as string, Valor = textBoxValor.Text,
                Excluir = comboBoxAcao.SelectedIndex == 1
            };
            PesquisaItensService.Validar(filtro);
            if (LerFiltros().Any(f => f.Coluna == filtro.Coluna && f.Relacao == filtro.Relacao
                && f.Valor == filtro.Valor && f.Excluir == filtro.Excluir))
                throw new ArgumentException("Este filtro já existe.");
            AdicionarLinha(filtro);
            _habilitadoAoAdicionar = true;
        }

        private List<FiltroPesquisa> LerFiltros()
        {
            DataGridViewFilter.EndEdit();
            var filtros = new List<FiltroPesquisa>();
            foreach (DataGridViewRow linha in DataGridViewFilter.Rows)
            {
                var filtro = ((FiltroPesquisa)linha.Tag).Copiar();
                filtro.Habilitado = Convert.ToBoolean(linha.Cells[0].Value);
                filtros.Add(filtro);
            }
            return filtros;
        }

        private void Aplicar()
        {
            var filtros = LerFiltros();
            foreach (var filtro in filtros)
                PesquisaItensService.Validar(filtro);
            _aplicar?.Invoke(filtros);
        }

        private void ExecutarSeguro(Action acao)
        {
            try { acao(); }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Filtro de pesquisa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogErroDetalhado(ex, "Erro nos filtros de pesquisa");
                MessageBox.Show(this, "Não foi possível concluir a operação com os filtros.", "Filtro de pesquisa", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
