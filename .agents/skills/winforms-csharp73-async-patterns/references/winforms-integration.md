# WinForms Integration

Integrar async com WinForms sem degradar previsibilidade da UI.

## Regras

- Separar leitura da UI, execucao da logica e atualizacao visual.
- Nao bloquear a thread principal enquanto aguarda I/O ou tarefa longa.
- Atualizar controles apenas em momento seguro do fluxo da UI.
- Reduzir handlers a coordenacao e mover trabalho reutilizavel para metodo ou classe independente.

## Cancelamento e progresso

- Deixar caminho simples para `CancellationToken` mesmo quando o cancelamento ainda nao foi solicitado.
- Se a espera puder ser relevante para o usuario, avaliar se deve perguntar sobre feedback de progresso.

## Riscos comuns

- clique repetido gerando reentrancia;
- estado visual sendo alterado fora de ordem;
- evento aguardando tarefa enquanto outro evento altera o mesmo estado;
- excecao em `async void` sem tratamento local.