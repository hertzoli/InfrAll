using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GerenciadorSistemas
{
    public partial class FormNovoItem : Form
    {
        private const string ChaveIconePasta = "__folder__";
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
                TreeNode noSelecionado = TreeViewImages.SelectedNode;

                if (noSelecionado == null || !_arquivosPorChave.ContainsKey(noSelecionado.Name))
                    return string.Empty;

                return noSelecionado.Name;
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
            _imageListIcons.Images.Add(ChaveIconePasta, new Bitmap(Properties.Resources.Folder, new Size(16, 16)));

            TreeViewImages.ImageList = _imageListIcons;

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
                    string chave = AdicionarImagemAoTreeView(arquivoDestino, true);

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
            _imageListIcons.Images.Add(ChaveIconePasta, new Bitmap(Properties.Resources.Folder, new Size(16, 16)));
            _arquivosPorChave.Clear();
            TreeViewImages.Nodes.Clear();

            foreach (string pasta in Directory.GetDirectories(PastaImagens, "*", SearchOption.AllDirectories)
                .OrderBy(ObterChaveRelativaDaImagem))
            {
                AdicionarPastaAoTreeView(ObterChaveRelativaDaImagem(pasta));
            }

            foreach (string arquivo in Directory.GetFiles(PastaImagens, "*.*", SearchOption.AllDirectories)
                .Where(EhArquivoDeImagemValido)
                .OrderBy(ObterChaveRelativaDaImagem))
            {
                AdicionarImagemAoTreeView(arquivo, false);
            }

            TreeViewImages.ExpandAll();

            string iconePadrao = CarregarIconePadraoPersistido();
            if (!string.IsNullOrWhiteSpace(iconePadrao) && _arquivosPorChave.ContainsKey(iconePadrao))
                SelecionarItemPorChave(iconePadrao);
            else
                SelecionarPrimeiraImagemDisponivel();
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

        private TreeNodeCollection AdicionarPastaAoTreeView(string chavePasta)
        {
            TreeNodeCollection colecao = TreeViewImages.Nodes;
            string[] partes = (chavePasta ?? string.Empty).Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < partes.Length; i++)
            {
                string caminhoPasta = string.Join("/", partes.Take(i + 1).ToArray());
                TreeNode noPasta = colecao.ContainsKey(caminhoPasta)
                    ? colecao[caminhoPasta]
                    : colecao.Add(caminhoPasta, partes[i]);

                noPasta.ImageKey = ChaveIconePasta;
                noPasta.SelectedImageKey = ChaveIconePasta;
                colecao = noPasta.Nodes;
            }

            return colecao;
        }

        private string AdicionarImagemAoTreeView(string caminhoArquivo, bool selecionarAoFinal)
        {
            string chave = ObterChaveRelativaDaImagem(caminhoArquivo);

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

            string[] partes = chave.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string chavePasta = partes.Length > 1
                ? string.Join("/", partes.Take(partes.Length - 1).ToArray())
                : string.Empty;
            TreeNodeCollection colecao = AdicionarPastaAoTreeView(chavePasta);

            string nomeArquivo = partes.Length > 0 ? partes[partes.Length - 1] : Path.GetFileName(caminhoArquivo);
            TreeNode item = colecao.Add(chave, nomeArquivo);
            item.ImageKey = chave;
            item.SelectedImageKey = chave;
            item.ToolTipText = caminhoArquivo;

            if (selecionarAoFinal)
                SelecionarItemPorChave(chave);

            return chave;
        }

        private void SelecionarItemPorChave(string chave)
        {
            TreeNode item = EncontrarNoPorChave(TreeViewImages.Nodes, chave);

            if (item == null)
                return;

            TreeViewImages.SelectedNode = item;
            item.EnsureVisible();
        }

        private void SelecionarPrimeiraImagemDisponivel()
        {
            foreach (string chave in _arquivosPorChave.Keys.OrderBy(k => k))
            {
                SelecionarItemPorChave(chave);
                return;
            }
        }

        private static TreeNode EncontrarNoPorChave(TreeNodeCollection nos, string chave)
        {
            if (nos == null || string.IsNullOrWhiteSpace(chave))
                return null;

            if (nos.ContainsKey(chave))
                return nos[chave];

            foreach (TreeNode no in nos)
            {
                TreeNode encontrado = EncontrarNoPorChave(no.Nodes, chave);

                if (encontrado != null)
                    return encontrado;
            }

            return null;
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

        private void buttonDefinirIconePadrao_Click(object sender, EventArgs e)
        {
            string iconeSelecionado = IconeSelecionado;

            if (string.IsNullOrWhiteSpace(iconeSelecionado))
            {
                MessageBox.Show("Selecione uma imagem para definir como icone padrao.", "Icone padrao",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                SalvarIconePadraoPersistido(iconeSelecionado);
                MessageBox.Show("Icone padrao salvo.", "Icone padrao",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel salvar o icone padrao.\r\n" + ex.Message,
                    "Icone padrao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string ObterChaveRelativaDaImagem(string caminhoArquivo)
        {
            string caminhoBase = Path.GetFullPath(PastaImagens).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string caminhoCompleto = Path.GetFullPath(caminhoArquivo);

            if (!caminhoCompleto.StartsWith(caminhoBase, StringComparison.OrdinalIgnoreCase))
                return Path.GetFileName(caminhoArquivo);

            string relativo = caminhoCompleto.Substring(caminhoBase.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return relativo.Replace(Path.DirectorySeparatorChar, '/').Replace(Path.AltDirectorySeparatorChar, '/');
        }

        private static string ObterCaminhoArquivoCadastro()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cadastro.yaml");
        }

        public static string CarregarIconePadraoPersistido()
        {
            try
            {
                string caminhoArquivo = ObterCaminhoArquivoCadastro();

                if (!File.Exists(caminhoArquivo))
                    return string.Empty;

                IDeserializer deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .IgnoreUnmatchedProperties()
                    .Build();

                using (StreamReader reader = new StreamReader(caminhoArquivo))
                {
                    CadastroPersistido cadastro = deserializer.Deserialize<CadastroPersistido>(reader);
                    return cadastro != null && cadastro.Configuracoes != null
                        ? cadastro.Configuracoes.IconePadrao ?? string.Empty
                        : string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private static void SalvarIconePadraoPersistido(string iconePadrao)
        {
            CadastroPersistido cadastro = CarregarCadastroPersistidoParaConfiguracao();

            if (cadastro.Configuracoes == null)
                cadastro.Configuracoes = new ConfiguracoesPersistidas();

            cadastro.Configuracoes.IconePadrao = iconePadrao ?? string.Empty;
            cadastro.SavedAt = DateTime.Now.ToString("o");

            ISerializer serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            string caminhoArquivo = ObterCaminhoArquivoCadastro();
            string caminhoTemporario = caminhoArquivo + ".tmp";

            using (StreamWriter writer = new StreamWriter(caminhoTemporario, false))
                serializer.Serialize(writer, cadastro);

            if (File.Exists(caminhoArquivo))
                File.Delete(caminhoArquivo);

            File.Move(caminhoTemporario, caminhoArquivo);
        }

        private static CadastroPersistido CarregarCadastroPersistidoParaConfiguracao()
        {
            string caminhoArquivo = ObterCaminhoArquivoCadastro();

            if (!File.Exists(caminhoArquivo))
            {
                return new CadastroPersistido
                {
                    SchemaVersion = 1,
                    Items = new List<ItemPersistido>()
                };
            }

            IDeserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            using (StreamReader reader = new StreamReader(caminhoArquivo))
            {
                CadastroPersistido cadastro = deserializer.Deserialize<CadastroPersistido>(reader);

                if (cadastro == null)
                    cadastro = new CadastroPersistido();

                if (cadastro.Items == null)
                    cadastro.Items = new List<ItemPersistido>();

                return cadastro;
            }
        }
    }
}
