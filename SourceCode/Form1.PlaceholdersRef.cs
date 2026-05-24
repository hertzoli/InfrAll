using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    public partial class Form1
    {
        private void ConfigurarDragDropPlaceholderEmValor()
        {
            RichTextBoxValor.AllowDrop = true;
            RichTextBoxValor.DragEnter += RichTextBoxValor_DragEnter;
            RichTextBoxValor.DragOver += RichTextBoxValor_DragOver;
            RichTextBoxValor.DragDrop += RichTextBoxValor_DragDrop;
        }

        private bool TryResolverTextoComReferenciasRef(string template, out string valorResolvido, out string erro)
        {
            List<RefItemNode> itensRaiz = treeViewItens.Nodes
                .Cast<TreeNode>()
                .Select(no => CriarRefItemNode(no, null))
                .ToList();

            return _refPlaceholderResolver.TryResolverTexto(
                template,
                EncontrarRefNodeCorrespondente(itensRaiz, treeViewItens.SelectedNode),
                itensRaiz,
                out valorResolvido,
                out erro);
        }

        private string MontarPlaceholderAbsolutoDoItem(TreeNode no)
        {
            InfrastructureItem item = no != null ? no.Tag as InfrastructureItem : null;

            if (item == null)
                return string.Empty;

            return _refPlaceholderResolver.MontarPlaceholderAbsoluto(item.ID, item.NomeExibicao);
        }

        private void RichTextBoxValor_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(typeof(TreeNode))
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        }

        private void RichTextBoxValor_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(typeof(TreeNode))
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        }

        private void RichTextBoxValor_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode noSolto = e.Data.GetData(typeof(TreeNode)) as TreeNode;

            if (noSolto == null)
                return;

            string placeholder = MontarPlaceholderRelativoDoItem(noSolto);

            if (string.IsNullOrWhiteSpace(placeholder))
                return;

            System.Drawing.Point ponto = RichTextBoxValor.PointToClient(new System.Drawing.Point(e.X, e.Y));
            int indiceInsercao = RichTextBoxValor.GetCharIndexFromPosition(ponto);

            if (indiceInsercao < 0 || indiceInsercao > RichTextBoxValor.TextLength)
                indiceInsercao = RichTextBoxValor.TextLength;

            RichTextBoxValor.Focus();
            RichTextBoxValor.Select(indiceInsercao, 0);
            RichTextBoxValor.SelectedText = placeholder;
            RichTextBoxValor.Select(indiceInsercao + placeholder.Length, 0);
        }

        private void RestaurarContextoVisualAnteriorAoDrag()
        {
            TreeNode noParaSelecionar = _noSelecionadoAntesDoDrag;
            TreeNode noParaEditar = _noItemEmEdicaoAntesDoDrag ?? noParaSelecionar;

            _suspenderConfirmacaoSelecaoPorDrag = true;
            _suspenderAtualizacaoSelecaoPorDrag = true;

            try
            {
                if (noParaSelecionar != null && noParaSelecionar.TreeView == treeViewItens)
                    treeViewItens.SelectedNode = noParaSelecionar;

                if (noParaEditar != null && noParaEditar.TreeView == treeViewItens)
                {
                    _noItemEmEdicao = noParaEditar;
                    AtualizarDataGridViewItem(noParaSelecionar ?? noParaEditar, noParaEditar);
                }
            }
            finally
            {
                _suspenderAtualizacaoSelecaoPorDrag = _arrastandoItemTreeView;
                _suspenderConfirmacaoSelecaoPorDrag = false;
            }
        }

        private string MontarPlaceholderRelativoDoItem(TreeNode noDestino)
        {
            TreeNode noOrigem = _noItemEmEdicaoAntesDoDrag ?? ObterNoItemEmEdicao() ?? treeViewItens.SelectedNode;

            if (noOrigem == null || noDestino == null)
                return string.Empty;

            string referencia = MontarReferenciaRelativaEntreNos(noOrigem, noDestino);

            if (string.IsNullOrWhiteSpace(referencia))
                return string.Empty;

            return "~REF[" + referencia + "]~";
        }

        private static string MontarReferenciaRelativaEntreNos(TreeNode noOrigem, TreeNode noDestino)
        {
            List<string> partesOrigem = ObterPartesDoCaminhoDoNo(noOrigem).ToList();
            List<string> partesDestino = ObterPartesDoCaminhoDoNo(noDestino).ToList();
            int comuns = 0;

            while (comuns < partesOrigem.Count
                && comuns < partesDestino.Count
                && string.Equals(partesOrigem[comuns], partesDestino[comuns], System.StringComparison.OrdinalIgnoreCase))
            {
                comuns++;
            }

            if (comuns == partesOrigem.Count)
            {
                List<string> descendentes = partesDestino.Skip(comuns).ToList();

                if (descendentes.Count == 0)
                    return ".";

                return "/" + string.Join("/", descendentes);
            }

            List<string> partesReferencia = new List<string>();

            for (int i = comuns; i < partesOrigem.Count; i++)
                partesReferencia.Add("..");

            partesReferencia.AddRange(partesDestino.Skip(comuns));

            return string.Join("/", partesReferencia);
        }

        private static RefItemNode EncontrarRefNodeCorrespondente(IEnumerable<RefItemNode> itensRaiz, TreeNode noOriginal)
        {
            InfrastructureItem itemOriginal = noOriginal != null ? noOriginal.Tag as InfrastructureItem : null;
            string idOriginal = itemOriginal != null ? itemOriginal.ID ?? string.Empty : string.Empty;

            if (string.IsNullOrWhiteSpace(idOriginal))
                return null;

            return EnumerarRefNodes(itensRaiz)
                .FirstOrDefault(item => string.Equals(item.Id, idOriginal, System.StringComparison.OrdinalIgnoreCase));
        }

        private static IEnumerable<RefItemNode> EnumerarRefNodes(IEnumerable<RefItemNode> itens)
        {
            if (itens == null)
                yield break;

            foreach (RefItemNode item in itens)
            {
                if (item == null)
                    continue;

                yield return item;

                foreach (RefItemNode filho in EnumerarRefNodes(item.Filhos))
                    yield return filho;
            }
        }

        private RefItemNode CriarRefItemNode(TreeNode no, RefItemNode pai)
        {
            if (no == null)
                return null;

            InfrastructureItem item = no.Tag as InfrastructureItem;
            RefItemNode node = new RefItemNode
            {
                Id = item != null ? item.ID ?? string.Empty : string.Empty,
                Nome = item != null ? item.NomeExibicao ?? string.Empty : no.Text ?? string.Empty,
                Valor = item != null ? item.Valor ?? string.Empty : string.Empty,
                Pai = pai
            };

            foreach (TreeNode filho in no.Nodes)
            {
                RefItemNode filhoRef = CriarRefItemNode(filho, node);
                if (filhoRef != null)
                    node.Filhos.Add(filhoRef);
            }

            return node;
        }
    }
}
