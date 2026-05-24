using System;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorSistemas
{
    internal sealed class ItemIdService
    {
        private const string AlfabetoBase36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private readonly Random _random = new Random();

        public string GerarIdUnico(ISet<string> idsExistentes)
        {
            if (idsExistentes == null)
                idsExistentes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            string id;

            do
            {
                string codigo = GerarCodigoBase36(6);
                id = codigo + CalcularDigitoVerificador(codigo);
            }
            while (idsExistentes.Contains(id));

            idsExistentes.Add(id);
            return id;
        }

        public bool EhIdValido(string id)
        {
            string normalizado = NormalizarId(id);

            if (normalizado.Length < 4)
                return false;

            string codigo = normalizado.Substring(0, normalizado.Length - 1);
            char digito = normalizado[normalizado.Length - 1];

            return codigo.Length >= 3
                && codigo.All(c => AlfabetoBase36.IndexOf(c) >= 0)
                && AlfabetoBase36.IndexOf(digito) >= 0
                && digito == CalcularDigitoVerificador(codigo);
        }

        public string NormalizarId(string id)
        {
            return new string((id ?? string.Empty)
                .Trim()
                .ToUpperInvariant()
                .Where(c => AlfabetoBase36.IndexOf(c) >= 0)
                .ToArray());
        }

        private string GerarCodigoBase36(int tamanho)
        {
            char[] caracteres = new char[tamanho];

            for (int i = 0; i < caracteres.Length; i++)
                caracteres[i] = AlfabetoBase36[_random.Next(AlfabetoBase36.Length)];

            return new string(caracteres);
        }

        private static char CalcularDigitoVerificador(string codigo)
        {
            int soma = 0;

            for (int i = 0; i < codigo.Length; i++)
            {
                int valor = AlfabetoBase36.IndexOf(codigo[i]);
                if (valor < 0)
                    valor = 0;

                soma += valor * (i + 2);
            }

            return AlfabetoBase36[soma % AlfabetoBase36.Length];
        }
    }
}
