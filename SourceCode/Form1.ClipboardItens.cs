using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    public partial class Form1
    {
        private const int IntervaloProgressoColagemItens = 25;
        private bool _colandoItemClipboard;

        private void CopiarItemSelecionadoParaClipboard()
        {
            TreeNode noSelecionado = ObterDestinoColagemPeloFoco();

            if (noSelecionado == null)
            {
                MessageBox.Show("Selecione um item para copiar.", "Copiar item",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Item item = MapearNoParaItem(noSelecionado);
            string conteudo = _itemClipboardService.Serializar(item);

            DataObject dados = new DataObject();
            dados.SetData(ItemClipboardService.ClipboardFormat, conteudo);
            dados.SetText(conteudo);
            Clipboard.SetDataObject(dados, true);
        }

        private async void ColarItemDoClipboardNoDestino(TreeNode noDestino)
        {
            if (_colandoItemClipboard)
            {
                RegistrarMensagemLogBar("Colagem ignorada: ja existe uma colagem de item em andamento.");
                return;
            }

            if (noDestino == null)
            {
                MessageBox.Show("Selecione o item de destino antes de colar.", "Colar item",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string conteudo;
            if (!TryObterConteudoDeItemDoClipboard(out conteudo))
            {
                MessageBox.Show("O clipboard nao contem um item valido para colar.", "Colar item",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _colandoItemClipboard = true;

            try
            {
                Item item;
                string erro;
                if (!_itemClipboardService.TryDesserializar(conteudo, out item, out erro))
                {
                    MessageBox.Show(erro, "Colar item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int totalItens = ContarItensRecursivamente(item);
                RegistrarMensagemLogBar(string.Format(
                    "Iniciando colagem de item em '{0}'. Total estimado: {1} item(ns).",
                    noDestino.Text,
                    totalItens));
                await Task.Yield();

                HashSet<string> idsExistentes = ColetarIdsExistentesDaTreeView();
                ProgressoColagemItens progresso = new ProgressoColagemItens(totalItens);
                TreeNode novoNo;
                bool suspenderPersistenciaAnterior = _suspenderPersistencia;
                _suspenderPersistencia = true;

                try
                {
                    _itemHierarchyService.RenovarIdentidadeRecursiva(item, idsExistentes);
                    RegistrarMensagemLogBar("Identidades renovadas para a estrutura colada.");
                    await Task.Yield();

                    novoNo = await CriarNoAPartirDoItemComProgressoAsync(item, progresso);
                }
                finally
                {
                    _suspenderPersistencia = suspenderPersistenciaAnterior;
                }

                RegistrarHistoricoAntesDaAcao("Colar item");

                treeViewItens.BeginUpdate();
                try
                {
                    noDestino.Nodes.Add(novoNo);
                    noDestino.Expand();
                    AtualizarAposOperacaoEstrutural(novoNo);
                }
                finally
                {
                    treeViewItens.EndUpdate();
                }

                RegistrarMensagemLogBar("Estrutura colada na arvore. Salvando cadastro...");
                await Task.Yield();

                PersistirCadastro();
                RegistrarMensagemLogBar(string.Format(
                    "Colagem concluida: {0} item(ns) adicionados em '{1}'.",
                    progresso.Processados,
                    noDestino.Text));
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogErroDetalhado(ex, "Erro ao colar item do clipboard", new
                {
                    Destino = noDestino.Text
                });

                MessageBox.Show(
                    "Nao foi possivel colar o item do clipboard.\r\n" + ex.Message,
                    "Colar item",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                _colandoItemClipboard = false;
            }
        }

        private bool TryObterConteudoDeItemDoClipboard(out string conteudo)
        {
            conteudo = string.Empty;

            IDataObject dados = Clipboard.GetDataObject();
            if (dados != null && dados.GetDataPresent(ItemClipboardService.ClipboardFormat))
            {
                conteudo = dados.GetData(ItemClipboardService.ClipboardFormat) as string;
                return !string.IsNullOrWhiteSpace(conteudo);
            }

            if (Clipboard.ContainsText())
            {
                string texto = Clipboard.GetText();
                if (_itemClipboardService.EhTextoMarcadoComoItem(texto))
                {
                    conteudo = texto;
                    return true;
                }
            }

            return false;
        }

        private TreeNode ObterDestinoColagemPeloFoco()
        {
            if (DataGridViewItem.Focused || DataGridViewItem.ContainsFocus)
                return ObterNoSelecionadoNoDataGridViewItem();

            return treeViewItens.SelectedNode;
        }

        private bool ProcessarAtalhoClipboardItens(KeyEventArgs e)
        {
            if (e == null || !e.Control)
                return false;

            if (e.KeyCode == Keys.Z)
                return ProcessarAtalhoDesfazer(e);

            if (e.KeyCode == Keys.C)
            {
                CopiarItemSelecionadoParaClipboard();
                e.SuppressKeyPress = true;
                e.Handled = true;
                return true;
            }

            if (e.KeyCode == Keys.V)
            {
                ColarItemDoClipboardNoDestino(ObterDestinoColagemPeloFoco());
                e.SuppressKeyPress = true;
                e.Handled = true;
                return true;
            }

            return false;
        }

        private void DataGridViewItem_KeyDown(object sender, KeyEventArgs e)
        {
            ProcessarAtalhoClipboardItens(e);
        }

        private void AtualizarAposOperacaoEstrutural(TreeNode noParaSelecionar)
        {
            AtualizarCaminhosDosItensDaArvore();

            if (noParaSelecionar != null)
            {
                treeViewItens.SelectedNode = noParaSelecionar;
                AtualizarDataGridViewItem(noParaSelecionar, noParaSelecionar);
                CarregarItemNosCamposEdicao(noParaSelecionar);
                noParaSelecionar.EnsureVisible();
            }
            else
            {
                AtualizarDataGridViewItem(null, null);
                LimparCamposEdicao();
                LimparContextoDaPropriedadeSelecionada();
            }
        }

        private async Task<TreeNode> CriarNoAPartirDoItemComProgressoAsync(
            Item itemPersistido,
            ProgressoColagemItens progresso)
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

            progresso.Processados++;
            if (DeveAtualizarProgressoColagem(progresso))
            {
                RegistrarMensagemLogBar(string.Format(
                    "Colando itens: {0}/{1} processado(s)...",
                    progresso.Processados,
                    progresso.Total));
                await Task.Yield();
            }

            if (itemPersistido.Subitens != null)
            {
                foreach (Item filho in itemPersistido.Subitens)
                    no.Nodes.Add(await CriarNoAPartirDoItemComProgressoAsync(filho, progresso));
            }

            return no;
        }

        private static bool DeveAtualizarProgressoColagem(ProgressoColagemItens progresso)
        {
            if (progresso == null)
                return false;

            return progresso.Processados == 1
                || progresso.Processados == progresso.Total
                || progresso.Processados % IntervaloProgressoColagemItens == 0;
        }

        private static int ContarItensRecursivamente(Item item)
        {
            if (item == null)
                return 0;

            int total = 1;

            if (item.Subitens != null)
            {
                foreach (Item filho in item.Subitens)
                    total += ContarItensRecursivamente(filho);
            }

            return total;
        }

        private HashSet<string> ColetarIdsExistentesDaTreeView()
        {
            HashSet<string> ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (TreeNode no in treeViewItens.Nodes)
                ColetarIdsExistentesDoNo(no, ids);

            return ids;
        }

        private void ColetarIdsExistentesDoNo(TreeNode no, ISet<string> ids)
        {
            if (no == null || ids == null)
                return;

            InfrastructureItem item = no.Tag as InfrastructureItem;
            string id = item != null ? _itemIdService.NormalizarId(item.ID) : string.Empty;

            if (!string.IsNullOrWhiteSpace(id))
                ids.Add(id);

            foreach (TreeNode filho in no.Nodes)
                ColetarIdsExistentesDoNo(filho, ids);
        }

        private void RegistrarMensagemLogBar(string mensagem)
        {
            if (richTextBoxLogBar == null || richTextBoxLogBar.IsDisposed)
                return;

            string linha = string.Format(
                "[{0:HH:mm:ss}] {1}{2}",
                DateTime.Now,
                mensagem ?? string.Empty,
                Environment.NewLine);

            richTextBoxLogBar.AppendText(linha);
            richTextBoxLogBar.SelectionStart = richTextBoxLogBar.TextLength;
            richTextBoxLogBar.ScrollToCaret();
            richTextBoxLogBar.Update();
        }

        private sealed class ProgressoColagemItens
        {
            public ProgressoColagemItens(int total)
            {
                Total = Math.Max(0, total);
            }

            public int Total { get; private set; }
            public int Processados { get; set; }
        }
    }
}
