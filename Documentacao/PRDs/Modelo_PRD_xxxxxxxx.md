# PRD - Product Requirements Document

## Funcioalidade: Permitir Cancelar Envio de Teclas

### Descrição:
[Descrição do PRD. Exemplo:]
Implementar um sistema de cores para o richTextBoxLog para facilitar a distinção visual entre datas, ações e tempos. O log deve utilizar diferentes cores para partes específicas de cada linha registrada.

### Justificativa:
[justificativa ou motifação para criar essa funcionalidade. Exemplo:]
Melhorar a legibilidade e a experiência do usuário, permitindo identificar rapidamente eventos importantes (como início e parada) e os valores de tempo no histórico.

### Histórias de Usuário:
[Uma lista de histórias de usuário no formato 'Como um [tipo de usuário], eu quero [ação], para que [benefício]'. Exemplo:]
Como um usuário, eu quero que o log use cores diferentes para datas, ações e tempos, para que eu possa analisar o histórico de forma mais rápida e intuitiva.



### Requisitos Funcionais:
[Exemplo:]
O método de adição ao log deve aplicar as seguintes cores:
1. **Azul Claro (LightBlue):** Data e Hora do sistema (dd/MM/yyyy HH:mm:ss.fff).
2. **Vermelho Claro (LightCoral/Red):** O texto "Iniciou a Contagem".
3. **Verde Claro (LightGreen):** O texto "Parou a contagem em ->" e "Pausa na contagem em ->".
4. **Amarelo (Yellow):** Intervalos de tempo (Total, Intervalo, e tempos formatados como hhh:mm:ss.fff).
5. **Branco (White):** Todos os demais textos (incluindo "retomada da contagem...", "Zerou o Contador", "TimePoint (LAP):", "Descrição:").

**Nota Visual:** Para que o texto branco seja visível, o BackColor do richTextBoxLog deve ser alterado para uma cor Black.


### Requisitos Não Funcionais:
[Exemplo:]
- Utilizar as propriedades SelectionColor e AppendText do RichTextBox para evitar a manipulação direta de RTF bruto, mantendo o código limpo.
- O código deve garantir que a cor de seleção seja restaurada ou gerenciada corretamente para a próxima entrada.
- Performance: Minimizar o número de chamadas de UI ao formatar a linha.

### Critérios de Aceitação: 
[Para cada funcionalidade ou história de usuário, defina os critérios que devem ser atendidos para que a funcionalidade seja considerada completa. Exemplo:]
- Ao registrar uma volta, a data aparece em azul claro, o tempo em amarelo e as labels em branco.
- Ao iniciar, o texto de ação aparece em vermelho claro.
- Ao parar/pausar, o texto de ação aparece em verde claro.
- O fundo do richTextBoxLog deve ser escuro o suficiente para o texto branco ser legível.


### Fluxos de Usuário:
[Descrição dos passos que o usuário fará para interagir com a nova funcionalidade. exemplo:]
1. Usuário interage com o cronômetro.
2. O log é preenchido automaticamente.
3. O usuário visualiza as cores correspondentes a cada tipo de dado sem precisar ler palavra por palavra.

### Tarefas Pendentes:
- [ ] Configurar o BackColor do richTextBoxLog para a cor Black no construtor do Form1.
- [ ] Implementar o método auxiliar AppendTextWithColor(string texto, Color cor) para facilitar a formatação.
- [ ] Refatorar o método AdicionarMensagemLog (do PRD anterior) para decompor a mensagem e aplicar as cores conforme as regras.
- [ ] Implementar a detecção de padrões (regex ou busca de strings) para identificar onde aplicar o amarelo nos tempos dentro de uma linha de log.
- [ ] Garantir que o tratamento de erro LogErroDetalhado capture falhas na manipulação do controle RichTextBox.
- [ ] implemtar para o jogador 1 usar as teclas [W, S] para controlar a barra
- [ ] implemtar para o jogador 2 usar as teclas [seta para cima, seta para baixo] para controlar a barra