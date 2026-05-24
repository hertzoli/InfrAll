using System;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorSistemas
{
    internal sealed class ItemIdService
    {
        private const string AlfabetoBase62 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private long _intBaseID = 1;

        public event EventHandler IntBaseIDConsumido;

        public long IntBaseID
        {
            get { return _intBaseID; }
        }

        public void DefinirIntBaseID(long intBaseID)
        {
            _intBaseID = intBaseID < 1 ? 1 : intBaseID;
        }

        public string GerarIdUnico(ISet<string> idsExistentes)
        {
            if (idsExistentes == null)
                idsExistentes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            string id;

            do
            {
                string codigo = ConverterParaBase62(ConsumirIntBaseID());
                id = codigo + CalcularDigitoVerificador(codigo);
            }
            while (idsExistentes.Contains(id));

            idsExistentes.Add(id);
            return id;
        }

        public bool EhIdValido(string id)
        {
            string normalizado = NormalizarId(id);

            if (normalizado.Length < 2)
                return false;

            string codigo = normalizado.Substring(0, normalizado.Length - 1);
            char digito = normalizado[normalizado.Length - 1];

            return codigo.Length >= 1
                && codigo.All(c => AlfabetoBase62.IndexOf(c) >= 0)
                && AlfabetoBase62.IndexOf(digito) >= 0
                && digito == CalcularDigitoVerificador(codigo);
        }

        public string NormalizarId(string id)
        {
            return new string((id ?? string.Empty)
                .Trim()
                .Where(c => AlfabetoBase62.IndexOf(c) >= 0)
                .ToArray());
        }

        private long ConsumirIntBaseID()
        {
            long valor = _intBaseID;

            if (_intBaseID == long.MaxValue)
                throw new InvalidOperationException("IntBaseID atingiu o limite maximo de Int64.");

            _intBaseID++;
            OnIntBaseIDConsumido();
            return valor;
        }

        private void OnIntBaseIDConsumido()
        {
            EventHandler handler = IntBaseIDConsumido;

            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private static string ConverterParaBase62(long valor)
        {
            if (valor < 0)
                throw new ArgumentOutOfRangeException("valor", "O valor base do ID nao pode ser negativo.");

            if (valor == 0)
                return "0";

            List<char> caracteres = new List<char>();

            while (valor > 0)
            {
                int indice = (int)(valor % AlfabetoBase62.Length);
                caracteres.Add(AlfabetoBase62[indice]);
                valor = valor / AlfabetoBase62.Length;
            }

            caracteres.Reverse();
            return new string(caracteres.ToArray());
        }

        private static char CalcularDigitoVerificador(string codigo)
        {
            int soma = 0;

            for (int i = 0; i < codigo.Length; i++)
            {
                int valor = AlfabetoBase62.IndexOf(codigo[i]);
                if (valor < 0)
                    valor = 0;

                soma += valor * (i + 2);
            }

            return AlfabetoBase62[soma % AlfabetoBase62.Length];
        }
    }
}
