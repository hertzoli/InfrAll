---
name: winforms-mstest-netfx-tests
description: "Escreve, revisa e adapta testes MSTest para logica extraida de formularios Windows Forms e para classes reutilizaveis em projetos .NET Framework 4.6 e 4.7.2 com C# 7.3. Use quando for necessario testar classes independentes, validacoes, regras de negocio, transformacoes de dados e servicos sem acoplar o teste a UI visual, orientar o que e viavel em MSTest no stack legado, revisar anti-patterns de testes e estruturar testes claros, manuteniveis e compativeis com projetos antigos."
---

# WinForms MSTest NetFx Tests

Priorizar testes sobre logica extraida da UI, mantendo o escopo compativel com stack legado e evitando acoplamento desnecessario ao formulario visual.

## Workflow

1. Identificar qual logica pode ser testada sem depender de controles visuais.
2. Priorizar classes independentes, validacoes, regras de negocio, transformacoes e servicos.
3. Estruturar testes com Arrange, Act e Assert.
4. Usar MSTest compativel com o projeto existente, sem assumir recursos modernos indisponiveis.
5. Cobrir comportamento observavel, nao detalhes internos irrelevantes.
6. Manter testes pequenos, legiveis e com nomes descritivos.
7. Evitar testar o Designer ou detalhes de layout WinForms.

## Decision Rules

- Se a logica puder receber entrada e retornar resultado sem UI: testar diretamente.
- Se a regra estiver misturada ao formulario: sugerir extracao antes de ampliar a cobertura de teste.
- Se o teste depender de controle visual, thread de UI ou comportamento do Designer: reduzir escopo ou redesenhar a unidade testavel.
- Se o projeto usar MSTest legado: adaptar recomendacoes ao que estiver realmente disponivel.
- Se o valor do teste estiver em regra de negocio e validacao: preferir esse caminho ao teste de evento visual.

## Guardrails

- Nao acoplar teste a posicionamento, layout ou Designer.
- Nao usar o formulario visual como unidade primaria de teste quando a logica puder ser extraida.
- Nao assumir APIs modernas de MSTest sem conferir compatibilidade do stack.
- Nao escrever teste confuso, inchado ou com assert generico quando houver assert mais claro.
- Nao trocar clareza por cobertura artificial.

## Load References As Needed

- Ler `references/test-scope.md` para decidir o que testar no contexto WinForms legado.
- Ler `references/mstest-patterns.md` para padroes compativeis com MSTest em .NET Framework.
- Ler `references/anti-patterns.md` para erros comuns ao testar logica extraida de formularios.
- Ler `references/checklists.md` antes de concluir implementacao ou revisao de testes.