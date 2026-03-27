# Checklist

Antes de concluir tratamento de erro ou revisao:

## Tratamento

- Confirmar que handlers e pontos sensiveis relevantes estao protegidos por `try-catch`.
- Confirmar que nao existe `throw ex;`.
- Confirmar que nenhuma excecao relevante foi engolida silenciosamente.

## Logging

- Confirmar que o log inclui contexto operacional suficiente.
- Confirmar uso de `Logger.LogWriter.LogErroDetalhado(...)` quando `Logger` existir.
- Confirmar que dados sensiveis nao foram expostos no log sem necessidade.

## Usuario e fallback

- Confirmar que a mensagem ao usuario esta clara e segura.
- Confirmar que fallback, se existir, e previsivel e nao mascara falha grave.
- Confirmar que o fluxo continua em estado seguro apos a falha.