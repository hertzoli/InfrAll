---
name: winforms-netfx-service-extraction
description: "Extrai logica de formularios WinForms para classes independentes em projetos .NET Framework 4.6 e 4.7.2 com C# 7.3 e orienta refatoracao incremental de codigo legado sem quebrar build, comportamento ou UI. Use quando for necessario decidir o que permanece no formulario, o que deve virar servico ou metodo puro, como separar regras de negocio de eventos e controles, como eliminar duplicacao entre formularios, como dividir arquivos por responsabilidade e como reorganizar arquitetura WinForms com risco baixo. Tambem use ao revisar classes reutilizaveis fora da UI quando a mesma disciplina de extracao e coesao se aplicar."
---

# Service Extraction and Architecture for WinForms Legacy

Reduzir acoplamento de formularios sem refatoracao brusca e sem mover codigo estavel de uma vez so.

## Workflow

1. Localizar o caso de uso solicitado e os formularios afetados.
2. Classificar o codigo em tres grupos: UI pura, logica pura e codigo misto.
3. Extrair primeiro a parte pura do codigo misto, preservando comportamento.
4. Criar classe independente apenas quando houver entrada e saida explicitas.
5. Deixar no formulario somente coordenacao de UI, estado visual e eventos.
6. Refatorar em etapas pequenas e recompilar entre etapas relevantes.
7. Atualizar PRD quando a tarefa fizer parte de um documento de requisitos.

## Decision Rules

- Se precisa de `this`, controles, bindings, estado visual ou eventos WinForms: manter no formulario em arquivo parcial separado quando necessario.
- Se pode receber parametros e retornar resultado sem depender de UI: extrair para classe independente em arquivo proprio.
- Se mistura UI e regra: separar primeiro a regra, depois reduzir o handler a orquestracao.
- Se a logica puder ser compartilhada por mais de um formulario: extrair.
- Se a extracao gerar uma classe generica demais ou sem responsabilidade clara: parar e redesenhar a fronteira.

## Guardrails

- Nao mover blocos grandes de codigo estavel sem necessidade.
- Nao duplicar logica entre formularios.
- Nao criar arquivo utilitario generico sem responsabilidade clara.
- Nao criar classe gigante; separar por caso de uso quando crescer demais.
- Preservar vocabulario de dominio existente nos nomes visiveis ao usuario e nos membros ja consolidados.
- Priorizar classes pequenas, coesas e reutilizaveis.

## Execution Pattern

Aplicar este padrao ao refatorar:

1. Isolar entradas e saidas do trecho atual.
2. Extrair validacoes, transformacoes e regras de negocio.
3. Manter leitura de controles, exibicao de mensagens e alteracao visual no formulario.
4. Adaptar o formulario para chamar a nova classe.
5. Verificar se houve reducao real de acoplamento e duplicacao.

## Load References As Needed

- Ler `references/refactor-steps.md` quando precisar conduzir refatoracao incremental em etapas pequenas.
- Ler `references/examples.md` quando houver duvida sobre o que fica no formulario e o que vai para classe independente.
- Ler `references/checklists.md` antes de finalizar ou revisar uma extracao.