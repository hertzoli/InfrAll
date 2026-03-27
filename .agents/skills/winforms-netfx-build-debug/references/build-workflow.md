# Build Workflow

Aplicar este fluxo ao investigar falhas de build em stack legado.

## Passos

1. Localizar o projeto correto.
2. Compilar com `MSBuild.exe "[Projeto.csproj]"`.
3. Ler o primeiro erro util e seu contexto imediato.
4. Verificar se o erro e raiz ou efeito de falha anterior.
5. Abrir o `.csproj` e os arquivos relacionados ao erro.
6. Confirmar se o problema e de codigo, referencia, pacote, import ou configuracao.

## Sinal de cascata

- varios projetos falham com o mesmo sintoma;
- projeto dependente nao chegou a compilar corretamente;
- erro atual desaparece quando a referencia raiz e corrigida.

## Sinal de problema estrutural

- `HintPath` invalido;
- import ausente;
- pacote esperado nao esta restaurado;
- configuracao de plataforma ou versao divergente entre projetos.