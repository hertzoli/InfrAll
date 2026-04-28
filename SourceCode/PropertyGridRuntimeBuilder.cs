using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

public class PropertyGridSelectionInfo
{
    public string Nome { get; set; }
    public object Valor { get; set; }
    public string Descricao { get; set; }
    public string Categoria { get; set; }
    public string Local { get; set; }
    public TipoValorPropriedade Tipo { get; set; }
}

public static class TipoValorPropriedadePersistencia
{
    public const string Texto = "Texto";
    public const string Comando = "Comando";
    public const string Script = "Script";

    public static string ParaPersistencia(TipoValorPropriedade tipo)
    {
        switch (tipo)
        {
            case TipoValorPropriedade.Comando:
                return Comando;
            case TipoValorPropriedade.Script:
                return Script;
            default:
                return Texto;
        }
    }

    public static bool TryParse(string valor, out TipoValorPropriedade tipo)
    {
        if (string.Equals(valor, Comando, StringComparison.OrdinalIgnoreCase))
        {
            tipo = TipoValorPropriedade.Comando;
            return true;
        }

        if (string.Equals(valor, Script, StringComparison.OrdinalIgnoreCase))
        {
            tipo = TipoValorPropriedade.Script;
            return true;
        }

        if (string.Equals(valor, Texto, StringComparison.OrdinalIgnoreCase))
        {
            tipo = TipoValorPropriedade.Texto;
            return true;
        }

        tipo = TipoValorPropriedade.Texto;
        return false;
    }
}

[Obfuscation(Exclude = true, ApplyToMembers = true)]
public enum TipoValorPropriedade
{
    Texto = 0,
    Comando = 1,
    Script = 2
}

public class PropertyGridRuntimeBuilder
{
    public DynamicPropertyBag Root { get; } = new DynamicPropertyBag("Raiz");

    public void AdicionarPropriedade(
        string nomePropriedade,
        object valorPropriedade,
        string descricaoPropriedade = "",
        string categoriaPropriedade = "",
        string localPropriedade = "",
        TipoValorPropriedade tipoValorPropriedade = TipoValorPropriedade.Texto)
    {
        if (string.IsNullOrWhiteSpace(nomePropriedade))
            throw new ArgumentException("O nome da propriedade não pode ser vazio.", nameof(nomePropriedade));

        string localNormalizado = NormalizarCaminho(localPropriedade);
        DynamicPropertyBag destino = ObterOuCriarLocal(localNormalizado);
        DynamicPropertyItem item = destino.GetOrCreatePropertyNode(nomePropriedade, localNormalizado);

        item.Value = valorPropriedade;
        item.Description = descricaoPropriedade ?? string.Empty;
        item.Category = categoriaPropriedade ?? string.Empty;
        item.Path = localNormalizado;
        item.Tipo = tipoValorPropriedade;
    }

    public bool RemoverPropriedade(string nomePropriedade, string localPropriedade = "")
    {
        DynamicPropertyBag destino = ObterLocalExistente(NormalizarCaminho(localPropriedade));

        if (destino == null)
            return false;

        return destino.RemoveProperty(nomePropriedade);
    }

    public bool ContemPropriedade(string nomePropriedade, string localPropriedade = "")
    {
        DynamicPropertyBag destino = ObterLocalExistente(NormalizarCaminho(localPropriedade));

        if (destino == null)
            return false;

        DynamicPropertyItem propriedadeExistente;
        return destino.TryGetProperty(nomePropriedade, out propriedadeExistente);
    }

