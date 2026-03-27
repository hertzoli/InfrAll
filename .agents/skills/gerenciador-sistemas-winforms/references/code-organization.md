# Organização de código neste projeto

## Objetivo

Reduzir o acoplamento do `Form1.cs` sem lutar contra o WinForms clássico.

## Estratégia preferida

Fazer extrações incrementais por tema. Evitar reescrever a arquitetura inteira.

## Divisão recomendada

### Partials da `Form1`

Usar quando o código estiver fortemente preso à tela.

Temas típicos:

- inicialização e configuração visual
- manipulação do `TreeView`
- edição de propriedades
- persistência disparada pela UI
- drag-and-drop
- cópia/execução de comandos

Exemplos:

- `Form1.Inicializacao.cs`
- `Form1.TreeView.cs`
- `Form1.Edicao.cs`
- `Form1.Persistencia.cs`
- `Form1.Comandos.cs`

### Classes independentes

Usar quando houver regra de negócio, transformação de dados ou IO reaproveitável.

Candidatos claros:

- serialização/desserialização YAML
- resolução de placeholders
- normalização de caminhos e referências
- execução de comandos/processos
- validações sem dependência da tela

## Anti-padrões

- Colocar nova lógica extensa direto em `Form1.cs`.
- Criar arquivo `Helpers.cs` com responsabilidades variadas.
- Misturar acesso a controles com parsing, IO e regras de domínio no mesmo método.
- Extrair serviço que ainda depende de metade dos controles da tela.

## Procedimento seguro para refatorar

1. Isolar um grupo pequeno de métodos.
2. Mover para um novo arquivo mantendo assinatura e comportamento.
3. Compilar.
4. Testar o fluxo manual afetado.
5. Só depois iniciar a próxima extração.

## Atualização do csproj

Sempre que criar um novo `.cs`, adicionar um item `Compile Include` em `SourceCode/GerenciadorSistemas.csproj`.

Exemplo:

```xml
<Compile Include="Form1.TreeView.cs">
  <SubType>Form</SubType>
</Compile>
```

Para classes independentes:

```xml
<Compile Include="Services\PlaceholderResolver.cs" />
```

Use `DependentUpon` apenas quando fizer sentido para a experiência do Visual Studio. Para arquivos `partial` da `Form1`, manter a relação visual com `Form1.cs` pode ser útil, mas não é obrigatório para compilar.
