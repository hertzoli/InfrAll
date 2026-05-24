using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GerenciadorSistemas
{
    internal sealed class RefPlaceholderResolver
    {
        private static readonly Regex PlaceholderRegex =
            new Regex(@"~REF\[(.*?)\]~", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public bool TryResolverTexto(string texto, RefItemNode itemAtual, IEnumerable<RefItemNode> itensRaiz, out string valorResolvido, out string erro)
        {
            valorResolvido = texto ?? string.Empty;
            erro = string.Empty;

            MatchCollection matches = PlaceholderRegex.Matches(valorResolvido);

            foreach (Match match in matches)
            {
                string referencia = match.Groups[1].Value.Trim();
                RefItemNode itemReferenciado;

                if (!TryResolverReferencia(referencia, itemAtual, itensRaiz, out itemReferenciado, out erro))
                    return false;

                valorResolvido = valorResolvido.Replace(match.Value, itemReferenciado.Valor ?? string.Empty);
            }

            return true;
        }

        public string MontarPlaceholderAbsoluto(string id, string nomeAuxiliar)
        {
            string idNormalizado = (id ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(idNormalizado))
                return string.Empty;

            string auxiliar = NormalizarAuxiliar(nomeAuxiliar);
            string referencia = string.IsNullOrWhiteSpace(auxiliar)
                ? idNormalizado
                : idNormalizado + "_" + auxiliar;

            return "~REF[" + referencia + "]~";
        }

        private bool TryResolverReferencia(string referencia, RefItemNode itemAtual, IEnumerable<RefItemNode> itensRaiz, out RefItemNode itemReferenciado, out string erro)
        {
            itemReferenciado = null;
            erro = string.Empty;

            if (string.IsNullOrWhiteSpace(referencia))
            {
                erro = "Placeholder REF vazio: ~REF[]~";
                return false;
            }

            List<RefItemNode> todos = EnumerarTodos(itensRaiz).ToList();

            if (EhReferenciaRelativa(referencia))
            {
                if (!TryResolverReferenciaRelativa(referencia, itemAtual, out itemReferenciado))
                {
                    erro = "Referencia relativa nao encontrada: ~REF[" + referencia + "]~";
                    return false;
                }

                return true;
            }

            string id = ExtrairIdDaReferenciaAbsoluta(referencia);
            itemReferenciado = todos.FirstOrDefault(item =>
                string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase));

            if (itemReferenciado == null)
            {
                erro = "Referencia por ID nao encontrada: ~REF[" + referencia + "]~";
                return false;
            }

            return true;
        }

        private static bool TryResolverReferenciaRelativa(string referencia, RefItemNode itemAtual, out RefItemNode itemReferenciado)
        {
            itemReferenciado = null;

            if (itemAtual == null)
                return false;

            RefItemNode atual = itemAtual;
            string[] partes = referencia
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(parte => parte.Trim())
                .Where(parte => !string.IsNullOrWhiteSpace(parte))
                .ToArray();

            foreach (string parte in partes)
            {
                if (string.Equals(parte, ".", StringComparison.Ordinal))
                    continue;

                if (string.Equals(parte, "..", StringComparison.Ordinal))
                {
                    if (atual.Pai == null)
                        return false;

                    atual = atual.Pai;
                    continue;
                }

                atual = atual.Filhos.FirstOrDefault(filho =>
                    string.Equals(filho.Nome, parte, StringComparison.OrdinalIgnoreCase));

                if (atual == null)
                    return false;
            }

            itemReferenciado = atual;
            return true;
        }

        private static bool EhReferenciaRelativa(string referencia)
        {
            string texto = (referencia ?? string.Empty).Trim();
            return texto.StartsWith("/", StringComparison.Ordinal)
                || texto.StartsWith(".", StringComparison.Ordinal)
                || texto.IndexOf("/", StringComparison.Ordinal) >= 0;
        }

        private static string ExtrairIdDaReferenciaAbsoluta(string referencia)
        {
            string texto = (referencia ?? string.Empty).Trim();
            int indiceSeparador = texto.IndexOf('_');

            if (indiceSeparador < 0)
                return texto;

            return texto.Substring(0, indiceSeparador);
        }

        private static IEnumerable<RefItemNode> EnumerarTodos(IEnumerable<RefItemNode> itens)
        {
            if (itens == null)
                yield break;

            foreach (RefItemNode item in itens)
            {
                if (item == null)
                    continue;

                yield return item;

                foreach (RefItemNode filho in EnumerarTodos(item.Filhos))
                    yield return filho;
            }
        }

        private static string NormalizarAuxiliar(string nome)
        {
            string texto = (nome ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            char[] caracteres = texto
                .Select(c => char.IsLetterOrDigit(c) ? c : '_')
                .ToArray();

            return new string(caracteres).Trim('_');
        }
    }

    internal sealed class RefItemNode
    {
        public RefItemNode()
        {
            Filhos = new List<RefItemNode>();
        }

        public string Id { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public RefItemNode Pai { get; set; }
        public List<RefItemNode> Filhos { get; private set; }
    }
}
