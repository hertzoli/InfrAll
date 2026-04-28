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
[assembly: AssemblyVersion("1.6.3")]
[assembly: AssemblyFileVersion("1.6.3")]

/*


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