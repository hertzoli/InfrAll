using System;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorSistemas
{
    internal sealed class ItemHierarchyService
    {
        private readonly ItemIdService _idService;

        public ItemHierarchyService(ItemIdService idService)
        {
            _idService = idService ?? new ItemIdService();
        }

        public void PrepararHierarquia(IEnumerable<Item> itens)
        {
            HashSet<string> idsUsados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (Item item in EnumerarItens(itens))
            {
                string idNormalizado = _idService.NormalizarId(item.ID);

                if (string.IsNullOrWhiteSpace(idNormalizado)
                    || !_idService.EhIdValido(idNormalizado)
                    || idsUsados.Contains(idNormalizado))
                {
                    item.ID = _idService.GerarIdUnico(idsUsados);
                }
                else
                {
                    item.ID = idNormalizado;
                    idsUsados.Add(idNormalizado);
                }

                if (item.DataDeCriacao == DateTime.MinValue)
                    item.DataDeCriacao = DateTime.Now;

                if (item.DataDeEdicao == DateTime.MinValue)
                    item.DataDeEdicao = item.DataDeCriacao;

                if (item.Subitens == null)
                    item.Subitens = new List<Item>();
            }

            RecalcularCaminhos(itens);
        }

        public void RenovarIdentidadeRecursiva(Item item, ISet<string> idsExistentes)
        {
            if (item == null)
                return;

            DateTime agora = DateTime.Now;
            item.ID = _idService.GerarIdUnico(idsExistentes);
            item.DataDeCriacao = agora;
            item.DataDeEdicao = agora;

            if (item.Subitens == null)
                item.Subitens = new List<Item>();

            foreach (Item subitem in item.Subitens)
                RenovarIdentidadeRecursiva(subitem, idsExistentes);
        }

        public HashSet<string> ColetarIds(IEnumerable<Item> itens)
        {
            return new HashSet<string>(
                EnumerarItens(itens)
                    .Select(item => _idService.NormalizarId(item.ID))
                    .Where(id => !string.IsNullOrWhiteSpace(id)),
                StringComparer.OrdinalIgnoreCase);
        }

        public void RecalcularCaminhos(IEnumerable<Item> itens)
        {
            RecalcularCaminhos(itens, string.Empty);
        }

        private void RecalcularCaminhos(IEnumerable<Item> itens, string caminhoPai)
        {
            if (itens == null)
                return;

            foreach (Item item in itens)
            {
                string nome = (item.Nome ?? string.Empty).Trim();
                item.Caminho = string.IsNullOrWhiteSpace(caminhoPai)
                    ? nome
                    : string.Concat(caminhoPai, "/", nome);

                RecalcularCaminhos(item.Subitens, item.Caminho);
            }
        }

        private IEnumerable<Item> EnumerarItens(IEnumerable<Item> itens)
        {
            if (itens == null)
                yield break;

            foreach (Item item in itens)
            {
                if (item == null)
                    continue;

                yield return item;

                foreach (Item subitem in EnumerarItens(item.Subitens))
                    yield return subitem;
            }
        }
    }
}
