using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GerenciadorSistemas
{
    internal static class CadastroSerializer
    {
        public static ISerializer Criar()
        {
            return new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .ConfigureDefaultValuesHandling(
                    DefaultValuesHandling.OmitNull
                    | DefaultValuesHandling.OmitDefaults
                    | DefaultValuesHandling.OmitEmptyCollections)
                .Build();
        }
    }
}
