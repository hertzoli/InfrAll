# Async WinForms

Aplicar estes padroes ao lidar com operacoes potencialmente demoradas em UI.

## Regras basicas

- Nao usar `.Result` ou `.Wait()` em fluxo que possa executar na thread de UI.
- Usar `async void` apenas em handlers de eventos.
- Fora de eventos, preferir `Task` ou `Task<T>`.
- Encapsular a logica demorada fora do codigo visual.

## Cancelamento e progresso

- Aceitar `CancellationToken` opcional quando a operacao puder ser cancelada no futuro.
- Se a operacao puder demorar e nao houver orientacao explicita, avaliar se deve perguntar ao usuario sobre feedback de progresso.
- Preparar o codigo para adicionar cancelamento sem reescrever a assinatura principal.

## Seguranca de thread

- Evitar compartilhar estado mutavel entre tarefas sem sincronizacao adequada.
- Garantir que atualizacoes de controles ocorram de forma segura para a UI.
- Separar claramente execucao da logica e atualizacao visual para reduzir risco de corrida e reentrancia.

## Escopo de uso

- Em codigo de biblioteca ou servico sem dependencia de UI, evitar captura desnecessaria de contexto quando fizer sentido.
- Em codigo de formulario, priorizar legibilidade e previsibilidade do fluxo assíncrono.