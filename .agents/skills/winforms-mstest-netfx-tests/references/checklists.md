# Checklist

Antes de concluir testes ou revisao de cobertura:

## Escopo

- Confirmar que a unidade testada nao depende desnecessariamente da UI visual.
- Confirmar que validacoes, regras e servicos receberam prioridade.

## Clareza

- Confirmar que o nome do teste descreve o comportamento esperado.
- Confirmar que Arrange, Act e Assert estao legiveis.
- Confirmar que asserts genericos foram evitados quando havia opcao mais clara.

## Compatibilidade

- Confirmar que a abordagem escolhida e compativel com MSTest e .NET Framework 4.6 e 4.7.2 do projeto.
- Confirmar que nenhuma recomendacao depende de recurso moderno inexistente no ambiente real.