# Common Errors

## CS0246 ou tipo nao encontrado

Verificar:

- `using` ausente;
- referencia de assembly ou projeto faltando;
- `HintPath` quebrado;
- pacote ausente em `packages.config`.

## MSB4019 ou import nao encontrado

Verificar:

- caminho do import;
- arquivo realmente existente;
- dependencia de ferramenta ou extensao do Visual Studio;
- import opcional sem protecao adequada.

## Erros de WinForms Designer e recursos

Verificar:

- nomes de classe e namespace alinhados;
- arquivos `.Designer.cs` e `.resx` correspondentes;
- `SubType` e inclusao correta no projeto legado.

## Conflitos de referencia

Verificar:

- duplicidade de assembly;
- versoes divergentes entre projetos;
- mistura entre referencia local, GAC e pacote.