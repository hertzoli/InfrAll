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

        [YamlMember(Alias = "items")]
        public List<ItemPersistido> Items { get; set; }

        [YamlMember(Alias = "configuracoes")]
        public ConfiguracoesPersistidas Configuracoes { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    internal sealed class ConfiguracoesPersistidas
    {
        [YamlMember(Alias = "iconePadrao")]
        public string IconePadrao { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    internal sealed class ItemPersistido
    {
        [YamlMember(Alias = "nomeExibicao")]
        public string NomeExibicao { get; set; }

        [YamlMember(Alias = "descricao")]
        public string Descricao { get; set; }

        [YamlMember(Alias = "iconeKey")]
        public string IconeKey { get; set; }

        [YamlMember(Alias = "observacao")]
        public string Observacao { get; set; }

        [YamlMember(Alias = "criadoEm")]
        public string CriadoEm { get; set; }

        [YamlMember(Alias = "tipoItem")]
        public string TipoItem { get; set; }

        [YamlMember(Alias = "propriedades")]
        public List<PropriedadePersistida> Propriedades { get; set; }

        [YamlMember(Alias = "children")]
        public List<ItemPersistido> Children { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    internal sealed class PropriedadePersistida
    {
        [YamlMember(Alias = "nome")]
        public string Nome { get; set; }

        [YamlMember(Alias = "valor")]
        public string Valor { get; set; }

        [YamlMember(Alias = "descricao")]
        public string Descricao { get; set; }

        [YamlMember(Alias = "categoria")]
        public string Categoria { get; set; }

        [YamlMember(Alias = "local")]
        public string Local { get; set; }

        [YamlMember(Alias = "tipo")]
        public string Tipo { get; set; }
    }
}
