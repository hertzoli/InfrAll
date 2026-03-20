# Gerenciador de Sistemas

## Visao Geral
O Gerenciador de Sistemas e um aplicativo desktop para organizar informacoes operacionais de infraestrutura em uma arvore hierarquica. Ele centraliza servidores, VMs, containers, servicos, sistemas e ativos tecnicos com suas propriedades, observacoes, caminhos, links e comandos de uso recorrente.

O objetivo e reduzir dependencia de planilhas, blocos de notas e historicos de terminal espalhados. Cada item da arvore passa a concentrar o contexto tecnico necessario para consulta rapida e operacao do ambiente.

## Para Que Serve
O software e util para:

- mapear ambientes complexos por hierarquia
- registrar IPs, usuarios, caminhos, links e observacoes
- armazenar comandos prontos para execucao
- reutilizar valores com placeholders como `{Rede/MAC/IP}`
- copiar rapidamente referencias ou comandos resolvidos
- manter o cadastro salvo em `cadastro.yaml`

Exemplos de estrutura:

- `Infraestrutura > Hypervisor > VM Banco > MariaDB`
- `Ambiente Linux > Docker > Portainer > Traefik`
- `Clientes > Cliente A > VPN > Acesso SSH`

## Funcionalidades
- criacao de itens raiz e subitens
- duplicacao de item com propriedades e filhos
- remocao com confirmacao
- reorganizacao da arvore por drag-and-drop
- menu de contexto no `treeViewItens`
- propriedades dinamicas por item
- propriedades com valor e subpropriedades
- icones carregados da pasta `Imagens`
- importacao de novos icones pela interface
- expansao automatica da arvore e do painel de propriedades
- persistencia automatica em YAML
- interpolacao de placeholders no valor das propriedades
- copia do valor resolvido para a area de transferencia
- execucao direta de comandos, URLs, scripts e executaveis

## Como Usar
1. Abra o programa. Se existir um `cadastro.yaml` na pasta do executavel, o cadastro sera carregado automaticamente.
2. Clique em `Novo Item` para criar um item raiz ou selecione um item e use `Novo Sub-Item`.
3. Escolha um icone em `FormNovoItem` ou importe uma imagem para a pasta `Imagens`.
4. Selecione um item da arvore para editar suas propriedades no painel da direita.
5. Use `+ Item` para criar uma propriedade no nivel atual e `+ Sub-Prop` para criar uma propriedade filha.
6. Preencha `Nome`, `Valor`, `Descricao`, `Categoria` e `Local`, depois clique em `Salvar`.
7. Para reutilizar dados de outra propriedade do mesmo item, use placeholders como `ping {Rede/MAC/IP} -t`.
8. Use `Copy` para copiar o valor final resolvido ou `Run` para executar o comando.
9. Use `Duplicar`, `Delete`, arrastar com o mouse ou o menu de contexto para reorganizar a arvore.

## Execucao e Build
O binario pode ser executado a partir de `SourceCode/bin/Debug/` ou `SourceCode/bin/Release/`.

Para compilar:

```powershell
msbuild SourceCode\GerenciadorSistemas.sln /p:Configuration=Debug
msbuild SourceCode\GerenciadorSistemas.sln /p:Configuration=Release
```

## Estrutura do Projeto
```text
/
|-- SourceCode/
|   |-- GerenciadorSistemas.sln
|   |-- Form1.cs
|   |-- FormNovoItem.cs
|   |-- PropertyGridRuntimeBuilder.cs
|   `-- Imagens/
|-- Documentacao/
|   `-- PRDs/
`-- README.md
```

## Tecnologias
- C#
- Windows Forms
- .NET Framework 4.7.2
- YamlDotNet

## Autor
Hertz Oliveira Araujo

- LinkedIn: https://www.linkedin.com/in/hertz-oliveira-5aa66230/
- GitHub: https://github.com/hertzoli
