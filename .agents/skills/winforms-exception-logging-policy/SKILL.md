---
name: winforms-exception-logging-policy
description: "Padroniza tratamento de excecoes, logging e mensagens de erro em aplicacoes Windows Forms e em camadas de apoio no .NET Framework 4.6 e 4.7.2 com C# 7.3. Use quando for necessario envolver handlers e operacoes sensiveis em try-catch, verificar se o namespace Logger existe, registrar falhas com Logger.LogWriter.LogErroDetalhado(...), preservar stack trace, diferenciar erros de UI, I/O, rede e integracao, definir mensagens seguras para o usuario e aplicar fallback seguro sem esconder contexto tecnico relevante. Tambem use em bibliotecas, servicos e integracoes do mesmo ecossistema quando os mesmos padroes de tratamento se aplicarem."
---

# WinForms Exception Logging Policy

Padronizar tratamento de falhas sem perder contexto tecnico, sem expor detalhes sensiveis ao usuario e sem deixar excecoes relevantes escaparem de fluxos de UI.

## Workflow

1. Identificar onde a excecao pode ocorrer e qual impacto ela tem na UI, no dado e no fluxo do usuario.
2. Definir se o ponto precisa de tratamento local ou se a excecao deve subir para uma camada superior controlada.
3. Em handlers de evento e pontos de integracao, envolver o fluxo com `try-catch`.
4. Registrar a excecao com contexto suficiente, usando `Logger.LogWriter.LogErroDetalhado(...)` quando o namespace `Logger` existir.
5. Preservar stack trace; nao relancar com perda de contexto.
6. Exibir ao usuario apenas mensagem adequada ao dominio e ao risco de exposicao.
7. Aplicar fallback seguro quando houver opcao previsivel e consistente.

## Decision Rules

- Se o codigo estiver em handler de evento WinForms: tratar localmente para impedir excecao nao tratada na UI.
- Se a falha puder ser enriquecida com contexto util naquele ponto: registrar ali.
- Se a camada atual nao conseguir decidir recuperacao ou mensagem: relancar preservando stack trace.
- Se a operacao mexer com arquivo, rede, banco, API ou processo externo: sempre adicionar contexto operacional ao log.
- Se a mensagem for para o usuario: nunca expor caminho interno, stack trace, segredo, credencial ou detalhe tecnico desnecessario.

## Guardrails

- Nao usar `throw ex;`.
- Nao engolir excecao silenciosamente.
- Nao registrar erro sem contexto minimo de operacao.
- Nao exibir detalhe tecnico bruto ao usuario final.
- Nao mascarar falhas graves com fallback inseguro ou inconsistente.

## Load References As Needed

- Ler `references/logging-policy.md` para regras de log, relancamento e uso de `Logger`.
- Ler `references/error-classification.md` para diferenciar UI, I/O, rede e integracao.
- Ler `references/user-messages.md` para orientar mensagens seguras ao usuario.
- Ler `references/checklists.md` antes de concluir implementacao ou revisao de tratamento de erro.