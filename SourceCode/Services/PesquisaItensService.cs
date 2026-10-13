using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GerenciadorSistemas
{
    internal static class PesquisaItensService
    {
        public static readonly string[] Colunas =
            { "icone", "Nome", "Valor", "Tipo", "Descrição", "Local", "ID", "Referencia", "Criado em", "Editado em" };
        public static readonly string[] RelacoesTexto =
            { "é", "não é", "contém", "não contém", "inicia com", "termina com" };
        public static readonly string[] RelacoesData = { "é", "não é", "maior que", "menor que" };

        public static List<ResultadoPesquisaItem> Pesquisar(IEnumerable<ResultadoPesquisaItem> itens, string texto)
        {
            texto = (texto ?? string.Empty).Trim();
            if (texto.Length < 2)
                throw new ArgumentException("Informe pelo menos 2 caracteres para pesquisar.");
            return itens.Where(i => i.Campos.Any(c => (c ?? string.Empty).IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0))
                .GroupBy(i => i.ID).Select(g => g.First()).ToList();
        }

        public static void Validar(FiltroPesquisa filtro)
        {
            if (filtro.Coluna < 0 || filtro.Coluna >= Colunas.Length)
                throw new ArgumentException("Selecione uma coluna.");
            string[] relacoes = filtro.Coluna >= 8 ? RelacoesData : RelacoesTexto;
            if (!relacoes.Contains(filtro.Relacao))
                throw new ArgumentException("Selecione uma relação válida para a coluna.");
            if (filtro.Coluna >= 8)
                LerData(filtro.Valor);
            else if (filtro.Relacao != "é" && filtro.Relacao != "não é" && string.IsNullOrWhiteSpace(filtro.Valor))
                throw new ArgumentException("Informe um valor para a relação selecionada.");
        }

        private static DateTime LerData(string valor)
        {
            DateTime data;
            if (!DateTime.TryParseExact(valor, new[] { "dd/MM/yyyy", "dd/MM/yyyy HH:mm:ss" },
                CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out data))
                throw new ArgumentException("Informe uma data em dd/MM/yyyy ou dd/MM/yyyy HH:mm:ss.");
            return data;
        }

        public static List<ResultadoPesquisaItem> Filtrar(IEnumerable<ResultadoPesquisaItem> itens, IEnumerable<FiltroPesquisa> filtros)
        {
            List<FiltroPesquisa> ativos = filtros.Where(f => f.Habilitado).ToList();
            foreach (FiltroPesquisa filtro in ativos)
                Validar(filtro);
            var grupos = ativos.Where(f => !f.Excluir).GroupBy(f => f.Coluna).ToList();
            var exclusoes = ativos.Where(f => f.Excluir).ToList();
            return itens.Where(i => grupos.All(g => g.Any(f => Corresponde(i, f)))
                && !exclusoes.Any(f => Corresponde(i, f))).ToList();
        }

        private static bool Corresponde(ResultadoPesquisaItem item, FiltroPesquisa filtro)
        {
            if (filtro.Coluna >= 8)
            {
                DateTime esperado = LerData(filtro.Valor);
                DateTime atual = filtro.Coluna == 8 ? item.CriadoEm : item.EditadoEm;
                atual = filtro.Valor.Length == 10 ? atual.Date : atual.AddTicks(-(atual.Ticks % TimeSpan.TicksPerSecond));
                switch (filtro.Relacao)
                {
                    case "é": return atual == esperado;
                    case "não é": return atual != esperado;
                    case "maior que": return atual > esperado;
                    case "menor que": return atual < esperado;
                }
            }
            string texto = item.Campos[filtro.Coluna] ?? string.Empty;
            string valor = filtro.Valor ?? string.Empty;
            switch (filtro.Relacao)
            {
                case "é": return string.Equals(texto, valor, StringComparison.OrdinalIgnoreCase);
                case "não é": return !string.Equals(texto, valor, StringComparison.OrdinalIgnoreCase);
                case "contém": return texto.IndexOf(valor, StringComparison.OrdinalIgnoreCase) >= 0;
                case "não contém": return texto.IndexOf(valor, StringComparison.OrdinalIgnoreCase) < 0;
                case "inicia com": return texto.StartsWith(valor, StringComparison.OrdinalIgnoreCase);
                case "termina com": return texto.EndsWith(valor, StringComparison.OrdinalIgnoreCase);
                default: return false;
            }
        }
    }
}
