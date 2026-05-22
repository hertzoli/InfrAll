using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    public partial class FormIconGenerator : Form
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly string pastaDestino;
        private CancellationTokenSource previewCancellationTokenSource;
        private int previewRequestVersion;
        private System.Drawing.Image imagemPreviewOriginal;

        public FormIconGenerator(string pastaDestino)
        {
            if (string.IsNullOrWhiteSpace(pastaDestino))
                throw new ArgumentException("A pasta de destino dos icones deve ser informada.", "pastaDestino");

            this.pastaDestino = pastaDestino;
            InitializeComponent();
        }

        public string NomeIconeGerado { get; private set; }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CancelarAtualizacaoPreview();
            TrocarImagemPreview(null);
            base.OnFormClosed(e);
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
            e.Graphics.DrawImage(imagemPreviewOriginal, new System.Drawing.Rectangle(x, y, largura, altura));
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
                Directory.CreateDirectory(pastaDestino);

                string nomeIconeSaida = ObterNomeIconeSaidaComExtensaoPng(textBoxNomeIconeSaida.Text);
                if (string.IsNullOrWhiteSpace(nomeIconeSaida))
                {
                    MessageBox.Show("Informe o nome do icone de saida.", "Nome nao informado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                textBoxNomeIconeSaida.Text = nomeIconeSaida;

                string caminhoIcone = ObterCaminhoUnico(pastaDestino, nomeIconeSaida);

                GerarPng16x16(imagemOriginal, caminhoImagem, caminhoIcone);

                NomeIconeGerado = Path.GetFileName(caminhoIcone);

                DialogResult = DialogResult.OK;
                Close();
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
            try
            {
                await Task.Delay(300, cancellationToken);

                caminhoImagem = caminhoImagem.Trim();
                if (string.IsNullOrWhiteSpace(caminhoImagem))
                {
                    AplicarPreviewSeAtual(null, versao);
                    return;
                }

                byte[] imagemOriginal = await ObterBytesImagemAsync(caminhoImagem);
                cancellationToken.ThrowIfCancellationRequested();

                System.Drawing.Image preview = CriarImagemPreview(imagemOriginal, caminhoImagem);
                if (!AplicarPreviewSeAtual(preview, versao))
                {
                    preview.Dispose();
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch
            {
                AplicarPreviewSeAtual(null, versao);
            }
        }

        private bool AplicarPreviewSeAtual(System.Drawing.Image preview, int versao)
        {
            if (versao != previewRequestVersion)
            {
                return false;
            }

            TrocarImagemPreview(preview);
            return true;
        }

        private void TrocarImagemPreview(System.Drawing.Image novaImagem)
        {
            System.Drawing.Image imagemAnterior = imagemPreviewOriginal;
            imagemPreviewOriginal = novaImagem;
            pictureBoxImagem.Invalidate();

            if (imagemAnterior != null)
            {
                imagemAnterior.Dispose();
            }
        }

        private static System.Drawing.Image CriarImagemPreview(byte[] imagemOriginal, string caminhoImagem)
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
            using (System.Drawing.Image imagemPreview = System.Drawing.Image.FromStream(streamImagem))
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
            if (EhArquivoIco(caminhoImagem))
            {
                GerarIcoComSystemDrawing(imagemOriginal, caminhoIcone);
                return;
            }

            try
            {
                byte[] imagemParaGerar = EhArquivoWebp(caminhoImagem)
                    ? WebpToPngConverter.ConvertToPngBytes(imagemOriginal)
                    : imagemOriginal;

                GerarImagemComSystemDrawing(imagemParaGerar, caminhoIcone);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "O arquivo informado nao pode ser lido como PNG, JPEG, ICO ou WEBP. Para WEBP, coloque dwebp.exe na mesma pasta do programa.",
                    ex);
            }
        }

        private static void GerarImagemComSystemDrawing(byte[] imagemOriginal, string caminhoIcone)
        {
            try
            {
                using (var streamImagem = new MemoryStream(imagemOriginal))
                using (System.Drawing.Image imagem = System.Drawing.Image.FromStream(streamImagem))
                {
                    SalvarPng16x16(imagem, caminhoIcone);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("O arquivo informado nao pode ser lido como imagem.", ex);
            }
        }

        private static void GerarIcoComSystemDrawing(byte[] imagemOriginal, string caminhoIcone)
        {
            try
            {
                using (var streamIcone = new MemoryStream(imagemOriginal))
                using (var icon = new Icon(streamIcone))
                using (System.Drawing.Image imagem = icon.ToBitmap())
                {
                    SalvarPng16x16(imagem, caminhoIcone);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("O arquivo ICO informado nao pode ser lido.", ex);
            }
        }

        private static void SalvarPng16x16(System.Drawing.Image imagem, string caminhoIcone)
        {
            using (var icone = new Bitmap(16, 16))
            using (Graphics graphics = Graphics.FromImage(icone))
            {
                graphics.Clear(System.Drawing.Color.Transparent);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(imagem, new System.Drawing.Rectangle(0, 0, 16, 16));
                icone.Save(caminhoIcone, ImageFormat.Png);
            }
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
