

## General Instructions:
 - Escreva o código em C# compatível com Windows Forms do .net frameworks 4.6 e prefira bibliotecas nativas sempre que possível ou API do Windows; 
 - Escreva o código para ser compatível com linguagem C# versão 7.3;
 - Prefira que métodos semelhantes sejam unificados para evitar duplicação de código. A lógica central deve ser implementada em um único método, enquanto os métodos auxiliares apenas convertem os dados de entrada, quando necessário, e chamam o método principal. Isso simplifica a manutenção e garante a consistência no código.
 - O código deve ser otimizado para usar minimizar o uso dos recursos de memória e CPU (eficaz);
 - Precisa seguir as melhores práticas de segurança para evitar vulnerabilidades. Exemplo: Prever ameaças como injeção de código, ataques de negação de serviço, ou exposição de dados sensíveis;
 - Deve ser fácil de manter, corrigir erros, adicionar funcionalidades ou modificar o comportamento sem afetar outras partes do sistema;
 - use nomes bem descritivos para variáveis, métodos, classes, controles e etc;
 - Este código será usado por desenvolvedores juniores e precisa ser bem comentado, com comentários claros e significativos;
 - Classes devem ser implementadas tendo em mente que possam ser reutilizável no futuro e seus métodos devem ser bem documentados;
 - O código deve possuir tratamento de exceções muito robusto e de nível profissional;
 - Use Programação Assíncrona (async/await) sempre que possível;
 - Use Programação Task-Based sempre que possível;
 - caso exista operações que podem ser demoradas e um feedback para essas operações não foi solicitado de forma explicita. então pergunte se o código deve ter uma forma de feedback de progresso para essas operações;
 - caso exista operações que podem ser demoradas e uma forma de cancelamento para essa operação não foi solicitado de forma explicita. então essa parte do código deve ser implementada de forma que seja simples de implementar uma forma de cancelamento pelo usuário para as operações Task-Based, Assíncronas e ou outras quando solicitado. (exemplo: já prever no código um `CancelationToken` opcional, ou seja o método pode ou não ser chamado com o  `CancelationToken`);
 - o código Task-Based deve ser robusta e evitar os erros comuns da programação Task-Based como por exemplo: Race Conditions, Deadlocks, Starvation, Erros de Sincronização, Timing Errors e etc;
 - Sempre envolva o código dentro dos handlers de eventos (especialmente aqueles que interagem com UI ou outras operações que podem falhar) em blocos try-catch para evitar que exceções não tratadas se propaguem para a biblioteca ou framework que disparou o evento;
 - para registrar o erro no log, dentro do bloco catch de tratamento de exceções, sempre use o método `LogErroDetalhado()`, veja um exemplo do seu uso onde "exErro" é do tipo Exception: `Logger.LogWriter.LogErroDetalhado(exErro, "adicione aqui informações adicionais sobre a exceção");`
 - após finalizar alguma implementação de tarefa de um "PRD - Product Requirements Document" retire as tarefas que foram implementadas de `Tarefas Pendentes:` e coloque em `### Tarefas Finalizadas aguardando validação Humana:` só coloque alguma tarega em `### Tarefas Concluidas e verificadas por Humano:` se solicitado de forma explícita;
 - para compilar o projeto para fins de teste utilize o comando `MSBuild.exe "[nome_arquivo_Projeto.csproj]"` afim de evitar erros de caminho, no comando coloque o [nome_arquivo_Projeto.csproj] entre aspas;
 - informações sobre o projeto pode ser encontrada em "README.md"
 - o código fonte de projeto fica dentro da pasta "SourceCode"
 - evite fazer alterações na interface do formulário a não ser que explicitamente solicitado. ou seja. evitar adicionar, remover ou ajustar posições dos controles do formulário. 


## Coding Style:

- Use 4 spaces for indentation.
- Dentro de condições `if` sempre adicione os operadores para fazer uma **comparação explícita**.


## Regarding Dependencies:

- Avoid introducing new external dependencies unless absolutely necessary.
- If a new dependency is required, please state the reason.


## Planejamento:
Se solicitado a implementação de um código na qual não foi fornecido um bom planejamento da execução das tarefas. então separar a implementação em tarefas e suas subtarefas de forma bem planejada. Criando um "Nested Task List" em markdown utilizando **identação** no documento "PRD - Product Requirements Document" referente a funcionalidade.