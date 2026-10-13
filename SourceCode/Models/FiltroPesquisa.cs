namespace GerenciadorSistemas
{
    internal sealed class FiltroPesquisa
    {
        public bool Habilitado { get; set; }
        public int Coluna { get; set; }
        public string Relacao { get; set; }
        public string Valor { get; set; }
        public bool Excluir { get; set; }

        public FiltroPesquisa Copiar()
        {
            return (FiltroPesquisa)MemberwiseClone();
        }
    }
}
