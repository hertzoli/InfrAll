# PRD - Product Requirements Document

## Funcionalidade: Destacar campos de texto alterados e ainda nao salvos

### Descricao:
Implementar um feedback visual no painel de edicao de propriedades para indicar quando existem alteracoes locais ainda nao persistidas. Sempre que o usuario modificar algum valor dos campos de texto editaveis e ainda nao clicar em `Salvar`, o fundo do respectivo campo deve permanecer amarelo, facilitando a identificacao visual de informacoes pendentes de gravacao.

### Justificativa:
O fluxo atual permite alterar os campos e navegar pela interface sem um indicativo claro de que houve modificacao local ainda nao salva. Isso aumenta o risco de perda de alteracoes e dificulta perceber rapidamente o que foi editado. O destaque em amarelo reduz esse risco e melhora a legibilidade do estado atual da edicao.

### Historias de Usuario:
Como um usuario, eu quero ver imediatamente quais campos foram alterados e ainda nao salvos para nao perder modificacoes por engano.
Como um usuario, eu quero que o destaque visual permaneca enquanto a alteracao estiver pendente para eu identificar rapidamente o que ainda falta salvar.
Como um usuario, eu quero que o destaque desapareca apos salvar com sucesso para saber que o valor ja foi persistido.

### Requisitos Funcionais:
1. O sistema deve monitorar alteracoes nos campos `textBoxNome`, `textBoxValor`, `textBoxDescricao`, `textBoxCategoria` e `textBoxLocal`.
2. O requisito acima se refere ao campo de descricao exibido na interface e atualmente representado pelo controle `textBoxDescricao/textBoxDescrição`.
3. Ao detectar alteracao manual do usuario em qualquer um desses campos, o fundo do campo alterado deve mudar para amarelo.
4. O destaque amarelo deve permanecer enquanto o valor atual do campo for diferente do valor carregado por ultimo da propriedade selecionada ou do ultimo estado salvo com sucesso.
5. Ao clicar em `Salvar` e a operacao for concluida com sucesso, os campos destacados devem retornar para a cor padrao do formulario.
6. Ao selecionar outra propriedade no `propertyGridItem`, os campos devem ser recarregados sem herdar o destaque amarelo da selecao anterior.
7. Ao limpar o formulario por troca de item, ausencia de selecao ou criacao de novo contexto de edicao, os campos devem voltar para a cor padrao.
8. O campo `textBoxReferenciaPropriedade` nao deve participar dessa regra, pois e somente leitura.
9. Alteracoes realizadas apenas por carregamento programatico da interface nao devem marcar os campos como pendentes de salvamento.
10. Se o usuario alterar um campo e depois restaurar manualmente o valor original carregado, o destaque desse campo deve ser removido.
11. O comportamento deve funcionar tanto para propriedades existentes quanto para novas propriedades ainda nao salvas.

### Regras de Negocio:
1. O estado "nao salvo" deve refletir diferenca entre o valor exibido no controle e o ultimo snapshot considerado persistido pela tela.
2. O destaque visual e por campo, nao apenas global do formulario.
3. A cor amarela indica pendencia de salvamento, nao erro de validacao.
4. Validacoes que bloqueiam o `Salvar` nao devem remover o amarelo, pois a alteracao continua pendente.
5. O sistema nao deve depender exclusivamente do foco do controle para manter ou remover o destaque.

### Requisitos Nao Funcionais:
- A atualizacao de cor deve ocorrer em tempo real, sem perceptivel atraso durante digitacao.
- A implementacao deve permanecer compativel com Windows Forms e .NET Framework 4.6.
- O codigo deve evitar falso positivo durante preenchimento programatico dos campos ao selecionar itens ou propriedades.
- A cor escolhida deve ter contraste suficiente para manter a leitura do texto.

### Criterios de Aceitacao:
- Ao selecionar uma propriedade existente e alterar o valor de `textBoxValor` sem salvar, o fundo de `textBoxValor` fica amarelo.
- Ao alterar `textBoxNome` e `textBoxCategoria`, apenas os campos modificados ficam amarelos.
- Ao restaurar manualmente o mesmo valor que estava carregado originalmente, o campo correspondente volta para a cor padrao.
- Ao clicar em `Salvar` com sucesso, todos os campos salvos deixam de ficar amarelos.
- Ao tentar salvar e a operacao ser bloqueada por validacao, os campos alterados permanecem amarelos.
- Ao selecionar outra propriedade no `propertyGridItem`, os campos da nova propriedade aparecem com cor padrao ate que o usuario altere algo.
- Ao clicar em `Nova propriedade` e preencher campos antes de salvar, os campos editados ficam amarelos enquanto houver alteracoes pendentes.

### Fluxos de Usuario:
1. Usuario seleciona uma propriedade existente no `propertyGridItem`.
2. O sistema carrega os valores nos campos laterais com cor padrao.
3. Usuario altera o conteudo de um ou mais campos de texto.
4. O sistema destaca em amarelo cada campo cujo valor divergir do ultimo estado salvo/carregado.
5. Usuario clica em `Salvar`.
6. O sistema persiste a propriedade e remove o destaque amarelo dos campos salvos.

1. Usuario seleciona uma propriedade existente.
2. Usuario altera um campo, que passa a ficar amarelo.
3. Usuario desfaz manualmente a alteracao, retornando ao valor original.
4. O sistema remove o destaque do campo, pois nao ha mais diferenca pendente.

1. Usuario inicia a criacao de uma nova propriedade.
2. Usuario preenche `Nome`, `Valor`, `Descricao`, `Categoria` e `Local`.
3. Cada campo editado fica amarelo enquanto a nova propriedade ainda nao tiver sido salva.
4. Apos salvar com sucesso, os campos retornam para a cor padrao.

### Dependencias:
- Fluxo de selecao de propriedade em `propertyGridItem_SelectedGridItemChanged`.
- Fluxo de salvamento em `buttonSalvar_Click`.
- Fluxo de limpeza de tela em `LimparCamposEdicao`.
- Definicao centralizada do estado original/carregado dos campos editaveis.

### Tarefas Pendentes:
- [ ] Mapear os campos de texto editaveis que participam do controle de alteracao pendente.
- [ ] Criar uma estrutura para armazenar o ultimo snapshot carregado ou salvo dos campos.
- [ ] Diferenciar alteracoes programaticas de alteracoes feitas pelo usuario para evitar marcacao indevida.
- [ ] Implementar rotina central para atualizar a cor de fundo de cada campo com base na comparacao entre valor atual e snapshot.
- [ ] Integrar a limpeza do estado visual aos fluxos de selecao, criacao, limpeza e salvamento.
- [ ] Definir e padronizar a cor amarela e a cor padrao usada no reset visual.
- [ ] Validar manualmente o comportamento para propriedade existente, nova propriedade, erro de validacao e troca de selecao.
