# PRD - Product Requirements Document

## Funcionalidade: Resolução global de placeholders por caminho completo da árvore

### Descrição:
Evoluir a funcionalidade de placeholders para que a resolução de referências não fique restrita ao item atualmente selecionado em `treeViewItens`. A nova implementação deve permitir que um placeholder aponte para propriedades de qualquer item da árvore, desde que utilize o caminho completo do item na árvore somado ao caminho completo da propriedade.

### Contexto atual:
Na implementação atual, o placeholder exibido em `textBoxReferenciaPropriedade` e copiado por `buttonCopyPlaceholder` é montado apenas com base no contexto da propriedade atual, no formato `{Local/Propriedade}`.

Exemplo atual:

`{Rede/MAC/IP}`

Esse formato funciona somente quando a resolução procura a propriedade dentro do `Builder` do item selecionado no momento da ação. Como consequência, placeholders não conseguem apontar para propriedades de outros itens da árvore.

### Objetivo:
Definir um padrão de referência absoluto que identifique de forma única qualquer propriedade cadastrada em qualquer item da árvore, permitindo interpolação global e consistente.

### Justificativa:
O sistema centraliza informações técnicas distribuídas em vários itens relacionados, como ambientes, servidores, containers, bancos, aplicações e links operacionais. Em cenários reais, um comando ou texto de um item frequentemente depende de propriedades armazenadas em outro item.

Sem uma referência absoluta:
- o usuário precisa duplicar valores em múltiplos itens;
- a manutenção fica mais sujeita a erro;
- a utilidade dos placeholders fica limitada a um contexto local.

### Histórias de Usuário:
Como um usuário, eu quero referenciar uma propriedade de qualquer item da árvore, para reutilizar dados sem duplicação.

Como um usuário, eu quero copiar o placeholder de uma propriedade já no formato absoluto, para poder colar essa referência em qualquer outro item.

Como um usuário, eu quero que a resolução do placeholder seja determinística, para evitar ambiguidades entre itens ou propriedades com nomes iguais.

### Escopo:
Esta funcionalidade cobre:
1. Novo formato de placeholder com caminho completo do item e da propriedade.
2. Ajuste da geração do placeholder mostrado em `textBoxReferenciaPropriedade`.
3. Ajuste da resolução de placeholders em ações como `buttonCopy` e `buttonRun`.
4. Busca de propriedades em toda a árvore de itens.

Esta funcionalidade não cobre:
1. Renomeação automática de placeholders já gravados quando um item da árvore for movido ou renomeado.
2. Alias, IDs imutáveis ou referências simbólicas.
3. Edição assistida, autocomplete ou navegador visual de placeholders.

### Formato da referência:
O placeholder deve passar a representar:

`{CaminhoCompletoDoItem/LocalDaPropriedade/NomeDaPropriedade}`

Exemplos:

- `{Ambiente 01/Servidor APP 01/Rede/MAC/IP}`
- `{Clientes/Empresa X/ERP Produção/Acesso/URL}`
- `{Datacenter/SP/Firewall B/Observacao/Local}`

### Requisitos Funcionais:
1. O sistema deve reconhecer placeholders no formato `{Item/.../Local/.../Propriedade}` usando um caminho absoluto da árvore.
2. O caminho absoluto deve incluir toda a hierarquia do item em `treeViewItens`, da raiz visual até o item dono da propriedade.
3. O caminho da propriedade deve incluir todo o `Local` da propriedade e, ao final, o `Nome` da propriedade.
4. O sistema deve conseguir resolver placeholders que apontem para propriedades do item atual.
5. O sistema deve conseguir resolver placeholders que apontem para propriedades de outros itens da árvore.
6. O `textBoxReferenciaPropriedade` deve exibir o placeholder absoluto da propriedade selecionada.
7. O `buttonCopyPlaceholder` deve copiar o placeholder absoluto para a área de transferência.
8. O `buttonCopy` deve interpolar placeholders absolutos antes de copiar o valor final.
9. O `buttonRun` deve interpolar placeholders absolutos antes de executar o valor final.
10. O sistema deve suportar múltiplos placeholders no mesmo texto.
11. O sistema deve preservar o texto literal fora dos placeholders.
12. O sistema deve manter salvo o valor bruto com placeholders, sem substituir permanentemente as referências.
13. Se o placeholder apontar para item ou propriedade inexistente, a ação deve falhar e informar qual referência não pôde ser resolvida.

