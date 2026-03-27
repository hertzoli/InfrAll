---
name: winforms-csharp73-async-patterns
description: "Aplica padroes robustos de programacao assincrona e Task-Based em C# 7.3 no .NET Framework 4.6 e 4.7.2, com foco principal em WinForms e reutilizacao em bibliotecas, servicos e integracoes do mesmo ecossistema. Use quando for necessario modelar ou revisar Task, CancellationToken, composicao de tarefas, sincronizacao entre UI e servicos, uso apropriado de ConfigureAwait em codigo nao-UI, prevencao de deadlocks, race conditions, starvation, sync-over-async, tratamento seguro de cancelamento e integracao de async/await com formularios Windows Forms."
---

# C# 7.3 Async Task-Based Patterns

Aplicar assincrono compativel com o stack legado sem introduzir travamento de UI, perda de controle de thread ou complexidade desnecessaria.

## Workflow

1. Identificar se a operacao e I/O-bound, CPU-bound ou mista.
2. Decidir se o fluxo deve ser assincrono de ponta a ponta ou permanecer sincronico.
3. Manter `async void` apenas em handlers de evento.
4. Passar `CancellationToken` opcional quando houver chance real de cancelamento futuro.
5. Separar coordenacao de UI da execucao assincrona de negocio ou integracao.
6. Revisar pontos de bloqueio, reentrancia, disputa de estado e captura de contexto.
7. Validar se `ConfigureAwait(false)` faz sentido apenas fora da UI.

## Decision Rules

- Se o metodo toca UI diretamente: manter retorno compativel com o contexto de formulario e evitar `ConfigureAwait(false)` no fluxo visual.
- Se o metodo e de biblioteca, servico ou integracao sem dependencia de UI: considerar `ConfigureAwait(false)`.
- Se a operacao e CPU-bound pesada: nao fingir async; avaliar `Task.Run` no ponto de consumo adequado.
- Se o metodo seria apenas um wrapper assincrono sobre codigo sincronico puro: nao criar async artificial sem necessidade real.
- Se houver necessidade de compor varias tarefas: controlar ordem, erro, cancelamento e acesso a estado compartilhado explicitamente.

## Guardrails

- Nao usar `.Result` ou `.Wait()` em fluxo de UI.
- Nao expor wrapper sincronico sobre metodo assincrono.
- Nao usar `async void` fora de eventos.
- Nao compartilhar estado mutavel entre tarefas sem estrategia clara.
- Nao aplicar `ConfigureAwait(false)` cegamente em codigo de aplicacao WinForms.

## Load References As Needed

- Ler `references/patterns.md` para padroes de Task, cancelamento e composicao.
- Ler `references/winforms-integration.md` para integracao segura com UI WinForms.
- Ler `references/anti-patterns.md` para smells e correcoes comuns.
- Ler `references/checklists.md` antes de concluir implementacao ou revisao assincrona.