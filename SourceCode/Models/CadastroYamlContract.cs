using System.Collections.Generic;
using System.Reflection;
using YamlDotNet.Serialization;

namespace GerenciadorSistemas
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    internal sealed class CadastroPersistido
    {
        [YamlMember(Alias = "schemaVersion")]
        public int SchemaVersion { get; set; }

        [YamlMember(Alias = "savedAt")]
        public string SavedAt { get; set; }

        [YamlMember(Alias = "itens")]
        public List<ItemPersistido> Itens { get; set; }

        [YamlMember(Alias = "configuracoes")]
        public ConfiguracoesPersistidas Configuracoes { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    internal sealed class ConfiguracoesPersistidas
    {
        [YamlMember(Alias = "iconePadrao")]
        public string IconePadrao { get; set; }

        [YamlMember(Alias = "itensExpandidos")]
        public List<string> ItensExpandidos { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    internal sealed class ItemPersistido
    {
        [YamlMember(Alias = "nome")]
        public string Nome { get; set; }

        [YamlMember(Alias = "valor")]
        public string Valor { get; set; }

        [YamlMember(Alias = "tipoDoValor")]
        public string TipoDoValor { get; set; }

        [YamlMember(Alias = "descricao")]
        public string Descricao { get; set; }

        [YamlMember(Alias = "icone")]
        public string Icone { get; set; }

        [YamlMember(Alias = "id")]
        public string ID { get; set; }

        [YamlMember(Alias = "dataDeCriacao")]
        public string DataDeCriacao { get; set; }

        [YamlMember(Alias = "dataDeEdicao")]
        public string DataDeEdicao { get; set; }

        [YamlMember(Alias = "caminho")]
        public string Caminho { get; set; }

        [YamlMember(Alias = "subitens")]
        public List<ItemPersistido> Subitens { get; set; }
    }
}
