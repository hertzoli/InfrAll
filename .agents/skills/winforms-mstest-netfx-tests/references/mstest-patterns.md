# MSTest Patterns

Aplicar apenas o que fizer sentido para o projeto real.

## Estrutura

- Usar `[TestClass]` e `[TestMethod]`.
- Nomear testes de forma descritiva.
- Seguir Arrange, Act e Assert.
- Preferir uma responsabilidade por teste.

## Assert

- Preferir `Assert.AreEqual(expected, actual)` com ordem correta.
- Preferir asserts especificos como `Assert.IsNull`, `Assert.IsNotNull` e validacao de excecao adequada quando disponivel.
- Se a versao do MSTest for antiga, adaptar a tecnica sem forcar API inexistente.

## Dados de teste

- Usar `DataRow` quando houver suporte e ganho real.
- Nao sofisticar a estrutura de dados do teste alem do necessario para o stack legado.

## Inspiracao aproveitada das skills baixadas

- manter classes e nomes de teste claros;
- preferir asserts especificos ao inves de `Assert.IsTrue` generico quando possivel;
- tratar setup e cleanup com simplicidade e previsibilidade.