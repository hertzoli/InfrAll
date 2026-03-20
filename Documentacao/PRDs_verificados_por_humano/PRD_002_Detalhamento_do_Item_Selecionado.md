# PRD - Product Requirements Document

## Funcionalidade: Exibir e editar dados do item selecionado

### Descrição:
Implementar o vínculo entre o item selecionado no `treeViewItens` e os dados exibidos no `propertyGridItem` e no painel lateral de edição (`Nome`, `Valor`, `Descrição`, `Categoria`, `Local`).

### Justificativa:
Hoje o formulário carrega apenas propriedades de exemplo fixas no construtor. O `PropertyGrid` não representa um item real da árvore e o botão `Salvar` apenas adiciona propriedades a um objeto global, sem contexto por item. Isso não atende o cenário descrito no README, em que cada item deve ter seus próprios metadados técnicos.

### Histórias de Usuário:
Como um usuário, eu quero selecionar um item da árvore e ver somente as informações dele.
Como um usuário, eu quero editar os dados de um item para manter IP, usuário, links e observações atualizados.
Como um usuário, eu quero remover uma propriedade de um item sem afetar outros itens.

### Requisitos Funcionais:
1. Cada item da árvore deve possuir seu próprio conjunto de propriedades.
2. Ao selecionar um item no `TreeView`, o `PropertyGrid` deve ser atualizado com os dados daquele item.
3. O painel lateral deve refletir a propriedade atualmente selecionada no `PropertyGrid`.
4. O botão `Salvar` deve criar ou atualizar uma propriedade no item selecionado.
5. O botão `Del` da área de propriedades deve remover apenas a propriedade do item selecionado.
6. O sistema deve suportar campos comuns como IP, usuário, sistema operacional, link web, caminhos e observações.

### Requisitos Não Funcionais:
- A troca de seleção entre itens deve atualizar a interface imediatamente.
- O código deve evitar estado global compartilhado entre itens sem necessidade.
- Os nomes exibidos ao usuário devem permanecer em português.

### Critérios de Aceitação:
- Selecionar itens diferentes mostra conjuntos de propriedades diferentes.
- Alterar uma propriedade de um item não altera dados de outro item.
- Criar uma propriedade com o item selecionado faz a propriedade aparecer imediatamente no `PropertyGrid`.
- Excluir uma propriedade atualiza a interface sem precisar reiniciar a aplicação.

### Fluxos de Usuário:
1. Usuário seleciona um item na árvore.
2. O sistema carrega as propriedades daquele item no `PropertyGrid`.
3. Usuário seleciona uma propriedade existente ou preenche os campos laterais.
4. Clica em `Salvar` para incluir ou alterar o dado.

### Tarefas Pendentes:
- [ ] Substituir o `_builder` único por dados associados a cada item.
- [ ] Implementar evento `AfterSelect` no `treeViewItens`.
- [ ] Definir estrutura para propriedades técnicas por item.
- [ ] Corrigir o fluxo de edição para diferenciar inclusão e atualização.
- [ ] Revisar o botão `buttonNovaPropriedade`, que hoje insere dados de exemplo.
