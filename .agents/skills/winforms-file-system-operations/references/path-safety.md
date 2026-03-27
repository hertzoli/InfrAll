# Path Safety

Tratar caminho como dado nao confiavel quando vier do usuario, configuracao externa ou integracao.

## Regras

- Normalizar caminho antes de comparar ou acessar.
- Preferir APIs de caminho em vez de concatenacao manual de strings.
- Verificar se o caminho resultante permanece dentro da raiz esperada quando houver restricao de pasta.
- Validar existencia, extensao esperada e formato quando isso fizer parte do contrato.

## Path traversal

Sinais:

- uso de `..`;
- caminho absoluto quando apenas relativo era esperado;
- tentativa de escapar da pasta base;
- entrada externa usada diretamente em operacao de arquivo.

Acao:

- normalizar;
- comparar com a raiz permitida;
- rejeitar quando escapar da fronteira definida.