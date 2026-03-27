# Checklist

Antes de concluir operacao de arquivo ou sistema:

## Seguranca

- Confirmar que caminhos externos foram validados e normalizados.
- Confirmar que risco de path traversal foi considerado.
- Confirmar que dados sensiveis nao foram expostos em log ou mensagem.

## Robustez

- Confirmar tratamento para inexistencia, permissao negada e recurso em uso.
- Confirmar liberacao correta de `Stream`, handle ou recurso equivalente.
- Confirmar que encoding foi definido quando relevante.

## Arquitetura

- Confirmar que a logica de arquivo ou sistema ficou fora da coordenacao visual.
- Confirmar que API do Windows foi usada apenas quando trouxe ganho real.
- Confirmar que o fluxo esta pronto para integrar async, cancelamento ou progresso quando fizer sentido.