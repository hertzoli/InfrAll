using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GerenciadorSistemas.Services;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GerenciadorSistemas
{
    public partial class Form1 : Form
    {
        private const int VersaoSchemaCadastro = 2;
        private const int LarguraMinimaTreeView = 200;
        private const int LarguraMinimaPropertyGrid = 200;
        private const int LarguraMinimaGroupBoxPropriedade = 397;
        private const int AlturaMinimaTreeView = 500;
        private const int AlturaMinimaPropertyGrid = 500;
        private const string ChaveIconeErro = "__error__";
        private const string ChaveIconeVazio = "Vazio.png";
        private string _nomePropriedadeSelecionadaOriginal;
        private string _localPropriedadeSelecionadaOriginal;
        private ContextMenuStrip _menuContextoTreeView;
        private TreeNode _noDropHover;
        private TreeDropPosition? _posicaoDropHover;
        private bool _suspenderPersistencia;
        private bool _suspenderMonitoramentoCamposEdicao;
        private bool _suspenderConfirmacaoSaidaPropriedade;
        private bool _suspenderSelecaoDataGridItem;
        private bool _suspenderFormatacaoRichTextBoxValor;
        private bool _persistindoCadastro;
        private bool _intBaseIDConsumidoSemPersistir;
        private bool _arrastandoItemTreeView;
        private bool _suspenderAtualizacaoSelecaoPorDrag;
        private bool _suspenderConfirmacaoSelecaoPorDrag;
        private bool _ignorarSelecaoPorBotaoExpansaoTreeView;
        private TreeNode _noCliqueBotaoExpansaoTreeView;
        private TipoValorPropriedade _tipoPropriedadeSelecionadaOriginal;
        private TreeNode _noItemEmEdicao;
        private TreeNode _noSelecionadoAntesDoDrag;
        private TreeNode _noItemEmEdicaoAntesDoDrag;
        private TreeNode _noPaiNovoItemEmCriacao;
        private bool _criandoNovoItem;
        private string _iconeSelecionadoCamposEdicao;
        private string _iconeSelecionadoCamposEdicaoOriginal;
        private readonly ToolTip _toolTipBotoes;
        private readonly Dictionary<Control, string> _snapshotCamposEdicao;
        private readonly List<Control> _camposTextoEditaveis;
        private readonly Color _corCampoEdicaoPadrao;
        private readonly Color _corCampoEdicaoNaoSalvo = Color.LightYellow;
        private readonly Font _fonteValorPadrao;
        private readonly Font _fonteValorComando;
        private readonly Color _corFundoValorPadrao;
        private readonly Color _corTextoValorPadrao;
        private readonly ItemIdService _itemIdService;
        private readonly ItemHierarchyService _itemHierarchyService;
        private readonly ItemClipboardService _itemClipboardService;
        private readonly ItemUndoHistoryService _undoHistoryService;
        private readonly RefPlaceholderResolver _refPlaceholderResolver;
        private readonly VirtualKeyboardService _virtualKeyboardService;
        private CancellationTokenSource _envioPasswordTecladoCts;
        private bool _restaurandoHistorico;
        private ItemUndoSnapshot _snapshotAntesAlteracaoPropertyGrid;

        public Form1()
        {
            InitializeComponent();
            this.Text += "  v" + Application.ProductVersion;

            //---------------- Verifica update online    
            OnlineAutoUpdate.OnlineAutoUpdateAsync.OnlineAutoUpdateAsync2();
            //------------------------------------
            _toolTipBotoes = new ToolTip(components);
            _snapshotCamposEdicao = new Dictionary<Control, string>();
            _camposTextoEditaveis = new List<Control>
            {
                textBoxNome,
                RichTextBoxValor,
                textBoxDescricao,
                textBoxID,
                textBoxLocal
            };
            _corCampoEdicaoPadrao = textBoxNome.BackColor;
            _fonteValorPadrao = RichTextBoxValor.Font;
            _fonteValorComando = new Font("Consolas", RichTextBoxValor.Font.Size, RichTextBoxValor.Font.Style);
            _corFundoValorPadrao = RichTextBoxValor.BackColor;
            _corTextoValorPadrao = RichTextBoxValor.ForeColor;
            _itemIdService = new ItemIdService();
            _itemIdService.IntBaseIDConsumido += itemIdService_IntBaseIDConsumido;
            _itemHierarchyService = new ItemHierarchyService(_itemIdService);
            _itemClipboardService = new ItemClipboardService();
            _undoHistoryService = new ItemUndoHistoryService(50);
            _refPlaceholderResolver = new RefPlaceholderResolver();
            _virtualKeyboardService = new VirtualKeyboardService();
            KeyPreview = true;
            KeyDown += Form1_KeyDown;
            FormClosing += Form1_FormClosing;
            buttonCopyPlaceholder.Click += buttonCopyPlaceholder_Click;
            DataGridViewItem.KeyDown += DataGridViewItem_KeyDown;
            RichTextBoxValor.TextChanged += CampoTipoOuValorAlterado;
            ConfigurarDragDropPlaceholderEmValor();
            comboBoxTipo.SelectedIndexChanged += CampoTipoOuValorAlterado;
            treeViewItens.BeforeSelect += treeViewItens_BeforeSelect;
            treeViewItens.MouseDown += treeViewItens_MouseDown;
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
            ConfigurarDataGridViewItem();
            ConfigurarMenuContextoTreeView();
            ConfigurarComboBoxTipo();
            ConfigurarToolTips();
            LimparCamposEdicao();
            CarregarCadastroPersistido();
        }

        private void itemIdService_IntBaseIDConsumido(object sender, EventArgs e)
        {
            if (_suspenderPersistencia || _persistindoCadastro)
            {
                _intBaseIDConsumidoSemPersistir = true;
                return;
            }

            PersistirCadastro();
        }

        private void ConfigurarComboBoxTipo()
        {
            comboBoxTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTipo.DataSource = Enum.GetValues(typeof(TipoDoValor));
            comboBoxTipo.SelectedItem = TipoDoValor.Texto;
            AtualizarEstadoButtonRun();
        }

        private void ConfigurarDataGridViewItem()
        {
            DataGridViewItem.Columns.Clear();

            DataGridViewImageColumn colunaIcone = new DataGridViewImageColumn
            {
                Name = "Icone",
                HeaderText = "Icone",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 48,
                FillWeight = 45
            };
            colunaIcone.DefaultCellStyle.NullValue = null;

            DataGridViewItem.Columns.Add(colunaIcone);
            DataGridViewItem.Columns.Add(CriarColunaTextoDataGridViewItem("Nome", "Nome", 140));
            DataGridViewItem.Columns.Add(CriarColunaTextoDataGridViewItem("Valor", "Valor", 180));
            DataGridViewItem.Columns.Add(CriarColunaTextoDataGridViewItem("Local", "Local", 160));

            DataGridViewItem.ReadOnly = false;
            DataGridViewItem.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewItem.CellEndEdit += DataGridViewItem_CellEndEdit;

            foreach (DataGridViewColumn coluna in DataGridViewItem.Columns)
                coluna.ReadOnly = true;

            DataGridViewItem.Columns["Valor"].ReadOnly = false;
            DataGridViewItem.Columns["Valor"].DefaultCellStyle.BackColor = Color.LightYellow;
        }

        private static DataGridViewTextBoxColumn CriarColunaTextoDataGridViewItem(string nome, string titulo, float fillWeight)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = nome,
                HeaderText = titulo,
                FillWeight = fillWeight
            };
        }

        private void ConfigurarToolTips()
        {
            _toolTipBotoes.SetToolTip(buttonNovoItem, "Criar um novo item na raiz da lista.");
            _toolTipBotoes.SetToolTip(buttonNovoSubItem, "Criar um subitem dentro do item selecionado.");
            _toolTipBotoes.SetToolTip(buttonDuplicar, "Duplicar o item atualmente selecionado.");
            _toolTipBotoes.SetToolTip(buttonExcluirItem, "Excluir o item atualmente selecionado.");
            _toolTipBotoes.SetToolTip(buttonRun, "Executar o valor informado como comando.");
            _toolTipBotoes.SetToolTip(buttonCopy, "Copiar o valor informado para a area de transferencia.");
            _toolTipBotoes.SetToolTip(buttonCopyPlaceholder, "Copiar o placeholder da propriedade para a area de transferencia.");
            _toolTipBotoes.SetToolTip(buttonAlterarIcone, "Selecionar o icone do item atual ou do novo item.");
            _toolTipBotoes.SetToolTip(buttonSalvar, "Salvar as alteracoes da propriedade atual.");
            _toolTipBotoes.SetToolTip(textBoxLocal, "Informar o local associado a propriedade selecionada.");
            _toolTipBotoes.SetToolTip(textBoxID, "Informar a categoria usada para organizar a propriedade.");
            _toolTipBotoes.SetToolTip(comboBoxTipo, "Definir se o valor da propriedade e um texto, comando ou script.");
        }

        private void InicializarMonitoramentoCamposEdicao()
        {
            foreach (Control campo in _camposTextoEditaveis)
            {
                _snapshotCamposEdicao[campo] = string.Empty;
                campo.TextChanged += CampoEdicao_TextChanged;
            }
        }

        private void CampoEdicao_TextChanged(object sender, EventArgs e)
        {
            if (_suspenderMonitoramentoCamposEdicao)
                return;

            Control campo = sender as Control;

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
            foreach (Control campo in _camposTextoEditaveis)
            {
                _snapshotCamposEdicao[campo] = campo.Text ?? string.Empty;
                campo.BackColor = ObterCorFundoPadraoDoCampo(campo);
            }

            AtualizarFormatoRichTextBoxValor();

            _tipoPropriedadeSelecionadaOriginal = ObterTipoSelecionado();
            _iconeSelecionadoCamposEdicaoOriginal = _iconeSelecionadoCamposEdicao ?? string.Empty;
        }

        private bool ExistemAlteracoesNaoSalvasNaPropriedadeSelecionada()
        {
            if (_suspenderConfirmacaoSaidaPropriedade)
                return false;

            foreach (Control campo in ObterCamposEditaveisDoItem())
            {
                string valorBase;

                if (!_snapshotCamposEdicao.TryGetValue(campo, out valorBase))
                    valorBase = string.Empty;

                string valorAtual = campo.Text ?? string.Empty;
                if (!string.Equals(valorAtual, valorBase ?? string.Empty, StringComparison.Ordinal))
                    return true;
            }

            return ObterTipoSelecionado() != _tipoPropriedadeSelecionadaOriginal
                || !string.Equals(_iconeSelecionadoCamposEdicao ?? string.Empty, _iconeSelecionadoCamposEdicaoOriginal ?? string.Empty, StringComparison.Ordinal);
        }

        private IEnumerable<Control> ObterCamposEditaveisDoItem()
        {
            yield return textBoxNome;
            yield return RichTextBoxValor;
            yield return textBoxDescricao;
        }

        private bool ConfirmarSaidaDaPropriedadeComAlteracoes()
        {
            if (!ExistemAlteracoesNaoSalvasNaPropriedadeSelecionada())
                return true;

            DialogResult resultado = MessageBox.Show(
                "Existem dados nao salvos nos campos do item. Deseja salvar antes de sair?",
                "Alteracoes nao salvas",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1);

            if (resultado == DialogResult.Cancel)
                return false;

            if (resultado == DialogResult.No)
                return true;

            return SalvarCamposEdicaoAtual();
        }

        private void ExecutarSemConfirmacaoSaidaPropriedade(Action acao)
        {
            bool suspensaoAnterior = _suspenderConfirmacaoSaidaPropriedade;
            _suspenderConfirmacaoSaidaPropriedade = true;

            try
            {
                acao();
            }
            finally
            {
                _suspenderConfirmacaoSaidaPropriedade = suspensaoAnterior;
            }
        }

        private void AtualizarDestaqueCampoEdicao(Control campo)
        {
            string valorBase;

            if (!_snapshotCamposEdicao.TryGetValue(campo, out valorBase))
                valorBase = string.Empty;

            string valorAtual = campo.Text ?? string.Empty;
            if (campo == RichTextBoxValor && PodeExecutarTipoSelecionado())
            {
                AtualizarFormatoRichTextBoxValor();
                return;
            }

            campo.BackColor = string.Equals(valorAtual, valorBase ?? string.Empty, StringComparison.Ordinal)
                ? ObterCorFundoPadraoDoCampo(campo)
                : _corCampoEdicaoNaoSalvo;

            if (campo == RichTextBoxValor)
                AtualizarFormatoRichTextBoxValor();
        }

        private Color ObterCorFundoPadraoDoCampo(Control campo)
        {
            return campo == RichTextBoxValor ? _corFundoValorPadrao : _corCampoEdicaoPadrao;
        }

        private void AtualizarFormatoRichTextBoxValor()
        {
            if (_suspenderFormatacaoRichTextBoxValor || RichTextBoxValor == null)
                return;

            _suspenderFormatacaoRichTextBoxValor = true;

            try
            {
                int inicioSelecao = RichTextBoxValor.SelectionStart;
                int tamanhoSelecao = RichTextBoxValor.SelectionLength;
                bool usaFormatoCodigo = UsaFormatoCodigoTipoSelecionado();

                RichTextBoxValor.Font = usaFormatoCodigo ? _fonteValorComando : _fonteValorPadrao;
                RichTextBoxValor.BackColor = usaFormatoCodigo
                    ? Color.Black
                    : (CampoPossuiAlteracao(RichTextBoxValor) ? _corCampoEdicaoNaoSalvo : _corFundoValorPadrao);
                RichTextBoxValor.ForeColor = usaFormatoCodigo ? Color.Yellow : _corTextoValorPadrao;

                RichTextBoxValor.SelectAll();
                RichTextBoxValor.SelectionColor = RichTextBoxValor.ForeColor;

                if (usaFormatoCodigo)
                    AplicarCorTextoComReferencias();

                inicioSelecao = Math.Min(inicioSelecao, RichTextBoxValor.TextLength);
                tamanhoSelecao = Math.Min(tamanhoSelecao, RichTextBoxValor.TextLength - inicioSelecao);
                RichTextBoxValor.Select(inicioSelecao, tamanhoSelecao);
                RichTextBoxValor.SelectionColor = usaFormatoCodigo ? Color.Yellow : _corTextoValorPadrao;
            }
            finally
            {
                _suspenderFormatacaoRichTextBoxValor = false;
            }
        }

        private void AplicarCorTextoComReferencias()
        {
            foreach (Match referencia in Regex.Matches(RichTextBoxValor.Text, "~REF\\[[^\\]]+\\]~"))
            {
                RichTextBoxValor.Select(referencia.Index, referencia.Length);
                RichTextBoxValor.SelectionColor = Color.Cyan;
            }
        }

        private bool CampoPossuiAlteracao(Control campo)
        {
            string valorBase;

            if (!_snapshotCamposEdicao.TryGetValue(campo, out valorBase))
                valorBase = string.Empty;

            return !string.Equals(campo.Text ?? string.Empty, valorBase ?? string.Empty, StringComparison.Ordinal);
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
                AlturaMinimaPropertyGrid);

            if (alturaAreaSuperior < alturaMinimaNecessaria)
                panel1.Height = ClientSize.Height - splitter1.Height - alturaMinimaNecessaria;

            if (panel1.Height < 60)
                panel1.Height = 60;
        }

        private void buttonNovoItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmarSaidaDaPropriedadeComAlteracoes())
                return;

            IniciarCriacaoNovoItem(null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ConfirmarSaidaDaPropriedadeComAlteracoes())
            {
                e.Cancel = true;
                return;
            }

            CancelarEnvioPasswordPendente();
            PersistirCadastro();
        }

        private void buttonNovoSubItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmarSaidaDaPropriedadeComAlteracoes())
                return;

            TreeNode noPai = treeViewItens.SelectedNode;

            if (noPai == null)
            {
                MessageBox.Show("Selecione um item pai antes de criar um subitem.", "Criar subitem",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            IniciarCriacaoNovoItem(noPai);
        }

        private void buttonExcluirItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmarSaidaDaPropriedadeComAlteracoes())
                return;

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

            RegistrarHistoricoAntesDaAcao("Excluir item");

            if (noSelecionado.Parent == null)
                treeViewItens.Nodes.Remove(noSelecionado);
            else
                noSelecionado.Parent.Nodes.Remove(noSelecionado);

            AtualizarAposOperacaoEstrutural(treeViewItens.SelectedNode);

            PersistirCadastro();
        }

        private void buttonDuplicar_Click(object sender, EventArgs e)
        {
            if (!ConfirmarSaidaDaPropriedadeComAlteracoes())
                return;

            DuplicarItemSelecionado();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            string valorResolvido;
            string erro;

            if (!TryResolverTextoComReferenciasRef(RichTextBoxValor.Text, out valorResolvido, out erro))
            {
                MessageBox.Show(erro, "Referencia invalida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Clipboard.SetText(valorResolvido ?? string.Empty);
        }

        private async void buttonRun_Click(object sender, EventArgs e)
        {
            CancelarEnvioPasswordPendente();

            if (!PodeExecutarTipoSelecionado())
            {
                MessageBox.Show("O tipo da propriedade atual nao permite execucao.", "Run",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string valorResolvido;
            string erro;

            if (!TryResolverTextoComReferenciasRef(RichTextBoxValor.Text, out valorResolvido, out erro))
            {
                MessageBox.Show(erro, "Referencia invalida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TipoDoValor tipoDoValor = ObterTipoDoValorSelecionado();

            if (string.IsNullOrWhiteSpace(valorResolvido))
            {
                string mensagem = tipoDoValor == TipoDoValor.Password
                    ? "Nao ha Password para enviar."
                    : "Nao ha comando para executar.";

                MessageBox.Show(mensagem, "Run", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (tipoDoValor == TipoDoValor.Password)
            {
                await PrepararEnvioPasswordPorTecladoAsync(valorResolvido);
                return;
            }

            try
            {
                ProcessStartInfo processInfo = CriarProcessStartInfoParaExecucao(valorResolvido, ObterTipoSelecionado());
                IniciarProcessoComElevacaoQuandoNecessario(processInfo);
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogErroDetalhado(ex, "Erro ao executar comando", new
                {
                    ValorResolvido = valorResolvido,
                    Tipo = ObterTipoSelecionado().ToString()
                });
                MessageBox.Show("Nao foi possivel executar o comando.\r\n" + ex.Message,
                    "Erro ao executar comando", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task PrepararEnvioPasswordPorTecladoAsync(string password)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            _envioPasswordTecladoCts = cts;
            CancellationToken cancellationToken = cts.Token;

            try
            {
                if (!ProcessoAtualEstaElevado())
                {
                    MessageBox.Show(
                        "O programa nao esta sendo executado com direitos de administrador.\r\n\r\n"
                        + "As teclas enviadas podem nao funcionar se o programa de destino tiver privilegio superior.",
                        "Envio de Password",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                labelInfo.Text = "Preparado para enviar Password";

                await AguardarPerdaDeFocoDoFormularioAsync(cancellationToken);

                labelInfo.Text = "Contagem regressiva do envio do Password";
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);

                await Task.Run(() => _virtualKeyboardService.EnviarTexto(password, cancellationToken), cancellationToken);

                labelInfo.Text = "Password enviado";
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                labelInfo.Text = string.Empty;
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogErroDetalhado(ex, "Erro ao enviar Password por teclado virtual", new
                {
                    TamanhoPassword = password != null ? password.Length : 0
                });

                labelInfo.Text = string.Empty;
                MessageBox.Show("Nao foi possivel enviar o Password pelo teclado virtual.\r\n" + ex.Message,
                    "Erro ao enviar Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ReferenceEquals(_envioPasswordTecladoCts, cts))
                    _envioPasswordTecladoCts = null;

                cts.Dispose();
            }
        }

        private Task AguardarPerdaDeFocoDoFormularioAsync(CancellationToken cancellationToken)
        {
            if (!ContainsFocus)
                return Task.FromResult(true);

            TaskCompletionSource<bool> espera = new TaskCompletionSource<bool>();
            CancellationTokenRegistration registroCancelamento = new CancellationTokenRegistration();
            EventHandler handler = null;

            handler = (sender, e) =>
            {
                Deactivate -= handler;
                registroCancelamento.Dispose();
                espera.TrySetResult(true);
            };

            registroCancelamento = cancellationToken.Register(() =>
            {
                Deactivate -= handler;
                espera.TrySetCanceled();
            });

            Deactivate += handler;
            return espera.Task;
        }

        private void CancelarEnvioPasswordPendente()
        {
            if (_envioPasswordTecladoCts == null)
                return;

            _envioPasswordTecladoCts.Cancel();
            _envioPasswordTecladoCts.Dispose();
            _envioPasswordTecladoCts = null;
        }

        private static bool ProcessoAtualEstaElevado()
        {
            using (WindowsIdentity identidade = WindowsIdentity.GetCurrent())
            {
                if (identidade == null)
                    return false;

                WindowsPrincipal principal = new WindowsPrincipal(identidade);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
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
            if (arquivoExecutavel == "cmd.exe")
            {
                processInfoCmd.Arguments = argumentos;
            }
            else
            {
                processInfoCmd.Arguments = "/S /C \"" + comandoNormalizado.Replace("\"", "\\\"") + "\"";
            }
            processInfoCmd.UseShellExecute = true;
            return processInfoCmd;
        }

        private static ProcessStartInfo CriarProcessStartInfoParaExecucao(string conteudoResolvido, TipoValorPropriedade tipo)
        {
            if (tipo == TipoValorPropriedade.Script)
                return CriarProcessStartInfoParaScriptBatchTemporario(conteudoResolvido);

            return CriarProcessStartInfoParaComando(conteudoResolvido);
        }

        private static void IniciarProcessoComElevacaoQuandoNecessario(ProcessStartInfo processInfo)
        {
            try
            {
                Process.Start(processInfo);
            }
            catch (Win32Exception ex) when (ExigeElevacao(ex) && !JaSolicitaElevacao(processInfo))
            {
                ProcessStartInfo processInfoElevado = CriarProcessStartInfoElevado(processInfo);
                Process.Start(processInfoElevado);
            }
        }

        private static bool ExigeElevacao(Win32Exception ex)
        {
            return ex != null
                && ex.NativeErrorCode == 740;
        }

        private static bool JaSolicitaElevacao(ProcessStartInfo processInfo)
        {
            return processInfo != null
                && string.Equals(processInfo.Verb, "runas", StringComparison.OrdinalIgnoreCase);
        }

        private static ProcessStartInfo CriarProcessStartInfoElevado(ProcessStartInfo processInfoOriginal)
        {
            if (processInfoOriginal == null)
                throw new ArgumentNullException("processInfoOriginal");

            ProcessStartInfo processInfoElevado = new ProcessStartInfo();
            processInfoElevado.FileName = processInfoOriginal.FileName;
            processInfoElevado.Arguments = processInfoOriginal.Arguments;
            processInfoElevado.WorkingDirectory = processInfoOriginal.WorkingDirectory;
            processInfoElevado.WindowStyle = processInfoOriginal.WindowStyle;
            processInfoElevado.ErrorDialog = processInfoOriginal.ErrorDialog;
            processInfoElevado.UseShellExecute = true;
            processInfoElevado.Verb = "runas";
            return processInfoElevado;
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

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            SalvarCamposEdicaoAtual();
        }

        private bool SalvarCamposEdicaoAtual()
        {
            TreeNode noEmEdicao = ObterNoItemEmEdicao();
            InfrastructureItem itemSelecionado = noEmEdicao != null ? noEmEdicao.Tag as InfrastructureItem : null;

            if (_criandoNovoItem)
                return CriarNovoItem(_noPaiNovoItemEmCriacao);

            if (itemSelecionado == null)
            {
                MessageBox.Show("Selecione um item antes de salvar.", "Salvar item",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            string nomeItem = (textBoxNome.Text ?? string.Empty).Trim();

            if (!ValidarNomeItem(nomeItem, "Salvar item"))
                return false;

            RegistrarHistoricoAntesDaAcao("Editar item");

            itemSelecionado.NomeExibicao = nomeItem;
            itemSelecionado.Valor = RichTextBoxValor.Text ?? string.Empty;
            itemSelecionado.TipoDoValor = ObterTipoDoValorSelecionado();
            itemSelecionado.Descricao = textBoxDescricao.Text ?? string.Empty;
            itemSelecionado.IconeKey = _iconeSelecionadoCamposEdicao ?? string.Empty;
            itemSelecionado.MarcarEditado();
            itemSelecionado.SincronizarMetadados();

            noEmEdicao.Text = itemSelecionado.NomeExibicao;
            noEmEdicao.Name = itemSelecionado.NomeExibicao;
            string chaveIcone = ResolverIconeDoItem(itemSelecionado);
            AplicarIconeNoNo(noEmEdicao, chaveIcone);

            ExecutarSemConfirmacaoSaidaPropriedade(() =>
            {
                AtualizarDataGridViewItem(treeViewItens.SelectedNode, noEmEdicao);
                CarregarItemNosCamposEdicao(noEmEdicao);
            });
            SincronizarEstadoCamposEdicao();
            PersistirCadastro();
            return true;
        }
         

        private void CarregarPropriedadeNosCamposEdicao(PropertyGridSelectionInfo info)
        {
            if (info == null)
                return;

            ExecutarSemMonitoramentoCamposEdicao(() =>
            {
                textBoxNome.Text = info.Nome;
                RichTextBoxValor.Text = Convert.ToString(info.Valor);
                textBoxDescricao.Text = info.Descricao;
                textBoxID.Text = info.Categoria;
                textBoxLocal.Text = info.Local;
                SelecionarTipoNoCombo(info.Tipo);
                textBoxReferenciaPropriedade.Text = MontarPlaceholderDaPropriedade(info, treeViewItens.SelectedNode);
            });
            AtualizarContextoDaPropriedadeSelecionada(info.Nome, info.Local);
            SincronizarEstadoCamposEdicao();
            AtualizarEstadoButtonRun();
            _snapshotAntesAlteracaoPropertyGrid = CapturarSnapshotHistoricoAtual("Editar propriedade");
        }

        private void treeViewItens_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (DeveIgnorarSelecaoPorBotaoExpansao(e.Node))
            {
                e.Cancel = true;
                return;
            }

            if (_suspenderConfirmacaoSelecaoPorDrag)
                return;

            if (!ConfirmarSaidaDaPropriedadeComAlteracoes())
                e.Cancel = true;
        }

        private void treeViewItens_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_suspenderAtualizacaoSelecaoPorDrag)
                return;

            InfrastructureItem itemSelecionado = ObterItemSelecionado();

            if (itemSelecionado == null)
            {
                AtualizarDataGridViewItem(null, null);
                LimparCamposEdicao();
                LimparContextoDaPropriedadeSelecionada();
                textBoxReferenciaPropriedade.Text = string.Empty;
                AtualizarEstadoButtonRun();
                return;
            }

            itemSelecionado.SincronizarMetadados();
            AtualizarDataGridViewItem(e.Node, e.Node);
            CarregarItemNosCamposEdicao(e.Node);
            LimparContextoDaPropriedadeSelecionada();
            AtualizarEstadoButtonRun();
        }

        private void treeViewItens_KeyDown(object sender, KeyEventArgs e)
        {
            if (ProcessarAtalhoDesfazer(e))
                return;

            if (ProcessarAtalhoClipboardItens(e))
                return;

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
            if (e.Node != null && CliqueSelecionaNoTreeView(e.X, e.Y))
                treeViewItens.SelectedNode = e.Node;
        }

        private void treeViewItens_MouseDown(object sender, MouseEventArgs e)
        {
            _ignorarSelecaoPorBotaoExpansaoTreeView = false;
            _noCliqueBotaoExpansaoTreeView = null;

            if (e.Button != MouseButtons.Left)
                return;

            TreeViewHitTestInfo hitTest = treeViewItens.HitTest(e.Location);
            if (hitTest.Node == null)
                return;

            if (hitTest.Location == TreeViewHitTestLocations.PlusMinus)
            {
                _ignorarSelecaoPorBotaoExpansaoTreeView = true;
                _noCliqueBotaoExpansaoTreeView = hitTest.Node;
                return;
            }

            _noSelecionadoAntesDoDrag = treeViewItens.SelectedNode;
            _noItemEmEdicaoAntesDoDrag = ObterNoItemEmEdicao();
        }

        private bool DeveIgnorarSelecaoPorBotaoExpansao(TreeNode no)
        {
            return _ignorarSelecaoPorBotaoExpansaoTreeView
                && no != null
                && no == _noCliqueBotaoExpansaoTreeView
                && Control.MouseButtons == MouseButtons.Left;
        }

        private bool CliqueSelecionaNoTreeView(int x, int y)
        {
            TreeViewHitTestInfo hitTest = treeViewItens.HitTest(x, y);

            return hitTest.Location == TreeViewHitTestLocations.Label
                || hitTest.Location == TreeViewHitTestLocations.Image;
        }

        private void treeViewItens_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item == null)
                return;

            _arrastandoItemTreeView = true;
            _suspenderAtualizacaoSelecaoPorDrag = true;

            try
            {
                RestaurarContextoVisualAnteriorAoDrag();
                DragDropEffects efeito = DoDragDrop(e.Item, DragDropEffects.Copy | DragDropEffects.Move);

                if (efeito == DragDropEffects.Copy)
                    RestaurarContextoVisualAnteriorAoDrag();
            }
            finally
            {
                _arrastandoItemTreeView = false;
                _suspenderAtualizacaoSelecaoPorDrag = false;
                _noSelecionadoAntesDoDrag = null;
                _noItemEmEdicaoAntesDoDrag = null;
            }
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

            RegistrarHistoricoAntesDaAcao("Mover item");

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
            AtualizarAposOperacaoEstrutural(noArrastado);
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

        private void IniciarCriacaoNovoItem(TreeNode noPai)
        {
            _criandoNovoItem = true;
            _noPaiNovoItemEmCriacao = noPai;
            _noItemEmEdicao = null;
            _iconeSelecionadoCamposEdicao = string.Empty;

            ExecutarSemMonitoramentoCamposEdicao(() =>
            {
                textBoxNome.Text = string.Empty;
                RichTextBoxValor.Text = string.Empty;
                textBoxDescricao.Text = string.Empty;
                textBoxID.Text = string.Empty;
                textBoxLocal.Text = noPai == null ? string.Empty : ObterCaminhoCompletoDoNo(noPai);
                textBoxDataCriacao.Text = string.Empty;
                textBoxDataEdicao.Text = string.Empty;
                textBoxReferenciaPropriedade.Text = string.Empty;
                SelecionarTipoDoValorNoCombo(TipoDoValor.Texto);
            });

            AtualizarImagemLateral(null);
            SincronizarEstadoCamposEdicao();
            AtualizarEstadoButtonRun();
            textBoxNome.Focus();
        }

        private bool CriarNovoItem(TreeNode noPai)
        {
            string nomeItem = (textBoxNome.Text ?? string.Empty).Trim();

            if (!ValidarNomeItem(nomeItem, "Novo item"))
                return false;

            string iconeSelecionado = (_iconeSelecionadoCamposEdicao ?? string.Empty).Trim();
            GarantirIconeCarregadoNoImageListPrincipal(iconeSelecionado);

            InfrastructureItem novoItem = new InfrastructureItem(
                nomeItem,
                textBoxDescricao.Text ?? string.Empty,
                iconeSelecionado,
                string.Empty);
            novoItem.Valor = RichTextBoxValor.Text ?? string.Empty;
            novoItem.TipoDoValor = ObterTipoDoValorSelecionado();
            PrepararNovoItem(novoItem);

            TreeNode novoNo = CriarNoParaItem(novoItem);

            RegistrarHistoricoAntesDaAcao(noPai == null ? "Adicionar item raiz" : "Adicionar subitem");

            if (noPai == null)
                treeViewItens.Nodes.Add(novoNo);
            else
            {
                noPai.Nodes.Add(novoNo);
                noPai.Expand();
            }

            _criandoNovoItem = false;
            _noPaiNovoItemEmCriacao = null;

            ExecutarSemConfirmacaoSaidaPropriedade(() =>
            {
                treeViewItens.SelectedNode = novoNo;
                novoNo.EnsureVisible();
                CarregarItemNosCamposEdicao(novoNo);
            });

            PersistirCadastro();
            return true;
        }

        private static bool NomeItemPossuiCaractereInvalido(string nomeItem)
        {
            return (nomeItem ?? string.Empty).IndexOf('/') >= 0;
        }

        private bool ValidarNomeItem(string nomeItem, string tituloMensagem)
        {
            if (string.IsNullOrWhiteSpace(nomeItem))
            {
                MessageBox.Show("Informe o nome do item.", tituloMensagem,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (NomeItemPossuiCaractereInvalido(nomeItem))
            {
                MessageBox.Show("O nome do item nao pode conter o caractere '/'.", tituloMensagem,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private TreeNode CriarNoParaItem(InfrastructureItem item)
        {
            string chaveIcone = ResolverIconeDoItem(item);
            TreeNode no = new TreeNode(item.NomeExibicao);
            no.Name = item.NomeExibicao;
            no.Tag = item;
            AplicarIconeNoNo(no, chaveIcone);
            return no;
        }

        private void PrepararNovoItem(InfrastructureItem item)
        {
            if (item == null)
                return;

            HashSet<string> idsExistentes = _itemHierarchyService.ColetarIds(
                treeViewItens.Nodes.Cast<TreeNode>().Select(MapearNoParaItem));

            item.ID = _itemIdService.GerarIdUnico(idsExistentes);
            item.CriadoEm = DateTime.Now;
            item.DataDeEdicao = item.CriadoEm;
            item.SincronizarMetadados();
        }

        private void RenovarIdentidadeDoNoRecursivamente(TreeNode no, ISet<string> idsExistentes)
        {
            if (no == null)
                return;

            InfrastructureItem item = no.Tag as InfrastructureItem;
            if (item != null)
            {
                DateTime agora = DateTime.Now;
                item.ID = _itemIdService.GerarIdUnico(idsExistentes);
                item.CriadoEm = agora;
                item.DataDeEdicao = agora;
                item.SincronizarMetadados();
            }

            foreach (TreeNode filho in no.Nodes)
                RenovarIdentidadeDoNoRecursivamente(filho, idsExistentes);
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

            RegistrarHistoricoAntesDaAcao("Duplicar item");

            Item itemDuplicado = MapearNoParaItem(noSelecionado);
            itemDuplicado.Nome = GerarNomeDuplicado(noSelecionado.Text, noSelecionado.Parent);

            HashSet<string> idsExistentes = _itemHierarchyService.ColetarIds(
                treeViewItens.Nodes.Cast<TreeNode>().Select(MapearNoParaItem));
            _itemHierarchyService.RenovarIdentidadeRecursiva(itemDuplicado, idsExistentes);

            TreeNode duplicado = CriarNoAPartirDoItem(itemDuplicado);

            if (noSelecionado.Parent == null)
                treeViewItens.Nodes.Add(duplicado);
            else
            {
                noSelecionado.Parent.Nodes.Add(duplicado);
                noSelecionado.Parent.Expand();
            }

            AtualizarAposOperacaoEstrutural(duplicado);
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
                _suspenderPersistencia = true;

                IDeserializer deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .IgnoreUnmatchedProperties()
                    .Build();

                using (StreamReader reader = new StreamReader(caminhoArquivo))
                {
                    CadastroPersistido cadastro = deserializer.Deserialize<CadastroPersistido>(reader);

                    if (cadastro == null)
                        throw new InvalidDataException("O arquivo cadastro.yaml nao contem um cadastro valido.");

                    if (cadastro.SchemaVersion != VersaoSchemaCadastro)
                    {
                        throw new InvalidDataException(
                            string.Format(
                                "schemaVersion {0} nao suportado. Versao suportada: {1}.",
                                cadastro.SchemaVersion,
                                VersaoSchemaCadastro));
                    }

                    if (cadastro.Itens == null)
                        throw new InvalidDataException("O arquivo cadastro.yaml nao contem a lista de itens.");

                    _itemIdService.DefinirIntBaseID(ObterIntBaseIDPersistido(cadastro));

                    List<Item> itens = cadastro.Itens.Select(MapearPersistenciaParaItem).ToList();
                    _itemHierarchyService.PrepararHierarquia(itens);

                    treeViewItens.Nodes.Clear();

                    foreach (Item item in itens)
                        treeViewItens.Nodes.Add(CriarNoAPartirDoItem(item));

                    AplicarEstadoDeExpansao(cadastro.Configuracoes != null
                        ? cadastro.Configuracoes.ItensExpandidos
                        : null);

                    if (treeViewItens.Nodes.Count > 0)
                        treeViewItens.SelectedNode = treeViewItens.Nodes[0];
                }
            }
            catch (Exception ex)
            {
                TratarFalhaAoCarregarCadastro(caminhoArquivo, ex);
            }
            finally
            {
                _suspenderPersistencia = false;
                if (_intBaseIDConsumidoSemPersistir)
                    PersistirCadastro();
                LimparHistoricoDeAcoes();
            }
        }

        private static long ObterIntBaseIDPersistido(CadastroPersistido cadastro)
        {
            if (cadastro == null || cadastro.Configuracoes == null || cadastro.Configuracoes.IntBaseID < 1)
                return 1;

            return cadastro.Configuracoes.IntBaseID;
        }

        private void TratarFalhaAoCarregarCadastro(string caminhoArquivo, Exception erroOriginal)
        {
            try
            {
                Logger.LogWriter.LogErroDetalhado(erroOriginal, "Erro ao carregar cadastro.yaml", new
                {
                    CaminhoArquivo = caminhoArquivo,
                    SchemaSuportado = VersaoSchemaCadastro
                });
            }
            catch
            {
            }

            try
            {
                string caminhoBackup = GerarCaminhoCadastroComTimestamp(caminhoArquivo);

                if (File.Exists(caminhoArquivo))
                    File.Move(caminhoArquivo, caminhoBackup);

                treeViewItens.Nodes.Clear();
                AtualizarDataGridViewItem(null, null);
                LimparCamposEdicao();
                LimparContextoDaPropriedadeSelecionada();
                CriarCadastroVazio(caminhoArquivo);

                MessageBox.Show(
                    "Nao foi possivel carregar o cadastro atual.\r\n\r\n"
                    + "Erro: " + ObterMensagemSeguraErroCadastro(erroOriginal) + "\r\n\r\n"
                    + "Acao executada: o arquivo com erro foi renomeado para \""
                    + Path.GetFileName(caminhoBackup)
                    + "\" e um novo cadastro.yaml vazio foi criado.",
                    "Cadastro recriado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                try
                {
                    Logger.LogWriter.LogErroDetalhado(ex, "Erro ao isolar cadastro.yaml invalido", new
                    {
                        CaminhoArquivo = caminhoArquivo
                    });
                }
                catch
                {
                }

                MessageBox.Show(
                    "Nao foi possivel carregar o cadastro atual e a recuperacao automatica falhou.\r\n\r\n"
                    + "Erro original: " + ObterMensagemSeguraErroCadastro(erroOriginal) + "\r\n"
                    + "Erro na recuperacao: " + ObterMensagemSeguraErroCadastro(ex),
                    "Erro ao recuperar cadastro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static string GerarCaminhoCadastroComTimestamp(string caminhoArquivo)
        {
            string pasta = Path.GetDirectoryName(caminhoArquivo);
            string nomeBase = Path.GetFileNameWithoutExtension(caminhoArquivo);
            string extensao = Path.GetExtension(caminhoArquivo);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string caminhoBackup = Path.Combine(pasta, nomeBase + "_" + timestamp + extensao);
            int contador = 2;

            while (File.Exists(caminhoBackup))
            {
                caminhoBackup = Path.Combine(
                    pasta,
                    string.Format("{0}_{1}_{2}{3}", nomeBase, timestamp, contador, extensao));
                contador++;
            }

            return caminhoBackup;
        }

        private void CriarCadastroVazio(string caminhoArquivo)
        {
            CadastroPersistido cadastro = new CadastroPersistido
            {
                SchemaVersion = VersaoSchemaCadastro,
                SavedAt = DateTime.Now.ToString("o"),
                Configuracoes = new ConfiguracoesPersistidas
                {
                    IconePadrao = FormSelectIcon.CarregarIconePadraoPersistido(),
                    IntBaseID = _itemIdService.IntBaseID,
                    ItensExpandidos = new List<string>()
                },
                Itens = new List<ItemPersistido>()
            };

            ISerializer serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            using (StreamWriter writer = new StreamWriter(caminhoArquivo, false))
                serializer.Serialize(writer, cadastro);
        }

        private static string ObterMensagemSeguraErroCadastro(Exception ex)
        {
            if (ex == null)
                return "Erro desconhecido.";

            return string.IsNullOrWhiteSpace(ex.Message)
                ? ex.GetType().Name
                : ex.Message;
        }

        private void PersistirCadastro()
        {
            if (_suspenderPersistencia)
            {
                _intBaseIDConsumidoSemPersistir = true;
                return;
            }

            try
            {
                _persistindoCadastro = true;
                AtualizarCaminhosDosItensDaArvore();

                CadastroPersistido cadastro = new CadastroPersistido
                {
                    SchemaVersion = VersaoSchemaCadastro,
                    SavedAt = DateTime.Now.ToString("o"),
                    Configuracoes = new ConfiguracoesPersistidas
                    {
                        IconePadrao = FormSelectIcon.CarregarIconePadraoPersistido(),
                        IntBaseID = _itemIdService.IntBaseID,
                        ItensExpandidos = ColetarIdsDosNosExpandidos()
                    },
                    Itens = treeViewItens.Nodes.Cast<TreeNode>()
                        .Select(MapearNoParaItemPersistido)
                        .ToList()
                };

                List<Item> itens = cadastro.Itens.Select(MapearPersistenciaParaItem).ToList();
                _itemHierarchyService.PrepararHierarquia(itens);
                cadastro.Itens = itens.Select(MapearItemParaPersistencia).ToList();

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
                _intBaseIDConsumidoSemPersistir = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Nao foi possivel salvar o cadastro.\r\n" + ex.Message,
                    "Erro ao salvar cadastro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                _persistindoCadastro = false;
            }
        }

        private static string ObterCaminhoArquivoCadastro()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cadastro.yaml");
        }

        private void AtualizarCaminhosDosItensDaArvore()
        {
            AtualizarCaminhosDosItensDaColecao(treeViewItens.Nodes, string.Empty);
        }

        private void AtualizarCaminhosDosItensDaColecao(TreeNodeCollection nos, string caminhoPai)
        {
            foreach (TreeNode no in nos)
            {
                InfrastructureItem item = no.Tag as InfrastructureItem;
                string nome = item != null ? item.NomeExibicao : no.Text;
                string caminho = string.IsNullOrWhiteSpace(caminhoPai)
                    ? nome
                    : string.Concat(caminhoPai, "/", nome);

                if (item != null)
                {
                    item.Caminho = caminho;
                    item.SincronizarMetadados();
                }

                AtualizarCaminhosDosItensDaColecao(no.Nodes, caminho);
            }
        }

        private TreeNode CriarNoAPartirDoItem(Item itemPersistido)
        {
            InfrastructureItem item = new InfrastructureItem(
                itemPersistido.Nome,
                itemPersistido.Descricao,
                itemPersistido.Icone,
                string.Empty);

            item.ID = itemPersistido.ID;
            item.Valor = itemPersistido.Valor ?? string.Empty;
            item.TipoDoValor = itemPersistido.TipoDoValor;
            item.CriadoEm = itemPersistido.DataDeCriacao;
            item.DataDeEdicao = itemPersistido.DataDeEdicao;
            item.Caminho = itemPersistido.Caminho ?? string.Empty;
            item.SincronizarMetadados();

            TreeNode no = CriarNoParaItem(item);

            if (itemPersistido.Subitens != null)
            {
                foreach (Item filho in itemPersistido.Subitens)
                    no.Nodes.Add(CriarNoAPartirDoItem(filho));
            }

            return no;
        }

        private ItemPersistido MapearNoParaItemPersistido(TreeNode no)
        {
            return MapearItemParaPersistencia(MapearNoParaItem(no));
        }

        private Item MapearNoParaItem(TreeNode no)
        {
            InfrastructureItem itemVisual = no.Tag as InfrastructureItem;
            Item item = new Item
            {
                Nome = itemVisual != null ? itemVisual.NomeExibicao : no.Text,
                Valor = itemVisual != null ? itemVisual.Valor ?? string.Empty : string.Empty,
                TipoDoValor = itemVisual != null ? itemVisual.TipoDoValor : TipoDoValor.Texto,
                Descricao = itemVisual != null ? itemVisual.Descricao : string.Empty,
                Icone = itemVisual != null ? itemVisual.IconeKey : string.Empty,
                ID = itemVisual != null ? itemVisual.ID : string.Empty,
                DataDeCriacao = itemVisual != null ? itemVisual.CriadoEm : DateTime.Now,
                DataDeEdicao = itemVisual != null ? itemVisual.DataDeEdicao : DateTime.Now,
                Caminho = itemVisual != null ? itemVisual.Caminho : string.Empty,
                Subitens = new List<Item>()
            };

            if (itemVisual != null)
            {
                foreach (DynamicPropertyItem propriedade in itemVisual.Builder.Root.EnumeratePropertiesRecursive()
                    .Where(p => !EhPropriedadeProtegida(p.Name, p.Path)))
                {
                    item.Subitens.Add(MapearPropriedadeParaItem(propriedade));
                }
            }

            item.Subitens.AddRange(no.Nodes.Cast<TreeNode>().Select(MapearNoParaItem));
            return item;
        }

        private static Item MapearPropriedadeParaItem(DynamicPropertyItem propriedade)
        {
            return new Item
            {
                Nome = propriedade.Name,
                Valor = Convert.ToString(propriedade.Value) ?? string.Empty,
                Descricao = propriedade.Description ?? string.Empty,
                TipoDoValor = ConverterTipoPropriedadeParaTipoDoValor(propriedade.Tipo),
                Caminho = propriedade.Path ?? string.Empty,
                Subitens = new List<Item>()
            };
        }

        private static ItemPersistido MapearItemParaPersistencia(Item item)
        {
            return new ItemPersistido
            {
                Nome = item.Nome ?? string.Empty,
                Valor = item.Valor ?? string.Empty,
                TipoDoValor = item.TipoDoValor.ToString(),
                Descricao = item.Descricao ?? string.Empty,
                Icone = item.Icone ?? string.Empty,
                ID = item.ID ?? string.Empty,
                DataDeCriacao = item.DataDeCriacao.ToString("o"),
                DataDeEdicao = item.DataDeEdicao.ToString("o"),
                Caminho = item.Caminho ?? string.Empty,
                Subitens = item.Subitens != null
                    ? item.Subitens.Select(MapearItemParaPersistencia).ToList()
                    : new List<ItemPersistido>()
            };
        }

        private static Item MapearPersistenciaParaItem(ItemPersistido itemPersistido)
        {
            Item item = new Item
            {
                Nome = itemPersistido.Nome ?? string.Empty,
                Valor = itemPersistido.Valor ?? string.Empty,
                TipoDoValor = ConverterTextoParaTipoDoValor(itemPersistido.TipoDoValor),
                Descricao = itemPersistido.Descricao ?? string.Empty,
                Icone = itemPersistido.Icone ?? string.Empty,
                ID = itemPersistido.ID ?? string.Empty,
                Caminho = itemPersistido.Caminho ?? string.Empty,
                Subitens = itemPersistido.Subitens != null
                    ? itemPersistido.Subitens.Select(MapearPersistenciaParaItem).ToList()
                    : new List<Item>()
            };

            DateTime data;
            if (DateTime.TryParse(itemPersistido.DataDeCriacao, null, DateTimeStyles.RoundtripKind, out data))
                item.DataDeCriacao = data;

            if (DateTime.TryParse(itemPersistido.DataDeEdicao, null, DateTimeStyles.RoundtripKind, out data))
                item.DataDeEdicao = data;

            return item;
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



        private void AtualizarDataGridViewItem(TreeNode noEscopo, TreeNode noParaSelecionar)
        {
            _suspenderSelecaoDataGridItem = true;

            try
            {
                DataGridViewItem.Rows.Clear();

                if (noEscopo == null)
                {
                    _noItemEmEdicao = null;
                    return;
                }

                foreach (TreeNode no in EnumerarNoESeusDescendentes(noEscopo))
                    AdicionarLinhaDataGridViewItem(no, ObterNivelRelativoDataGridViewItem(noEscopo, no));

                if (noParaSelecionar != null)
                    SelecionarLinhaDataGridViewItem(noParaSelecionar);
                else if (DataGridViewItem.Rows.Count > 0)
                    DataGridViewItem.Rows[0].Selected = true;
            }
            finally
            {
                _suspenderSelecaoDataGridItem = false;
            }
        }

        private void AdicionarLinhaDataGridViewItem(TreeNode no, int nivelRelativo)
        {
            InfrastructureItem item = no != null ? no.Tag as InfrastructureItem : null;
            if (item == null)
                return;

            int indice = DataGridViewItem.Rows.Add(
                ObterImagemDoItem(item),
                ObterNomeIndentadoDataGridViewItem(item.NomeExibicao, nivelRelativo),
                item.Valor ?? string.Empty,
                item.Caminho ?? string.Empty);

            DataGridViewRow linha = DataGridViewItem.Rows[indice];
            linha.Tag = no;
            AplicarEstiloHierarquiaDataGridViewItem(linha, nivelRelativo);
            linha.Cells["Valor"].Style.BackColor = Color.FromArgb(240 ,255, 240);
        }

        private void DataGridViewItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            DataGridViewColumn coluna = DataGridViewItem.Columns[e.ColumnIndex];
            if (coluna == null || !string.Equals(coluna.Name, "Valor", StringComparison.OrdinalIgnoreCase))
                return;

            TreeNode noEditado = DataGridViewItem.Rows[e.RowIndex].Tag as TreeNode;
            InfrastructureItem itemEditado = noEditado != null ? noEditado.Tag as InfrastructureItem : null;
            if (itemEditado == null)
                return;

            string novoValor = Convert.ToString(DataGridViewItem.Rows[e.RowIndex].Cells["Valor"].Value) ?? string.Empty;
            if (string.Equals(itemEditado.Valor ?? string.Empty, novoValor, StringComparison.Ordinal))
                return;

            try
            {
                RegistrarHistoricoAntesDaAcao("Editar valor do item");
                itemEditado.Valor = novoValor;
                itemEditado.MarcarEditado();
                itemEditado.SincronizarMetadados();

                if (ReferenceEquals(_noItemEmEdicao, noEditado))
                {
                    ExecutarSemMonitoramentoCamposEdicao(() =>
                    {
                        RichTextBoxValor.Text = novoValor;
                    });
                    SincronizarEstadoCamposEdicao();
                }

                PersistirCadastro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Nao foi possivel salvar o valor editado no grid.\r\n" + ex.Message,
                    "Erro ao salvar valor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private static int ObterNivelRelativoDataGridViewItem(TreeNode noEscopo, TreeNode no)
        {
            if (noEscopo == null || no == null)
                return 0;

            int nivelRelativo = no.Level - noEscopo.Level;
            return nivelRelativo < 0 ? 0 : nivelRelativo;
        }

        private static string ObterNomeIndentadoDataGridViewItem(string nome, int nivelRelativo)
        {
            string nomeNormalizado = nome ?? string.Empty;

            if (nivelRelativo <= 0)
                return nomeNormalizado;

            if (nivelRelativo == 1)
                return "   " + nomeNormalizado;

            if (nivelRelativo == 2)
                return "      " + nomeNormalizado;

            return "         " + nomeNormalizado;
        }

        private void AplicarEstiloHierarquiaDataGridViewItem(DataGridViewRow linha, int nivelRelativo)
        {
            if (linha == null)
                return;

            FontStyle estiloFonte = nivelRelativo <= 1 ? FontStyle.Bold : FontStyle.Regular;
            Color corTexto = ObterCorTextoHierarquiaDataGridViewItem(nivelRelativo);

            linha.DefaultCellStyle.Font = new Font(DataGridViewItem.Font, estiloFonte);
            linha.DefaultCellStyle.ForeColor = corTexto;
            linha.DefaultCellStyle.SelectionForeColor = corTexto;
            linha.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
        }

        private static Color ObterCorTextoHierarquiaDataGridViewItem(int nivelRelativo)
        {
            if (nivelRelativo <= 0)
                return Color.Blue;

            if (nivelRelativo <= 2)
                return Color.Black;

            return Color.Gray;
        }

        private Image ObterImagemDoItem(InfrastructureItem item)
        {
            string chaveIcone = ResolverIconeDoItem(item);

            if (!string.IsNullOrWhiteSpace(chaveIcone) && imageList1.Images.ContainsKey(chaveIcone))
                return imageList1.Images[chaveIcone];

            if (string.IsNullOrWhiteSpace(chaveIcone))
                return null;

            GarantirIconeErroCarregado();
            return imageList1.Images[ChaveIconeErro];
        }

        private void SelecionarLinhaDataGridViewItem(TreeNode no)
        {
            DataGridViewItem.ClearSelection();

            foreach (DataGridViewRow linha in DataGridViewItem.Rows)
            {
                if (ReferenceEquals(linha.Tag, no))
                {
                    linha.Selected = true;
                    DataGridViewItem.CurrentCell = linha.Cells["Nome"];
                    return;
                }
            }
        }

        private void DataGridViewItem_SelectionChanged(object sender, EventArgs e)
        {
            if (_suspenderSelecaoDataGridItem)
                return;

            TreeNode noSelecionado = ObterNoSelecionadoNoDataGridViewItem();
            if (noSelecionado == null)
                return;

            if (!ConfirmarSaidaDaPropriedadeComAlteracoes())
            {
                if (_noItemEmEdicao != null)
                {
                    _suspenderSelecaoDataGridItem = true;
                    try
                    {
                        SelecionarLinhaDataGridViewItem(_noItemEmEdicao);
                    }
                    finally
                    {
                        _suspenderSelecaoDataGridItem = false;
                    }
                }

                return;
            }

            CarregarItemNosCamposEdicao(noSelecionado);
        }

        private void DataGridViewItem_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex >= 0
                && string.Equals(DataGridViewItem.Columns[e.ColumnIndex].Name, "Valor", StringComparison.OrdinalIgnoreCase))
            {
                DataGridViewItem.BeginEdit(true);
                return;
            }

            TreeNode noSelecionado = DataGridViewItem.Rows[e.RowIndex].Tag as TreeNode;

            if (noSelecionado == null)
                return;

            if (ExecutarItemSeForComandoOuScript(noSelecionado))
                return;
        }

        private TreeNode ObterNoSelecionadoNoDataGridViewItem()
        {
            if (DataGridViewItem.CurrentRow != null)
                return DataGridViewItem.CurrentRow.Tag as TreeNode;

            if (DataGridViewItem.SelectedRows.Count > 0)
                return DataGridViewItem.SelectedRows[0].Tag as TreeNode;

            return null;
        }

        private TreeNode ObterNoItemEmEdicao()
        {
            if (_noItemEmEdicao != null)
                return _noItemEmEdicao;

            TreeNode noGrid = ObterNoSelecionadoNoDataGridViewItem();
            return noGrid ?? treeViewItens.SelectedNode;
        }

        private void CarregarItemNosCamposEdicao(TreeNode no)
        {
            InfrastructureItem item = no != null ? no.Tag as InfrastructureItem : null;

            if (item == null)
            {
                _noItemEmEdicao = null;
                _criandoNovoItem = false;
                _noPaiNovoItemEmCriacao = null;
                LimparCamposEdicao();
                AtualizarImagemLateral(null);
                return;
            }

            _noItemEmEdicao = no;
            _criandoNovoItem = false;
            _noPaiNovoItemEmCriacao = null;
            _iconeSelecionadoCamposEdicao = item.IconeKey ?? string.Empty;

            ExecutarSemMonitoramentoCamposEdicao(() =>
            {
                textBoxNome.Text = item.NomeExibicao;
                RichTextBoxValor.Text = item.Valor ?? string.Empty;
                textBoxDescricao.Text = item.Descricao ?? string.Empty;
                textBoxID.Text = item.ID ?? string.Empty;
                textBoxLocal.Text = item.Caminho ?? string.Empty;
                textBoxDataCriacao.Text = item.CriadoEm.ToString("dd/MM/yyyy HH:mm:ss");
                textBoxDataEdicao.Text = item.DataDeEdicao.ToString("dd/MM/yyyy HH:mm:ss");
                SelecionarTipoDoValorNoCombo(item.TipoDoValor);
                textBoxReferenciaPropriedade.Text = MontarPlaceholderAbsolutoDoItem(no);
            });

            AtualizarImagemLateral(item);
            SincronizarEstadoCamposEdicao();
            AtualizarEstadoButtonRun();
        }

        private void AtualizarImagemLateral(InfrastructureItem item)
        {
            if (pictureBoxImagem == null)
                return;

            pictureBoxImagem.Image = item == null ? ObterImagemPorChave(_iconeSelecionadoCamposEdicao) : ObterImagemDoItem(item);
            pictureBoxImagem.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private Image ObterImagemPorChave(string chaveIcone)
        {
            if (string.IsNullOrWhiteSpace(chaveIcone))
                return null;

            GarantirIconeCarregadoNoImageListPrincipal(chaveIcone);

            if (imageList1.Images.ContainsKey(chaveIcone))
                return imageList1.Images[chaveIcone];

            return null;
        }

        private void CarregarImagensDaPastaDoPrograma()
        {
            string pastaImagens = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens");

            Directory.CreateDirectory(pastaImagens);
            GarantirIconeVazioCarregado();
            GarantirIconeErroCarregado();

            foreach (string arquivo in Directory.GetFiles(pastaImagens, "*.*", SearchOption.AllDirectories))
            {
                if (EhArquivoDeImagemValido(arquivo))
                    GarantirIconeCarregadoNoImageListPrincipal(ObterChaveRelativaDaImagem(arquivo, pastaImagens));
            }
        }

        private void GarantirIconeErroCarregado()
        {
            if (imageList1.Images.ContainsKey(ChaveIconeErro))
                return;

            imageList1.Images.Add(ChaveIconeErro, new Bitmap(Properties.Resources.ErrorSmall, new Size(16, 16)));
        }

        private void GarantirIconeVazioCarregado()
        {
            //imageList1.Images["Vazio.png"];
        }

        private void GarantirIconeCarregadoNoImageListPrincipal(string chaveIcone)
        {
            if (string.IsNullOrWhiteSpace(chaveIcone) || imageList1.Images.ContainsKey(chaveIcone))
                return;

            string caminhoArquivo = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Imagens",
                chaveIcone.Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(caminhoArquivo))
                return;

            using (Image imagemOriginal = TentarCarregarImagemParaImageList(caminhoArquivo))
            {
                if (imagemOriginal == null)
                    return;

                using (Bitmap miniatura = new Bitmap(imagemOriginal, new Size(16, 16)))
                {
                    imageList1.Images.Add(chaveIcone, new Bitmap(miniatura));
                }
            }
        }

        private static Image TentarCarregarImagemParaImageList(string caminhoArquivo)
        {
            try
            {
                return CarregarImagemParaImageList(caminhoArquivo);
            }
            catch
            {
                return null;
            }
        }

        private static Image CarregarImagemParaImageList(string caminhoArquivo)
        {
            string extensao = Path.GetExtension(caminhoArquivo);

            if (string.Equals(extensao, ".ico", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    using (Icon icon = new Icon(caminhoArquivo))
                    {
                        return icon.ToBitmap();
                    }
                }
                catch (ArgumentException)
                {
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

        private string ResolverIconeDoItem(InfrastructureItem item)
        {
            if (item == null)
                return string.Empty;

            string chaveIcone = (item.IconeKey ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(chaveIcone))
                return string.Empty;

            if (!string.IsNullOrWhiteSpace(chaveIcone))
            {
                GarantirIconeCarregadoNoImageListPrincipal(chaveIcone);

                if (imageList1.Images.ContainsKey(chaveIcone))
                    return chaveIcone;
            }

            string chaveEncontrada = ProcurarIconePorNomeDeArquivo(chaveIcone);

            if (!string.IsNullOrWhiteSpace(chaveEncontrada))
            {
                GarantirIconeCarregadoNoImageListPrincipal(chaveEncontrada);

                if (imageList1.Images.ContainsKey(chaveEncontrada))
                {
                    item.IconeKey = chaveEncontrada;
                    item.SincronizarMetadados();
                    return chaveEncontrada;
                }
            }

            GarantirIconeErroCarregado();
            return ChaveIconeErro;
        }

        private static void AplicarIconeNoNo(TreeNode no, string chaveIcone)
        {
            if (no == null)
                return;

            if (string.IsNullOrWhiteSpace(chaveIcone))
            {
                no.ImageKey = ChaveIconeVazio;
                no.SelectedImageKey = ChaveIconeVazio;
                return;
            }

            no.ImageKey = chaveIcone;
            no.SelectedImageKey = chaveIcone;
        }

        private string ProcurarIconePorNomeDeArquivo(string chaveIcone)
        {
            string nomeArquivo = Path.GetFileName((chaveIcone ?? string.Empty).Replace('/', Path.DirectorySeparatorChar));

            if (string.IsNullOrWhiteSpace(nomeArquivo))
                return string.Empty;

            string pastaImagens = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens");

            if (!Directory.Exists(pastaImagens))
                return string.Empty;

            foreach (string arquivo in Directory.GetFiles(pastaImagens, "*.*", SearchOption.AllDirectories)
                .Where(EhArquivoDeImagemValido)
                .OrderBy(a => a))
            {
                if (string.Equals(Path.GetFileName(arquivo), nomeArquivo, StringComparison.OrdinalIgnoreCase))
                    return ObterChaveRelativaDaImagem(arquivo, pastaImagens);
            }

            return string.Empty;
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

        private static string ObterChaveRelativaDaImagem(string caminhoArquivo, string pastaImagens)
        {
            string caminhoBase = Path.GetFullPath(pastaImagens).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string caminhoCompleto = Path.GetFullPath(caminhoArquivo);

            if (!caminhoCompleto.StartsWith(caminhoBase, StringComparison.OrdinalIgnoreCase))
                return Path.GetFileName(caminhoArquivo);

            string relativo = caminhoCompleto.Substring(caminhoBase.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return relativo.Replace(Path.DirectorySeparatorChar, '/').Replace(Path.AltDirectorySeparatorChar, '/');
        }

        private static bool EhPropriedadeProtegida(string nomePropriedade, string localPropriedade)
        {
            if (!string.Equals((localPropriedade ?? string.Empty).Trim(), "", StringComparison.OrdinalIgnoreCase))
                return false;

            return string.Equals(nomePropriedade, "Nome", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "Valor", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "Tipo do Valor", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "Descricao Item", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "Imagem/Icone", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "ID", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "Criado em", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "Editado em", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "Caminho", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "Observacao/Local", StringComparison.OrdinalIgnoreCase);
        }

        private bool AtualizarMetadadoDoItem(InfrastructureItem item, string nomePropriedade, string valor)
        {
            TreeNode noSelecionado = treeViewItens.SelectedNode;

            if (string.Equals(nomePropriedade, "Nome", StringComparison.OrdinalIgnoreCase))
            {
                string nomeAtualizado = (valor ?? string.Empty).Trim();

                if (!ValidarNomeItem(nomeAtualizado, "Salvar propriedade"))
                    return false;

                item.NomeExibicao = nomeAtualizado;

                if (noSelecionado != null)
                {
                    noSelecionado.Text = nomeAtualizado;
                    noSelecionado.Name = nomeAtualizado;
                }
            }
            else if (string.Equals(nomePropriedade, "Valor", StringComparison.OrdinalIgnoreCase))
                item.Valor = valor ?? string.Empty;
            else if (string.Equals(nomePropriedade, "Tipo do Valor", StringComparison.OrdinalIgnoreCase))
                item.TipoDoValor = ConverterTextoParaTipoDoValor(valor);
            else if (string.Equals(nomePropriedade, "Descricao Item", StringComparison.OrdinalIgnoreCase))
                item.Descricao = valor ?? string.Empty;
            else if (string.Equals(nomePropriedade, "Imagem/Icone", StringComparison.OrdinalIgnoreCase))
            {
                string icone = (valor ?? string.Empty).Trim();
                item.IconeKey = icone;
                string chaveIcone = ResolverIconeDoItem(item);

                if (noSelecionado != null)
                    AplicarIconeNoNo(noSelecionado, chaveIcone);
            }
            else if (string.Equals(nomePropriedade, "ID", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("O ID e gerado automaticamente e nao pode ser alterado manualmente.", "Salvar propriedade",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (string.Equals(nomePropriedade, "Criado em", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("A data de criacao nao pode ser alterada depois que o item foi criado.", "Salvar propriedade",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (string.Equals(nomePropriedade, "Editado em", StringComparison.OrdinalIgnoreCase)
                || string.Equals(nomePropriedade, "Caminho", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Este campo e calculado automaticamente.", "Salvar propriedade",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (string.Equals(nomePropriedade, "Observacao/Local", StringComparison.OrdinalIgnoreCase))
                item.Observacao = valor ?? string.Empty;
            else if (string.Equals(nomePropriedade, "Tipo do Item", StringComparison.OrdinalIgnoreCase))
                item.TipoItem = valor ?? string.Empty;

            item.MarcarEditado();
            item.SincronizarMetadados();
            return true;
        }

        private void LimparCamposEdicao()
        {
            _iconeSelecionadoCamposEdicao = string.Empty;

            ExecutarSemMonitoramentoCamposEdicao(() =>
            {
                textBoxNome.Text = string.Empty;
                RichTextBoxValor.Text = string.Empty;
                textBoxDescricao.Text = string.Empty;
                textBoxID.Text = string.Empty;
                textBoxLocal.Text = string.Empty;
                textBoxDataCriacao.Text = string.Empty;
                textBoxDataEdicao.Text = string.Empty;
                textBoxReferenciaPropriedade.Text = string.Empty;
                SelecionarTipoNoCombo(TipoValorPropriedade.Texto);
            });
            AtualizarImagemLateral(null);
            SincronizarEstadoCamposEdicao();
            AtualizarEstadoButtonRun();
        }

        private void buttonAlterarIcone_Click(object sender, EventArgs e)
        {
            using (FormSelectIcon dialog = new FormSelectIcon())
            {
                dialog.ConfigurarSelecao(_iconeSelecionadoCamposEdicao);

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                AplicarIconesAtualizados(dialog.IconesAtualizados);

                _iconeSelecionadoCamposEdicao = dialog.IconeSelecionado ?? string.Empty;
                GarantirIconeCarregadoNoImageListPrincipal(_iconeSelecionadoCamposEdicao);
                AtualizarImagemLateral(null);
            }
        }

        private void AplicarIconesAtualizados(IDictionary<string, string> iconesAtualizados)
        {
            if (iconesAtualizados == null || iconesAtualizados.Count == 0)
                return;

            CarregarImagensDaPastaDoPrograma();

            if (!string.IsNullOrWhiteSpace(_iconeSelecionadoCamposEdicao))
                _iconeSelecionadoCamposEdicao = ResolverChaveIconeAtualizada(_iconeSelecionadoCamposEdicao, iconesAtualizados);

            foreach (TreeNode no in treeViewItens.Nodes)
                AplicarIconesAtualizados(no, iconesAtualizados);

            PersistirCadastro();
        }

        private void AplicarIconesAtualizados(TreeNode no, IDictionary<string, string> iconesAtualizados)
        {
            if (no == null)
                return;

            InfrastructureItem item = no.Tag as InfrastructureItem;

            if (item != null && !string.IsNullOrWhiteSpace(item.IconeKey))
            {
                string chaveAtualizada = ResolverChaveIconeAtualizada(item.IconeKey, iconesAtualizados);

                if (!string.Equals(item.IconeKey, chaveAtualizada, StringComparison.OrdinalIgnoreCase))
                {
                    item.IconeKey = chaveAtualizada;
                    item.SincronizarMetadados();
                    GarantirIconeCarregadoNoImageListPrincipal(chaveAtualizada);
                    AplicarIconeNoNo(no, ResolverIconeDoItem(item));
                }
            }

            foreach (TreeNode noFilho in no.Nodes)
                AplicarIconesAtualizados(noFilho, iconesAtualizados);
        }

        private static string ResolverChaveIconeAtualizada(string chaveIcone, IDictionary<string, string> iconesAtualizados)
        {
            string chaveAtual = chaveIcone ?? string.Empty;

            for (int i = 0; i < iconesAtualizados.Count; i++)
            {
                string proximaChave;

                if (!iconesAtualizados.TryGetValue(chaveAtual, out proximaChave) ||
                    string.Equals(chaveAtual, proximaChave, StringComparison.OrdinalIgnoreCase))
                {
                    return chaveAtual;
                }

                chaveAtual = proximaChave;
            }

            return chaveAtual;
        }

        private void CampoTipoOuValorAlterado(object sender, EventArgs e)
        {
            AtualizarFormatoRichTextBoxValor();
            AtualizarEstadoButtonRun();
        }

        private void AtualizarEstadoButtonRun()
        {
            buttonRun.Enabled = PodeExecutarTipoSelecionado() && !string.IsNullOrWhiteSpace(RichTextBoxValor.Text);
        }

        private bool PodeExecutarTipoSelecionado()
        {
            TipoDoValor tipoSelecionado = ObterTipoDoValorSelecionado();
            return tipoSelecionado == TipoDoValor.Comando
                || tipoSelecionado == TipoDoValor.Script
                || tipoSelecionado == TipoDoValor.Password;
        }

        private bool UsaFormatoCodigoTipoSelecionado()
        {
            TipoDoValor tipoSelecionado = ObterTipoDoValorSelecionado();
            return tipoSelecionado == TipoDoValor.Comando
                || tipoSelecionado == TipoDoValor.Script;
        }

        private TipoValorPropriedade ObterTipoSelecionado()
        {
            object valorSelecionado = comboBoxTipo.SelectedItem;

            if (valorSelecionado is TipoValorPropriedade)
                return (TipoValorPropriedade)valorSelecionado;

            if (valorSelecionado is TipoDoValor)
            {
                TipoDoValor tipoDoValor = (TipoDoValor)valorSelecionado;

                if (tipoDoValor == TipoDoValor.Comando)
                    return TipoValorPropriedade.Comando;

                if (tipoDoValor == TipoDoValor.Script)
                    return TipoValorPropriedade.Script;
            }

            return TipoValorPropriedade.Texto;
        }

        private void SelecionarTipoNoCombo(TipoValorPropriedade tipo)
        {
            comboBoxTipo.SelectedItem = ConverterTipoPropriedadeParaTipoDoValor(tipo);
        }

        private TipoDoValor ObterTipoDoValorSelecionado()
        {
            object valorSelecionado = comboBoxTipo.SelectedItem;

            if (valorSelecionado is TipoDoValor)
                return (TipoDoValor)valorSelecionado;

            if (valorSelecionado is TipoValorPropriedade)
                return ConverterTipoPropriedadeParaTipoDoValor((TipoValorPropriedade)valorSelecionado);

            return TipoDoValor.Texto;
        }

        private void SelecionarTipoDoValorNoCombo(TipoDoValor tipo)
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

        private static TipoDoValor ConverterTextoParaTipoDoValor(string tipo)
        {
            TipoDoValor tipoConvertido;

            if (Enum.TryParse(tipo ?? string.Empty, true, out tipoConvertido))
                return tipoConvertido;

            return TipoDoValor.Texto;
        }

        private static TipoDoValor ConverterTipoPropriedadeParaTipoDoValor(TipoValorPropriedade tipo)
        {
            switch (tipo)
            {
                case TipoValorPropriedade.Comando:
                    return TipoDoValor.Comando;
                case TipoValorPropriedade.Script:
                    return TipoDoValor.Script;
                default:
                    return TipoDoValor.Texto;
            }
        }

        private string MontarPlaceholderDaPropriedade(PropertyGridSelectionInfo info, TreeNode noItem)
        {
            if (info == null || noItem == null)
                return string.Empty;

            string referenciaPropriedade = MontarReferenciaDaPropriedade(info.Local, info.Nome);
            string referenciaCompleta = MontarReferenciaCompleta(string.Empty, referenciaPropriedade);

            return string.IsNullOrWhiteSpace(referenciaCompleta) ? string.Empty : "~REF[/" + referenciaCompleta + "]~";
        }

        private bool TryResolverTextoComReferencias(string template, out string valorResolvido, out string erro)
        {
            return TryResolverTextoComReferenciasRef(template, out valorResolvido, out erro);
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
                RichTextBoxValor.Text,
                textBoxDescricao.Text,
                textBoxID.Text,
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

        private static PropertyGridSelectionInfo CriarInfoDaPropriedade(DynamicPropertyItem item)
        {
            if (item == null)
                return null;

            return new PropertyGridSelectionInfo
            {
                Nome = item.Name,
                Valor = item.Value,
                Descricao = item.Description,
                Categoria = item.Category,
                Local = item.Path,
                Tipo = item.Tipo
            };
        }

        private static string ObterValorTextoPropertyGrid(object valor)
        {
            DynamicPropertyItem item = valor as DynamicPropertyItem;

            if (item != null)
                return Convert.ToString(item.Value) ?? string.Empty;

            return Convert.ToString(valor) ?? string.Empty;
        }

        private static string ObterValorAtualPropriedadeProtegida(
            InfrastructureItem item,
            string nomePropriedade,
            object valorFallback)
        {
            if (item == null)
                return ObterValorTextoPropertyGrid(valorFallback);

            if (string.Equals(nomePropriedade, "Nome", StringComparison.OrdinalIgnoreCase))
                return item.NomeExibicao ?? string.Empty;

            if (string.Equals(nomePropriedade, "Valor", StringComparison.OrdinalIgnoreCase))
                return item.Valor ?? string.Empty;

            if (string.Equals(nomePropriedade, "Tipo do Valor", StringComparison.OrdinalIgnoreCase))
                return item.TipoDoValor.ToString();

            if (string.Equals(nomePropriedade, "Descricao Item", StringComparison.OrdinalIgnoreCase))
                return item.Descricao ?? string.Empty;

            if (string.Equals(nomePropriedade, "Imagem/Icone", StringComparison.OrdinalIgnoreCase))
                return item.IconeKey ?? string.Empty;

            if (string.Equals(nomePropriedade, "Criado em", StringComparison.OrdinalIgnoreCase))
                return item.CriadoEm.ToString("dd/MM/yyyy HH:mm:ss");

            if (string.Equals(nomePropriedade, "ID", StringComparison.OrdinalIgnoreCase))
                return item.ID ?? string.Empty;

            if (string.Equals(nomePropriedade, "Editado em", StringComparison.OrdinalIgnoreCase))
                return item.DataDeEdicao.ToString("dd/MM/yyyy HH:mm:ss");

            if (string.Equals(nomePropriedade, "Caminho", StringComparison.OrdinalIgnoreCase))
                return item.Caminho ?? string.Empty;

            if (string.Equals(nomePropriedade, "Observacao/Local", StringComparison.OrdinalIgnoreCase))
                return item.Observacao ?? string.Empty;

            if (string.Equals(nomePropriedade, "Tipo do Item", StringComparison.OrdinalIgnoreCase))
                return item.TipoItem ?? string.Empty;

            return ObterValorTextoPropertyGrid(valorFallback);
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

            CarregarItemNosCamposEdicao(noSelecionado);
            textBoxNome.Focus();
        }

     
        private void treeViewItens_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
                treeViewItens.SelectedNode = e.Node;

            if (ExecutarItemSeForComandoOuScript(e.Node))
                return;

            buttonEditar_Click(sender, EventArgs.Empty);
        }

        private bool ExecutarItemSeForComandoOuScript(TreeNode no)
        {
            InfrastructureItem item = no != null ? no.Tag as InfrastructureItem : null;

            if (item == null || !EhTipoExecutavel(item.TipoDoValor))
                return false;

            CarregarItemNosCamposEdicao(no);
            buttonRun_Click(this, EventArgs.Empty);
            return true;
        }

        private static bool EhTipoExecutavel(TipoDoValor tipo)
        {
            return tipo == TipoDoValor.Comando
                || tipo == TipoDoValor.Script
                || tipo == TipoDoValor.Password;
        }

        private void buttonIssue_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/hertzoli/InfrAll/issues");
        }

        private void buttonDesfazer_Click(object sender, EventArgs e)
        {
            DesfazerUltimaAcao();
        }
    }

    internal sealed class InfrastructureItem : Item
    {
        public InfrastructureItem(string nomeExibicao, string descricao, string iconeKey, string observacao)
        {
            NomeExibicao = nomeExibicao;
            Descricao = descricao ?? string.Empty;
            IconeKey = iconeKey ?? string.Empty;
            Observacao = observacao ?? string.Empty;
            CriadoEm = DateTime.Now;
            DataDeEdicao = CriadoEm;
            Builder = new PropertyGridRuntimeBuilder();
            SincronizarMetadados();
        }

        public string NomeExibicao
        {
            get { return Nome ?? string.Empty; }
            set { Nome = value ?? string.Empty; }
        }

        public string TipoItem { get; set; }
        public string IconeKey
        {
            get { return Icone ?? string.Empty; }
            set { Icone = value ?? string.Empty; }
        }

        public DateTime CriadoEm
        {
            get { return DataDeCriacao; }
            set { DataDeCriacao = value; }
        }

        public string Observacao { get; set; }
        public PropertyGridRuntimeBuilder Builder { get; private set; }

        public void MarcarEditado()
        {
            DataDeEdicao = DateTime.Now;
        }

        public void SincronizarMetadados()
        {
            Builder.AdicionarPropriedade("Nome", NomeExibicao, "Nome exibido no item", " Info", "");
            Builder.AdicionarPropriedade("Valor", Valor ?? string.Empty, "Valor bruto do item", " Info", "");
            Builder.AdicionarPropriedade("Tipo do Valor", TipoDoValor.ToString(), "Tipo do valor do item", " Info", "");
            Builder.AdicionarPropriedade("Descricao Item", Descricao, "Descricao funcional do item", " Info", "");
            Builder.AdicionarPropriedade("Imagem/Icone", IconeKey, "Chave do icone usado no no da arvore", " Info", "");
            Builder.AdicionarPropriedade("ID", ID ?? string.Empty, "Identificador tecnico unico do item", " Info", "");
            Builder.AdicionarPropriedade("Criado em", CriadoEm.ToString("dd/MM/yyyy HH:mm:ss"), "Data e hora de criacao do item", " Info", "");
            Builder.AdicionarPropriedade("Editado em", DataDeEdicao.ToString("dd/MM/yyyy HH:mm:ss"), "Data e hora da ultima edicao do item", " Info", "");
            Builder.AdicionarPropriedade("Caminho", Caminho ?? string.Empty, "Caminho logico derivado da posicao na arvore", " Info", "");
            Builder.AdicionarPropriedade("Observacao/Local", Observacao, "Observacao, local fisico ou local logico do item", " Info", "");
        }

        public InfrastructureItem Clone()
        {
            InfrastructureItem clone = new InfrastructureItem(NomeExibicao, Descricao, IconeKey, Observacao);
            clone.TipoItem = TipoItem;
            clone.CriadoEm = CriadoEm;
            clone.DataDeEdicao = DataDeEdicao;
            clone.ID = ID;
            clone.Valor = Valor;
            clone.TipoDoValor = TipoDoValor;
            clone.Caminho = Caminho;
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
