---
name: winforms-netfx-build-debug
description: "Analisa build, compilacao e diagnostico de solucoes .NET Framework 4.6 e 4.7.2 com foco principal em Windows Forms, Visual Studio e bibliotecas de apoio do mesmo ecossistema, usando MSBuild.exe e convencoes de stack legado. Use quando for necessario compilar com MSBuild.exe \"[Projeto.csproj]\", interpretar erros comuns de compilacao, diagnosticar problemas de referencias, assembly references, packages.config, imports, configuracoes de solucao, inconsistencias de projeto legado e orientar investigacao de falhas de build sem sugerir migracoes inadequadas para stack moderna."
---

# NetFx MSBuild Visual Studio Build Debug

Diagnosticar build e compilacao do stack legado com foco em causa raiz real, nao em modernizacao indevida do projeto.

## Workflow

1. Identificar o projeto ou solucao afetada e localizar o `.csproj` relevante.
2. Compilar com `MSBuild.exe "[Projeto.csproj]"` quando a tarefa exigir validacao de build.
3. Ler os erros reais antes de propor mudanca estrutural.
4. Classificar a falha em codigo, referencia, pacote, importacao, configuracao Visual Studio ou cascata de dependencia.
5. Verificar `.csproj`, `packages.config`, assembly references e arquivos importados quando aplicavel.
6. Distinguir erro raiz de falhas em cascata.
7. Propor correcoes compativeis com .NET Framework 4.6 e 4.7.2, WinForms e Visual Studio.

## Decision Rules

- Se o erro vier de compilacao C#: localizar primeiro o arquivo e a linha raiz.
- Se o erro vier de referencia ausente ou tipo nao encontrado: verificar `Reference`, `HintPath`, `ProjectReference` e `packages.config`.
- Se o erro vier de importacao MSBuild: verificar caminho, existencia e compatibilidade do import.
- Se varios projetos falharem: diferenciar projeto raiz do restante da cascata.
- Se a correcao sugerir SDK-style, .NET moderno ou ferramenta fora do stack sem pedido explicito: nao seguir por esse caminho.

## Guardrails

- Nao assumir que erro de build e sempre problema do arquivo que apareceu por ultimo.
- Nao tratar falha em cascata como causa raiz.
- Nao propor migracao de framework, SDK-style ou NuGet moderno sem solicitacao explicita.
- Nao ignorar `packages.config`, assembly references ou `HintPath` em projeto legado.
- Nao trocar `MSBuild.exe` por outro comando quando a regra do projeto exigir esse fluxo.

## Load References As Needed

- Ler `references/build-workflow.md` para rotina de compilacao e triagem.
- Ler `references/common-errors.md` para erros frequentes de .NET Framework e WinForms legado.
- Ler `references/references-and-packages.md` para diagnostico de `Reference`, `HintPath` e `packages.config`.
- Ler `references/checklists.md` antes de concluir diagnostico ou orientar correcao.