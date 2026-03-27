# Windows APIs

Usar API do Windows apenas quando houver vantagem real sobre a API gerenciada disponivel.

## Bons motivos

- necessidade de controle fino de atributo, handle ou erro do sistema;
- interoperabilidade com componente nativo ja existente;
- comportamento especifico do Windows nao exposto claramente pela API gerenciada.

## Cuidados

- preferir encapsular a chamada nativa em classe propria;
- manter assinatura e liberacao de recurso corretas;
- nao espalhar P/Invoke pelo formulario;
- documentar por que a API nativa foi escolhida.

## Regra pratica

Se a API gerenciada nativa do .NET Framework resolver o problema com clareza, seguranca e desempenho suficiente, nao introduzir Win32 sem necessidade.