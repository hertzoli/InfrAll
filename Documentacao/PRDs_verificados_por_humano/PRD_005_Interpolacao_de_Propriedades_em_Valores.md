# PRD - Product Requirements Document

## Funcionalidade: Interpolação de propriedades em valores textuais

### Descrição:
Permitir que o campo `Valor` de uma propriedade aceite placeholders que referenciem outras propriedades do mesmo item, para gerar strings dinâmicas em tempo de uso. O caso inicial é copiar o valor resolvido para o clipboard ao acionar `buttonCopy`.

### Justificativa:
O produto serve como centralizador operacional. Muitos comandos, URLs e scripts repetem dados já cadastrados em outras propriedades, como IP, porta, usuário e caminhos. Sem interpolação, o usuário precisa duplicar informação e manter múltiplos campos manualmente sincronizados.

### Exemplo:
Se o item possui a propriedade `Rede > MAC > IP` com valor `192.168.0.1`, e outra propriedade `Comando Ping` tem valor:

`ping {Rede/MAC/IP} -t`

o sistema deve resolver o texto para:

`ping 192.168.0.1 -t`

Ao clicar em `buttonCopy`, o valor resolvido deve ser enviado ao clipboard.
Ao clicar em `buttonRun`, o valor deve ser excutado como comando do sistema.

### Histórias de Usuário:
Como um usuário, eu quero referenciar outras propriedades no campo `Valor`, para montar comandos dinâmicos.
Como um usuário, eu quero copiar o texto já resolvido para o clipboard, para executar o comando em outro programa.
Como um usuário, eu quero executar o texto já resolvido direto como comando do sistema.
Como um usuário, eu quero saber quando uma referência é inválida, para corrigir o cadastro.

### Requisitos Funcionais:
1. O sistema deve reconhecer placeholders no formato `{Caminho/Propriedade}`.
2. O caminho deve mapear a hierarquia lógica da propriedade dentro do item selecionado.
3. A resolução deve buscar propriedades apenas no item atual.
4. O sistema deve suportar múltiplos placeholders no mesmo valor.
5. O sistema deve preservar texto literal fora dos placeholders.
6. O `buttonCopy` deve copiar para o clipboard o valor já interpolado.
7. Se uma referência não existir, o sistema deve impedir a cópia e informar qual placeholder falhou.
8. O sistema deve suportar referências a propriedades com e sem subpropriedades.
9. O sistema deve manter o valor bruto salvo, sem substituir permanentemente o placeholder no cadastro.
10. ao selecionar uma propriedade o `textBoxReferenciaPropriedade` deve exibir o caminho de referencia da propriedade no formato `{Caminho/Propriedade}`
11. O `buttonRun` deve executar o valor como comando do sistema já interpolado.
12. O `buttonCopyPlaceholder` deve copiar para o clipboard o placeholder.

### Regras de Negócio:
1. O separador lógico da referência será `/` no placeholder, por exemplo `{Rede/MAC/IP}`.
2. A resolução de `{A/B/C}` deve apontar para a propriedade `C` no caminho `A/B`.
3. Placeholders são avaliados somente no momento de copiar ou executar a ação que consome o valor.
4. O valor persistido no arquivo YAML deve continuar armazenando o template original.

### Requisitos Não Funcionais:
- A resolução deve ser rápida para uso interativo.
- Mensagens de erro devem ser claras e em português.
- O parser deve ser tolerante a espaços externos, mas estrito na sintaxe interna do placeholder.

### Critérios de Aceitação:
- Um valor como `ping {Rede/MAC/IP} -t` gera `ping 192.168.0.1 -t` ao copiar.
- Um valor com múltiplas referências substitui todas corretamente.
- Um placeholder inválido exibe mensagem amigável e não copia texto incorreto.
- O valor original com placeholders permanece salvo após fechar e reabrir o sistema.

### Fluxos de Usuário:
1. Usuário cadastra propriedades base, como `Rede > MAC > IP`.
2. Usuário cria outra propriedade com placeholders no campo `Valor`.
3. Usuário seleciona a propriedade e aciona `buttonCopy`.
4. O sistema resolve as referências e envia o texto final ao clipboard.

### Dependências:
- Existência do `buttonCopy` na interface.
- Estrutura de propriedades navegável por caminho lógico.
- Persistência do valor bruto da propriedade no cadastro YAML.

### Tarefas Pendentes:
- [ ] Definir parser para placeholders `{///}`.
- [ ] Criar resolvedor de caminho lógico para propriedades do item atual.
- [ ] Implementar validação e mensagem de erro para referência ausente.
- [ ] Implementar ação de cópia com `Clipboard.SetText`.
- [ ] Garantir que o valor persistido continue sendo o template original.
