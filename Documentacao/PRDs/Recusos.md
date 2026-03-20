### Implementar:

melhor Concorrente com vários recursos: https://devolutions.net/products/remote-desktop-manager-free/
outro concorrente: https://mobaxterm.mobatek.net/
outro concorrente: https://www.royalapps.com/ts/win/features
    ícones bons para copiar aqui.


Fazer o Infrall buscar arquivos de dados "cadastro.yaml" na internet (ftp). a ideia é permitir que ele funcione com várias pessoas usando ao mesmo tempo. talvez o ideal é implementar um banco de dados MariaDB.

Implementar atualização automática.

Implementar um sitema de pesquisa para pesquisar dentro das propriedades de todos os itens.

Permitir renomear um item apenas pressionando [F2] e escrevendo o nome e dando enter. 
Permitir pesquisar e baixar templates Online na pela internet.
criar uma forma de simplificar a adição de comandos de ferramentas externas. exemplo: ao adicionar um comando, listar as ferramentas compatíveis e os comandos padrão. se a ferramenta não existir no computador o programa baixa ela e já deixa ela disponível em uma pasta tools. Exemplo: suponha que quer executar um teste de velocidade de rede. então o programa mostra a opção do executar o "iperf" onde o usuário pode apontar o caminho ou baixar automaticamente o "iperf"; 

Fazer o botão buttonRun e `buttonCopy` só ficar acessíveis (Enable) se a propriedade selecionada for uma sub-propriedade da propriedade com o nome "Comandos" ou "Commands"
 
Talvez uma boa prática seria adicionar um botão para entrar no modo de edição de uma propriedade. enquanto não tiver no modo edição, os controles textbox das propriedades ficam bloqueadas para edição mas permite cópia.
 
 Criar um método bem seguro de deixar as senha salvas localmente:
 
Criar um repositório online onde usuário podem publicar templates e ou comandos que podem ser baixados por outros usuários. 

no `propertyGridItem` Exibir o ícone do item em vez do nome do arquivo do icone. 

Implementar multi-linguagem

 
##### Baixa Prioridade:
em relação ao `treeViewItens` implementar os quatro itens abaixo:
 7. poder exportar um item para um arquivo. (opção exibida no menu de contexto)
 6. poder importar um item a partir de um arquivo.  (opção exibida no menu de contexto)
 7. Adicionar uma opção de criar "Nova Pasta" que vai criar um item já com o ícone de Pasta pré-selecionado.
 8. Adicioanr uma opção para expandir (+) e retrair (-) todos os nós.

ajuste para que no `propertyGridItem` os nomes das Propriedades que possui outras sub-propriedades sejam exibidas em Negrito enquanto seus valores sejam exibidos sem negrito; para isso é necessário trocar o controle de propriedades.


### Feito:

v1.1
Adiciona a versão no título do formulário:  this.Text += "  v" + Application.ProductVersion;

v1.0
 1. pressionar a tecla "delete" com algum item do `treeViewItens` selecionado. deve chamar o evento `buttonExcluirItem_Click`
 3. permitir mover um item do `treeViewItens` arrastando com o mouse
 4. permitir duplicar um item de `treeViewItens` com todas as sua propriedades existentes ao pressionar `buttonDuplicar`.
 5. Criar Menu de contexto para `treeViewItens` com os comandos; Novo Item, Novo Sub-Item, Remover, Duplicar.
 
 
 Ajustar o formulário que cria os itens
 
 
 é possivel ajustar para poder atribuir valores a propriedade que tem sub-propriedades?
    por exemplo na implementação atual a propriedade `MAC` precisa ter uma subpropriedade `MAC` para receber o valor do "MAC Address":
        que faz a estrutura das propriedades ficar assim:
            Sistema >
                Rede >
                    MAC:
                        MAC: AA:BB:CC:11:22:33
                        IP: 192.168.0.1
        o ideal seria ficar assim (uma unica propriedade `MAC` pode ter valor ser propriedade "mãe" de outras propriedades.):
            Sistema >
                Rede >
                    MAC: AA:BB:CC:11:22:33 >
                        IP: 192.168.0.1
                        
 1. permitir deletar "propriedades" de `propertyGridItem` que na verdade são sub-propriedade da arvore.


Implementar a função do botão `buttonRun` e do `buttonCopy`

quero poder colocar dentro de `textBoxValor` (Valor de uma propriedade) uma string com referencia a campos de outroas propriedades. 
Exemplo: suponha que tenho um item com a propriedade: Rede>MAC>IP que ser valor é "192.168.0.1"
então se eu colocar criar uma propriedade com o nome de "Commando Ping" com o `textBoxValor` ou (Valor de uma propriedade) similar a "ping {Rede.MAC.IP} -t" vai gerar a string: "ping 192.168.0.1 -t" no clipboard ao clicar em `buttonCopy`.
implemente um PRD para essa funcionalidade.


atualise o Readme.md para deixar ainda mais orientado a usuário final, com uma seção “Como usar” passo a passo.

ao `buttonSalvar_Click` alterando o `nomePropriedade` ou o `localPropriedade` faz remover todas as subpropriedaes que pertencem a ela. isso não pode ocorrer. faça as correções necessárias para ao editar uma propriedade preservar as sub-propriedades que ela possue.



resolver bug que se tentar deletar uma propriedade e cancelar, tentar deletar novamente a mesma dá erro


 os controles `treeViewItens` e `propertyGridItem` devem iniciar com a opção expandir (+) todos os nós habilitada por padrão.


arrastar `splitter3` não pode deixar `treeViewItens.Width` nem `propertyGridItem.Width` ficar menor que 200. arrastar `splitter4` não pode deixar `propertyGridItem.Width` ficar menor que 200 e nem `groupBox1.Width` ficar menor que 397. arrastar `splitter1` não pode deixar `treeViewItens.Width` nem `propertyGridItem.Height` ficar menor que 500. implemente isso no código.




selecionar um item em `treeViewItens` e disparar o evento `buttonEditar_Clic` deve abrir o formulário `FormNovoItem` para permitir o usuário editar o item selecionado. a edição deste item não pode comprometer todas a propriedades e sub-propriedades pré-existes deste item. implemente essa funcionalidade.

clique duplo em um item em `treeViewItens` deve abrir o formulário para editar o item.



































