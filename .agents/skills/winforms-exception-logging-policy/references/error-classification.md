# Error Classification

Classificar a falha antes de decidir log, mensagem e fallback.

## UI

Sinais:

- acesso invalido a controle;
- estado visual inconsistente;
- erro disparado por evento de formulario.

Acao:

- tratar localmente;
- restaurar estado visual seguro quando possivel;
- evitar que a excecao saia do handler.

## I/O

Sinais:

- arquivo inexistente;
- acesso negado;
- arquivo em uso;
- falha de leitura ou escrita.

Acao:

- registrar caminho e operacao sem expor dado sensivel ao usuario;
- orientar mensagem objetiva;
- considerar retry apenas quando fizer sentido.

## Rede

Sinais:

- timeout;
- indisponibilidade remota;
- erro DNS;
- conexao interrompida.

Acao:

- registrar endpoint, operacao e tempo quando possivel;
- diferenciar indisponibilidade temporaria de falha de contrato;
- evitar mensagem generica que pareca erro local irreversivel.

## Integracao

Sinais:

- resposta invalida de API;
- contrato inesperado;
- retorno inconsistente de componente externo;
- falha de banco, servico ou processo externo.

Acao:

- registrar payload ou identificadores de forma segura;
- preservar evidencias tecnicas para diagnostico;
- aplicar fallback apenas se houver comportamento conhecido e seguro.