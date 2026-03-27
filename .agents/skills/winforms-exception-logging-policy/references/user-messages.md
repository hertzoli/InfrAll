# User Messages

Mensagens para o usuario devem proteger detalhes internos e ainda orientar o proximo passo.

## Regras

- Usar linguagem clara e coerente com o dominio.
- Informar o que falhou em nivel funcional, nao tecnico.
- Dizer o que o usuario pode fazer a seguir quando houver acao util.
- Nao expor stack trace, caminho interno, SQL, payload bruto, token ou segredo.

## Bons padroes

- "Nao foi possivel salvar o cadastro neste momento. Tente novamente."
- "O arquivo selecionado nao pode ser lido porque esta em uso por outro programa."
- "Nao foi possivel concluir a comunicacao com o servico externo."

## Maus padroes

- mensagem vaga sem acao: "Erro inesperado";
- detalhe tecnico bruto: excecao completa em `MessageBox`;
- mensagem que culpa o usuario sem evidencia.