### Regras de Negócio:
1. O separador oficial do caminho no placeholder deve ser `/`.
2. A resolução deve considerar o caminho completo do item como parte obrigatória da identidade da referência.
3. A resolução não deve depender do item atualmente selecionado para localizar a propriedade referenciada.
4. O placeholder deve ser tratado como absoluto por padrão.
5. A busca deve ser case-insensitive, mantendo o comportamento atual das propriedades.
6. Espaços externos ao conteúdo interno das chaves podem ser ignorados; a estrutura interna do caminho deve permanecer válida.
7. Se houver itens diferentes com mesmo nome sob pais diferentes, a referência continua válida porque o caminho completo desambigua o destino.
8. Se um item ou propriedade for renomeado e houver placeholders salvos apontando para o nome antigo, esses placeholders podem se tornar inválidos até atualização manual, salvo futura funcionalidade específica para refatoração de referências.

### Requisitos Não Funcionais:
- A resolução deve ocorrer com desempenho compatível com uso interativo.
- Mensagens de erro devem ser claras e em português.
- A mudança deve preservar compatibilidade com o cadastro atual, exceto pela semântica de novos placeholders absolutos.
- O comportamento deve ser previsível mesmo em árvores profundas.

### Compatibilidade e Migração:
1. O sistema deve definir explicitamente a estratégia para placeholders antigos no formato relativo.
2. A estratégia recomendada para implementação é:
- gerar apenas placeholders novos no formato absoluto;
- aceitar temporariamente placeholders antigos relativos apenas quando puderem ser resolvidos de forma inequívoca no item atual;
- tratar o formato absoluto como padrão oficial a partir desta funcionalidade.
3. Caso a equipe prefira simplificação da regra, também é aceitável descontinuar o formato antigo, desde que a mudança seja documentada e a interface informe isso com clareza.

### Impactos Esperados na Implementação:
1. A geração do placeholder não poderá mais depender apenas de `PropertyGridSelectionInfo`.
2. Será necessário obter também o caminho completo do item selecionado na árvore.
3. O resolvedor de placeholders não poderá mais consultar apenas `item.Builder`.
4. Será necessário percorrer ou indexar a árvore completa de itens para localizar o item de destino antes de resolver a propriedade dentro dele.
5. A composição da referência deverá unificar:
- caminho completo do item;
- `Local` da propriedade;
- `Nome` da propriedade.

### Critérios de Aceitação:
- Ao selecionar uma propriedade de um item filho da árvore, `textBoxReferenciaPropriedade` mostra o placeholder absoluto completo.
- Ao copiar esse placeholder e colá-lo no valor de uma propriedade de outro item, a interpolação resolve corretamente.
- Um comando como `ping {Ambiente 01/Servidor APP 01/Rede/MAC/IP} -t` gera o texto final com o IP correto ao acionar `buttonCopy`.
- O mesmo comando gera o texto final correto ao acionar `buttonRun`.
- Um placeholder que aponta para item inexistente exibe erro amigável e não executa a ação.
- Um placeholder que aponta para propriedade inexistente exibe erro amigável e não executa a ação.
- Referências com múltiplos placeholders absolutos no mesmo valor são resolvidas integralmente.

### Fluxos de Usuário:
1. Usuário seleciona uma propriedade no `propertyGridItem`.
2. O sistema monta e exibe o placeholder absoluto com base no caminho do item na árvore e no caminho da propriedade.
3. Usuário copia esse placeholder por `buttonCopyPlaceholder`.
4. Usuário cola a referência no campo `Valor` de uma propriedade em qualquer outro item.
5. Ao acionar `buttonCopy` ou `buttonRun`, o sistema localiza o item referenciado, depois localiza a propriedade dentro dele, resolve o texto e executa a ação.

### Dependências:
- Estrutura navegável de `treeViewItens`.
- Estrutura de propriedades navegável por caminho lógico dentro de cada `PropertyGridRuntimeBuilder`.
- Método confiável para montar o caminho completo de um `TreeNode`.
- Método confiável para localizar um item a partir de um caminho absoluto da árvore.

### Riscos:
- Quebra de compatibilidade com placeholders já cadastrados no formato relativo.
- Ambiguidade temporária se a aplicação tentar suportar simultaneamente formatos relativo e absoluto sem regra clara de precedência.
- Maior acoplamento entre a árvore visual de itens e o resolvedor de placeholders.

### Tarefas Pendentes:
- [ ] Definir formato final oficial do placeholder absoluto.
- [ ] Definir estratégia de compatibilidade para placeholders antigos.
- [ ] Criar método para obter o caminho completo do item selecionado em `treeViewItens`.
- [ ] Ajustar a montagem exibida em `textBoxReferenciaPropriedade`.
- [ ] Criar resolvedor global capaz de localizar o item alvo na árvore.
- [ ] Ajustar a resolução de placeholders usada por `buttonCopy`.
- [ ] Ajustar a resolução de placeholders usada por `buttonRun`.
- [ ] Implementar mensagens de erro separando falha por item inexistente e propriedade inexistente.
- [ ] Validar cenários com itens homônimos em ramos diferentes.
