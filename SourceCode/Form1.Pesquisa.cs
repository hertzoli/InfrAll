using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    public partial class Form1
    {
        private FormPesquisar _formularioPesquisa;

        private void buttonPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_formularioPesquisa == null || _formularioPesquisa.IsDisposed)
                {
                    _formularioPesquisa = new FormPesquisar(CriarSnapshotPesquisa, SelecionarResultadoPesquisa,
                        ObterImagemPesquisa);
                    _formularioPesquisa.FormClosed += (s, args) => _formularioPesquisa = null;
                    _formularioPesquisa.Show(this);
                }
                else
                {
                    if (_formularioPesquisa.WindowState == FormWindowState.Minimized)
                        _formularioPesquisa.WindowState = FormWindowState.Normal;
                    _formularioPesquisa.BringToFront();
                    _formularioPesquisa.Activate();
                }
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogErroDetalhado(ex, "Erro ao abrir pesquisa");
                MessageBox.Show(this, "Não foi possível abrir a pesquisa.", "Pesquisa", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private System.Drawing.Image ObterImagemPesquisa(string chave)
        {
            if (string.IsNullOrWhiteSpace(chave))
                return null;
            var imagem = ObterImagemPorChave(chave);
            if (imagem != null)
                return imagem;
            GarantirIconeErroCarregado();
            return imageList1.Images[ChaveIconeErro];
        }

        private List<ResultadoPesquisaItem> CriarSnapshotPesquisa()
        {
            var resultados = new List<ResultadoPesquisaItem>();
            foreach (TreeNode no in EnumerarNosPesquisa())
            {
                var item = no.Tag as InfrastructureItem;
                if (item == null)
                    continue;
                resultados.Add(new ResultadoPesquisaItem
                {
                    ID = item.ID,
                    CriadoEm = item.CriadoEm,
                    EditadoEm = item.DataDeEdicao,
                    Campos = new[] { item.IconeKey, item.NomeExibicao, item.Valor, item.TipoDoValor.ToString(),
                        item.Descricao, no.Parent == null ? string.Empty : ObterCaminhoCompletoDoNo(no.Parent),
                        item.ID, MontarPlaceholderAbsolutoDoItem(no),
                        item.CriadoEm.ToString("dd/MM/yyyy HH:mm:ss"), item.DataDeEdicao.ToString("dd/MM/yyyy HH:mm:ss") }
                });
            }
            return resultados;
        }

        private IEnumerable<TreeNode> EnumerarNosPesquisa()
        {
            var pilha = new Stack<TreeNode>(treeViewItens.Nodes.Cast<TreeNode>().Reverse());
            while (pilha.Count > 0)
            {
                TreeNode no = pilha.Pop();
                yield return no;
                foreach (TreeNode filho in no.Nodes.Cast<TreeNode>().Reverse())
                    pilha.Push(filho);
            }
        }

        private bool SelecionarResultadoPesquisa(string id)
        {
            TreeNode no = EnumerarNosPesquisa().FirstOrDefault(n => (n.Tag as InfrastructureItem)?.ID == id);
            if (no == null)
            {
                MessageBox.Show("O item está indisponível. Execute uma nova pesquisa.", "Pesquisa");
                return false;
            }
            if (!ConfirmarSaidaDaPropriedadeComAlteracoes())
                return false;
            ExecutarSemConfirmacaoSaidaPropriedade(() => treeViewItens.SelectedNode = no);
            if (treeViewItens.SelectedNode != no)
                return false;
            for (TreeNode pai = no.Parent; pai != null; pai = pai.Parent)
                pai.Expand();
            no.EnsureVisible();
            CarregarItemNosCamposEdicao(no);
            Activate();
            treeViewItens.Focus();
            return true;
        }
    }
}
