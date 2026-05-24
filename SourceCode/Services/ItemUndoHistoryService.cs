using System;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorSistemas
{
    internal sealed class ItemUndoHistoryService
    {
        private readonly Stack<ItemUndoSnapshot> _historico = new Stack<ItemUndoSnapshot>();
        private readonly int _limite;

        public ItemUndoHistoryService(int limite)
        {
            _limite = limite > 0 ? limite : 50;
        }

        public bool PodeDesfazer
        {
            get { return _historico.Count > 0; }
        }

        public void Registrar(string descricao, IEnumerable<Item> itens, string idSelecionado, IEnumerable<string> idsExpandidos)
        {
            List<Item> copiaItens = ClonarItens(itens);
            _historico.Push(new ItemUndoSnapshot(descricao, copiaItens, idSelecionado ?? string.Empty, idsExpandidos));
            AplicarLimite();
        }

        public bool TryDesfazer(out ItemUndoSnapshot snapshot)
        {
            snapshot = null;

            if (_historico.Count == 0)
                return false;

            snapshot = _historico.Pop();
            return true;
        }

        public void Limpar()
        {
            _historico.Clear();
        }

        public ItemUndoSnapshot CriarSnapshot(string descricao, IEnumerable<Item> itens, string idSelecionado, IEnumerable<string> idsExpandidos)
        {
            return new ItemUndoSnapshot(descricao, ClonarItens(itens), idSelecionado ?? string.Empty, idsExpandidos);
        }

        public void RegistrarSnapshot(ItemUndoSnapshot snapshot)
        {
            if (snapshot == null)
                return;

            _historico.Push(new ItemUndoSnapshot(
                snapshot.Descricao,
                ClonarItens(snapshot.Itens),
                snapshot.IdSelecionado,
                snapshot.IdsExpandidos));
            AplicarLimite();
        }

        private void AplicarLimite()
        {
            if (_historico.Count <= _limite)
                return;

            ItemUndoSnapshot[] snapshots = _historico.Take(_limite).ToArray();
            _historico.Clear();

            for (int i = snapshots.Length - 1; i >= 0; i--)
                _historico.Push(snapshots[i]);
        }

        private static List<Item> ClonarItens(IEnumerable<Item> itens)
        {
            if (itens == null)
                return new List<Item>();

            return itens
                .Where(item => item != null)
                .Select(ClonarItem)
                .ToList();
        }

        private static Item ClonarItem(Item origem)
        {
            Item clone = new Item
            {
                Nome = origem.Nome,
                Valor = origem.Valor,
                TipoDoValor = origem.TipoDoValor,
                Descricao = origem.Descricao,
                Icone = origem.Icone,
                ID = origem.ID,
                DataDeCriacao = origem.DataDeCriacao,
                DataDeEdicao = origem.DataDeEdicao,
                Caminho = origem.Caminho,
                Subitens = ClonarItens(origem.Subitens)
            };

            return clone;
        }
    }

    internal sealed class ItemUndoSnapshot
    {
        public ItemUndoSnapshot(string descricao, List<Item> itens, string idSelecionado, IEnumerable<string> idsExpandidos)
        {
            Descricao = descricao ?? string.Empty;
            Itens = itens ?? new List<Item>();
            IdSelecionado = idSelecionado ?? string.Empty;
            IdsExpandidos = (idsExpandidos ?? Enumerable.Empty<string>())
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id => id.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        public string Descricao { get; private set; }
        public List<Item> Itens { get; private set; }
        public string IdSelecionado { get; private set; }
        public List<string> IdsExpandidos { get; private set; }
    }
}
