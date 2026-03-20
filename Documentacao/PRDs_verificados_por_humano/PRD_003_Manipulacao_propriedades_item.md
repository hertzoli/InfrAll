# PRD - Product Requirements Document

## Funcionalidade: Manipulação de propriedades do item selecionado

### Descrição:
Implementar o gerenciamento completo das propriedades técnicas de cada item selecionado no `treeViewItens`, permitindo criar, editar, remover e organizar propriedades e subpropriedades exibidas no `propertyGridItem`.

### Justificativa:
O valor do produto está em centralizar informações operacionais de servidores, VMs, containers e serviços. Para isso, cada item precisa possuir seu próprio conjunto de propriedades customizáveis, como IP, usuário, porta, link web, caminhos de arquivos, observações e credenciais de referência. No estado atual, o formulário manipula um conjunto global de propriedades de exemplo, sem contexto por item e sem fluxo claro de edição.

### Histórias de Usuário:
Como um usuário, eu quero adicionar uma propriedade a um item selecionado para registrar uma informação técnica relevante.
Como um usuário, eu quero organizar propriedades em níveis hierárquicos para agrupar dados como `Sistema/Rede` ou `Sistema/Hardware`.
Como um usuário, eu quero editar uma propriedade existente para manter os dados atualizados.
Como um usuário, eu quero excluir uma propriedade de um item sem afetar os demais itens cadastrados.
Como um usuário, eu quero selecionar uma propriedade no `PropertyGrid` e ver seus dados preenchidos no painel lateral para edição rápida.

### Requisitos Funcionais:
1. O sistema deve permitir adicionar uma propriedade ao item atualmente selecionado no `treeViewItens`.
2. Cada propriedade deve possuir ao menos os campos `Nome`, `Valor`, `Descrição`, `Categoria` e `Local`.
3. O campo `Local` deve permitir organizar propriedades em subníveis, por exemplo `Sistema/Rede`.
4. O sistema deve impedir inclusão de propriedade quando nenhum item estiver selecionado.
5. O sistema deve impedir inclusão ou atualização de propriedade com `Nome` vazio.
6. O botão `Salvar` deve criar uma nova propriedade quando ela ainda não existir no item.
7. O botão `Salvar` deve atualizar a propriedade existente quando ela já existir no mesmo item e no mesmo caminho.
8. O botão `Del` deve remover apenas a propriedade do item selecionado.
9. Selecionar uma propriedade no `propertyGridItem` deve preencher os campos laterais com os dados correspondentes.
10. Após criar, editar ou excluir uma propriedade, o `propertyGridItem` deve ser atualizado imediatamente.
11. Propriedades de um item não devem aparecer em outro item.
12. O sistema deve permitir subpropriedades por meio do caminho informado em `Local`, sem exigir criação manual prévia de cada agrupador.

### Regras de Negócio:
1. O contexto de edição sempre é o item selecionado no `treeViewItens`.
2. A identidade de uma propriedade deve considerar ao menos `Nome` + `Local` dentro do item.
3. Dois itens diferentes podem possuir propriedades com o mesmo nome e mesmo caminho.
4. Remover uma propriedade não deve remover o item da árvore.
5. Alterar uma propriedade não deve afetar comandos ou propriedades de outros itens.

### Requisitos Não Funcionais:
- A atualização do `PropertyGrid` deve ocorrer sem travamentos perceptíveis.
- O código deve separar o modelo de dados do item da lógica visual do formulário.
- A implementação deve continuar compatível com Windows Forms e .NET Framework 4.6.
- Mensagens de erro e validação devem ser claras e em português.

### Critérios de Aceitação:
- Com um item selecionado, ao informar `Nome`, `Valor`, `Descrição`, `Categoria` e `Local` e clicar em `Salvar`, a propriedade aparece no `PropertyGrid`.
- Ao selecionar a propriedade exibida no `PropertyGrid`, os campos laterais são preenchidos com os valores corretos.
- Ao alterar um campo e clicar em `Salvar`, a propriedade é atualizada no mesmo item.
- Ao clicar em `Del`, a propriedade selecionada deixa de aparecer no `PropertyGrid`.
- Ao mudar de item no `TreeView`, o conjunto de propriedades exibido muda para refletir apenas o item selecionado.
- Ao tentar salvar sem item selecionado ou sem nome da propriedade, o sistema bloqueia a ação e informa o motivo.

### Fluxos de Usuário:
1. Usuário pressionao o botão `buttonNovaPropriedade`.
2. o sistema lê o Local da propriedade selecionada em `propertyGridItem` e carrega seu valor `textBoxLocal.Text` para a propriedade criada ficar no mesmo nível na arvore que a propriedade selecionada está.
3. Usuário preenche os campos `Nome`, `Valor`, `Descrição`, `Categoria` e `Local` (`textBoxNome.Text`, `textBoxValor.Text`, `textBoxDescrição.Text`, `textBoxCategoria.Text`, `textBoxLocal.Text`).
4. Usuário clica em `Salvar`.
5. O sistema cria a propriedade no `propertyGridItem`.

1. Usuário pressionao o botão `buttonNovoSubPropriedade`.
2. o sistema lê o Local da propriedade selecionada em `propertyGridItem` e carrega seu valor `textBoxLocal.Text` adicionando o nome para a propriedade criada ficar no nível acima que a propriedade selecionada está.
3. Usuário preenche os campos `Nome`, `Valor`, `Descrição`, `Categoria` e `Local` (`textBoxNome.Text`, `textBoxValor.Text`, `textBoxDescrição.Text`, `textBoxCategoria.Text`, `textBoxLocal.Text`).
4. Usuário clica em `Salvar`.
5. O sistema cria a propriedade no `propertyGridItem`.

1. Usuário seleciona uma propriedade já existente no `propertyGridItem`.
2. O sistema carrega os dados da propriedade no painel lateral direito (`textBoxNome.Text`, `textBoxValor.Text`, `textBoxDescrição.Text`, `textBoxCategoria.Text`, `textBoxLocal.Text`) .
3. Usuário altera um ou mais campos.
4. Usuário clica em `Salvar`.
5. O sistema atualiza a propriedade e reflete a alteração na interface.

1. Usuário seleciona uma propriedade existente.
2. Usuário clica em `Del`.
3. O sistema remove a propriedade do item atual e atualiza o `propertyGridItem`.

### Dependências:
- selecionar um item em `treeViewItens` deve carregar suas propriedades em `propertyGridItem`.
- Estrutura de dados que associe cada item da árvore ao seu próprio conjunto de propriedades.

### Tarefas Pendentes:
- [ ] Associar um `PropertyGridRuntimeBuilder` ou estrutura equivalente a cada item da árvore.
- [ ] Implementar validação para item selecionado e nome obrigatório.
- [ ] Ajustar `buttonSalvar_Click` para criar ou atualizar propriedades no item corrente.
- [ ] Ajustar `buttonExcluirPropriedade_Click` para remover a propriedade do item corrente.
- [ ] Implementar carregamento do `propertyGridItem` ao trocar a seleção do `treeViewItens`.
- [ ] Remover os dados de exemplo fixos usados apenas para prototipação.
- [ ] Definir feedback visual para sucesso e falha nas operações.
