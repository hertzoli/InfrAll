using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    public partial class Form1
    {
        private void RegistrarHistoricoAntesDaAcao(string descricao)
        {
            if (_restaurandoHistorico)
                return;

            _undoHistoryService.Registrar(
                descricao,
                CapturarItensDaArvore(),
                ObterIdDoNo(treeViewItens.SelectedNode),
                ColetarIdsDosNosExpandidos());
        }

        private ItemUndoSnapshot CapturarSnapshotHistoricoAtual(string descricao)
        {
            return _undoHistoryService.CriarSnapshot(
                descricao,
                CapturarItensDaArvore(),
                ObterIdDoNo(treeViewItens.SelectedNode),
                ColetarIdsDosNosExpandidos());
        }

        private void RegistrarSnapshotHistorico(ItemUndoSnapshot snapshot)
        {
            if (_restaurandoHistorico || snapshot == null)
                return;

            _undoHistoryService.RegistrarSnapshot(snapshot);
        }

        private List<Item> CapturarItensDaArvore()
        {
            return treeViewItens.Nodes.Cast<TreeNode>()
                .Select(MapearNoParaItem)
                .ToList();
        }

        private void LimparHistoricoDeAcoes()
        {
            _undoHistoryService.Limpar();
            _snapshotAntesAlteracaoPropertyGrid = null;
        }

        private bool ProcessarAtalhoDesfazer(KeyEventArgs e)
        {
            if (e == null || !e.Control || e.KeyCode != Keys.Z)
                return false;

            DesfazerUltimaAcao();
            e.SuppressKeyPress = true;
            e.Handled = true;
            return true;
        }

        private void DesfazerUltimaAcao()
        {
            ItemUndoSnapshot snapshot;

            if (!_undoHistoryService.TryDesfazer(out snapshot))
                return;

            RestaurarSnapshotHistorico(snapshot);
        }

        private void RestaurarSnapshotHistorico(ItemUndoSnapshot snapshot)
        {
            if (snapshot == null)
                return;

            _restaurandoHistorico = true;
            _suspenderPersistencia = true;

            try
            {
                treeViewItens.Nodes.Clear();

                foreach (Item item in snapshot.Itens)
                    treeViewItens.Nodes.Add(CriarNoAPartirDoItem(item));

                AplicarEstadoDeExpansao(snapshot.IdsExpandidos);
            }
            finally
            {
                _suspenderPersistencia = false;
                _restaurandoHistorico = false;
            }

            TreeNode noSelecionado = EncontrarNoPorId(snapshot.IdSelecionado);

            if (noSelecionado == null && treeViewItens.Nodes.Count > 0)
                noSelecionado = treeViewItens.Nodes[0];

            AtualizarAposOperacaoEstrutural(noSelecionado);
            _snapshotAntesAlteracaoPropertyGrid = CapturarSnapshotHistoricoAtual("Editar propriedade");
            PersistirCadastro();
        }

        private TreeNode EncontrarNoPorId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            foreach (TreeNode no in treeViewItens.Nodes)
            {
                TreeNode encontrado = EncontrarNoPorId(no, id);
                if (encontrado != null)
                    return encontrado;
            }

            return null;
        }

        private static TreeNode EncontrarNoPorId(TreeNode no, string id)
        {
            if (no == null)
                return null;

            if (string.Equals(ObterIdDoNo(no), id, StringComparison.OrdinalIgnoreCase))
                return no;

            foreach (TreeNode filho in no.Nodes)
            {
                TreeNode encontrado = EncontrarNoPorId(filho, id);
                if (encontrado != null)
                    return encontrado;
            }

            return null;
        }

        private static string ObterIdDoNo(TreeNode no)
        {
            InfrastructureItem item = no != null ? no.Tag as InfrastructureItem : null;
            return item != null ? item.ID ?? string.Empty : string.Empty;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            ProcessarAtalhoDesfazer(e);
        }
    }
}
