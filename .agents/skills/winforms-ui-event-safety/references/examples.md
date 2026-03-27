# Event Handler Template

Usar este modelo mental para manter handlers pequenos e previsiveis:

1. Ler dados da UI.
2. Validar entrada leve.
3. Chamar metodo de negocio ou servico.
4. Atualizar UI com o resultado.
5. Tratar excecoes e registrar log quando aplicavel.

## Bom sinal

- O handler coordena, mas nao concentra regra de negocio.
- A parte demorada esta fora do codigo visual.
- O erro esta contextualizado e tratado.
- A atualizacao da UI acontece no ponto final do fluxo.

## Mau sinal

- O handler cresce continuamente.
- Existe acesso espalhado a controles no meio da logica.
- O evento mistura validacao complexa, I/O e exibicao visual no mesmo bloco.
- O fluxo assincrono ficou dificil de ler ou sujeito a reentrancia.