using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GerenciadorSistemas
{
    public partial class Form1 : Form
    {
        private const int VersaoSchemaCadastro = 1;
        private const int LarguraMinimaTreeView = 200;
        private const int LarguraMinimaPropertyGrid = 200;
        private const int LarguraMinimaGroupBoxPropriedade = 397;
        private const int AlturaMinimaTreeView = 500;
        private const int AlturaMinimaPropertyGrid = 500;
        private string _nomePropriedadeSelecionadaOriginal;
        private string _localPropriedadeSelecionadaOriginal;
        private ContextMenuStrip _menuContextoTreeView;
        private TreeNode _noDropHover;
        private TreeDropPosition? _posicaoDropHover;
        private bool _suspenderPersistencia;
        private bool _suspenderMonitoramentoCamposEdicao;
        private readonly ToolTip _toolTipBotoes;
        private readonly Dictionary<TextBox, string> _snapshotCamposEdicao;
        private readonly List<TextBox> _camposTextoEditaveis;
        private readonly Color _corCampoEdicaoPadrao;
        private readonly Color _corCampoEdicaoNaoSalvo = Color.LightYellow;

        public Form1()
        {
            InitializeComponent();
            this.Text += "  v" + Application.ProductVersion;

            //---------------- Verifica update online    
            OnlineAutoUpdate.OnlineAutoUpdateAsync.OnlineAutoUpdateAsync2();
            //------------------------------------
            _toolTipBotoes = new ToolTip(components);
            _snapshotCamposEdicao = new Dictionary<TextBox, string>();
            _camposTextoEditaveis = new List<TextBox>
            {
                textBoxNome,
                textBoxValor,
                textBoxDescrição,
                textBoxCategoria,
                textBoxLocal
            };
            _corCampoEdicaoPadrao = textBoxNome.BackColor;
            FormClosing += Form1_FormClosing;
            buttonCopy.Click += buttonCopy_Click;
            buttonRun.Click += buttonRun_Click;
            buttonCopyPlaceholder.Click += buttonCopyPlaceholder_Click;
            textBoxValor.TextChanged += CampoTipoOuValorAlterado;
            comboBoxTipo.SelectedIndexChanged += CampoTipoOuValorAlterado;
            splitter1.SplitterMoved += splitter1_SplitterMoved;
            splitter3.SplitterMoved += splitter3_SplitterMoved;
            splitter4.SplitterMoved += splitter4_SplitterMoved;
            imageList1.ImageSize = new System.Drawing.Size(16, 16);   // tamanho dos Ã­cones
            InicializarMonitoramentoCamposEdicao();

            InicializarTela();
        }

        private void InicializarTela()
        {
            treeViewItens.ImageList = imageList1;
            CarregarImagensDaPastaDoPrograma();
            treeViewItens.Nodes.Clear();
            treeViewItens.AllowDrop = true;
            treeViewItens.DrawMode = TreeViewDrawMode.OwnerDrawText;
            propertyGridItem.UseWaitCursor = false;
            propertyGridItem.SelectedObject = null;
            ConfigurarMenuContextoTreeView();
            ConfigurarComboBoxTipo();
            ConfigurarToolTips();
            LimparCamposEdicao();
            CarregarCadastroPersistido();
        }

        private void ConfigurarComboBoxTipo()
        {
            comboBoxTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTipo.DataSource = Enum.GetValues(typeof(TipoValorPropriedade));
            comboBoxTipo.SelectedItem = TipoValorPropriedade.Texto;
            AtualizarEstadoButtonRun();
        }

        private void ConfigurarToolTips()
        {
            _toolTipBotoes.SetToolTip(buttonNovoItem, "Criar um novo item na raiz da lista.");
            _toolTipBotoes.SetToolTip(buttonNovoSubItem, "Criar um subitem dentro do item selecionado.");
            _toolTipBotoes.SetToolTip(buttonDuplicar, "Duplicar o item atualmente selecionado.");
            _toolTipBotoes.SetToolTip(buttonExcluirItem, "Excluir o item atualmente selecionado.");
            _toolTipBotoes.SetToolTip(buttonNovaPropriedade, "Adicionar uma nova propriedade ao item selecionado.");
            _toolTipBotoes.SetToolTip(buttonNovaSubPropriedade, "Adicionar uma subpropriedade na propriedade selecionada.");
            _toolTipBotoes.SetToolTip(buttonExcluirPropriedade, "Excluir a propriedade atualmente selecionada.");
            _toolTipBotoes.SetToolTip(buttonRun, "Executar o valor informado como comando.");
            _toolTipBotoes.SetToolTip(buttonCopy, "Copiar o valor informado para a area de transferencia.");
            _toolTipBotoes.SetToolTip(buttonCopyPlaceholder, "Copiar o placeholder da propriedade para a area de transferencia.");
            _toolTipBotoes.SetToolTip(buttonSalvar, "Salvar as alteracoes da propriedade atual.");
            _toolTipBotoes.SetToolTip(textBoxLocal, "Informar o local associado a propriedade selecionada.");
            _toolTipBotoes.SetToolTip(textBoxCategoria, "Informar a categoria usada para organizar a propriedade.");
            _toolTipBotoes.SetToolTip(comboBoxTipo, "Definir se o valor da propriedade e um texto, comando ou script.");
        }

        private void InicializarMonitoramentoCamposEdicao()
        {
            foreach (TextBox campo in _camposTextoEditaveis)
            {
                _snapshotCamposEdicao[campo] = string.Empty;
                campo.TextChanged += CampoEdicao_TextChanged;
            }
        }

        private void CampoEdicao_TextChanged(object sender, EventArgs e)
        {
            if (_suspenderMonitoramentoCamposEdicao)
                return;

            TextBox campo = sender as TextBox;

            if (campo == null)
                return;

            AtualizarDestaqueCampoEdicao(campo);
        }

        private void ExecutarSemMonitoramentoCamposEdicao(Action acao)
        {
            bool suspensaoAnterior = _suspenderMonitoramentoCamposEdicao;
            _suspenderMonitoramentoCamposEdicao = true;

            try
            {
                acao();
            }
            finally
            {
                _suspenderMonitoramentoCamposEdicao = suspensaoAnterior;
            }
        }

        private void SincronizarEstadoCamposEdicao()
        {
            foreach (TextBox campo in _camposTextoEditaveis)
            {
                _snapshotCamposEdicao[campo] = campo.Text ?? string.Empty;
                campo.BackColor = _corCampoEdicaoPadrao;
            }
        }

        private void AtualizarDestaqueCampoEdicao(TextBox campo)
        {
            string valorBase;

            if (!_snapshotCamposEdicao.TryGetValue(campo, out valorBase))
                valorBase = string.Empty;

            string valorAtual = campo.Text ?? string.Empty;
            campo.BackColor = string.Equals(valorAtual, valorBase ?? string.Empty, StringComparison.Ordinal)
                ? _corCampoEdicaoPadrao
                : _corCampoEdicaoNaoSalvo;
        }

        private void splitter3_SplitterMoved(object sender, SplitterEventArgs e)
        {
            int larguraPainelEsquerdo = panel2.Width;
            int larguraPainelDireito = panel4.Width;

            if (larguraPainelEsquerdo < LarguraMinimaTreeView)
                panel2.Width = LarguraMinimaTreeView;
            else if (larguraPainelDireito < LarguraMinimaPropertyGrid)
                panel2.Width = ClientSize.Width - panel3.Width - splitter4.Width - splitter3.Width - LarguraMinimaPropertyGrid;
        }

        private void splitter4_SplitterMoved(object sender, SplitterEventArgs e)
        {
            int larguraPainelDireito = panel3.Width;
            int larguraPainelCentral = panel4.Width;
            int larguraMinimaPainelDireito = LarguraMinimaGroupBoxPropriedade + (panel3.Width - groupBox1.Width);

            if (larguraPainelCentral < LarguraMinimaPropertyGrid)
                panel3.Width = ClientSize.Width - panel2.Width - splitter3.Width - splitter4.Width - LarguraMinimaPropertyGrid;
            else if (larguraPainelDireito < larguraMinimaPainelDireito)
                panel3.Width = larguraMinimaPainelDireito;
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            int alturaAreaSuperior = ClientSize.Height - panel1.Height - splitter1.Height;
            int alturaMinimaNecessaria = Math.Max(
                panel6.Height + AlturaMinimaTreeView,
                panel5.Height + AlturaMinimaPropertyGrid);

            if (alturaAreaSuperior < alturaMinimaNecessaria)
                panel1.Height = ClientSize.Height - splitter1.Height - alturaMinimaNecessaria;

            if (panel1.Height < 60)
                panel1.Height = 60;
        }

        private void buttonNovoItem_Click(object sender, EventArgs e)
        {
            CriarNovoItem(null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            PersistirCadastro();
        }

        private void buttonNovoSubItem_Click(object sender, EventArgs e)
        {
            TreeNode noPai = treeViewItens.SelectedNode;

            if (noPai == null)
            {
                MessageBox.Show("Selecione um item pai antes de criar um subitem.", "Criar subitem",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CriarNovoItem(noPai);
        }

        private void buttonExcluirItem_Click(object sender, EventArgs e)
        {
            TreeNode noSelecionado = treeViewItens.SelectedNode;

            if (noSelecionado == null)
            {
                MessageBox.Show("Selecione um item para excluir.", "Excluir item",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmacao = MessageBox.Show(
                "Excluir o item selecionado e todos os subitens?",
                "Confirmar exclusao",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacao != DialogResult.Yes)
                return;

            if (noSelecionado.Parent == null)
                treeViewItens.Nodes.Remove(noSelecionado);
            else
                noSelecionado.Parent.Nodes.Remove(noSelecionado);

            if (treeViewItens.SelectedNode == null)
            {
                propertyGridItem.SelectedObject = null;
                LimparCamposEdicao();
                LimparContextoDaPropriedadeSelecionada();
            }

            PersistirCadastro();
        }

        private void buttonDuplicar_Click(object sender, EventArgs e)
        {
            DuplicarItemSelecionado();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            string valorResolvido;
            string erro;

            if (!TryResolverTextoComReferencias(textBoxValor.Text, out valorResolvido, out erro))
            {
                MessageBox.Show(erro, "Referencia invalida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Clipboard.SetText(valorResolvido ?? string.Empty);
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (!PodeExecutarTipoSelecionado())
            {
                MessageBox.Show("O tipo da propriedade atual nao permite execucao.", "Run",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string valorResolvido;
            string erro;

            if (!TryResolverTextoComReferencias(textBoxValor.Text, out valorResolvido, out erro))
            {
                MessageBox.Show(erro, "Referencia invalida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(valorResolvido))
            {
                MessageBox.Show("Nao ha comando para executar.", "Run", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                ProcessStartInfo processInfo = CriarProcessStartInfoParaExecucao(valorResolvido, ObterTipoSelecionado());
                Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nao foi possivel executar o comando.\r\n" + ex.Message,
                    "Erro ao executar comando", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCopyPlaceholder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxReferenciaPropriedade.Text))
                return;

            Clipboard.SetText(textBoxReferenciaPropriedade.Text);
        }

        private static ProcessStartInfo CriarProcessStartInfoParaComando(string comando)
        {
            string comandoNormalizado = (comando ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(comandoNormalizado))
                throw new InvalidOperationException("Nao ha comando para executar.");

            if (EhUrlAbsoluta(comandoNormalizado))
                return CriarProcessStartInfoShell(comandoNormalizado);

            if (EhDiretorioExistente(comandoNormalizado))
                return CriarProcessStartInfoExplorer(comandoNormalizado);

            if (EhArquivoExistenteSemArgumentos(comandoNormalizado))
                return CriarProcessStartInfoParaArquivoOuScript(comandoNormalizado, string.Empty);

            string arquivoExecutavel;
            string argumentos;

            if (TryExtrairExecutavelEArgumentos(comandoNormalizado, out arquivoExecutavel, out argumentos)
                && !string.IsNullOrWhiteSpace(arquivoExecutavel))
            {
                if (EhUrlAbsoluta(arquivoExecutavel))
                    return CriarProcessStartInfoShell(comandoNormalizado);

                if (Directory.Exists(arquivoExecutavel))
                    return CriarProcessStartInfoExplorer(arquivoExecutavel);

                if (File.Exists(arquivoExecutavel))
                    return CriarProcessStartInfoParaArquivoOuScript(arquivoExecutavel, argumentos);
            }

            ProcessStartInfo processInfoCmd = new ProcessStartInfo();
            processInfoCmd.FileName = "cmd.exe";
            processInfoCmd.Arguments = "/S /C \"" + comandoNormalizado.Replace("\"", "\\\"") + "\"";
            processInfoCmd.UseShellExecute = true;
            return processInfoCmd;
        }

        private static ProcessStartInfo CriarProcessStartInfoParaExecucao(string conteudoResolvido, TipoValorPropriedade tipo)
        {
            if (tipo == TipoValorPropriedade.Script)
                return CriarProcessStartInfoParaScriptBatchTemporario(conteudoResolvido);

            return CriarProcessStartInfoParaComando(conteudoResolvido);
        }

        private static ProcessStartInfo CriarProcessStartInfoParaScriptBatchTemporario(string conteudoScript)
        {
            string conteudoNormalizado = NormalizarConteudoDeScriptBatch(conteudoScript);
            if (string.IsNullOrWhiteSpace(conteudoNormalizado))
                throw new InvalidOperationException("Nao ha script para executar.");

            string pastaTemporaria = Path.Combine(Path.GetTempPath(), "GerenciadorSistemas", "Scripts");
            Directory.CreateDirectory(pastaTemporaria);

            string nomeArquivo = string.Format(
                "script_{0}_{1}.bat",
                DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                Guid.NewGuid().ToString("N"));

            string caminhoArquivo = Path.Combine(pastaTemporaria, nomeArquivo);
            File.WriteAllText(caminhoArquivo, conteudoNormalizado, Encoding.Default);

            ProcessStartInfo processInfo = CriarProcessStartInfoShell(caminhoArquivo);
            processInfo.WorkingDirectory = pastaTemporaria;
            return processInfo;
        }

        private static string NormalizarConteudoDeScriptBatch(string conteudoScript)
        {
            string conteudo = conteudoScript ?? string.Empty;

            conteudo = conteudo.Replace("\r\n", "\n").Replace("\r", "\n");
            conteudo = conteudo.Replace("\n", "\r\n");

            if (!conteudo.EndsWith("\r\n", StringComparison.Ordinal))
                conteudo += "\r\n";

            return conteudo;
        }

        private static ProcessStartInfo CriarProcessStartInfoParaArquivoOuScript(string arquivo, string argumentos)
        {
            string extensao = Path.GetExtension(arquivo);
            string diretorioBase = Path.GetDirectoryName(arquivo);

            if (string.Equals(extensao, ".ps1", StringComparison.OrdinalIgnoreCase))
            {
                ProcessStartInfo processInfoPs = new ProcessStartInfo();
                processInfoPs.FileName = "powershell.exe";
                processInfoPs.Arguments = string.Format(
                    "-ExecutionPolicy Bypass -File \"{0}\" {1}",
                    arquivo,
                    argumentos ?? string.Empty);
                processInfoPs.UseShellExecute = true;
                processInfoPs.WorkingDirectory = string.IsNullOrWhiteSpace(diretorioBase)
                    ? AppDomain.CurrentDomain.BaseDirectory
                    : diretorioBase;
                return processInfoPs;
            }

            if (string.Equals(extensao, ".bat", StringComparison.OrdinalIgnoreCase)
                || string.Equals(extensao, ".cmd", StringComparison.OrdinalIgnoreCase))
            {
                ProcessStartInfo processInfoCmd = new ProcessStartInfo();
                processInfoCmd.FileName = "cmd.exe";
                processInfoCmd.Arguments = string.Format("/C \"\"{0}\" {1}\"", arquivo, argumentos ?? string.Empty);
                processInfoCmd.UseShellExecute = true;
                processInfoCmd.WorkingDirectory = string.IsNullOrWhiteSpace(diretorioBase)
                    ? AppDomain.CurrentDomain.BaseDirectory
                    : diretorioBase;
                return processInfoCmd;
            }

            if (string.Equals(extensao, ".exe", StringComparison.OrdinalIgnoreCase))
            {
                ProcessStartInfo processInfoExe = new ProcessStartInfo();
                processInfoExe.FileName = arquivo;
                processInfoExe.Arguments = argumentos ?? string.Empty;
                processInfoExe.UseShellExecute = false;
                processInfoExe.WorkingDirectory = string.IsNullOrWhiteSpace(diretorioBase)
                    ? AppDomain.CurrentDomain.BaseDirectory
                    : diretorioBase;
                return processInfoExe;
            }

            ProcessStartInfo processInfoShell = CriarProcessStartInfoShell(arquivo, argumentos);
            processInfoShell.WorkingDirectory = string.IsNullOrWhiteSpace(diretorioBase)
                ? AppDomain.CurrentDomain.BaseDirectory
                : diretorioBase;
            return processInfoShell;
        }

        private static ProcessStartInfo CriarProcessStartInfoShell(string arquivo)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = arquivo;
            processInfo.UseShellExecute = true;
            return processInfo;
        }

        private static ProcessStartInfo CriarProcessStartInfoShell(string arquivo, string argumentos)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = arquivo;
            processInfo.Arguments = argumentos ?? string.Empty;
            processInfo.UseShellExecute = true;
            return processInfo;
        }

        private static ProcessStartInfo CriarProcessStartInfoExplorer(string diretorio)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = "explorer.exe";
            processInfo.Arguments = "\"" + diretorio + "\"";
            processInfo.UseShellExecute = true;
            return processInfo;
        }

        private static bool EhUrlAbsoluta(string texto)
        {
            Uri uri;
            return Uri.TryCreate(texto, UriKind.Absolute, out uri)
                && (uri.Scheme == Uri.UriSchemeHttp
                    || uri.Scheme == Uri.UriSchemeHttps
                    || uri.Scheme == Uri.UriSchemeFtp);
        }

        private static bool EhDiretorioExistente(string texto)
        {
            string caminho = RemoverAspasExternas(texto);
            return Directory.Exists(caminho);
        }

        private static bool EhArquivoExistenteSemArgumentos(string texto)
        {
            string caminho = RemoverAspasExternas(texto);
            return File.Exists(caminho);
        }

        private static bool TryExtrairExecutavelEArgumentos(string comando, out string arquivoExecutavel, out string argumentos)
        {
            arquivoExecutavel = null;
            argumentos = string.Empty;

            if (string.IsNullOrWhiteSpace(comando))
                return false;

            if (comando.StartsWith("\"", StringComparison.Ordinal))
            {
                int indiceFechamento = comando.IndexOf('"', 1);

                if (indiceFechamento <= 1)
                    return false;

                arquivoExecutavel = comando.Substring(1, indiceFechamento - 1);
                argumentos = comando.Substring(indiceFechamento + 1).TrimStart();
                return true;
            }

            int indiceEspaco = comando.IndexOf(' ');

            if (indiceEspaco < 0)
            {
                arquivoExecutavel = comando;
                return true;
            }

            arquivoExecutavel = comando.Substring(0, indiceEspaco);
            argumentos = comando.Substring(indiceEspaco + 1).TrimStart();
            arquivoExecutavel = RemoverAspasExternas(arquivoExecutavel);
            return true;
        }

        private static string RemoverAspasExternas(string texto)
        {
            string valor = (texto ?? string.Empty).Trim();

            if (valor.Length >= 2 && valor.StartsWith("\"", StringComparison.Ordinal) && valor.EndsWith("\"", StringComparison.Ordinal))
                return valor.Substring(1, valor.Length - 2);

            return valor;
        }

        private void buttonNovaPropriedade_Click(object sender, EventArgs e)
        {
            InfrastructureItem itemSelecionado = ObterItemSelecionado();

            if (itemSelecionado == null)
            {
                MessageBox.Show("Selecione um item na arvore antes de adicionar propriedades.", "Nova propriedade",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PropertyGridSelectionInfo propriedadeSelecionada = ObterPropriedadeSelecionada(propertyGridItem);

            LimparCamposEdicao();
            LimparContextoDaPropriedadeSelecionada();
            ExecutarSemMonitoramentoCamposEdicao(() =>
            {
                textBoxCategoria.Text = string.Empty;
                textBoxLocal.Text = ObterLocalParaNovaPropriedade(itemSelecionado, propriedadeSelecionada);
                if (propriedadeSelecionada != null && !string.IsNullOrWhiteSpace(propriedadeSelecionada.Categoria))
                    textBoxCategoria.Text = string.Empty;
            });
            SincronizarEstadoCamposEdicao();
            textBoxNome.Focus();
        }

        private void buttonNovaSubPropriedade_Click(object sender, EventArgs e)
        {
            InfrastructureItem itemSelecionado = ObterItemSelecionado();

            if (itemSelecionado == null)
            {
                MessageBox.Show("Selecione um item na arvore antes de adicionar subpropriedades.", "Nova subpropriedade",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PropertyGridSelectionInfo propriedadeSelecionada = ObterPropriedadeSelecionada(propertyGridItem);

            if (propriedadeSelecionada == null)
            {
                MessageBox.Show("Selecione uma propriedade no PropertyGrid para criar uma subpropriedade.", "Nova subpropriedade",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            LimparCamposEdicao();
            LimparContextoDaPropriedadeSelecionada();
            ExecutarSemMonitoramentoCamposEdicao(() =>
            {
                textBoxCategoria.Text = string.Empty;
                textBoxLocal.Text = ObterLocalParaNovaSubpropriedade(propriedadeSelecionada);
            });
            SincronizarEstadoCamposEdicao();
            textBoxNome.Focus();
        }

        private void buttonExcluirPropriedade_Click(object sender, EventArgs e)
        {
            InfrastructureItem itemSelecionado = ObterItemSelecionado();

            if (itemSelecionado == null)
            {
                MessageBox.Show("Selecione um item na arvore antes de remover propriedades.", "Excluir propriedade",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxNome.Text))
            {
                MessageBox.Show("Selecione uma propriedade no PropertyGrid para remover.", "Excluir propriedade",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (EhPropriedadeProtegida(textBoxNome.Text, textBoxLocal.Text))
            {
                MessageBox.Show("As propriedades padrao do item nao podem ser removidas.", "Excluir propriedade",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (MessageBox.Show("Tem Certeza que deseja Excluir essa propriedade?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                bool removida = itemSelecionado.Builder.RemoverPropriedade(textBoxNome.Text, textBoxLocal.Text);

                if (!removida)
                {
                    MessageBox.Show("A propriedade informada nao foi encontrada no item selecionado.", "Excluir propriedade",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            AtualizarPropertyGrid(itemSelecionado);
            LimparCamposEdicao();
            LimparContextoDaPropriedadeSelecionada();
            PersistirCadastro();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            InfrastructureItem itemSelecionado = ObterItemSelecionado();

            if (itemSelecionado == null)
                return;

            itemSelecionado.SincronizarMetadados();
            AtualizarPropertyGrid(itemSelecionado);
        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            InfrastructureItem itemSelecionado = ObterItemSelecionado();

            if (itemSelecionado == null)
            {
                MessageBox.Show("Selecione um item na arvore antes de salvar propriedades.", "Salvar propriedade",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string nomePropriedade = (textBoxNome.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nomePropriedade))
            {
                MessageBox.Show("Informe o nome da propriedade.", "Salvar propriedade",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string localPropriedade = (textBoxLocal.Text ?? string.Empty).Trim();
            bool propriedadeExistente = itemSelecionado.Builder.ContemPropriedade(nomePropriedade, localPropriedade);
            bool propriedadeSelecionadaEmEdicao = !string.IsNullOrWhiteSpace(_nomePropriedadeSelecionadaOriginal);

            if (EhPropriedadeProtegida(nomePropriedade, localPropriedade))
            {
                if (!AtualizarMetadadoDoItem(itemSelecionado, nomePropriedade, textBoxValor.Text))
                    return;
            }
            else
            {
                if (!propriedadeSelecionadaEmEdicao && propriedadeExistente)
                {
                    DialogResult confirmacao = MessageBox.Show(
                        "Ja existe uma propriedade com este nome e local. Deseja atualiza-la?",
                        "Atualizar propriedade existente",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmacao != DialogResult.Yes)
                        return;
                }

                if (!RemoverPropriedadeAnteriorSeNecessario(itemSelecionado, nomePropriedade, localPropriedade))
                    return;
                itemSelecionado.Builder.AdicionarPropriedade(
                    nomePropriedade,
                    textBoxValor.Text,
                    textBoxDescri\u00E7\u00E3o.Text,
                    textBoxCategoria.Text,
                    localPropriedade,
                    ObterTipoSelecionado());
            }

            AtualizarPropertyGrid(itemSelecionado);
            AtualizarContextoDaPropriedadeSelecionada(nomePropriedade, localPropriedade);
            SincronizarEstadoCamposEdicao();
            PersistirCadastro();
        }

        private void propertyGridItem_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            PropertyGridSelectionInfo info = ObterPropriedadeSelecionada(propertyGridItem);

            if (info == null)
            {
                LimparCamposEdicao();
                return;
            }

            ExecutarSemMonitoramentoCamposEdicao(() =>
            {
                textBoxNome.Text = info.Nome;
                textBoxValor.Text = Convert.ToString(info.Valor);
                textBoxDescri\u00E7\u00E3o.Text = info.Descricao;
                textBoxCategoria.Text = info.Categoria;
                textBoxLocal.Text = info.Local;
                SelecionarTipoNoCombo(info.Tipo);
                textBoxReferenciaPropriedade.Text = MontarPlaceholderDaPropriedade(info, treeViewItens.SelectedNode);
            });
            AtualizarContextoDaPropriedadeSelecionada(info.Nome, info.Local);
            SincronizarEstadoCamposEdicao();
            AtualizarEstadoButtonRun();
        }

        private void treeViewItens_AfterSelect(object sender, TreeViewEventArgs e)
        {
            InfrastructureItem itemSelecionado = ObterItemSelecionado();

            if (itemSelecionado == null)
            {
                propertyGridItem.SelectedObject = null;
                LimparCamposEdicao();
                LimparContextoDaPropriedadeSelecionada();
                textBoxReferenciaPropriedade.Text = string.Empty;
                AtualizarEstadoButtonRun();
                return;
            }

            itemSelecionado.SincronizarMetadados();
            AtualizarPropertyGrid(itemSelecionado);
            LimparCamposEdicao();
            LimparContextoDaPropriedadeSelecionada();
            textBoxReferenciaPropriedade.Text = string.Empty;
            AtualizarEstadoButtonRun();
        }

        private void treeViewItens_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F2)
            {
                buttonEditar_Click(sender, EventArgs.Empty);
                e.Handled = true;
                return;
            }

            if (e.KeyCode != Keys.Delete)
                return;

            buttonExcluirItem_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }

        private void treeViewItens_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
                treeViewItens.SelectedNode = e.Node;
        }

        private void treeViewItens_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item == null)
                return;

            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeViewItens_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(typeof(TreeNode))
                ? DragDropEffects.Move
                : DragDropEffects.None;
        }

        private void treeViewItens_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            Point ponto = treeViewItens.PointToClient(new Point(e.X, e.Y));
            TreeNode noDestino = treeViewItens.GetNodeAt(ponto);
            treeViewItens.SelectedNode = noDestino;

            TreeNode noArrastado = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            if (noArrastado == null || noDestino == null || noDestino == noArrastado)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            TreeDropPosition posicao = ObterPosicaoDrop(noDestino, ponto);
            AtualizarIndicadorDrop(noDestino, posicao);

            if (posicao == TreeDropPosition.AsChild && ContemDescendente(noArrastado, noDestino))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            TreeNode paiDestino = ObterPaiParaDrop(noDestino, posicao);
            if (paiDestino != null && ContemDescendente(noArrastado, paiDestino))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;
        }

        private void treeViewItens_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode noArrastado = e.Data.GetData(typeof(TreeNode)) as TreeNode;

            if (noArrastado == null)
            {
                LimparIndicadorDrop();
                return;
            }

            Point ponto = treeViewItens.PointToClient(new Point(e.X, e.Y));
            TreeNode noDestino = treeViewItens.GetNodeAt(ponto);

            if (noDestino == null || noDestino == noArrastado)
            {
                LimparIndicadorDrop();
                return;
            }

            TreeDropPosition posicao = ObterPosicaoDrop(noDestino, ponto);

            if (posicao == TreeDropPosition.AsChild && ContemDescendente(noArrastado, noDestino))
            {
                LimparIndicadorDrop();
                return;
            }

            TreeNode paiDestino = ObterPaiParaDrop(noDestino, posicao);
            if (paiDestino != null && ContemDescendente(noArrastado, paiDestino))
            {
                LimparIndicadorDrop();
                return;
            }

            RemoverNoDaColecaoAtual(noArrastado);

            if (posicao == TreeDropPosition.AsChild)
            {
                noDestino.Nodes.Add(noArrastado);
                noDestino.Expand();
            }
            else
            {
                TreeNodeCollection colecaoDestino = paiDestino == null ? treeViewItens.Nodes : paiDestino.Nodes;
                int indiceDestino = noDestino.Index;

                if (posicao == TreeDropPosition.Below)
                    indiceDestino++;

                if (indiceDestino > colecaoDestino.Count)
                    indiceDestino = colecaoDestino.Count;

                colecaoDestino.Insert(indiceDestino, noArrastado);

                if (paiDestino != null)
                    paiDestino.Expand();
            }

            LimparIndicadorDrop();
            treeViewItens.SelectedNode = noArrastado;
            PersistirCadastro();
        }

        private void treeViewItens_DragLeave(object sender, EventArgs e)
        {
            LimparIndicadorDrop();
        }

        private void treeViewItens_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected)
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
            else
                e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);

            Color corTexto = ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected)
                ? SystemColors.HighlightText
                : treeViewItens.ForeColor;

            TextRenderer.DrawText(
                e.Graphics,
                e.Node.Text,
                treeViewItens.Font,
                e.Bounds,
                corTexto,
                TextFormatFlags.GlyphOverhangPadding);

            if (_noDropHover == e.Node && _posicaoDropHover.HasValue)
                DesenharIndicadorDrop(e.Graphics, e.Bounds, _posicaoDropHover.Value);
        }

        private void CriarNovoItem(TreeNode noPai)
        {
            using (FormNovoItem dialog = new FormNovoItem())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                GarantirIconeCarregadoNoImageListPrincipal(dialog.IconeSelecionado);

                InfrastructureItem novoItem = new InfrastructureItem(
                    dialog.ItemNome,
                    dialog.ItemDescricao,
                    dialog.IconeSelecionado,
                    dialog.Observacao);

                TreeNode novoNo = CriarNoParaItem(novoItem);

                if (noPai == null)
                    treeViewItens.Nodes.Add(novoNo);
                else
                {
                    noPai.Nodes.Add(novoNo);
                    noPai.Expand();
                }

                treeViewItens.ExpandAll();
                treeViewItens.SelectedNode = novoNo;
                novoNo.EnsureVisible();
                PersistirCadastro();
            }
        }

        private TreeNode CriarNoParaItem(InfrastructureItem item)
        {
            TreeNode no = new TreeNode(item.NomeExibicao);
            no.Name = item.NomeExibicao;
            no.Tag = item;
            no.ImageKey = item.IconeKey;
            no.SelectedImageKey = item.IconeKey;
            return no;
        }

        private void DuplicarItemSelecionado()
        {
            TreeNode noSelecionado = treeViewItens.SelectedNode;

            if (noSelecionado == null)
            {
                MessageBox.Show("Selecione um item para duplicar.", "Duplicar item",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TreeNode duplicado = ClonarNo(noSelecionado);
            duplicado.Text = GerarNomeDuplicado(noSelecionado.Text, noSelecionado.Parent);
            duplicado.Name = duplicado.Text;

            InfrastructureItem itemDuplicado = duplicado.Tag as InfrastructureItem;
            if (itemDuplicado != null)
            {
                itemDuplicado.NomeExibicao = duplicado.Text;
                itemDuplicado.CriadoEm = DateTime.Now;
                itemDuplicado.SincronizarMetadados();
            }

            if (noSelecionado.Parent == null)
                treeViewItens.Nodes.Add(duplicado);
            else
            {
                noSelecionado.Parent.Nodes.Add(duplicado);
                noSelecionado.Parent.Expand();
            }

            treeViewItens.ExpandAll();
            treeViewItens.SelectedNode = duplicado;
            duplicado.EnsureVisible();
            PersistirCadastro();
        }

        private InfrastructureItem ObterItemSelecionado()
        {
            TreeNode noSelecionado = treeViewItens.SelectedNode;
            if (noSelecionado == null)
                return null;

            return noSelecionado.Tag as InfrastructureItem;
        }

        private void ConfigurarMenuContextoTreeView()
        {
            if (_menuContextoTreeView != null)
                return;

            _menuContextoTreeView = new ContextMenuStrip();

            ToolStripMenuItem menuNovoItem = new ToolStripMenuItem("Novo Item");
            menuNovoItem.Click += buttonNovoItem_Click;

            ToolStripMenuItem menuNovoSubItem = new ToolStripMenuItem("Novo Sub-Item");
            menuNovoSubItem.Click += buttonNovoSubItem_Click;

            ToolStripMenuItem menuRemover = new ToolStripMenuItem("Remover");
            menuRemover.Click += buttonExcluirItem_Click;

            ToolStripMenuItem menuDuplicar = new ToolStripMenuItem("Duplicar");
            menuDuplicar.Click += buttonDuplicar_Click;

            _menuContextoTreeView.Items.Add(menuNovoItem);
            _menuContextoTreeView.Items.Add(menuNovoSubItem);
            _menuContextoTreeView.Items.Add(menuRemover);
            _menuContextoTreeView.Items.Add(menuDuplicar);

            treeViewItens.ContextMenuStrip = _menuContextoTreeView;
        }

        private void CarregarCadastroPersistido()
        {
            string caminhoArquivo = ObterCaminhoArquivoCadastro();

            if (!File.Exists(caminhoArquivo))
                return;

            try
            {
                IDeserializer deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .IgnoreUnmatchedProperties()
                    .Build();

                using (StreamReader reader = new StreamReader(caminhoArquivo))
                {
                    CadastroPersistido cadastro = deserializer.Deserialize<CadastroPersistido>(reader);

                    if (cadastro == null || cadastro.Items == null)
                        return;

                    _suspenderPersistencia = true;
                    treeViewItens.Nodes.Clear();

                    foreach (ItemPersistido itemPersistido in cadastro.Items)
                        treeViewItens.Nodes.Add(CriarNoAPartirDaPersistencia(itemPersistido));

                    treeViewItens.ExpandAll();

                    if (treeViewItens.Nodes.Count > 0)
                        treeViewItens.SelectedNode = treeViewItens.Nodes[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Nao foi possivel carregar o cadastro salvo.\r\n" + ex.Message,
                    "Erro ao carregar cadastro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                _suspenderPersistencia = false;
            }
        }

        private void PersistirCadastro()
        {
            if (_suspenderPersistencia)
                return;

            try
            {
                CadastroPersistido cadastro = new CadastroPersistido
                {
                    SchemaVersion = VersaoSchemaCadastro,
                    SavedAt = DateTime.Now.ToString("o"),
                    Items = treeViewItens.Nodes.Cast<TreeNode>()
                        .Select(MapearNoParaPersistencia)
                        .ToList()
                };

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
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Nao foi possivel salvar o cadastro.\r\n" + ex.Message,
                    "Erro ao salvar cadastro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private static string ObterCaminhoArquivoCadastro()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cadastro.yaml");
        }

        private TreeNode CriarNoAPartirDaPersistencia(ItemPersistido itemPersistido)
        {
            InfrastructureItem item = new InfrastructureItem(
                itemPersistido.NomeExibicao,
                itemPersistido.Descricao,
                itemPersistido.IconeKey,
                itemPersistido.Observacao);

            DateTime criadoEm;
            if (DateTime.TryParse(itemPersistido.CriadoEm, null, DateTimeStyles.RoundtripKind, out criadoEm))
                item.CriadoEm = criadoEm;

            item.TipoItem = itemPersistido.TipoItem ?? string.Empty;

            if (itemPersistido.Propriedades != null)
            {
                foreach (PropriedadePersistida propriedade in itemPersistido.Propriedades)
                {
                    item.Builder.AdicionarPropriedade(
                        propriedade.Nome,
                        propriedade.Valor,
                        propriedade.Descricao,
                        propriedade.Categoria,
                        propriedade.Local,
                        ConverterTextoParaTipoValor(propriedade.Tipo));
                }
            }

            item.SincronizarMetadados();

            TreeNode no = CriarNoParaItem(item);

            if (itemPersistido.Children != null)
            {
                foreach (ItemPersistido filho in itemPersistido.Children)
                    no.Nodes.Add(CriarNoAPartirDaPersistencia(filho));
            }

            return no;
        }

        private ItemPersistido MapearNoParaPersistencia(TreeNode no)
        {
            InfrastructureItem item = no.Tag as InfrastructureItem;

            return new ItemPersistido
            {
                NomeExibicao = item != null ? item.NomeExibicao : no.Text,
                Descricao = item != null ? item.Descricao : string.Empty,
                IconeKey = item != null ? item.IconeKey : string.Empty,
                Observacao = item != null ? item.Observacao : string.Empty,
                CriadoEm = item != null ? item.CriadoEm.ToString("o") : DateTime.Now.ToString("o"),
                TipoItem = item != null ? item.TipoItem : string.Empty,
                Propriedades = item != null
                    ? item.Builder.Root.EnumeratePropertiesRecursive()
                        .Where(p => !EhPropriedadeProtegida(p.Name, p.Path))
                        .Select(MapearPropriedadeParaPersistencia)
                        .ToList()
                    : new List<PropriedadePersistida>(),
                Children = no.Nodes.Cast<TreeNode>()
                    .Select(MapearNoParaPersistencia)
                    .ToList()
            };
        }

        private static PropriedadePersistida MapearPropriedadeParaPersistencia(DynamicPropertyItem propriedade)
        {
            return new PropriedadePersistida
            {
                Nome = propriedade.Name,
                Valor = Convert.ToString(propriedade.Value) ?? string.Empty,
                Descricao = propriedade.Description ?? string.Empty,
                Categoria = propriedade.Category ?? string.Empty,
                Local = propriedade.Path ?? string.Empty,
                Tipo = TipoValorPropriedadePersistencia.ParaPersistencia(propriedade.Tipo)
            };
        }

        private static bool ContemDescendente(TreeNode noPai, TreeNode possivelDescendente)
        {
            TreeNode atual = possivelDescendente;

            while (atual != null)
            {
                if (atual == noPai)
                    return true;

                atual = atual.Parent;
            }

            return false;
        }

        private void AtualizarIndicadorDrop(TreeNode noDestino, TreeDropPosition posicao)
        {
            bool mesmoEstado = _noDropHover == noDestino && _posicaoDropHover == posicao;
            if (mesmoEstado)
                return;

            TreeNode noAnterior = _noDropHover;
            _noDropHover = noDestino;
            _posicaoDropHover = posicao;

            if (noAnterior != null)
                treeViewItens.Invalidate(noAnterior.Bounds);

            if (_noDropHover != null)
                treeViewItens.Invalidate(_noDropHover.Bounds);
        }

        private void LimparIndicadorDrop()
        {
            if (_noDropHover == null && !_posicaoDropHover.HasValue)
                return;

            TreeNode noAnterior = _noDropHover;
            _noDropHover = null;
            _posicaoDropHover = null;

            if (noAnterior != null)
                treeViewItens.Invalidate(noAnterior.Bounds);
        }

        private static void DesenharIndicadorDrop(Graphics graphics, Rectangle bounds, TreeDropPosition posicao)
        {
            using (Pen caneta = new Pen(Color.DodgerBlue, 2))
            {
                if (posicao == TreeDropPosition.Above)
                    graphics.DrawLine(caneta, bounds.Left, bounds.Top, bounds.Right, bounds.Top);
                else if (posicao == TreeDropPosition.Below)
                    graphics.DrawLine(caneta, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
                else
                    graphics.DrawRectangle(caneta, bounds.Left, bounds.Top, Math.Max(1, bounds.Width - 1), Math.Max(1, bounds.Height - 1));
            }
        }

        private static TreeDropPosition ObterPosicaoDrop(TreeNode noDestino, Point pontoCliente)
        {
            Rectangle area = noDestino.Bounds;
            int alturaFaixa = Math.Max(4, area.Height / 4);

            if (pontoCliente.Y < area.Top + alturaFaixa)
                return TreeDropPosition.Above;

            if (pontoCliente.Y > area.Bottom - alturaFaixa)
                return TreeDropPosition.Below;

            return TreeDropPosition.AsChild;
        }

        private static TreeNode ObterPaiParaDrop(TreeNode noDestino, TreeDropPosition posicao)
        {
            return posicao == TreeDropPosition.AsChild ? noDestino : noDestino.Parent;
        }

        private static void RemoverNoDaColecaoAtual(TreeNode no)
        {
            if (no.Parent == null)
                no.TreeView.Nodes.Remove(no);
            else
                no.Parent.Nodes.Remove(no);
        }

        private TreeNode ClonarNo(TreeNode origem)
        {
            InfrastructureItem itemOrigem = origem.Tag as InfrastructureItem;
            InfrastructureItem itemClone = itemOrigem != null
                ? itemOrigem.Clone()
                : null;

            TreeNode clone = new TreeNode(origem.Text);
            clone.Name = origem.Name;
            clone.Tag = itemClone;
            clone.ImageKey = origem.ImageKey;
            clone.SelectedImageKey = origem.SelectedImageKey;

            foreach (TreeNode filho in origem.Nodes)
                clone.Nodes.Add(ClonarNo(filho));

            return clone;
        }

        private string GerarNomeDuplicado(string nomeBase, TreeNode pai)
        {
            string nomeDuplicadoBase = string.Concat(nomeBase, " - CÃ³pia");
            string nomeAtual = nomeDuplicadoBase;
            int contador = 2;

            while (ExisteNomeNoMesmoNivel(nomeAtual, pai))
            {
                nomeAtual = string.Format("{0} ({1})", nomeDuplicadoBase, contador);
                contador++;
            }

            return nomeAtual;
        }

        private bool ExisteNomeNoMesmoNivel(string nome, TreeNode pai)
        {
            TreeNodeCollection colecao = pai == null ? treeViewItens.Nodes : pai.Nodes;

            foreach (TreeNode no in colecao)
            {
                if (string.Equals(no.Text, nome, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private void AtualizarPropertyGrid(InfrastructureItem item)
        {
            propertyGridItem.SelectedObject = item.Builder.Root;
            propertyGridItem.Refresh();
            ExpandirTudoNoPropertyGrid();
        }

        private void ExpandirTudoNoPropertyGrid()
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                propertyGridItem.ExpandAllGridItems();
                propertyGridItem.Refresh();
            }));
        }

        private string[] ObterChavesDeImagem()
        {
            CarregarImagensDaPastaDoPrograma();

            List<string> chaves = new List<string>();

            for (int i = 0; i < imageList1.Images.Keys.Count; i++)
            {
                string chave = imageList1.Images.Keys[i];
                if (!string.IsNullOrWhiteSpace(chave))
                    chaves.Add(chave);
            }

            if (chaves.Count == 0)
                chaves.Add("Padrao");

            return chaves.ToArray();
        }

        private void CarregarImagensDaPastaDoPrograma()
        {
            string pastaImagens = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens");

            Directory.CreateDirectory(pastaImagens);

            foreach (string arquivo in Directory.GetFiles(pastaImagens))
                GarantirIconeCarregadoNoImageListPrincipal(Path.GetFileName(arquivo));
        }

        private void GarantirIconeCarregadoNoImageListPrincipal(string chaveIcone)
        {
            if (string.IsNullOrWhiteSpace(chaveIcone) || imageList1.Images.ContainsKey(chaveIcone))
                return;

            string caminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens", chaveIcone);

            if (!File.Exists(caminhoArquivo))
                return;

            using (Image imagemOriginal = CarregarImagemParaImageList(caminhoArquivo))
            {
                if (imagemOriginal == null)
                    return;

                using (Bitmap miniatura = new Bitmap(imagemOriginal, new Size(16, 16)))
                {
                    imageList1.Images.Add(chaveIcone, new Bitmap(miniatura));
                }
            }
        }

        private static Image CarregarImagemParaImageList(string caminhoArquivo)
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
                using (Image imagem = Image.FromStream(stream))
                {
                    return new Bitmap(imagem);
                }
            }
        }

        private static bool EhPropriedadeProtegida(string nomePropriedade, string localPropriedade)
        {
            if (!string.Equals((localPropriedade ?? string.Empty).Trim(), "", StringComparison.OrdinalIgnoreCase))
                return false;

            return string.Equals(nomePropriedade, "Nome", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "Descricao Item", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "Imagem/Icone", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "Criado em", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "Observacao/Local", StringComparison.OrdinalIgnoreCase);
        }

        private bool AtualizarMetadadoDoItem(InfrastructureItem item, string nomePropriedade, string valor)
        {
            TreeNode noSelecionado = treeViewItens.SelectedNode;

            if (string.Equals(nomePropriedade, "Nome", StringComparison.OrdinalIgnoreCase))
            {
                string nomeAtualizado = (valor ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(nomeAtualizado))
                {
                    MessageBox.Show("O nome do item nao pode ficar vazio.", "Salvar propriedade",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                item.NomeExibicao = nomeAtualizado;

                if (noSelecionado != null)
                {
                    noSelecionado.Text = nomeAtualizado;
                    noSelecionado.Name = nomeAtualizado;
                }
            }
            else if (string.Equals(nomePropriedade, "Descricao Item", StringComparison.OrdinalIgnoreCase))
                item.Descricao = valor ?? string.Empty;
            else if (string.Equals(nomePropriedade, "Imagem/Icone", StringComparison.OrdinalIgnoreCase))
            {
                string icone = (valor ?? string.Empty).Trim();
                if (!string.IsNullOrWhiteSpace(icone) && imageList1.Images.ContainsKey(icone))
                {
                    item.IconeKey = icone;

                    if (noSelecionado != null)
                    {
                        noSelecionado.ImageKey = icone;
                        noSelecionado.SelectedImageKey = icone;
                    }
                }
            }
            else if (string.Equals(nomePropriedade, "Criado em", StringComparison.OrdinalIgnoreCase))
            {
                DateTime criadoEm;
                if (DateTime.TryParse(valor, CultureInfo.CurrentCulture, DateTimeStyles.None, out criadoEm))
                    item.CriadoEm = criadoEm;
            }
            else if (string.Equals(nomePropriedade, "Observacao/Local", StringComparison.OrdinalIgnoreCase))
                item.Observacao = valor ?? string.Empty;
            else if (string.Equals(nomePropriedade, "Tipo do Item", StringComparison.OrdinalIgnoreCase))
                item.TipoItem = valor ?? string.Empty;

            item.SincronizarMetadados();
            return true;
        }

        private void LimparCamposEdicao()
        {
            ExecutarSemMonitoramentoCamposEdicao(() =>
            {
                textBoxNome.Text = string.Empty;
                textBoxValor.Text = string.Empty;
                textBoxDescri\u00E7\u00E3o.Text = string.Empty;
                textBoxCategoria.Text = string.Empty;
                textBoxLocal.Text = string.Empty;
                textBoxReferenciaPropriedade.Text = string.Empty;
                SelecionarTipoNoCombo(TipoValorPropriedade.Texto);
            });
            SincronizarEstadoCamposEdicao();
            AtualizarEstadoButtonRun();
        }

        private void CampoTipoOuValorAlterado(object sender, EventArgs e)
        {
            AtualizarEstadoButtonRun();
        }

        private void AtualizarEstadoButtonRun()
        {
            buttonRun.Enabled = PodeExecutarTipoSelecionado() && !string.IsNullOrWhiteSpace(textBoxValor.Text);
        }

        private bool PodeExecutarTipoSelecionado()
        {
            TipoValorPropriedade tipoSelecionado = ObterTipoSelecionado();
            return tipoSelecionado == TipoValorPropriedade.Comando
                || tipoSelecionado == TipoValorPropriedade.Script;
        }

        private TipoValorPropriedade ObterTipoSelecionado()
        {
            object valorSelecionado = comboBoxTipo.SelectedItem;

            if (valorSelecionado is TipoValorPropriedade)
                return (TipoValorPropriedade)valorSelecionado;

            return TipoValorPropriedade.Texto;
        }

        private void SelecionarTipoNoCombo(TipoValorPropriedade tipo)
        {
            comboBoxTipo.SelectedItem = tipo;
        }

        private static TipoValorPropriedade ConverterTextoParaTipoValor(string tipo)
        {
            TipoValorPropriedade tipoConvertido;

            if (TipoValorPropriedadePersistencia.TryParse(tipo, out tipoConvertido))
                return tipoConvertido;

            if (Enum.TryParse(tipo ?? string.Empty, true, out tipoConvertido))
                return tipoConvertido;

            return TipoValorPropriedade.Texto;
        }

        private string MontarPlaceholderDaPropriedade(PropertyGridSelectionInfo info, TreeNode noItem)
        {
            if (info == null || noItem == null)
                return string.Empty;

            string caminhoItem = ObterCaminhoCompletoDoNo(noItem);
            string referenciaPropriedade = MontarReferenciaDaPropriedade(info.Local, info.Nome);
            string referenciaCompleta = MontarReferenciaCompleta(caminhoItem, referenciaPropriedade);

            return string.IsNullOrWhiteSpace(referenciaCompleta) ? string.Empty : "{" + referenciaCompleta + "}";
        }

        private bool TryResolverTextoComReferencias(string template, out string valorResolvido, out string erro)
        {
            valorResolvido = template ?? string.Empty;
            erro = null;

            MatchCollection matches = Regex.Matches(valorResolvido, @"\{([^{}]+)\}");

            foreach (Match match in matches)
            {
                string referencia = match.Groups[1].Value.Trim();
                DynamicPropertyItem propriedadeReferenciada;

                if (!TryResolverPropriedadePorReferencia(referencia, out propriedadeReferenciada, out erro))
                {
                    return false;
                }

                valorResolvido = valorResolvido.Replace(
                    match.Value,
                    Convert.ToString(propriedadeReferenciada.Value) ?? string.Empty);
            }

            return true;
        }

        private bool TryResolverPropriedadePorReferencia(string referencia, out DynamicPropertyItem propriedadeReferenciada, out string erro)
        {
            propriedadeReferenciada = null;
            erro = null;

            string referenciaNormalizada = NormalizarReferencia(referencia);
            if (string.IsNullOrWhiteSpace(referenciaNormalizada))
            {
                erro = "Referencia invalida: {" + (referencia ?? string.Empty).Trim() + "}";
                return false;
            }

            TreeNode noAtual = treeViewItens.SelectedNode;
            if (noAtual != null)
            {
                InfrastructureItem itemAtual = noAtual.Tag as InfrastructureItem;
                if (itemAtual != null && itemAtual.Builder.TryGetPropriedadePorReferencia(referenciaNormalizada, out propriedadeReferenciada))
                    return true;
            }

            TreeNode noReferenciado;
            string referenciaDaPropriedade;

            if (!TryEncontrarNoEReferenciaDaPropriedade(referenciaNormalizada, out noReferenciado, out referenciaDaPropriedade))
            {
                erro = "Item referenciado nao encontrado: {" + referenciaNormalizada + "}";
                return false;
            }

            InfrastructureItem itemReferenciado = noReferenciado.Tag as InfrastructureItem;
            if (itemReferenciado == null)
            {
                erro = "Item referenciado nao encontrado: {" + referenciaNormalizada + "}";
                return false;
            }

            if (string.IsNullOrWhiteSpace(referenciaDaPropriedade))
            {
                erro = "Propriedade referenciada nao encontrada: {" + referenciaNormalizada + "}";
                return false;
            }

            if (!itemReferenciado.Builder.TryGetPropriedadePorReferencia(referenciaDaPropriedade, out propriedadeReferenciada))
            {
                erro = "Propriedade referenciada nao encontrada: {" + referenciaNormalizada + "}";
                return false;
            }

            return true;
        }

        private bool TryEncontrarNoEReferenciaDaPropriedade(string referencia, out TreeNode noEncontrado, out string referenciaDaPropriedade)
        {
            noEncontrado = null;
            referenciaDaPropriedade = string.Empty;

            string[] partesReferencia = ObterPartesDaReferencia(referencia);
            if (partesReferencia.Length < 2)
                return false;

            int melhorQuantidadePartes = -1;

            foreach (TreeNode no in EnumerarTodosOsNos())
            {
                string[] partesNo = ObterPartesDoCaminhoDoNo(no);
                if (partesNo.Length == 0 || partesNo.Length >= partesReferencia.Length)
                    continue;

                if (!ReferenciaComecaComCaminhoDoNo(partesReferencia, partesNo))
                    continue;

                if (partesNo.Length <= melhorQuantidadePartes)
                    continue;

                melhorQuantidadePartes = partesNo.Length;
                noEncontrado = no;
                referenciaDaPropriedade = string.Join("/", partesReferencia.Skip(partesNo.Length));
            }

            return noEncontrado != null;
        }

        private IEnumerable<TreeNode> EnumerarTodosOsNos()
        {
            foreach (TreeNode no in treeViewItens.Nodes)
            {
                foreach (TreeNode descendente in EnumerarNoESeusDescendentes(no))
                    yield return descendente;
            }
        }

        private IEnumerable<TreeNode> EnumerarNoESeusDescendentes(TreeNode no)
        {
            if (no == null)
                yield break;

            yield return no;

            foreach (TreeNode filho in no.Nodes)
            {
                foreach (TreeNode descendente in EnumerarNoESeusDescendentes(filho))
                    yield return descendente;
            }
        }

        private static bool ReferenciaComecaComCaminhoDoNo(string[] partesReferencia, string[] partesNo)
        {
            if (partesNo.Length > partesReferencia.Length)
                return false;

            for (int i = 0; i < partesNo.Length; i++)
            {
                if (!string.Equals(partesReferencia[i], partesNo[i], StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        private static string ObterCaminhoCompletoDoNo(TreeNode no)
        {
            if (no == null)
                return string.Empty;

            return string.Join("/", ObterPartesDoCaminhoDoNo(no));
        }

        private static string[] ObterPartesDoCaminhoDoNo(TreeNode no)
        {
            List<string> partes = new List<string>();
            TreeNode atual = no;

            while (atual != null)
            {
                string nome = (atual.Text ?? string.Empty).Trim();
                if (!string.IsNullOrWhiteSpace(nome))
                    partes.Add(nome);

                atual = atual.Parent;
            }

            partes.Reverse();
            return partes.ToArray();
        }

        private static string MontarReferenciaDaPropriedade(string local, string nome)
        {
            string localNormalizado = NormalizarReferencia(local);
            string nomeNormalizado = NormalizarReferencia(nome);

            if (string.IsNullOrWhiteSpace(localNormalizado))
                return nomeNormalizado;

            if (string.IsNullOrWhiteSpace(nomeNormalizado))
                return localNormalizado;

            return string.Concat(localNormalizado, "/", nomeNormalizado);
        }

        private static string MontarReferenciaCompleta(string caminhoItem, string referenciaDaPropriedade)
        {
            string caminhoItemNormalizado = NormalizarReferencia(caminhoItem);
            string referenciaPropriedadeNormalizada = NormalizarReferencia(referenciaDaPropriedade);

            if (string.IsNullOrWhiteSpace(caminhoItemNormalizado))
                return referenciaPropriedadeNormalizada;

            if (string.IsNullOrWhiteSpace(referenciaPropriedadeNormalizada))
                return caminhoItemNormalizado;

            return string.Concat(caminhoItemNormalizado, "/", referenciaPropriedadeNormalizada);
        }

        private static string NormalizarReferencia(string referencia)
        {
            return string.Join("/", ObterPartesDaReferencia(referencia));
        }

        private static string[] ObterPartesDaReferencia(string referencia)
        {
            return (referencia ?? string.Empty)
                .Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(parte => parte.Trim())
                .Where(parte => !string.IsNullOrWhiteSpace(parte))
                .ToArray();
        }

        private string ObterLocalParaNovaPropriedade(InfrastructureItem itemSelecionado, PropertyGridSelectionInfo propriedadeSelecionada)
        {
            if (propriedadeSelecionada != null && !string.IsNullOrWhiteSpace(propriedadeSelecionada.Local))
                return propriedadeSelecionada.Local;

            if (itemSelecionado.Builder.ContemPropriedade("Endereco IP", "Sistema/Rede"))
                return "Sistema/Rede";

            return "";
        }

        private static string ObterLocalParaNovaSubpropriedade(PropertyGridSelectionInfo propriedadeSelecionada)
        {
            string localBase = (propriedadeSelecionada.Local ?? string.Empty).Trim();
            string nomePai = (propriedadeSelecionada.Nome ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(localBase))
                return nomePai;

            if (string.IsNullOrWhiteSpace(nomePai))
                return localBase;

            return string.Concat(localBase, "/", nomePai);
        }

        private void AtualizarContextoDaPropriedadeSelecionada(string nomePropriedade, string localPropriedade)
        {
            _nomePropriedadeSelecionadaOriginal = (nomePropriedade ?? string.Empty).Trim();
            _localPropriedadeSelecionadaOriginal = (localPropriedade ?? string.Empty).Trim();
        }

        private void LimparContextoDaPropriedadeSelecionada()
        {
            _nomePropriedadeSelecionadaOriginal = string.Empty;
            _localPropriedadeSelecionadaOriginal = string.Empty;
        }

        private bool RemoverPropriedadeAnteriorSeNecessario(InfrastructureItem item, string novoNomePropriedade, string novoLocalPropriedade)
        {
            if (string.IsNullOrWhiteSpace(_nomePropriedadeSelecionadaOriginal))
                return true;

            bool mesmoIdentificador =
                string.Equals(_nomePropriedadeSelecionadaOriginal, novoNomePropriedade, StringComparison.OrdinalIgnoreCase)
                && string.Equals(_localPropriedadeSelecionadaOriginal, novoLocalPropriedade, StringComparison.OrdinalIgnoreCase);

            if (mesmoIdentificador)
                return true;

            if (EhPropriedadeProtegida(_nomePropriedadeSelecionadaOriginal, _localPropriedadeSelecionadaOriginal))
                return true;

            bool conflitoDestino;
            bool atualizada = item.Builder.AtualizarPropriedade(
                _nomePropriedadeSelecionadaOriginal,
                _localPropriedadeSelecionadaOriginal,
                novoNomePropriedade,
                textBoxValor.Text,
                textBoxDescri\u00E7\u00E3o.Text,
                textBoxCategoria.Text,
                novoLocalPropriedade,
                ObterTipoSelecionado(),
                out conflitoDestino);

            if (conflitoDestino)
            {
                MessageBox.Show(
                    "Ja existe outra propriedade com este nome e local. Escolha um identificador diferente.",
                    "Salvar propriedade",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return atualizada;
        }

        private PropertyGridSelectionInfo ObterPropriedadeSelecionada(PropertyGrid propertyGrid)
        {
            if (propertyGrid == null || propertyGrid.SelectedGridItem == null)
                return null;

            GridItem item = propertyGrid.SelectedGridItem;

            if (item.PropertyDescriptor == null)
                return null;

            DynamicNodePropertyDescriptor dynProp = item.PropertyDescriptor as DynamicNodePropertyDescriptor;
            if (dynProp == null)
                return null;

            return new PropertyGridSelectionInfo
            {
                Nome = dynProp.Item.Name,
                Valor = dynProp.Item.Value,
                Descricao = dynProp.Item.Description,
                Categoria = dynProp.Item.Category,
                Local = dynProp.Item.Path,
                Tipo = dynProp.Item.Tipo
            };
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            TreeNode noSelecionado = treeViewItens.SelectedNode;
            InfrastructureItem itemSelecionado = noSelecionado != null ? noSelecionado.Tag as InfrastructureItem : null;

            if (noSelecionado == null || itemSelecionado == null)
            {
                MessageBox.Show("Selecione um item para editar.", "Editar item",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (FormNovoItem dialog = new FormNovoItem())
            {
                dialog.ConfigurarParaEdicao(
                    itemSelecionado.NomeExibicao,
                    itemSelecionado.Descricao,
                    itemSelecionado.Observacao,
                    itemSelecionado.IconeKey);

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                GarantirIconeCarregadoNoImageListPrincipal(dialog.IconeSelecionado);

                itemSelecionado.NomeExibicao = dialog.ItemNome;
                itemSelecionado.Descricao = dialog.ItemDescricao;
                itemSelecionado.Observacao = dialog.Observacao;
                itemSelecionado.IconeKey = dialog.IconeSelecionado;
                itemSelecionado.SincronizarMetadados();

                noSelecionado.Text = itemSelecionado.NomeExibicao;
                noSelecionado.Name = itemSelecionado.NomeExibicao;
                noSelecionado.ImageKey = itemSelecionado.IconeKey;
                noSelecionado.SelectedImageKey = itemSelecionado.IconeKey;

                AtualizarPropertyGrid(itemSelecionado);
                treeViewItens.SelectedNode = noSelecionado;
                noSelecionado.EnsureVisible();
                PersistirCadastro();
            }
        }

     
        private void treeViewItens_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
                treeViewItens.SelectedNode = e.Node;

            buttonEditar_Click(sender, EventArgs.Empty);
        }

        private void buttonIssue_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/hertzoli/InfrAll/issues");
        }
    }

    internal sealed class InfrastructureItem
    {
        public InfrastructureItem(string nomeExibicao, string descricao, string iconeKey, string observacao)
        {
            NomeExibicao = nomeExibicao;
            Descricao = descricao ?? string.Empty;
            IconeKey = iconeKey ?? string.Empty;
            Observacao = observacao ?? string.Empty;
            CriadoEm = DateTime.Now;
            Builder = new PropertyGridRuntimeBuilder();
            SincronizarMetadados();
        }

        public string NomeExibicao { get; set; }
        public string TipoItem { get; set; }
        public string Descricao { get; set; }
        public string IconeKey { get; set; }
        public DateTime CriadoEm { get; set; }
        public string Observacao { get; set; }
        public PropertyGridRuntimeBuilder Builder { get; private set; }

        public void SincronizarMetadados()
        {
            Builder.AdicionarPropriedade("Nome", NomeExibicao, "Nome exibido no item", " Info", "");
            Builder.AdicionarPropriedade("Descricao Item", Descricao, "Descricao funcional do item", " Info", "");
            Builder.AdicionarPropriedade("Imagem/Icone", IconeKey, "Chave do icone usado no no da arvore", " Info", "");
            Builder.AdicionarPropriedade("Criado em", CriadoEm.ToString("dd/MM/yyyy HH:mm:ss"), "Data e hora de criacao do item", " Info", "");
            Builder.AdicionarPropriedade("Observacao/Local", Observacao, "Observacao, local fisico ou local logico do item", " Info", "");
        }

        public InfrastructureItem Clone()
        {
            InfrastructureItem clone = new InfrastructureItem(NomeExibicao, Descricao, IconeKey, Observacao);
            clone.TipoItem = TipoItem;
            clone.CriadoEm = CriadoEm;
            clone.Builder = Builder.Clone();
            clone.SincronizarMetadados();
            return clone;
        }
    }

    internal enum TreeDropPosition
    {
        Above,
        AsChild,
        Below
    }

}
