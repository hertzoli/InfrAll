# I/O Patterns

## Leitura e escrita

- Escolher encoding explicitamente quando a operacao depender disso.
- Preservar UTF-8 como padrao quando aplicavel ao projeto.
- Usar `using` ou liberacao equivalente para `Stream`, `TextReader`, `TextWriter` e objetos relacionados.

## Locking e concorrencia

- Assumir que o arquivo pode estar em uso por outro processo.
- Diferenciar arquivo inexistente, acesso negado e violacao de compartilhamento.
- Evitar abrir recurso por tempo maior que o necessario.

## Robustez

- Escrever de forma atomica quando perda parcial puder corromper dado.
- Validar se a pasta existe ou precisa ser criada.
- Registrar contexto util quando operacao de I/O falhar.

## Integracao com UI

- Se a operacao puder ser lenta, separar do codigo visual.
- Se houver processamento pesado ou I/O relevante, deixar o desenho pronto para integracao com cancelamento ou progresso quando solicitado.