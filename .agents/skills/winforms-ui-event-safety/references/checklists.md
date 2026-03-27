# Checklist

Antes de concluir alteracoes em eventos ou operacoes de UI:

## Estabilidade

- Confirmar que handlers de evento usam `try-catch`.
- Confirmar que nenhuma excecao relevante escapa do evento.
- Confirmar que falhas relevantes sao logadas quando `Logger` existir.

## Responsividade

- Confirmar que nao ha `.Result` ou `.Wait()` em fluxo de UI.
- Confirmar que a operacao demorada foi separada da coordenacao visual.
- Confirmar que o evento nao ficou inchado ou com responsabilidade ambigua.

## Evolucao futura

- Confirmar que o caminho para cancelamento futuro esta simples.
- Confirmar se deve perguntar ao usuario sobre feedback de progresso.
- Confirmar que os riscos de corrida, reentrancia e sincronizacao incorreta foram considerados.