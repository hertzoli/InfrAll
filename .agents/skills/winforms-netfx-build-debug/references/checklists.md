# Checklist

Antes de concluir diagnostico de build:

## Causa raiz

- Confirmar que a causa raiz foi separada de erros em cascata.
- Confirmar que o primeiro erro util foi analisado com contexto.

## Estrutura legado

- Confirmar que `Reference`, `ProjectReference`, `HintPath` e `packages.config` foram considerados quando aplicavel.
- Confirmar que nao houve sugestao de migracao fora do stack sem pedido explicito.

## Validacao

- Confirmar uso de `MSBuild.exe "[Projeto.csproj]"` quando a tarefa exigia build.
- Confirmar que a orientacao final aponta para correcao compativel com WinForms e .NET Framework 4.6 e 4.7.2.