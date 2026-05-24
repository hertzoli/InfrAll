using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GerenciadorSistemas
{
    public partial class FormSelectIcon : Form
    {
        private const string ChaveIconePasta = "__folder__";
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly ImageList _imageListIcons = new ImageList();
        private readonly Dictionary<string, string> _arquivosPorChave = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private CancellationTokenSource previewCancellationTokenSource;
        private int previewRequestVersion;
        private Image imagemPreviewOriginal;

        public FormSelectIcon()
        {
            InitializeComponent();
            InicializarFormulario();
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

        public string NomeIconeGerado { get; private set; }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CancelarAtualizacaoPreview();
            TrocarImagemPreview(null, null);
            base.OnFormClosed(e);
        }

        public void ConfigurarSelecao(string iconeSelecionado)
        {
            Text = "Selecionar icone";

            if (!string.IsNullOrWhiteSpace(iconeSelecionado))
                SelecionarItemPorChave(iconeSelecionado);
        }

        private void InicializarFormulario()
        {
            Text = "Selecionar icone";
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

            botaoCancelar.Click += BotaoCancelar_Click;

            AcceptButton = botaoOk;
            CancelButton = botaoCancelar;

            CarregarImagensDaPasta();
        }

        private void BotaoCancelar_Click(object sender, EventArgs e)
        {
            Close();
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
            if (cadastro.Configuracoes.IntBaseID < 1)
                cadastro.Configuracoes.IntBaseID = 1;

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
                    SchemaVersion = 2,
                    Itens = new List<ItemPersistido>()
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

                if (cadastro.Itens == null)
                    cadastro.Itens = new List<ItemPersistido>();

                return cadastro;
            }
        }

        private void buttonAbrirPastaImagens_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(PastaImagens);
                Process.Start("explorer.exe", PastaImagens);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel abrir a pasta de imagens.\r\n" + ex.Message,
                    "Pasta Imagens", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBoxCaminhoImagem_TextChanged(object sender, EventArgs e)
        {
            AtualizarNomeIconeSaida(textBoxCaminhoImagem.Text);
            IniciarAtualizacaoPreview(textBoxCaminhoImagem.Text);
        }

        private void textBoxCaminhoImagem_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var arquivos = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (arquivos != null && arquivos.Length > 0)
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }

            e.Effect = DragDropEffects.None;
        }

        private void textBoxCaminhoImagem_DragDrop(object sender, DragEventArgs e)
        {
            var arquivos = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (arquivos != null && arquivos.Length > 0)
            {
                textBoxCaminhoImagem.Text = arquivos[0];
            }
        }

        private void pictureBoxImagem_DragEnter(object sender, DragEventArgs e)
        {
            textBoxCaminhoImagem_DragEnter(sender, e);
        }

        private void pictureBoxImagem_DragDrop(object sender, DragEventArgs e)
        {
            textBoxCaminhoImagem_DragDrop(sender, e);
        }

        private void pictureBoxImagem_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(pictureBoxImagem.BackColor);

            if (imagemPreviewOriginal == null ||
                pictureBoxImagem.ClientSize.Width <= 0 ||
                pictureBoxImagem.ClientSize.Height <= 0)
            {
                return;
            }

            double escalaX = (double)pictureBoxImagem.ClientSize.Width / imagemPreviewOriginal.Width;
            double escalaY = (double)pictureBoxImagem.ClientSize.Height / imagemPreviewOriginal.Height;
            double escala = Math.Min(1.0, Math.Min(escalaX, escalaY));

            int largura = (int)Math.Round(imagemPreviewOriginal.Width * escala);
            int altura = (int)Math.Round(imagemPreviewOriginal.Height * escala);
            int x = (pictureBoxImagem.ClientSize.Width - largura) / 2;
            int y = (pictureBoxImagem.ClientSize.Height - altura) / 2;

            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.DrawImage(imagemPreviewOriginal, new Rectangle(x, y, largura, altura));
        }

        private void pictureBoxImagem_Resize(object sender, EventArgs e)
        {
            pictureBoxImagem.Invalidate();
        }

        private async void buttonGerarIcone_Click(object sender, EventArgs e)
        {
            string caminhoImagem = textBoxCaminhoImagem.Text.Trim();

            if (string.IsNullOrWhiteSpace(caminhoImagem))
            {
                MessageBox.Show("Informe o caminho local ou a URL de uma imagem.", "Imagem nao informada",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            buttonGerarIcone.Enabled = false;

            try
            {
                byte[] imagemOriginal = await ObterBytesImagemAsync(caminhoImagem);
                Directory.CreateDirectory(PastaImagens);

                string nomeIconeSaida = ObterNomeIconeSaidaComExtensaoPng(textBoxNomeIconeSaida.Text);
                if (string.IsNullOrWhiteSpace(nomeIconeSaida))
                {
                    MessageBox.Show("Informe o nome do icone de saida.", "Nome nao informado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                textBoxNomeIconeSaida.Text = nomeIconeSaida;

                string caminhoIcone = ObterCaminhoUnico(PastaImagens, nomeIconeSaida);

                GerarPng16x16(imagemOriginal, caminhoImagem, caminhoIcone);

                NomeIconeGerado = Path.GetFileName(caminhoIcone);
                CarregarImagensDaPasta();
                SelecionarItemPorChave(NomeIconeGerado);
                TreeViewImages.Focus();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Arquivo nao encontrado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show("Nao foi possivel baixar a imagem da URL informada.\n\n" + ex.Message,
                    "Falha ao baixar imagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel gerar o icone.\n\n" + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                buttonGerarIcone.Enabled = true;
            }
        }

        private void buttonProcurar_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Selecionar imagem";
                openFileDialog.Filter = "Imagens|*.png;*.jpg;*.jpeg;*.ico;*.webp|PNG (*.png)|*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|ICO (*.ico)|*.ico|WEBP (*.webp)|*.webp|Todos os arquivos (*.*)|*.*";
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxCaminhoImagem.Text = openFileDialog.FileName;
                }
            }
        }

        private void AtualizarNomeIconeSaida(string caminhoImagem)
        {
            string nomeArquivo = ObterNomeArquivoSemExtensao(caminhoImagem);
            textBoxNomeIconeSaida.Text = nomeArquivo;
        }

        private static string ObterNomeArquivoSemExtensao(string caminhoImagem)
        {
            if (string.IsNullOrWhiteSpace(caminhoImagem))
            {
                return string.Empty;
            }

            caminhoImagem = caminhoImagem.Trim();

            Uri uri;
            if (Uri.TryCreate(caminhoImagem, UriKind.Absolute, out uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                return Path.GetFileNameWithoutExtension(uri.LocalPath);
            }

            try
            {
                string caminhoLocal = Path.GetFullPath(caminhoImagem);
                if (!File.Exists(caminhoLocal))
                {
                    return string.Empty;
                }

                return Path.GetFileNameWithoutExtension(caminhoLocal);
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string ObterNomeIconeSaidaComExtensaoPng(string nomeIconeSaida)
        {
            if (string.IsNullOrWhiteSpace(nomeIconeSaida))
            {
                return string.Empty;
            }

            nomeIconeSaida = Path.GetFileName(nomeIconeSaida.Trim());
            if (string.IsNullOrWhiteSpace(nomeIconeSaida))
            {
                return string.Empty;
            }

            if (nomeIconeSaida.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                return nomeIconeSaida;
            }

            return nomeIconeSaida + ".png";
        }

        private static string ObterCaminhoUnico(string pastaDestino, string nomeIconeSaida)
        {
            string nomeBase = Path.GetFileNameWithoutExtension(nomeIconeSaida);
            string extensao = Path.GetExtension(nomeIconeSaida);
            string caminhoIcone = Path.Combine(pastaDestino, nomeIconeSaida);
            int contador = 1;

            while (File.Exists(caminhoIcone))
            {
                caminhoIcone = Path.Combine(pastaDestino,
                    string.Format("{0}_{1}{2}", nomeBase, contador, extensao));
                contador++;
            }

            return caminhoIcone;
        }

        private void IniciarAtualizacaoPreview(string caminhoImagem)
        {
            CancelarAtualizacaoPreview();

            previewCancellationTokenSource = new CancellationTokenSource();
            int versao = Interlocked.Increment(ref previewRequestVersion);
            _ = AtualizarPreviewAsync(caminhoImagem, previewCancellationTokenSource.Token, versao);
        }

        private void CancelarAtualizacaoPreview()
        {
            Interlocked.Increment(ref previewRequestVersion);

            if (previewCancellationTokenSource != null)
            {
                previewCancellationTokenSource.Cancel();
                previewCancellationTokenSource.Dispose();
                previewCancellationTokenSource = null;
            }
        }

        private async Task AtualizarPreviewAsync(string caminhoImagem, CancellationToken cancellationToken, int versao)
        {
            Image preview = null;
            Image previewIcone16x16 = null;

            try
            {
                await Task.Delay(300, cancellationToken);

                caminhoImagem = caminhoImagem.Trim();
                if (string.IsNullOrWhiteSpace(caminhoImagem))
                {
                    AplicarPreviewSeAtual(null, null, versao);
                    return;
                }

                byte[] imagemOriginal = await ObterBytesImagemAsync(caminhoImagem);
                cancellationToken.ThrowIfCancellationRequested();

                preview = CriarImagemPreview(imagemOriginal, caminhoImagem);
                previewIcone16x16 = CriarIcone16x16(imagemOriginal, caminhoImagem);
                if (!AplicarPreviewSeAtual(preview, previewIcone16x16, versao))
                {
                    preview.Dispose();
                    previewIcone16x16.Dispose();
                }

                preview = null;
                previewIcone16x16 = null;
            }
            catch (OperationCanceledException)
            {
            }
            catch
            {
                AplicarPreviewSeAtual(null, null, versao);
            }
            finally
            {
                if (preview != null)
                {
                    preview.Dispose();
                }

                if (previewIcone16x16 != null)
                {
                    previewIcone16x16.Dispose();
                }
            }
        }

        private bool AplicarPreviewSeAtual(Image preview, Image previewIcone16x16, int versao)
        {
            if (versao != previewRequestVersion)
            {
                return false;
            }

            TrocarImagemPreview(preview, previewIcone16x16);
            return true;
        }

        private void TrocarImagemPreview(Image novaImagem, Image novoIcone16x16)
        {
            Image imagemAnterior = imagemPreviewOriginal;
            Image iconeAnterior = pictureBoxPreviwIcon.Image;

            imagemPreviewOriginal = novaImagem;
            pictureBoxPreviwIcon.Image = novoIcone16x16;
            pictureBoxImagem.Invalidate();

            if (imagemAnterior != null)
            {
                imagemAnterior.Dispose();
            }

            if (iconeAnterior != null)
            {
                iconeAnterior.Dispose();
            }
        }

        private static Image CriarImagemPreview(byte[] imagemOriginal, string caminhoImagem)
        {
            if (EhArquivoIco(caminhoImagem))
            {
                using (var streamIcone = new MemoryStream(imagemOriginal))
                using (var icon = new Icon(streamIcone))
                {
                    return icon.ToBitmap();
                }
            }

            byte[] imagemParaPreview = EhArquivoWebp(caminhoImagem)
                ? WebpToPngConverter.ConvertToPngBytes(imagemOriginal)
                : imagemOriginal;

            using (var streamImagem = new MemoryStream(imagemParaPreview))
            using (Image imagemPreview = Image.FromStream(streamImagem))
            {
                return new Bitmap(imagemPreview);
            }
        }

        private static async Task<byte[]> ObterBytesImagemAsync(string caminhoImagem)
        {
            Uri uri;

            if (Uri.TryCreate(caminhoImagem, UriKind.Absolute, out uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                return await httpClient.GetByteArrayAsync(uri);
            }

            string caminhoLocal = Path.GetFullPath(caminhoImagem);

            if (!File.Exists(caminhoLocal))
            {
                throw new FileNotFoundException("O arquivo informado nao foi encontrado:\n" + caminhoLocal);
            }

            return File.ReadAllBytes(caminhoLocal);
        }

        private static void GerarPng16x16(byte[] imagemOriginal, string caminhoImagem, string caminhoIcone)
        {
            using (Bitmap icone = CriarIcone16x16(imagemOriginal, caminhoImagem))
            {
                icone.Save(caminhoIcone, ImageFormat.Png);
            }
        }

        private static Bitmap CriarIcone16x16(byte[] imagemOriginal, string caminhoImagem)
        {
            try
            {
                if (EhArquivoIco(caminhoImagem))
                {
                    return CriarIcone16x16DeIco(imagemOriginal);
                }

                byte[] imagemParaGerar = EhArquivoWebp(caminhoImagem)
                    ? WebpToPngConverter.ConvertToPngBytes(imagemOriginal)
                    : imagemOriginal;

                return CriarIcone16x16DeImagem(imagemParaGerar);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "O arquivo informado nao pode ser lido como PNG, JPEG, ICO ou WEBP. Para WEBP, coloque dwebp.exe na mesma pasta do programa.",
                    ex);
            }
        }

        private static Bitmap CriarIcone16x16DeImagem(byte[] imagemOriginal)
        {
            try
            {
                using (var streamImagem = new MemoryStream(imagemOriginal))
                using (Image imagem = Image.FromStream(streamImagem))
                {
                    return CriarBitmap16x16(imagem);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("O arquivo informado nao pode ser lido como imagem.", ex);
            }
        }

        private static Bitmap CriarIcone16x16DeIco(byte[] imagemOriginal)
        {
            try
            {
                using (var streamIcone = new MemoryStream(imagemOriginal))
                using (var icon = new Icon(streamIcone))
                using (Image imagem = icon.ToBitmap())
                {
                    return CriarBitmap16x16(imagem);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("O arquivo ICO informado nao pode ser lido.", ex);
            }
        }

        private static Bitmap CriarBitmap16x16(Image imagem)
        {
            var icone = new Bitmap(16, 16);

            using (Graphics graphics = Graphics.FromImage(icone))
            {
                graphics.Clear(Color.Transparent);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(imagem, new Rectangle(0, 0, 16, 16));
            }

            return icone;
        }

        private static bool EhArquivoIco(string caminhoImagem)
        {
            Uri uri;
            string caminhoParaExtensao = caminhoImagem;

            if (Uri.TryCreate(caminhoImagem, UriKind.Absolute, out uri))
            {
                caminhoParaExtensao = uri.LocalPath;
            }

            return string.Equals(Path.GetExtension(caminhoParaExtensao), ".ico", StringComparison.OrdinalIgnoreCase);
        }

        private static bool EhArquivoWebp(string caminhoImagem)
        {
            Uri uri;
            string caminhoParaExtensao = caminhoImagem;

            if (Uri.TryCreate(caminhoImagem, UriKind.Absolute, out uri))
            {
                caminhoParaExtensao = uri.LocalPath;
            }

            return string.Equals(Path.GetExtension(caminhoParaExtensao), ".webp", StringComparison.OrdinalIgnoreCase);
        }
    }
}
