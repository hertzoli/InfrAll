# UI Integration

Integrar Win32 com WinForms sem contaminar a camada visual.

## Regras

- Formularios coordenam; interop fica em classe propria.
- Eventos de UI nao devem carregar detalhes de assinatura nativa.
- Se a chamada nativa puder falhar, o handler deve tratar excecao localmente.
- Se a chamada nativa puder demorar, avaliar integracao com fluxo assincrono ou isolamento adequado.

## Maus sinais

- `DllImport` declarado dentro do formulario;
- manipulacao direta de `IntPtr` espalhada em eventos;
- UI dependendo do detalhe de marshalling para funcionar.