using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    public partial class FormPesquisar
    {
        internal Func<List<string>, Dictionary<int, string>, bool> SalvarEdicaoEmMassa { get; set; }
        private readonly Dictionary<Control, Color> _coresEmMassa = new Dictionary<Control, Color>();
        private string[] _referenciaEmMassa;
        private string _iconeEmMassa = string.Empty;
        private bool _carregandoEmMassa;
        private string _textoPesquisaExecutada;

        private void ConfigurarEdicaoEmMassa()
        {
            DataGridViewItemPesquisar.MultiSelect = true;
            DataGridViewItemPesquisar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            comboBoxEditarTipoEmMassa.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEditarTipoEmMassa.Items.AddRange(Enum.GetNames(typeof(TipoDoValor)));
            pictureBoxImagemEdicaoEmMassa.SizeMode = PictureBoxSizeMode.Zoom;
            foreach (var controle in ControlesEmMassa().Values)
            {
                _coresEmMassa[controle] = controle.BackColor;
                controle.TextChanged += (s, e) => AtualizarIndicadoresEmMassa();
            }
            DataGridViewItemPesquisar.SelectionChanged += (s, e) =>
            {
                try { CarregarSelecaoEmMassa(); }
                catch (Exception ex) { MostrarErro(ex); }
            };
            buttonAlterarIconeEmMassa.Click += AlterarIconeEmMassa;
            groupBoxEdicaoEmMassa.Paint += (s, e) =>
            {
                if (_referenciaEmMassa != null && _iconeEmMassa != _referenciaEmMassa[0])
                {
                    var retangulo = pictureBoxImagemEdicaoEmMassa.Bounds;
                    retangulo.Inflate(2, 2);
                    using (var caneta = new Pen(Color.Red, 2))
                        e.Graphics.DrawRectangle(caneta, retangulo);
                }
            };
            groupBoxEdicaoEmMassa.Enabled = false;
        }

        private Dictionary<int, Control> ControlesEmMassa()
        {
            return new Dictionary<int, Control> { { 1, textBoxEditarNomeEmMassa },
                { 2, RichTextBoxEditarValorEmMassa }, { 3, comboBoxEditarTipoEmMassa },
                { 4, textBoxEditarDescricaoEmMassa } };
        }

        private List<DataGridViewRow> LinhasSelecionadasEmMassa()
        {
            return DataGridViewItemPesquisar.SelectedRows.Cast<DataGridViewRow>()
                .Where(r => !r.IsNewRow).OrderBy(r => r.Index).ToList();
        }

        private void CarregarSelecaoEmMassa()
        {
            var primeira = LinhasSelecionadasEmMassa().FirstOrDefault();
            _carregandoEmMassa = true;
            try
            {
                _referenciaEmMassa = primeira == null ? null : (string[])((ResultadoPesquisaItem)primeira.Cells[0].Tag).Campos.Clone();
                foreach (var campo in ControlesEmMassa())
                    campo.Value.Text = _referenciaEmMassa == null ? string.Empty : _referenciaEmMassa[campo.Key] ?? string.Empty;
                _iconeEmMassa = _referenciaEmMassa == null ? string.Empty : _referenciaEmMassa[0] ?? string.Empty;
                pictureBoxImagemEdicaoEmMassa.Image = primeira == null ? null : primeira.Cells[0].Value as Image;
                // Comparar com o texto efetivamente exibido (incluindo normalização de quebras de linha).
                if (_referenciaEmMassa != null)
                {
                    _referenciaEmMassa[0] = _iconeEmMassa;
                    foreach (var campo in ControlesEmMassa())
                        _referenciaEmMassa[campo.Key] = campo.Value.Text;
                }
                groupBoxEdicaoEmMassa.Enabled = primeira != null;
            }
            finally { _carregandoEmMassa = false; }
            AtualizarIndicadoresEmMassa();
        }

        private Dictionary<int, string> ObterAlteracoesEmMassa()
        {
            var alteracoes = new Dictionary<int, string>();
            if (_referenciaEmMassa == null) return alteracoes;
            foreach (var campo in ControlesEmMassa())
                if (campo.Value.Text != _referenciaEmMassa[campo.Key])
                    alteracoes[campo.Key] = campo.Value.Text;
            if (_iconeEmMassa != _referenciaEmMassa[0]) alteracoes[0] = _iconeEmMassa;
            return alteracoes;
        }

        private void AtualizarIndicadoresEmMassa()
        {
            if (_carregandoEmMassa) return;
            var alteracoes = ObterAlteracoesEmMassa();
            foreach (var campo in ControlesEmMassa())
                campo.Value.BackColor = alteracoes.ContainsKey(campo.Key) ? Color.LightCoral : _coresEmMassa[campo.Value];
            groupBoxEdicaoEmMassa.Invalidate();
        }

        private void AlterarIconeEmMassa(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new FormSelectIcon())
                {
                    dialog.ConfigurarSelecao(_iconeEmMassa);
                    if (dialog.ShowDialog(this) != DialogResult.OK) return;
                    _iconeEmMassa = dialog.IconeSelecionado ?? string.Empty;
                    pictureBoxImagemEdicaoEmMassa.Image = _obterImagem?.Invoke(_iconeEmMassa);
                    AtualizarIndicadoresEmMassa();
                }
            }
            catch (Exception ex) { MostrarErro(ex); }
        }

        private void buttonSalvarEmMassa_Click(object sender, EventArgs e)
        {
            try
            {
                if (!buttonPesquisar.Enabled || SalvarEdicaoEmMassa == null) return;
                var ids = LinhasSelecionadasEmMassa().Select(r => (string)r.Tag).Distinct().ToList();
                var alteracoes = ObterAlteracoesEmMassa();
                if (ids.Count == 0 || alteracoes.Count == 0)
                {
                    MessageBox.Show(this, "Selecione itens e altere pelo menos um campo.", "Edição em massa");
                    return;
                }
                string campos = string.Join(", ", alteracoes.Keys.OrderBy(k => k).Select(k => PesquisaItensService.Colunas[k]));
                if (MessageBox.Show(this, $"Essa operação irá modificar os campos: ({campos}) de {ids.Count} itens. Tem certeza que deseja modificar?",
                    "Edição em massa", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                if (!SalvarEdicaoEmMassa(ids, alteracoes)) return;
                _resultados = PesquisaItensService.Pesquisar(_obterItens(), _textoPesquisaExecutada);
                MostrarResultados(PesquisaItensService.Filtrar(_resultados, _filtros));
            }
            catch (Exception ex) { MostrarErro(ex); }
        }
    }
}

