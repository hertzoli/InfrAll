# PRD - Product Requirements Document

## Funcionalidade: Estrutura hierárquica de itens no TreeView

### Descrição:
Implementar o cadastro e a navegação de itens e subitens no `treeViewItens`, representando a estrutura real de ambientes, servidores, VMs, containers, serviços e outros ativos.
Cada item precisa ter por padrão as propriedades abaixo que deve ser exibidas no `propertyGridItem`:  
```  
Nome (Nome do item que é o mesmo nome exibido no nó);
Descrição Item: (Descrição);
Imagem/Icone: (Imagem do item que deve aparecer no nó);
Criado em: (data e hora da criação do item)
Observação/Local: (Observação ou localição física ou lógica do item);
```

### Justificativa:
O objetivo central descrito no `README.md` depende de uma árvore navegável. No código atual, o `TreeView` existe visualmente, mas não é populado, não possui seleção integrada e os botões `+ Item`, `+ Sub-Item` e `Del` ainda não entregam o fluxo principal do produto.

### Histórias de Usuário:
Como um usuário, eu quero cadastrar itens em estrutura de árvore para mapear meu ambiente técnico.
Como um usuário, eu quero criar subitens dentro de um item existente para representar dependências e camadas da infraestrutura.
Como um usuário, eu quero excluir itens da árvore para manter o cadastro atualizado.
Como um usuário, eu quero poder atribuir um ícone a um item.
Como um usuário, eu quero poder atribuir uma decrição a um item e essa descrição apareca no `propertyGridItem`;
Como um usuário, eu quero poder saber a data e hora de quando o item foi criado e ver essa informação no `propertyGridItem` ;
Como um usuário, eu quero poder atribuir uma observação a um item e ver essa observação no `propertyGridItem` ;

### Requisitos Funcionais:
1. Permitir criar um item raiz a partir do botão `buttonNovoItem`.
2. Permitir criar subitens dentro do item selecionado a partir do botão `buttonNovoSubItem`.
3. Permitir excluir o item selecionado com confirmação antes da remoção.
4. Exibir no `TreeView` o nome do item e sua posição hierárquica.
5. Manter a seleção do item recém-criado ou recém-editado.
6. Impedir criação de item sem nome.
7. Permitir tipos de item como servidor, VM, container, serviço ou item genérico.

### Requisitos Não Funcionais:
- A navegação no `TreeView` deve responder sem travamentos perceptíveis.
- A solução deve manter o código de UI desacoplado do modelo de dados dos itens.
- O comportamento deve ser compatível com Windows Forms e .NET Framework 4.6.

### Critérios de Aceitação:
- Ao clicar em `+ Item`, um novo nó raiz pode ser criado e aparece no `TreeView`.
- Ao selecionar um nó e clicar em `+ Sub-Item`, um filho é criado abaixo do nó selecionado.
- Ao excluir um nó, ele deixa de aparecer na árvore e seus filhos são removidos junto.
- Ao abrir o sistema com dados carregados, a hierarquia aparece corretamente.

### Fluxos de Usuário:
1. Usuário clica em `+ Item`.
2. Informa o nome e tipo do item.
3. O sistema cria o nó na árvore e o seleciona.
4. Usuário seleciona um nó existente e clica em `+ Sub-Item`.
5. O sistema cria um novo nó filho no contexto selecionado.

### Tarefas Pendentes:
- [ ] Criar modelo de domínio para item hierárquico.
- [ ] Vincular cada `TreeNode` ao item correspondente.
- [ ] Implementar eventos de criação, seleção e exclusão de nós.
- [ ] Adicionar validação de nome obrigatório.
- [ ] Definir ícones por tipo de item usando `imageList1`.
