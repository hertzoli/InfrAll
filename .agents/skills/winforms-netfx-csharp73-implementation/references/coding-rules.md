# Coding Rules

Aplicar estas regras como base ao implementar ou refatorar:

## Compatibilidade

- Usar Windows Forms no .NET Framework 4.6 e 4.7.2 com C# 7.3.
- Gerar ou editar arquivos em UTF-8 com CRLF.
- Usar identacao de 4 espacos.
- Evitar recursos de linguagem ou API fora do stack definido.

## Estrutura

- Manter no formulario apenas coordenacao de UI, estado visual e eventos.
- Extrair logica reutilizavel para classes independentes em arquivos proprios.
- Criar arquivos pequenos e coesos.
- Evitar helpers genericos sem responsabilidade clara.
- Unificar metodos semelhantes em uma logica central unica.

## Qualidade

- Usar nomes descritivos para variaveis, metodos, classes e controles.
- Escrever comentarios claros e significativos para desenvolvedores juniores.
- Em condicoes `if`, fazer comparacoes explicitas.
- Otimizar CPU e memoria sem sacrificar legibilidade, seguranca ou manutencao.

## Seguranca e dependencias

- Validar entradas e tratar com cuidado I/O, processos externos e dados sensiveis.
- Preferir bibliotecas nativas e API do Windows.
- Nao introduzir dependencias externas sem necessidade clara e justificativa.
- Nao alterar `*.Designer.cs`, `*.resx` ou `Properties/*.Designer.cs` sem pedido explicito.

## Dominio

- Preservar vocabulario de dominio em portugues nos nomes visiveis ao usuario e nos membros ja consolidados.