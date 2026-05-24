using System;
using System.Collections.Generic;
using System.Reflection;

namespace GerenciadorSistemas
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    internal class Item
    {
        public Item()
        {
            TipoDoValor = TipoDoValor.Texto;
            DataDeCriacao = DateTime.Now;
            DataDeEdicao = DataDeCriacao;
            Subitens = new List<Item>();
        }

        public string Nome { get; set; }
        public string Valor { get; set; }
        public TipoDoValor TipoDoValor { get; set; }
        public string Descricao { get; set; }
        public string Icone { get; set; }
        public string ID { get; set; }
        public DateTime DataDeCriacao { get; set; }
        public DateTime DataDeEdicao { get; set; }
        public string Caminho { get; set; }
        public List<Item> Subitens { get; set; }
    }
}
