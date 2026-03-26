---
name: gerenciador-sistemas-winforms
description: Desenvolver, corrigir e refatorar o projeto GerenciadorSistemas em C# WinForms/.NET Framework 4.7.2. Use quando Codex estiver trabalhando neste repositório para adicionar funcionalidades, corrigir bugs, reorganizar `Form1.cs`, separar responsabilidades em arquivos e classes menores, atualizar o `.csproj` antigo com novos arquivos `.cs`, preservar o comportamento do WinForms Designer e manter a nomenclatura de domínio em português.
---

# Gerenciador Sistemas WinForms

## Objetivo

Trabalhar neste projeto sem quebrar o padrão atual de WinForms nem concentrar novas responsabilidades em `SourceCode/Form1.cs`.

Priorizar extrações pequenas, seguras e incrementais, com validação manual das telas e atualização explícita do arquivo `SourceCode/GerenciadorSistemas.csproj` quando surgirem novos arquivos `.cs`.

## Fluxo de trabalho

1. Ler os arquivos diretamente envolvidos antes de propor a mudança.
2. Identificar se a alteração pertence a UI, persistência YAML, resolução de placeholders, execução de comandos, manipulação da árvore ou utilidade compartilhada.
3. Escolher o destino correto da mudança:
   - Manter em arquivo separado da própria `Form1` usando `partial class Form1` quando o código depende fortemente de controles, eventos, `TreeView`, `PropertyGrid`, `TextBox`, `MessageBox` ou estado visual.
   - Extrair para classe independente quando a lógica puder receber dados por parâmetro e retornar resultado sem depender diretamente de controles.
4. Criar arquivos pequenos e coesos, evitando "arquivo utilitário genérico" ou "helpers" sem responsabilidade clara.
5. Atualizar `SourceCode/GerenciadorSistemas.csproj` para incluir cada novo arquivo `.cs`, porque o projeto usa formato antigo não-SDK.
6. Compilar e validar manualmente os fluxos impactados.

## Regras de organização

- Não editar `*.Designer.cs`, `*.resx` ou `Properties/*.Designer.cs` manualmente, salvo necessidade explícita da tarefa.
- Preservar vocabulário de domínio em português nos nomes visíveis ao usuário e nos métodos/eventos já existentes.
- Preferir uma responsabilidade por arquivo.
- Evitar criar novas classes enormes. Se um arquivo crescer demais, separar por caso de uso.
- Evitar mover código estável de uma vez só. Refatorar em etapas que mantenham build e comportamento.
- Manter no `Form1` apenas coordenação de UI, estado visual e eventos.
- Mover lógica reaproveitável para classes próprias.

## Padrões recomendados neste projeto

### 1. Código acoplado à tela

Usar arquivos `partial` para dividir a `Form1` por assunto sem mexer no Designer.

Exemplos de nomes aceitáveis:

- `SourceCode/Form1.Persistencia.cs`
- `SourceCode/Form1.TreeView.cs`
- `SourceCode/Form1.Propriedades.cs`
- `SourceCode/Form1.Execucao.cs`
- `SourceCode/Form1.Placeholders.cs`

Aplicar esse padrão quando o método usa vários controles da tela ou atualiza seleção, cores, foco, mensagens ou botões.

### 2. Lógica sem dependência direta da tela

Extrair para classes independentes em arquivos próprios.

Exemplos de destinos aceitáveis:

- `SourceCode/Services/CadastroYamlService.cs`
- `SourceCode/Services/PlaceholderResolver.cs`
- `SourceCode/Services/CommandExecutionService.cs`
- `SourceCode/Models/`
- `SourceCode/Infrastructure/`

Se criar subpastas, refletir isso também no `.csproj`.

### 3. Código compartilhado entre formulários

Extrair para classe independente. Não duplicar entre `Form1` e `FormNovoItem`.

## Critério de decisão rápido

- Se o código precisa de `this`, controles do formulário ou eventos WinForms: usar `partial class Form1` em arquivo separado.
- Se o código pode virar método puro ou serviço com entrada e saída explícitas: extrair para classe independente.
- Se parte do código é pura e parte é visual: separar a parte pura primeiro e deixar a orquestração no formulário.

## Estrutura alvo

Usar esta estrutura como direção, sem exigir migração completa de uma vez:

```text
SourceCode/
|-- Form1.cs
|-- Form1.Designer.cs
|-- Form1.Persistencia.cs
|-- Form1.TreeView.cs
|-- Form1.Propriedades.cs
|-- FormNovoItem.cs
|-- PropertyGridRuntimeBuilder.cs
|-- Services/
|   |-- CadastroYamlService.cs
|   |-- PlaceholderResolver.cs
|   `-- CommandExecutionService.cs
|-- Models/
`-- Infrastructure/
```

## Checklist de implementação

- Ler o arquivo atual e mapear a responsabilidade que será alterada.
- Escolher se a mudança fica em `partial class` ou classe independente.
- Criar ou atualizar o arquivo correto.
- Atualizar `SourceCode/GerenciadorSistemas.csproj` com novos `Compile Include`.
- Verificar `using`, namespace e dependências.
- Compilar com `msbuild SourceCode\GerenciadorSistemas.sln /p:Configuration=Debug`.
- Validar manualmente o fluxo afetado na interface.

## Recursos

- Ler `references/project-context.md` para lembrar as características concretas deste repositório.
- Ler `references/code-organization.md` quando a tarefa envolver divisão de arquivos, extração de classes ou redução do tamanho da `Form1`.
