# References and Packages

## Referencias de assembly

Em projeto legado, verificar:

- nome correto da referencia;
- `HintPath` valido quando houver DLL externa;
- copia local quando fizer sentido;
- consistencia entre projetos da solucao.

## ProjectReference

Verificar:

- caminho correto do projeto referenciado;
- se o projeto dependente realmente compila;
- se a ordem de falha indica cascata.

## packages.config

Quando existir:

- conferir se o pacote esperado esta listado;
- conferir se a pasta de pacotes ou caminho resolvido existe;
- nao presumir `PackageReference` em stack legado.

## Regra pratica

Se o projeto e legado, considerar seriamente referencias classicas antes de concluir que a estrutura esta errada por ser "antiga".