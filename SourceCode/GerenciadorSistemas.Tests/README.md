# Verificações de regressão da pesquisa

Este projeto executável verifica a lógica de pesquisa/filtros sem dependências externas e faz smoke tests dos formulários por reflexão. Não exige MSTest ou restauração de pacotes. Código de saída 0 indica sucesso; falhas retornam 1.

Execute da raiz do repositório, usando o MSBuild do Visual Studio:

```powershell
msbuild SourceCode\GerenciadorSistemas.csproj /p:Configuration=Debug /p:PostBuildEvent= /p:OutputPath=bin/ValidacaoPesquisa/
msbuild SourceCode\GerenciadorSistemas.Tests\GerenciadorSistemas.Tests.csproj /p:Configuration=Debug /p:PostBuildEvent=
& .\SourceCode\GerenciadorSistemas.Tests\bin\Debug\PesquisaRegressionTests.exe "$PWD\SourceCode\bin\ValidacaoPesquisa\InfrAll.exe"
```

O argumento do executável é opcional: sem ele, executar somente verificações da lógica independente. A pasta de saída alternativa permite compilar enquanto o executável em `bin/Debug` está aberto.

A medição de 10 mil itens cobre busca e avaliação de filtros em memória; não mede snapshot da árvore, carregamento de ícones ou renderização de linhas. Navegação na árvore, confirmação de dados não salvos, responsividade visual e regressão do cadastro precisam de validação manual na aplicação.
