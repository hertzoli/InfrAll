# Checklist

Antes de concluir implementacao ou refatoracao:

## Compatibilidade

- Confirmar compatibilidade com .NET Framework 4.6 e 4.7.2 e C# 7.3.
- Confirmar UTF-8 com CRLF nos arquivos gerados ou editados.

## Estrutura

- Confirmar que o formulario ficou apenas com coordenacao de UI, estado visual e eventos.
- Confirmar que logica reutilizavel foi movida para classe independente.
- Confirmar que nao foi criado helper generico sem responsabilidade clara.

## Robustez

- Confirmar que handlers de evento usam `try-catch` quando aplicavel.
- Confirmar que nao houve edicao manual indevida em arquivos de designer.
- Confirmar que nomes e comentarios ficaram claros para manutencao futura.

## Fluxo do projeto

- Confirmar que o build foi tentado com `MSBuild.exe "[Projeto.csproj]"` quando aplicavel.
- Confirmar que o PRD foi atualizado movendo tarefas implementadas para `### Tarefas Finalizadas aguardando validacao Humana:` quando existir PRD.