using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GerenciadorSistemas
{
    internal sealed class ItemClipboardService
    {
        public const string ClipboardFormat = "GerenciadorSistemas.ItemClipboard.v1";
        private const string TextMarker = "GerenciadorSistemas.ItemClipboard:v1";
        private const int PayloadVersion = 1;

        public string Serializar(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            ItemClipboardPayload payload = new ItemClipboardPayload
            {
                Formato = ClipboardFormat,
                Versao = PayloadVersion,
                Item = item
            };

            ISerializer serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return TextMarker + Environment.NewLine + serializer.Serialize(payload);
        }

        public bool TryDesserializar(string conteudo, out Item item, out string erro)
        {
            item = null;
            erro = string.Empty;

            if (string.IsNullOrWhiteSpace(conteudo))
            {
                erro = "O clipboard nao contem um item valido.";
                return false;
            }

            string yaml = conteudo;

            if (conteudo.StartsWith(TextMarker, StringComparison.Ordinal))
                yaml = conteudo.Substring(TextMarker.Length).TrimStart();

            try
            {
                IDeserializer deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .IgnoreUnmatchedProperties()
                    .Build();

                ItemClipboardPayload payload = deserializer.Deserialize<ItemClipboardPayload>(yaml);

                if (payload == null
                    || !string.Equals(payload.Formato, ClipboardFormat, StringComparison.Ordinal)
                    || payload.Versao != PayloadVersion
                    || payload.Item == null)
                {
                    erro = "O conteudo do clipboard nao e um item reconhecido pelo sistema.";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(payload.Item.Nome))
                {
                    erro = "O item do clipboard esta incompleto e nao pode ser colado.";
                    return false;
                }

                item = payload.Item;
                return true;
            }
            catch (Exception ex)
            {
                erro = "Nao foi possivel ler o item do clipboard.\r\n" + ex.Message;
                return false;
            }
        }

        public bool EhTextoMarcadoComoItem(string conteudo)
        {
            return !string.IsNullOrWhiteSpace(conteudo)
                && conteudo.StartsWith(TextMarker, StringComparison.Ordinal);
        }

        private sealed class ItemClipboardPayload
        {
            public string Formato { get; set; }
            public int Versao { get; set; }
            public Item Item { get; set; }
        }
    }
}
