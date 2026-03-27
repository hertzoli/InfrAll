# Checklist

Antes de concluir fluxo assincrono ou revisao Task-Based:

## Assinaturas

- Confirmar que `async void` existe apenas em handlers de evento.
- Confirmar que `CancellationToken` foi considerado quando fizer sentido.
- Confirmar que nao existe wrapper sincronico bloqueando metodo assincrono.

## Fluxo

- Confirmar que nao ha `.Result` ou `.Wait()` em caminho de UI.
- Confirmar que trabalho CPU-bound nao foi mascarado como async sem criterio.
- Confirmar que `ConfigureAwait(false)` foi usado apenas fora do codigo de UI.

## Concorrencia

- Confirmar que estado compartilhado tem estrategia clara de acesso.
- Confirmar que reentrancia, corrida e ordem de conclusao foram avaliadas.
- Confirmar que cancelamento e erro estao tratados de forma previsivel.