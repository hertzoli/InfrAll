---
name: winforms-netfx-csharp73-implementation
description: "Implementa e refatora funcionalidades em aplicacoes Windows Forms no .NET Framework 4.6 e 4.7.2 com C# 7.3 em ambiente Visual Studio no Windows 10. Use quando for necessario escrever codigo novo ou alterar codigo existente sem migrar stack, preservar UTF-8 com CRLF, evitar edicao manual de arquivos de designer, manter o formulario focado em UI, extrair logica reutilizavel para classes independentes, seguir regras de seguranca e manutencao e trabalhar dentro do fluxo com README, SourceCode, PRD e compilacao por MSBuild.exe. Tambem use em classes de apoio, servicos e bibliotecas da mesma solucao quando as mesmas restricoes tecnicas se aplicarem."
---

# WinForms .NET Framework 4.6 e 4.7.2 Implementation

Implementar ou refatorar codigo WinForms legado sem romper compatibilidade, sem inflar formularios e sem fugir das restricoes fixas do projeto.

## Workflow

1. Ler `README.md` e localizar o codigo em `SourceCode`.
2. Identificar se a solicitacao afeta UI, logica pura ou codigo misto.
3. Se faltar planejamento, criar um "Nested Task List" no PRD antes da implementacao.
4. Implementar primeiro a logica independente da tela.
5. Integrar no formulario apenas o necessario para coordenacao visual e eventos.
6. Proteger handlers com `try-catch` e registrar erros quando `Logger` existir.
7. Compilar com `MSBuild.exe "[Projeto.csproj]"` quando aplicavel.
8. Atualizar o PRD movendo tarefas implementadas para `### Tarefas Finalizadas aguardando validacao Humana:`.

## Decision Rules

- Se precisa de `this`, controles, bindings, eventos ou estado visual: manter no formulario, preferencialmente em arquivo parcial separado.
- Se pode receber parametros e retornar resultado sem depender de UI: extrair para classe independente em arquivo proprio.
- Se mistura UI e logica: separar primeiro a parte pura, depois reduzir o evento a orquestracao.
- Se a logica puder ser compartilhada por outros formularios: extrair.

## Guardrails

- Nao editar manualmente `*.Designer.cs`, `*.resx` ou `Properties/*.Designer.cs`, salvo pedido explicito.
- Nao introduzir dependencias externas sem necessidade clara.
- Nao criar helpers genericos sem responsabilidade definida.
- Nao duplicar regras de negocio entre formularios ou classes.
- Preservar vocabulario de dominio em portugues nos nomes visiveis ao usuario e nos membros ja consolidados.

## Load References As Needed

- Ler `references/coding-rules.md` para regras fixas de estilo, compatibilidade e seguranca.
- Ler `references/examples.md` quando houver duvida entre manter codigo no formulario ou extrair.
- Ler `references/checklists.md` antes de concluir uma implementacao ou revisao.