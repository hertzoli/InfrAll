using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    public partial class FormNovoItem : Form
    {
        private readonly ImageList _imageListIcons = new ImageList();
        private readonly Dictionary<string, string> _arquivosPorChave = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public FormNovoItem()
        {
            InitializeComponent();
            InicializarFormulario();
        }

        public string ItemNome
        {
            get { return (_textBoxNome.Text ?? string.Empty).Trim(); }
        }

        public string ItemDescricao
        {
            get { return (_textBoxDescricao.Text ?? string.Empty).Trim(); }
        }

        public string Observacao
        {
            get { return (_textBoxObservacao.Text ?? string.Empty).Trim(); }
        }

        public string IconeSelecionado
        {
            get
            {
                if (listViewImages.SelectedItems.Count == 0)
                    return string.Empty;

                return listViewImages.SelectedItems[0].Name;
            }
        }

        public string PastaImagens
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens"); }
        }

        public void ConfigurarParaEdicao(string nome, string descricao, string observacao, string iconeSelecionado)
        {
            Text = "Editar item";
            _textBoxNome.Text = nome ?? string.Empty;
            _textBoxDescricao.Text = descricao ?? string.Empty;
            _textBoxObservacao.Text = observacao ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(iconeSelecionado))
                SelecionarItemPorChave(iconeSelecionado);
        }

        private void InicializarFormulario()
        {
            Text = "Novo item";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;

            _imageListIcons.ColorDepth = ColorDepth.Depth32Bit;
            _imageListIcons.ImageSize = new Size(16, 16);

            listViewImages.View = View.SmallIcon;
            listViewImages.MultiSelect = false;
            listViewImages.SmallImageList = _imageListIcons;

            botaoOk.DialogResult = DialogResult.OK;
            botaoCancelar.DialogResult = DialogResult.Cancel;

            botaoOk.Click += BotaoOk_Click;
            botaoCancelar.Click += BotaoCancelar_Click;
            buttonNovoIcone.Click += ButtonNovoIcone_Click;

            AcceptButton = botaoOk;
            CancelButton = botaoCancelar;

            CarregarImagensDaPasta();
        }

        private void BotaoOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ItemNome))
                return;

            MessageBox.Show("Informe um nome para o item.", "Novo item",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
        }

        private void BotaoCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonNovoIcone_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Selecionar icone";
                dialog.Filter = "Imagens|*.bmp;*.jpg;*.png;*.ico;*.gif;*.jpeg";
                dialog.Multiselect = false;

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    string arquivoDestino = CopiarImagemParaPastaDoPrograma(dialog.FileName);
                    string chave = AdicionarImagemAoListView(arquivoDestino, true);

                    if (!string.IsNullOrWhiteSpace(chave))
                        _textBoxNome.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nao foi possivel importar o icone.\r\n" + ex.Message,
                        "Novo icone", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CarregarImagensDaPasta()
        {
            Directory.CreateDirectory(PastaImagens);

            _imageListIcons.Images.Clear();
            _arquivosPorChave.Clear();
            listViewImages.Items.Clear();

            foreach (string arquivo in Directory.GetFiles(PastaImagens)
                .Where(EhArquivoDeImagemValido)
                .OrderBy(Path.GetFileName))
            {
                AdicionarImagemAoListView(arquivo, false);
            }

            if (listViewImages.Items.Count > 0)
                SelecionarItemPorChave(listViewImages.Items[0].Name);
        }

        private string CopiarImagemParaPastaDoPrograma(string arquivoOrigem)
        {
            Directory.CreateDirectory(PastaImagens);

            string nomeArquivo = Path.GetFileName(arquivoOrigem);
            string destino = Path.Combine(PastaImagens, nomeArquivo);

            if (string.Equals(Path.GetFullPath(arquivoOrigem), Path.GetFullPath(destino), StringComparison.OrdinalIgnoreCase))
                return destino;

            if (File.Exists(destino))
            {
                string nomeBase = Path.GetFileNameWithoutExtension(nomeArquivo);
                string extensao = Path.GetExtension(nomeArquivo);
                int contador = 1;

                do
                {
                    destino = Path.Combine(PastaImagens, string.Format("{0}_{1}{2}", nomeBase, contador, extensao));
                    contador++;
                }
                while (File.Exists(destino));
            }

            File.Copy(arquivoOrigem, destino, false);
            return destino;
        }

        private string AdicionarImagemAoListView(string caminhoArquivo, bool selecionarAoFinal)
        {
            string chave = Path.GetFileName(caminhoArquivo);

            if (_arquivosPorChave.ContainsKey(chave))
            {
                if (selecionarAoFinal)
                    SelecionarItemPorChave(chave);

                return chave;
            }

            using (Image imagemOriginal = CarregarImagem(caminhoArquivo))
            {
                if (imagemOriginal == null)
                    return string.Empty;

                using (Bitmap miniatura = new Bitmap(imagemOriginal, new Size(16, 16)))
                {
                    _imageListIcons.Images.Add(chave, new Bitmap(miniatura));
                }
            }

            _arquivosPorChave[chave] = caminhoArquivo;

            ListViewItem item = new ListViewItem(chave);
            item.Name = chave;
            item.ImageKey = chave;
            item.ToolTipText = caminhoArquivo;
            listViewImages.Items.Add(item);

            if (selecionarAoFinal)
                SelecionarItemPorChave(chave);

            return chave;
        }

        private void SelecionarItemPorChave(string chave)
        {
            if (!listViewImages.Items.ContainsKey(chave))
                return;

            listViewImages.SelectedItems.Clear();

            ListViewItem item = listViewImages.Items[chave];
            item.Selected = true;
            item.Focused = true;
            item.EnsureVisible();
        }

        private static bool EhArquivoDeImagemValido(string caminhoArquivo)
        {
            string extensao = Path.GetExtension(caminhoArquivo);

            return string.Equals(extensao, ".bmp", StringComparison.OrdinalIgnoreCase)
                || string.Equals(extensao, ".jpg", StringComparison.OrdinalIgnoreCase)
                || string.Equals(extensao, ".png", StringComparison.OrdinalIgnoreCase)
                || string.Equals(extensao, ".ico", StringComparison.OrdinalIgnoreCase)
                || string.Equals(extensao, ".gif", StringComparison.OrdinalIgnoreCase)
                || string.Equals(extensao, ".jpeg", StringComparison.OrdinalIgnoreCase);
        }

        private static Image CarregarImagem(string caminhoArquivo)
        {
            string extensao = Path.GetExtension(caminhoArquivo);

            if (string.Equals(extensao, ".ico", StringComparison.OrdinalIgnoreCase))
            {
                using (Icon icon = new Icon(caminhoArquivo))
                {
                    return icon.ToBitmap();
                }
            }

            using (FileStream stream = new FileStream(caminhoArquivo, FileMode.Open, FileAccess.Read))
            {
                return Image.FromStream(stream);
            }
        }
    }
}
