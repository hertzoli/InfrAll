# Patterns

Aplicar estes padroes em C# 7.3 e .NET Framework 4.6 e 4.7.2.

## Task e assinaturas

- Preferir `Task` ou `Task<T>` para metodos assincronos.
- Reservar `async void` para handlers de evento.
- Modelar cancelamento com `CancellationToken` opcional quando fizer sentido para evolucao futura.

## Composicao

- Coordenar `Task.WhenAll` e `Task.WhenAny` explicitando erro e cancelamento esperados.
- Se houver varias tarefas sobre o mesmo estado, desenhar a sincronizacao antes de implementar.
- Tratar cancelamento como fluxo esperado, nao como erro inesperado.

## CPU-bound vs I/O-bound

- I/O-bound: usar `async/await` real.
- CPU-bound: considerar `Task.Run` no ponto de consumo, especialmente quando a UI nao pode bloquear.
- Nao transformar metodo sincronico barato em async sem beneficio concreto.

## ConfigureAwait

- Em codigo de servico, integracao ou biblioteca sem dependencia de UI, considerar `ConfigureAwait(false)`.
- Em codigo de formulario ou em fluxo que precise retornar ao contexto visual, evitar `ConfigureAwait(false)`.

## Inspiracoes aproveitadas das skills baixadas

- Evitar wrapper assincrono artificial para metodo sincronico de biblioteca.
- Evitar wrapper sincronico que bloqueia metodo assincrono.
- Tratar sync-over-async como risco de deadlock e starvation, nao apenas estilo ruim.