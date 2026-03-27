---
name: winforms-win32-pinvoke
description: "Implementa e revisa integracao Win32 por P/Invoke em aplicacoes Windows Forms e em bibliotecas auxiliares no .NET Framework 4.6 e 4.7.2 com C# 7.3 quando API nativa do Windows fizer mais sentido que codigo gerenciado puro. Use quando for necessario declarar DllImport, mapear tipos corretamente no stack legado, lidar com IntPtr, handles, liberacao de recursos, marshalling de string, chamadas seguras a API do Windows, encapsular interop fora do formulario e proteger a UI contra uso incorreto de handle, thread e recursos nativos."
---

# Win32 PInvoke for WinForms

Usar P/Invoke com foco em seguranca de assinatura, ownership claro de recurso e integracao limpa com UI WinForms legado.

## Workflow

1. Confirmar que a API gerenciada nao resolve o problema com clareza suficiente.
2. Ler a assinatura nativa e mapear tipos com cuidado para .NET Framework 4.6 e 4.7.2.
3. Preferir encapsular interop em classe propria, fora do formulario.
4. Definir ownership de memoria, handle e string antes de chamar a API.
5. Usar `DllImport` no stack legado e validar convencao de chamada quando relevante.
6. Proteger liberacao de recurso e erro nativo com padrao previsivel.
7. Integrar o resultado na UI sem espalhar detalhes nativos pelo formulario.

## Decision Rules

- Se a API gerenciada resolver o caso com seguranca e clareza: nao introduzir P/Invoke.
- Se a chamada envolver handle nativo: considerar encapsulamento forte e liberacao explicita.
- Se a API nativa retornar string, buffer ou ponteiro: definir ownership antes de codificar.
- Se o fluxo tocar UI: manter a chamada nativa fora do handler e deixar o evento apenas coordenar.
- Se houver risco de bloqueio ou latencia: preparar a integracao para nao travar a UI.

## Guardrails

- Nao espalhar `DllImport` diretamente em formularios.
- Nao mapear tipo nativo por suposicao.
- Nao usar `IntPtr` como desculpa para ignorar ownership ou liberacao.
- Nao deixar handle ou memoria nativa sem ciclo de vida definido.
- Nao misturar interop e logica visual no mesmo bloco.

## Load References As Needed

- Ler `references/type-mapping.md` para mapeamento de tipos comuns no stack Win32 legado.
- Ler `references/resource-lifetime.md` para ownership, handles e liberacao.
- Ler `references/ui-integration.md` para integrar chamada nativa ao fluxo WinForms sem acoplamento indevido.
- Ler `references/checklists.md` antes de concluir implementacao ou revisao de P/Invoke.