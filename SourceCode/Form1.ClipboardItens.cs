using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    public partial class Form1
    {
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

        private void ColarItemDoClipboardNoDestino(TreeNode noDestino)
        {
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

            Item item;
            string erro;
            if (!_itemClipboardService.TryDesserializar(conteudo, out item, out erro))
            {
                MessageBox.Show(erro, "Colar item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HashSet<string> idsExistentes = _itemHierarchyService.ColetarIds(
                treeViewItens.Nodes.Cast<TreeNode>().Select(MapearNoParaItem));

            _itemHierarchyService.RenovarIdentidadeRecursiva(item, idsExistentes);

            TreeNode novoNo = CriarNoAPartirDoItem(item);
            RegistrarHistoricoAntesDaAcao("Colar item");
            noDestino.Nodes.Add(novoNo);
            noDestino.Expand();

            AtualizarAposOperacaoEstrutural(novoNo);
            PersistirCadastro();
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
    }
}
