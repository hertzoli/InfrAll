# Type Mapping

Mapear tipos Win32 com conservadorismo e clareza.

## Tipos comuns

- `BOOL` -> atencao ao tamanho e semantica.
- `DWORD` -> tipo inteiro sem sinal de 32 bits.
- `HANDLE`, `HWND` -> nao tratar como valor arbitrario sem ownership claro.
- `LPWSTR` -> string UTF-16 em API Windows ampla.
- `IntPtr` -> usar quando representar ponteiro ou handle, nao como tipo coringa para tudo.

## Regras

- Confiar na assinatura nativa, nao em memoria vaga do nome da funcao.
- Ser explicito com convencao de chamada quando necessario.
- Preferir assinaturas simples e encapsuladas para consumo pelo restante do codigo.

## Inspiracao aproveitada das skills baixadas

- mapear tipos perigosos com cautela;
- nao improvisar ownership de memoria;
- tratar assinatura incorreta como causa real de crash, corrupcao ou comportamento intermitente.