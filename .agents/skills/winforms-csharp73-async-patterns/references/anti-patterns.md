# Anti-Patterns

## Sync-over-async

Sinais:

- `.Result`;
- `.Wait()`;
- wrapper sincronico sobre metodo `Async`.

Impacto:

- deadlock;
- starvation;
- travamento de UI.

## Async artificial

Sinais:

- `Task.Run` escondido dentro de biblioteca so para parecer assincrona;
- metodo `Async` que apenas delega trabalho sincronico barato.

Impacto:

- overhead desnecessario;
- sem beneficio real;
- fronteira enganosa para o consumidor.

## Estado compartilhado sem controle

Sinais:

- varias tarefas alterando a mesma colecao ou estado visual;
- flags mutaveis sem sincronizacao;
- ordem de conclusao afetando comportamento sem regra explicita.

Impacto:

- race condition;
- bugs intermitentes;
- comportamento dificil de reproduzir.