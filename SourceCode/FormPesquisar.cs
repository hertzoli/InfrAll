using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    public partial class FormPesquisar : Form
    {
        private readonly Func<List<ResultadoPesquisaItem>> _obterItens;
        private readonly Func<string, bool> _selecionar;
        private readonly Func<string, Image> _obterImagem;
        private List<ResultadoPesquisaItem> _resultados = new List<ResultadoPesquisaItem>();
        private List<FiltroPesquisa> _filtros = new List<FiltroPesquisa>();
        private bool _fechado;

        public FormPesquisar()
        {
            InitializeComponent();
            ConfigurarTabela();
            buttonPesquisar.Click += Pesquisar_Click;
            buttonFiltrar.Click += Filtrar_Click;
            DataGridViewItemPesquisar.CellDoubleClick += Resultado_DoubleClick;
            Shown += (s, e) => textBoxPesquisar.Focus();
            FormClosed += (s, e) => _fechado = true;
        }

        internal FormPesquisar(Func<List<ResultadoPesquisaItem>> obterItens, Func<string, bool> selecionar,
            Func<string, Image> obterImagem) : this()
        {
            _obterItens = obterItens;
            _selecionar = selecionar;
            _obterImagem = obterImagem;
        }

        private void ConfigurarTabela()
        {
            var grid = DataGridViewItemPesquisar;
            grid.Columns.Clear();
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            grid.Columns.Add(new DataGridViewImageColumn
            {
                Name = "icone", HeaderText = "icone", Width = 45,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            });
            grid.Columns[0].DefaultCellStyle.NullValue = null;
            foreach (string coluna in PesquisaItensService.Colunas.Skip(1))
                grid.Columns.Add(new DataGridViewTextBoxColumn { Name = coluna, HeaderText = coluna, Width = 140 });
        }

        private async void Pesquisar_Click(object sender, EventArgs e)
        {
            string texto = textBoxPesquisar.Text.Trim();
            if (texto.Length < 2)
            {
                MessageBox.Show(this, "Informe pelo menos 2 caracteres para pesquisar.", "Pesquisa");
                textBoxPesquisar.Focus();
                return;
            }
            if (!buttonPesquisar.Enabled)
                return;
            try
            {
                buttonPesquisar.Enabled = buttonFiltrar.Enabled = false;
                var snapshot = _obterItens != null ? _obterItens() : new List<ResultadoPesquisaItem>();
                var filtros = _filtros.Select(f => f.Copiar()).ToList();
                var originais = await Task.Run(() => PesquisaItensService.Pesquisar(snapshot, texto));
                var visiveis = await Task.Run(() => PesquisaItensService.Filtrar(originais, filtros));
                if (_fechado || IsDisposed)
                    return;
                MostrarResultados(visiveis);
                _resultados = originais;
            }
            catch (Exception ex)
            {
                if (!_fechado)
                    MostrarErro(ex);
            }
            finally
            {
                if (!_fechado && !IsDisposed)
                    buttonPesquisar.Enabled = buttonFiltrar.Enabled = true;
            }
        }

        private void MostrarResultados(List<ResultadoPesquisaItem> itens)
        {
            // Preparar as linhas antes de substituir o último resultado válido.
            var linhas = new List<DataGridViewRow>();
            var imagens = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in itens)
            {
                var valores = item.Campos.Cast<object>().ToArray();
                string chave = item.Campos[0] ?? string.Empty;
                Image imagem;
                if (!imagens.TryGetValue(chave, out imagem))
                {
                    imagem = _obterImagem != null ? _obterImagem(chave) : null;
                    imagens[chave] = imagem;
                }
                valores[0] = imagem;
                var linha = new DataGridViewRow();
                linha.CreateCells(DataGridViewItemPesquisar, valores);
                linha.Tag = item.ID;
                linhas.Add(linha);
            }
            DataGridViewItemPesquisar.Rows.Clear();
            if (linhas.Count > 0)
                DataGridViewItemPesquisar.Rows.AddRange(linhas.ToArray());
        }

        private void Filtrar_Click(object sender, EventArgs e)
        {
            try
            {
                using (var formulario = new FormFiltroPesquisa(_filtros, AplicarFiltros))
                    formulario.ShowDialog(this);
            }
            catch (Exception ex) { MostrarErro(ex); }
        }

        private void AplicarFiltros(List<FiltroPesquisa> filtros)
        {
            var resultados = PesquisaItensService.Filtrar(_resultados, filtros);
            MostrarResultados(resultados);
            _filtros = filtros.Select(f => f.Copiar()).ToList();
        }

        private void Resultado_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            try
            {
                if (_selecionar != null && _selecionar((string)DataGridViewItemPesquisar.Rows[e.RowIndex].Tag))
                    Owner?.Activate();
            }
            catch (Exception ex) { MostrarErro(ex); }
        }

        private void MostrarErro(Exception ex)
        {
            Logger.LogWriter.LogErroDetalhado(ex, "Erro na pesquisa de itens");
            MessageBox.Show(this, "Não foi possível concluir a operação de pesquisa.", "Pesquisa", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
