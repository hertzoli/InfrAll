---
name: winforms-file-system-operations
description: "Executa e revisa operacoes de arquivo, diretorio e sistema em aplicacoes Windows Forms e em classes de apoio no .NET Framework 4.6 e 4.7.2 com C# 7.3, com foco em desktop Windows corporativo. Use quando for necessario lidar com caminhos, permissao, encoding UTF-8, locking, validacao de entrada, protecao contra path traversal, leitura e escrita robusta, manipulacao segura de arquivos em uso, acesso a diretorios, integracao com APIs do Windows e separacao entre logica de sistema e coordenacao de UI."
---

# WinForms File and System Operations

Implementar operacoes de desktop Windows com seguranca, robustez e comportamento previsivel em ambiente legado.

## Workflow

1. Identificar se a operacao envolve caminho, arquivo, diretorio, permissao, encoding, lock ou API do Windows.
2. Validar entrada e normalizar caminhos antes de acessar o sistema.
3. Separar logica de arquivo e sistema da coordenacao visual do formulario.
4. Escolher API nativa do .NET Framework ou Win32 quando houver ganho real de seguranca, compatibilidade ou controle.
5. Tratar erros de acesso, compartilhamento, inexistencia, lock e permissao com contexto claro.
6. Preservar UTF-8 e comportamento consistente de leitura e escrita.
7. Garantir que recursos sejam liberados corretamente mesmo em falha.

## Decision Rules

- Se a operacao puder ser implementada com API nativa do .NET Framework com clareza suficiente: preferir essa opcao.
- Se houver necessidade de comportamento especifico do Windows, controle fino de handle ou interoperabilidade: avaliar API Win32.
- Se o caminho vier de entrada do usuario ou fonte externa: validar e normalizar antes de usar.
- Se a logica nao depender de controles: extrair para classe independente.
- Se houver risco de arquivo grande, lock ou latencia relevante: preparar a operacao para integracao segura com fluxo assincrono da UI.

## Guardrails

- Nao confiar em caminho recebido sem validacao.
- Nao concatenar caminhos manualmente quando houver API apropriada.
- Nao assumir permissao ou disponibilidade do arquivo.
- Nao esquecer de liberar `Stream`, handle ou recurso equivalente.
- Nao misturar acesso a sistema com exibicao visual no mesmo bloco de logica.

## Load References As Needed

- Ler `references/path-safety.md` para validacao, normalizacao e protecao contra traversal.
- Ler `references/io-patterns.md` para leitura, escrita, locking, encoding e liberacao de recursos.
- Ler `references/windows-apis.md` quando houver ganho real em usar API do Windows.
- Ler `references/checklists.md` antes de concluir implementacao ou revisao.