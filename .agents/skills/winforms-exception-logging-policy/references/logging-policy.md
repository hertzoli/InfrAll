# Logging Policy

Aplicar estas regras ao tratar excecoes em WinForms legado.

## Regra principal

- Se o namespace `Logger` existir no projeto atual, registrar a falha no `catch` com `Logger.LogWriter.LogErroDetalhado(exErro, "contexto adicional")`.
- Se `Logger` nao existir, ainda assim estruturar o `catch` para facilitar a insercao futura de logging.

## Contexto minimo do log

Sempre incluir, quando houver acesso a essa informacao:

- operacao que falhou;
- origem da chamada ou evento;
- identificador relevante do registro, arquivo ou entidade;
- impacto esperado ou etapa do fluxo;
- parametro sensivel somente se puder ser descrito sem expor segredo.

## Relancamento

- Usar `throw;` quando precisar propagar a excecao sem perder stack trace.
- Nao usar `throw ex;`.
- Se criar nova excecao, manter a original em `InnerException` e adicionar contexto real, nao ruido.

## Quando tratar localmente

- handlers de evento WinForms;
- bordas de integracao;
- pontos em que existe fallback claro e seguro;
- pontos em que a mensagem ao usuario depende do contexto atual.

## Quando evitar tratamento local

- quando o bloco apenas logaria e reempacotaria sem agregar contexto;
- quando a camada superior e o local correto para decidir recuperacao;
- quando tratar ali esconderia falha que precisa abortar o fluxo.