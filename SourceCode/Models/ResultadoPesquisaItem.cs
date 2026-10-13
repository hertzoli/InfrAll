using System;

namespace GerenciadorSistemas
{
    internal sealed class ResultadoPesquisaItem
    {
        public string ID { get; set; }
        public string[] Campos { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime EditadoEm { get; set; }
    }
}
