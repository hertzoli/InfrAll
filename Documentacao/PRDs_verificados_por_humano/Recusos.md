### Implementar:


Se quiser, eu posso fazer um segundo passe para deixar o README ainda mais orientado a usuário final, com uma seção “Como usar” passo a passo.




resolver bug que se tentar deletar uma propriedade e cancelar, tentar deletar novamente a mesma dá erro

 
 os controles `treeViewItens` e `propertyGridItem` devem iniciar com a opção expandir (+) todos os nós habilitada por padrão.

 
### Baixa Prioridade:
em relação ao `treeViewItens` implementar os quatro itens abaixo:
 7. poder exportar um item para um arquivo. (opção exibida no menu de contexto)
 6. poder importar um item a partir de um arquivo.  (opção exibida no menu de contexto)
 7. Adicionar uma opção de criar "Nova Pasta" que vai criar um item já com o ícone de Pasta pré-selecionado.
 8. Adicioanr uma opção para expandir (+) e retrair (-) todos os nós.
 9. ter uma forma de editar o ícone de um ítem existente sem alterar todas as suas propriedades existentes.

ajuste para que no `propertyGridItem` os nomes das Propriedades que possui outras sub-propriedades sejam exibidas em Negrito enquanto seus valores sejam exibidos sem negrito; para isso é necessário trocar o controle de propriedades.
melhorar os ajustes de dimencionamento do formulário principal

### Feito:

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
















































