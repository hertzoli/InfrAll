using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    public partial class Form1
    {
        private List<string> ColetarIdsDosNosExpandidos()
        {
            return EnumerarTodosOsNos()
                .Where(no => no != null && no.IsExpanded)
                .Select(ObterIdDoNo)
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private void AplicarEstadoDeExpansao(IEnumerable<string> idsExpandidos)
        {
            HashSet<string> ids = new HashSet<string>(
                idsExpandidos ?? Enumerable.Empty<string>(),
                StringComparer.OrdinalIgnoreCase);

            if (ids.Count == 0)
                return;

            foreach (TreeNode no in EnumerarTodosOsNos())
            {
                string id = ObterIdDoNo(no);

                if (!string.IsNullOrWhiteSpace(id) && ids.Contains(id))
                    no.Expand();
            }
        }

        private static List<string> NormalizarIdsExpandidos(IEnumerable<string> idsExpandidos)
        {
            return (idsExpandidos ?? Enumerable.Empty<string>())
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id => id.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
