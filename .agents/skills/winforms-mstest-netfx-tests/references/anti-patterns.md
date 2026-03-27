# Anti-Patterns

## Teste acoplado a UI

Sinais:

- depende de controle visual real;
- valida layout, posicao ou Designer;
- falha por detalhes visuais, nao por regra.

## Teste amplo demais

Sinais:

- cobre muitas regras ao mesmo tempo;
- mistura varios cenarios;
- dificulta entender a causa da falha.

## Assert fraco

Sinais:

- uso excessivo de `Assert.IsTrue` sem mensagem clara;
- ordem invertida em `Assert.AreEqual`;
- falta de validacao exata do comportamento esperado.

## Unidade errada

Sinais:

- o formulario inteiro vira unidade de teste;
- a logica nao foi extraida quando poderia ter sido.