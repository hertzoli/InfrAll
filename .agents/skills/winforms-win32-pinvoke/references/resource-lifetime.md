# Resource Lifetime

## Ownership

Antes de chamar a API nativa, responder:

- quem aloca;
- quem libera;
- com qual funcao ou mecanismo libera;
- por quanto tempo o recurso precisa ficar vivo.

## Handles

- Nao perder handle em excecao.
- Nao liberar duas vezes.
- Nao usar handle apos liberado.
- Encapsular o ciclo de vida em classe apropriada quando o caso justificar.

## Buffers e strings

- Saber se a API copia ou apenas referencia o buffer.
- Nao devolver para o codigo gerenciado um ponteiro sem contrato claro.
- Liberar com o mecanismo compativel com a origem da alocacao.