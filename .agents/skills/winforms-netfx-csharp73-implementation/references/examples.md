# Examples

## Extrair para classe independente

Extrair quando o trecho:

- recebe dados por parametro;
- retorna resultado sem depender de controles;
- pode ser reutilizado por mais de um formulario;
- representa validacao, regra de negocio, transformacao de dados ou integracao.

Exemplos comuns:

- montar parametros para consulta;
- validar campos antes de salvar;
- transformar dados para exibicao ou exportacao;
- encapsular acesso a arquivo, banco, rede ou servico.

## Manter no formulario

Manter no formulario quando o trecho:

- le ou escreve propriedades de controles;
- altera estado visual;
- abre mensagens, dialogs ou confirmações;
- responde diretamente a evento da tela;
- coordena a chamada da logica e a exibicao do resultado.

## Caso misto

Se o codigo atual faz leitura da UI, aplica regra e mostra resultado:

1. separar a leitura dos controles;
2. extrair a regra para metodo ou classe independente;
3. retornar um resultado claro;
4. manter no formulario apenas a exibicao e a coordenacao.

## Sinal de fronteira ruim

Rever a extracao quando a nova classe:

- continua dependendo de controles;
- exige parametros demais sem coesao;
- vira um helper generico sem responsabilidade definida;
- mistura regra de negocio com detalhes visuais.