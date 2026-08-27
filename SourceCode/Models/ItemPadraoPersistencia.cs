namespace GerenciadorSistemas
{
    internal static class ItemPadraoPersistencia
    {
        public const string ValorPadrao = "";
        public const string TipoDoValorPadrao = "Texto";
        public const string DescricaoPadrao = "";
        public const string IconePadrao = "";

        public static bool EhValorPadrao(string valor)
        {
            return valor == ValorPadrao;
        }

        public static bool EhTipoDoValorPadrao(string tipoDoValor)
        {
            return tipoDoValor == TipoDoValorPadrao;
        }

        public static bool EhDescricaoPadrao(string descricao)
        {
            return descricao == DescricaoPadrao;
        }

        public static bool EhIconePadrao(string icone)
        {
            return icone == IconePadrao;
        }

        public static void AplicarPadroes(ItemPersistido item)
        {
            if (item == null)
                return;

            item.Valor = item.Valor ?? ValorPadrao;
            item.TipoDoValor = item.TipoDoValor ?? TipoDoValorPadrao;
            item.Descricao = item.Descricao ?? DescricaoPadrao;
            item.Icone = item.Icone ?? IconePadrao;

            if (item.Subitens == null)
                return;

            foreach (ItemPersistido subitem in item.Subitens)
                AplicarPadroes(subitem);
        }
    }
}
