using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace GerenciadorSistemas
{
    static class Program
    {
        private const string RecursoYamlDotNet = "GerenciadorSistemas.Dependencies.YamlDotNet.dll";

        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblySolicitada = new AssemblyName(args.Name);

            if (!string.Equals(assemblySolicitada.Name, "YamlDotNet", StringComparison.OrdinalIgnoreCase))
                return null;

            Assembly assemblyAtual = typeof(Program).Assembly;

            using (Stream stream = assemblyAtual.GetManifestResourceStream(RecursoYamlDotNet))
            {
                if (stream == null)
                    return null;

                byte[] dadosAssembly = new byte[stream.Length];
                int totalLido = stream.Read(dadosAssembly, 0, dadosAssembly.Length);

                if (totalLido != dadosAssembly.Length)
                    return null;

                return Assembly.Load(dadosAssembly);
            }
        }
    }
}
