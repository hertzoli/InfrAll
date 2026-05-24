using System.Reflection;

namespace GerenciadorSistemas
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    internal enum TipoDoValor
    {
        Texto = 0,
        Comando = 1,
        Script = 2,
        Imagem = 3,
        Video = 4,
        Binario = 5,
        Password = 6
    }
}