    public bool AtualizarPropriedade(
        string nomePropriedadeOriginal,
        string localPropriedadeOriginal,
        string novoNomePropriedade,
        object novoValorPropriedade,
        string novaDescricaoPropriedade,
        string novaCategoriaPropriedade,
        string novoLocalPropriedade,
        TipoValorPropriedade novoTipoValorPropriedade,
        out bool conflitoDestino)
    {
        conflitoDestino = false;

        string localOriginalNormalizado = NormalizarCaminho(localPropriedadeOriginal);
        string novoLocalNormalizado = NormalizarCaminho(novoLocalPropriedade);
        DynamicPropertyBag origem = ObterLocalExistente(localOriginalNormalizado);

        if (origem == null)
            return false;

        DynamicPropertyItem item;
        if (!origem.TryGetProperty(nomePropriedadeOriginal, out item))
            return false;

        bool mesmoIdentificador =
            string.Equals(nomePropriedadeOriginal, novoNomePropriedade, StringComparison.OrdinalIgnoreCase)
            && string.Equals(localOriginalNormalizado, novoLocalNormalizado, StringComparison.OrdinalIgnoreCase);

        if (!mesmoIdentificador)
        {
            DynamicPropertyBag destino = ObterOuCriarLocal(novoLocalNormalizado);
            DynamicPropertyItem itemExistenteNoDestino;

            if (destino.TryGetProperty(novoNomePropriedade, out itemExistenteNoDestino)
                && !ReferenceEquals(itemExistenteNoDestino, item))
            {
                conflitoDestino = true;
                return false;
            }

            origem.RemoveProperty(nomePropriedadeOriginal);
            item.Name = novoNomePropriedade;
            item.Path = novoLocalNormalizado;
            destino.AdicionarItemExistente(item);
        }

        item.Value = novoValorPropriedade;
        item.Description = novaDescricaoPropriedade ?? string.Empty;
        item.Category = novaCategoriaPropriedade ?? string.Empty;
        item.Path = novoLocalNormalizado;
        item.Tipo = novoTipoValorPropriedade;
        AtualizarCaminhoDosDescendentes(item);
        return true;
    }

    public PropertyGridRuntimeBuilder Clone()
    {
        PropertyGridRuntimeBuilder clone = new PropertyGridRuntimeBuilder();

        foreach (DynamicPropertyItem item in Root.EnumeratePropertiesRecursive())
        {
            clone.AdicionarPropriedade(
                item.Name,
                item.Value,
                item.Description,
                item.Category,
                item.Path,
                item.Tipo);
        }

        return clone;
    }

