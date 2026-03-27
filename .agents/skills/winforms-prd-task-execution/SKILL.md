---
name: winforms-prd-task-execution
description: "Transforma pedidos em plano executavel e mantem consistencia de execucao em PRD para projetos Windows Forms no .NET Framework 4.6 e 4.7.2 com C# 7.3. Use quando for necessario criar um Nested Task List em markdown, organizar tarefas e subtarefas antes da implementacao, mover itens implementados de Tarefas Pendentes: para ### Tarefas Finalizadas aguardando validacao Humana:, preservar rastreabilidade no PRD e evitar marcar qualquer item como validado por humano sem solicitacao explicita."
---

# WinForms PRD Task Execution WinForms

Converter pedido em fluxo executavel, rastreavel e consistente com o processo operacional definido para o projeto.

## Workflow

1. Ler o pedido e localizar o PRD relacionado.
2. Se o planejamento estiver fraco ou ausente, criar um Nested Task List em markdown com tarefas e subtarefas.
3. Organizar as tarefas em ordem de dependencia e validacao.
4. Durante a implementacao, retirar do bloco `Tarefas Pendentes:` somente o que foi realmente implementado.
5. Mover o que foi implementado para `### Tarefas Finalizadas aguardando validacao Humana:`.
6. Nunca mover item para `### Tarefas Concluidas e verificadas por Humano:` sem pedido explicito.
7. Manter o PRD coerente com o estado real da implementacao.

## Decision Rules

- Se nao houver planejamento suficiente: criar ou reestruturar o Nested Task List antes de codificar.
- Se uma tarefa for grande demais: quebrar em subtarefas orientadas por entrega real.
- Se parte da tarefa foi implementada e parte nao: mover apenas o que foi concluido de fato.
- Se nao existir PRD claro para a funcionalidade: estruturar o documento minimo antes da execucao relevante.
- Se houver duvida entre progresso tecnico e validacao humana: considerar apenas como finalizada aguardando validacao.

## Guardrails

- Nao marcar como validado por humano sem pedido explicito.
- Nao mover tarefa pendente sem implementacao real correspondente.
- Nao deixar o PRD atrasado em relacao ao estado tecnico da tarefa.
- Nao criar lista superficial que nao ajude a executar.
- Nao misturar conclusao tecnica com aceite humano.

## Load References As Needed

- Ler `references/nested-task-list.md` para padrao de decomposicao em markdown.
- Ler `references/status-rules.md` para regra de movimentacao entre secoes do PRD.
- Ler `references/checklists.md` antes de atualizar o documento apos implementacao.