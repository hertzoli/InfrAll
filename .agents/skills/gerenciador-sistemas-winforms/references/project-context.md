# Contexto do projeto

## Resumo

- Aplicativo desktop WinForms em C#.
- Projeto principal em `SourceCode/`.
- Framework atual no `.csproj`: `.NET Framework 4.7.2`.
- Persistência principal em YAML.
- Arquivo mais sensível para crescimento de complexidade: `SourceCode/Form1.cs`.

## Arquivos centrais

- `SourceCode/Program.cs`: bootstrap da aplicação.
- `SourceCode/Form1.cs`: tela principal, eventos, árvore, edição e persistência.
- `SourceCode/Form1.Designer.cs`: layout da tela principal.
- `SourceCode/FormNovoItem.cs`: formulário auxiliar de criação.
- `SourceCode/PropertyGridRuntimeBuilder.cs`: estrutura dinâmica das propriedades.
- `SourceCode/OnlineAutoUpdateUltra.cs`: atualização online.
- `SourceCode/SimpleLogger.cs`: logging simples.

## Restrições práticas

- O projeto usa `.csproj` antigo; novos arquivos `.cs` não entram automaticamente.
- Evitar alterações manuais no Designer.
- A validação disponível hoje é principalmente manual pela interface.
- Há uma pasta `SourceCode/WindowsFormsApp1/` que parece paralela ou experimental; não mexer nela sem necessidade explícita.

## Comandos úteis

```powershell
msbuild SourceCode\GerenciadorSistemas.sln /p:Configuration=Debug
msbuild SourceCode\GerenciadorSistemas.sln /p:Configuration=Release
```

## Fluxos manuais relevantes

- Abrir a aplicação sem erro.
- Criar item e subitem.
- Adicionar, editar e remover propriedade.
- Selecionar item e sincronizar `PropertyGrid` e campos de edição.
- Salvar e recarregar o cadastro persistido.
- Executar comando e copiar valor/placeholders quando a alteração tocar esses fluxos.
