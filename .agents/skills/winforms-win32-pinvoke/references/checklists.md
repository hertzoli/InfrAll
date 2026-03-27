# Checklist

Antes de concluir P/Invoke em WinForms legado:

## Assinatura

- Confirmar que a assinatura gerenciada bate com a nativa.
- Confirmar que tipos, string e convencao de chamada foram avaliados.

## Recursos

- Confirmar ownership claro de memoria, buffer e handle.
- Confirmar liberacao correta mesmo em falha.
- Confirmar que `IntPtr` nao esta sendo usado sem contrato claro.

## Arquitetura

- Confirmar que interop foi encapsulado fora do formulario.
- Confirmar que a UI nao ficou acoplada ao detalhe nativo.
- Confirmar que o fluxo esta seguro para erro, latencia e uso de recurso.