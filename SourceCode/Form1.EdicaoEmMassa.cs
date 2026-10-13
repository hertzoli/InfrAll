using System;
using System.Linq;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    public partial class Form1
    {
        private bool AplicarEdicaoEmMassa(System.Collections.Generic.List<string> ids,
            System.Collections.Generic.Dictionary<int, string> alteracoes)
        {
            var selecionados = new System.Collections.Generic.HashSet<string>(ids);
            var nos = EnumerarNosPesquisa().Where(n => selecionados.Contains((n.Tag as InfrastructureItem)?.ID)).ToList();
            if (nos.Count != selecionados.Count)
            {
                MessageBox.Show("Há itens indisponíveis. Execute uma nova pesquisa.", "Edição em massa");
                return false;
            }
            if (alteracoes.ContainsKey(1) && !ValidarNomeItem(alteracoes[1].Trim(), "Edição em massa")) return false;
            if (!ConfirmarSaidaDaPropriedadeComAlteracoes()) return false;
            RegistrarHistoricoAntesDaAcao("Editar itens em massa");
            foreach (var no in nos)
            {
                var item = (InfrastructureItem)no.Tag;
                if (alteracoes.ContainsKey(0)) item.IconeKey = alteracoes[0];
                if (alteracoes.ContainsKey(1)) item.NomeExibicao = alteracoes[1].Trim();
                if (alteracoes.ContainsKey(2)) item.Valor = alteracoes[2];
                if (alteracoes.ContainsKey(3)) item.TipoDoValor = (TipoDoValor)Enum.Parse(typeof(TipoDoValor), alteracoes[3]);
                if (alteracoes.ContainsKey(4)) item.Descricao = alteracoes[4];
                item.MarcarEditado();
                item.SincronizarMetadados();
                no.Text = no.Name = item.NomeExibicao;
                GarantirIconeCarregadoNoImageListPrincipal(item.IconeKey);
                AplicarIconeNoNo(no, ResolverIconeDoItem(item));
            }
            AtualizarCaminhosDosItensDaArvore();
            ExecutarSemConfirmacaoSaidaPropriedade(() =>
            {
                var atual = ObterNoItemEmEdicao();
                AtualizarDataGridViewItem(treeViewItens.SelectedNode, atual);
                if (atual != null) CarregarItemNosCamposEdicao(atual);
            });
            SincronizarEstadoCamposEdicao();
            PersistirCadastro();
            return true;
        }
    }
}
