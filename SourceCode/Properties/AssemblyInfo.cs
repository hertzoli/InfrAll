using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// As informações gerais sobre um assembly são controladas por
// conjunto de atributos. Altere estes valores de atributo para modificar as informações
// associadas a um assembly.
[assembly: AssemblyTitle("GerenciadorSistemas")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("GerenciadorSistemas")]
[assembly: AssemblyCopyright("Copyright ©  2026")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Definir ComVisible como false torna os tipos neste assembly invisíveis
// para componentes COM. Caso precise acessar um tipo neste assembly de
// COM, defina o atributo ComVisible como true nesse tipo.
[assembly: ComVisible(false)]

// O GUID a seguir será destinado à ID de typelib se este projeto for exposto para COM
[assembly: Guid("470c5abb-3d12-49b0-a087-8762f78fa5ca")]

// As informações da versão de um assembly consistem nos quatro valores a seguir:
//
//      Versão Principal
//      Versão Secundária 
//      Número da Versão
//      Revisão
//
// É possível especificar todos os valores ou usar como padrão os Números de Build e da Revisão
// usando o "*" como mostrado abaixo:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.6.5")]
[assembly: AssemblyFileVersion("1.6.5")]

/*



v1.6.5
 - listViewImages foi substituído por TreeViewImages em /D:/OneDrive/PROJETOS/C#/Gerenciador de sitemas/SourceCode/FormNovoItem.Designer.cs:39.
  - /D:/OneDrive/PROJETOS/C#/Gerenciador de sitemas/SourceCode/FormNovoItem.cs:135 agora lê Imagens recursivamente e monta a árvore conforme a hierarquia real de pastas.
  - As imagens são salvas/selecionadas por caminho relativo, por exemplo Hardware/Memoria.png, preservando subpastas.
  - buttonDefinirIconePadrao_Click agora salva configuracoes.iconePadrao em cadastro.yaml.
  - /D:/OneDrive/PROJETOS/C#/Gerenciador de sitemas/SourceCode/Form1.cs:1671 agora carrega ícones recursivamente da pasta Imagens, para que itens com ícones em subpastas também apareçam corretamente na árvore principal.
  - /D:/OneDrive/PROJETOS/C#/Gerenciador de sitemas/SourceCode/Models/CadastroYamlContract.cs:20 recebeu o bloco configuracoes.
 - nós de pasta usam a chave interna __folder__
  - imagens continuam usando suas próprias miniaturas
  - o ícone de pasta é reaplicado após limpar/recarregar o ImageList

 Agora, ao criar/carregar/editar um InfrastructureItem:
  - tenta usar o IconeKey atual;
  - se o arquivo não existir nesse caminho, procura recursivamente em Imagens por um arquivo com o mesmo nome;
  - se encontrar, atualiza item.IconeKey para o caminho relativo correto encontrado;
  - se não encontrar, usa o recurso Properties.Resources.ErrorSmall como ícone visual de fallback.

  Também ajustei a edição da propriedade Imagem/Icone e a edição via FormNovoItem para passarem pela mesma resolução.

v1.6.4
 - substituir o controle textBoxValor por um controle RichTextBox para definir cores diferentes caso seja um comando.

v1.6.3
 - gerar um alerta se o usuário editar um propriedade e sair da propriedade selecionada sem salvar (informando se o usuário quer sair sem salvar a modificação)
 - implementar uma forma permitir que o usuário consiga editar o valor de uma propriedade direto do controle propertyGridItem

v1.6.2
 - corrigido bug que retorna bug ao executar comando que começa com: `cmd.exe /k ....."
 - corrigido bug que ao executar o evento de comando em um executável que requer "elevação" para ser executado. dá o erro abaixo:
ex > Valor: = "A operação solicitada requer elevação"

v1.6.1
 - adicionado o botão superior direito para acessar o issus do github direto para registrar bugs e melhorias

v1.6
 - os controles textbox das propriedades agora se ajustam preenchendo o formulário quando o usuário aumenta a região desses controles.

v1.5
 - Retirado a obfuscação de classes para não quebrar o arquivo YAML
 - adicionao os skill agent no projeto

v1.4
   implementado PRD_007_Destaque_visual_de_campos_nao_salvos.md

v1.2.1
Adicionei o controle `comboBoxTipo` que deve receber um Enun com os tipos (texto, comando, script) que será usado para distinguir o tipo de dado do `textBoxValor` e habilitar ou não o `buttonRun`. lembre que o usuário deve poder editar o tipo de dado.
Fazer o botão `buttonRun` só ficar acessíveis (Enable) se o valor da propriedade for do tipo Comando ou script



v1.2
Implementar atualização automática.
pressionando [F2] abre o formulário de edição para renomear o item selecionado.
alterar para que a resolução de placeholders buscar propriedades em qualquer item e não ficar restrita ao item atual. para implementar isso será necessário que o caminho do placeholders passar a ser um caminho completo de toda a arvore de itens e não só do item atual.


Programa Criado:
V1.0





















*/