    public bool TryGetPropriedadePorReferencia(string referencia, out DynamicPropertyItem item)
    {
        item = null;

        if (string.IsNullOrWhiteSpace(referencia))
            return false;

        string[] partes = referencia
            .Split(new[] { '.', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToArray();

        if (partes.Length == 0)
            return false;

        DynamicPropertyBag atual = Root;

        for (int i = 0; i < partes.Length; i++)
        {
            if (!atual.TryGetProperty(partes[i], out item))
                return false;

            if (i < partes.Length - 1)
                atual = item.Children;
        }

        return item != null;
    }

    private DynamicPropertyBag ObterOuCriarLocal(string caminho)
    {
        DynamicPropertyBag atual = Root;

        if (string.IsNullOrWhiteSpace(caminho))
            return atual;

        string[] partes = caminho
            .Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToArray();

        List<string> caminhoAtual = new List<string>();

        foreach (string parte in partes)
        {
            string pathPai = string.Join("/", caminhoAtual);
            DynamicPropertyItem item = atual.GetOrCreatePropertyNode(parte, pathPai);
            caminhoAtual.Add(parte);
            atual = item.Children;
        }

        return atual;
    }

    private DynamicPropertyBag ObterLocalExistente(string caminho)
    {
        DynamicPropertyBag atual = Root;

        if (string.IsNullOrWhiteSpace(caminho))
            return atual;

        string[] partes = caminho
            .Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToArray();

        foreach (string parte in partes)
        {
            DynamicPropertyItem item;

            if (!atual.TryGetProperty(parte, out item))
                return null;

            atual = item.Children;
        }

        return atual;
    }

    private static string NormalizarCaminho(string caminho)
    {
        if (string.IsNullOrWhiteSpace(caminho))
            return string.Empty;

        string[] partes = caminho
            .Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToArray();

        return string.Join("/", partes);
    }

    private static void AtualizarCaminhoDosDescendentes(DynamicPropertyItem itemPai)
    {
        string caminhoPai = MontarCaminhoCompleto(itemPai.Path, itemPai.Name);

        foreach (DynamicPropertyItem filho in itemPai.Children.EnumerarPropriedadesDiretas())
        {
            filho.Path = caminhoPai;
            AtualizarCaminhoDosDescendentes(filho);
        }
    }

    private static string MontarCaminhoCompleto(string caminhoPai, string nomeAtual)
    {
        string pai = caminhoPai ?? string.Empty;
        string nome = nomeAtual ?? string.Empty;

        if (string.IsNullOrWhiteSpace(pai))
            return nome;

        if (string.IsNullOrWhiteSpace(nome))
            return pai;

        return string.Concat(pai, "/", nome);
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public class DynamicPropertyBag : ICustomTypeDescriptor
{
    private readonly Dictionary<string, DynamicPropertyItem> _properties =
        new Dictionary<string, DynamicPropertyItem>(StringComparer.OrdinalIgnoreCase);

    public string DisplayName { get; }

    public DynamicPropertyBag(string displayName)
    {
        DisplayName = displayName;
    }

    public DynamicPropertyItem GetOrCreatePropertyNode(string name, string pathPai)
    {
        DynamicPropertyItem item;

        if (!_properties.TryGetValue(name, out item))
        {
            item = new DynamicPropertyItem
            {
                Name = name,
                Value = string.Empty,
                Description = string.Empty,
                Category = string.Empty,
                Path = pathPai ?? string.Empty,
                Tipo = TipoValorPropriedade.Texto
            };

            _properties[name] = item;
        }

        return item;
    }

    public bool TryGetProperty(string name, out DynamicPropertyItem item)
    {
        return _properties.TryGetValue(name, out item);
    }

    public bool RemoveProperty(string nome)
    {
        return _properties.Remove(nome);
    }

    public void AdicionarItemExistente(DynamicPropertyItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        _properties[item.Name] = item;
    }

    public IEnumerable<DynamicPropertyItem> EnumerarPropriedadesDiretas()
    {
        return _properties.Values.OrderBy(p => p.Name);
    }

    public IEnumerable<DynamicPropertyItem> EnumeratePropertiesRecursive()
    {
        foreach (DynamicPropertyItem item in EnumerarPropriedadesDiretas())
        {
            yield return item;

            foreach (DynamicPropertyItem child in item.Children.EnumeratePropertiesRecursive())
                yield return child;
        }
    }

    public bool HasProperties
    {
        get { return _properties.Count > 0; }
    }

    public override string ToString()
    {
        return string.Empty;
    }

    public AttributeCollection GetAttributes()
    {
        return AttributeCollection.Empty;
    }

    public string GetClassName()
    {
        return GetType().Name;
    }

    public string GetComponentName()
    {
        return DisplayName;
    }

    public TypeConverter GetConverter()
    {
        return new ExpandableObjectConverter();
    }

    public EventDescriptor GetDefaultEvent()
    {
        return null;
    }

    public PropertyDescriptor GetDefaultProperty()
    {
        return null;
    }

    public object GetEditor(Type editorBaseType)
    {
        return null;
    }

    public EventDescriptorCollection GetEvents()
    {
        return EventDescriptorCollection.Empty;
    }

    public EventDescriptorCollection GetEvents(Attribute[] attributes)
    {
        return EventDescriptorCollection.Empty;
    }

    public PropertyDescriptorCollection GetProperties()
    {
        return GetProperties(null);
    }

    public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
    {
        List<PropertyDescriptor> descriptors = new List<PropertyDescriptor>();

        foreach (DynamicPropertyItem prop in _properties.Values.OrderBy(p => p.Name))
            descriptors.Add(new DynamicNodePropertyDescriptor(prop));

        return new PropertyDescriptorCollection(descriptors.ToArray(), true);
    }

    public object GetPropertyOwner(PropertyDescriptor pd)
    {
        return this;
    }
}

[TypeConverter(typeof(DynamicPropertyItemValueConverter))]
public class DynamicPropertyItem : ICustomTypeDescriptor
{
    public DynamicPropertyItem()
    {
        Children = new DynamicPropertyBag(string.Empty);
    }

    public string Name { get; set; }
    public object Value { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Path { get; set; }
    public TipoValorPropriedade Tipo { get; set; }
    public DynamicPropertyBag Children { get; }

    public override string ToString()
    {
        return Convert.ToString(Value) ?? string.Empty;
    }

    public AttributeCollection GetAttributes()
    {
        return AttributeCollection.Empty;
    }

    public string GetClassName()
    {
        return GetType().Name;
    }

    public string GetComponentName()
    {
        return Name;
    }

    public TypeConverter GetConverter()
    {
        return new DynamicPropertyItemValueConverter();
    }

    public EventDescriptor GetDefaultEvent()
    {
        return null;
    }

    public PropertyDescriptor GetDefaultProperty()
    {
        return null;
    }

    public object GetEditor(Type editorBaseType)
    {
        return null;
    }

    public EventDescriptorCollection GetEvents()
    {
        return EventDescriptorCollection.Empty;
    }

    public EventDescriptorCollection GetEvents(Attribute[] attributes)
    {
        return EventDescriptorCollection.Empty;
    }

    public PropertyDescriptorCollection GetProperties()
    {
        return Children.GetProperties();
    }

    public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
    {
        return Children.GetProperties(attributes);
    }

    public object GetPropertyOwner(PropertyDescriptor pd)
    {
        return this;
    }
}

public class DynamicPropertyItemValueConverter : ExpandableObjectConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof(string))
            return true;

        return base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        if (destinationType == typeof(string))
            return true;

        return base.CanConvertTo(context, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    {
        string valorTexto = value as string;

        if (valorTexto != null)
        {
            return new DynamicPropertyItem
            {
                Value = valorTexto
            };
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == typeof(string))
        {
            DynamicPropertyItem item = value as DynamicPropertyItem;

            if (item != null)
                return Convert.ToString(item.Value) ?? string.Empty;
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}

public class DynamicNodePropertyDescriptor : PropertyDescriptor
{
    private readonly DynamicPropertyItem _item;

    public DynamicPropertyItem Item
    {
        get { return _item; }
    }

    public DynamicNodePropertyDescriptor(DynamicPropertyItem item)
        : base(item.Name, MontarAtributos(item))
    {
        _item = item;
    }

    private static Attribute[] MontarAtributos(DynamicPropertyItem item)
    {
        List<Attribute> attrs = new List<Attribute>
        {
            new TypeConverterAttribute(typeof(DynamicPropertyItemValueConverter))
        };

        if (!string.IsNullOrWhiteSpace(item.Description))
            attrs.Add(new DescriptionAttribute(item.Description));

        if (!string.IsNullOrWhiteSpace(item.Category))
            attrs.Add(new CategoryAttribute(item.Category));

        return attrs.ToArray();
    }

    public override Type ComponentType
    {
        get { return typeof(DynamicPropertyBag); }
    }

    public override bool IsReadOnly
    {
        get { return false; }
    }

    public override Type PropertyType
    {
        get { return typeof(DynamicPropertyItem); }
    }

    public override bool CanResetValue(object component)
    {
        return false;
    }

    public override object GetValue(object component)
    {
        return _item;
    }

    public override void ResetValue(object component)
    {
    }

    public override void SetValue(object component, object value)
    {
        DynamicPropertyItem itemConvertido = value as DynamicPropertyItem;
        _item.Value = itemConvertido != null ? itemConvertido.Value : value;
        OnValueChanged(component, EventArgs.Empty);
    }

    public override bool ShouldSerializeValue(object component)
    {
        return false;
    }

    public override string Description
    {
        get { return _item.Description ?? string.Empty; }
    }

    public override string Category
    {
        get { return string.IsNullOrWhiteSpace(_item.Category) ? base.Category : _item.Category; }
    }

    public override string DisplayName
    {
        get { return _item.Name; }
    }
}
