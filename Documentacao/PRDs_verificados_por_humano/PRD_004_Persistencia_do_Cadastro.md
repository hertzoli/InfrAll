# PRD - Product Requirements Document

## Funcionalidade: Persistência de itens, propriedades e comandos

### Descrição:
Implementar armazenamento e carregamento do cadastro completo da aplicação, incluindo árvore de itens do`treeViewItens` e suas propriedades em `propertyGridItem`. os dados devem ser salvo em aquivo YAML usando a biblioteca YamlDotNet do aplicativo.

### Justificativa:
O produto proposto no README depende de manter um inventário configurado pelo usuário. No estado atual, todos os dados  são perdidos ao fechar a aplicação.

### Histórias de Usuário:
Como um usuário, eu quero reabrir o sistema e continuar com minha estrutura já cadastrada.
Como um usuário, eu quero transportar a configuração junto com o binário portable.
Como um usuário, eu quero reduzir risco de perder informações operacionais importantes.

### Requisitos Funcionais:
1. Salvar automaticamente ou sob ação explícita o cadastro completo em arquivo local.
2. Carregar o arquivo de dados ao iniciar a aplicação.
3. Persistir hierarquia de itens, propriedades e comandos.
4. Tratar ausência do arquivo inicializando uma base vazia.
5. Tratar arquivo inválido exibindo erro amigável e evitando quebra da aplicação.
6. Permitir evolução futura do formato de dados.

### Requisitos Não Funcionais:
- O formato deve ser YAML usando YamlDotNet.
- A gravação deve ser confiável para uso em aplicação portable.
- O carregamento inicial deve ser rápido para volumes pequenos e médios.

### Critérios de Aceitação:
- Após cadastrar itens e fechar a aplicação, os dados permanecem disponíveis na próxima abertura.
- Um arquivo inexistente não impede a abertura do sistema.
- Um arquivo corrompido gera aviso ao usuário sem travar a interface.
- Itens e propriedades mantêm sua associação correta após recarga.

### Fluxos de Usuário:
1. Usuário cadastra ou altera itens e propriedades.
2. O sistema salva os dados em arquivo local.
3. Ao abrir o aplicativo novamente, a estrutura é restaurada automaticamente.

### Tarefas Pendentes:
- [ ] Definir contrato serializável para o modelo de dados.
- [ ] Escolher local do arquivo de persistência no contexto portable.
- [ ] Implementar carregamento no startup do formulário.
- [ ] Implementar salvamento com tratamento de exceção.
- [ ] Prever versionamento simples do schema de dados.
