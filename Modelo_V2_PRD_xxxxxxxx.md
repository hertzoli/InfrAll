# PRD - Product Requirements Document
[Apenas como referencia, este é um exemplo de PRD que descreve a implementação definir cores distintas para a saída de log `richTextBoxLog` de um programa de cronometro]

## Funcioalidade: [Título]Sistema de cores no log de saída


### Descrição:
[Descrição geral do PRD. Exemplo:]
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
[Para cada funcionalidade e ou história de usuário, defina os critérios que devem ser atendidos para que a funcionalidade seja considerada completa. Exemplo:]
- Ao registrar uma volta, no RichTextBox a informação de "data" deve estar na cor em azul claro, o "tempo" em amarelo e as "labels" em branco.
- Ao iniciar a contagem do cronometro, o texto da ação "Iniciou a Contagem" deve aparecer no RichTextBox em vermelho claro.
- Ao parar/pausar, o texto de ação "Pausa na contagem em -> " deve aparecer no RichTextBox em verde claro.
- O fundo do richTextBoxLog deve ser escuro o suficiente para o texto branco ser legível.


### Fluxos de Usuário:
[Descrição dos passos que o usuário fará para interagir com a nova funcionalidade. exemplo:]
1. Usuário interage com o cronômetro pressionando os botões Iniciar/Volta/Pausar/Zerar.
2. para cada ação do usuário é registrado no RichTextBox o log com as cores pre definidas para cada ação e ou tipo de dados.
3. O usuário visualiza as cores correspondentes a cada tipo de dado sem precisar ler palavra por palavra.

### Tarefas:
[Lista com a descrição das tarefas que precisam ser executadas para a correta implementação deste PRD. exemplo:]
- [ ] Configurar o BackColor do richTextBoxLog para a cor Black no construtor do Form1.
- [ ] Implementar o método auxiliar AppendTextWithColor(string texto, Color cor) para facilitar a formatação.
- [ ] Refatorar o método AdicionarMensagemLog (do PRD anterior) para decompor a mensagem e aplicar as cores conforme as regras.
- [ ] Implementar a detecção de padrões (regex ou busca de strings) para identificar onde aplicar o amarelo nos tempos dentro de uma linha de log.
- [ ] Garantir que o tratamento de erro LogErroDetalhado capture falhas na manipulação do controle RichTextBox.

