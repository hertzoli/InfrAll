---
name: winforms-ui-event-safety
description: "Protege handlers de eventos WinForms e operacoes assincronas em .NET Framework 4.6 e 4.7.2 com C# 7.3 contra travamento de UI, deadlocks, falhas nao tratadas e acoplamento excessivo entre tela e logica. Use quando houver botoes, timers, carregamento de formulario, chamadas demoradas, atualizacao de controles, necessidade de async/await seguro, tratamento robusto de excecoes, preparo para cancelamento e decisao sobre quando perguntar ao usuario por feedback de progresso."
---

# WinForms UI Event Safety

Coordenar eventos de UI com seguranca, responsividade e baixo acoplamento.

## Workflow

1. Identificar qual evento dispara a acao, quais controles participam e se existe operacao potencialmente demorada.
2. Separar leitura da UI, execucao da logica e atualizacao visual.
3. Manter no handler apenas coordenacao, validacao leve e exibicao de resultado.
4. Usar `async/await` quando houver I/O ou tarefa demorada.
5. Envolver o corpo do evento em `try-catch`.
6. Registrar excecoes com `Logger.LogWriter.LogErroDetalhado(...)` quando `Logger` existir.
7. Avaliar se a operacao exige pergunta explicita sobre progresso e deixar cancelamento facil de adicionar.

## Decision Rules

- Se o codigo toca controles, estado visual ou fluxo do evento: manter no handler ou em metodo de apoio do formulario.
- Se o codigo executa regra de negocio, transformacao de dados ou integracao: extrair para metodo ou classe independente.
- Se a operacao puder travar a UI: converter para fluxo assincrono ou delegar para logica fora do handler.
- Se o evento estiver crescendo demais: quebrar em metodos pequenos com responsabilidade clara.

## Guardrails

- Nao usar `.Result` ou `.Wait()` em fluxo de UI.
- Nao deixar excecao escapar de handler de evento.
- Nao misturar logica demorada com atualizacao direta de controles sem separacao clara.
- Nao ignorar risco de reentrancia, corrida, sincronizacao incorreta ou starvation.
- Nao perguntar sobre progresso em toda operacao; perguntar apenas quando o custo ou a espera justificar.

## Load References As Needed

- Ler `references/async-winforms.md` para padroes de async, cancelamento e seguranca de thread.
- Ler `references/examples.md` quando precisar estruturar handlers finos e previsiveis.
- Ler `references/checklists.md` antes de concluir uma alteracao em eventos ou operacoes de UI.