using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

using System.Linq;


/*

todo: usar o caminho padrão: C:\PerfLogs\ para salvar os aquivos de Logs.

 captura erro não tratado em qualquer parte do aplicativo
esse código gera log de erro detalhado sempre que a aplicação não tiver em modo debug.
 código recomendado para todas as aplicações 

        [STAThread]
        static void Main()
        {
#if (DEBUG == false)

            Logger.LogWriter.InfoLogIfDebugModeOnly(">>>>>>>>>>Iniciou o Programa<<<<<<<<<");
            try
            {
#endif
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
#if (DEBUG == false)
            }
            catch (Exception erro)
            {

                Logger.LogWriter.LogErroDetalhado(erro, "SemInfoAdicional");
                MessageBox.Show("Erro interno detectado. Reinicie o sistema e tente novamente. Caso o problema persista, entre em contato com o desenvolvedor. www.etecnet.com.br");
            }
            Logger.LogWriter.InfoLogIfDebugModeOnly("<<<<<<<<<<<<Finalizou o Programa>>>>>>>>>>>>>");
            
#endif
        }

exemplo de saída:
-----------
20/09/2022 21:37:11.468 ► Informacao: >>>>>>>>>>Iniciou o Programa<<<<<<<<<

-----------
20/09/2022 21:37:14.866 ♦ Version: 1.0.0.0
▬ Erro: 
	O sistema não pode encontrar o arquivo especificado
► Info: 
	InfoAdicional
○ Tipo: 
	System.ComponentModel.Win32Exception
▲ Source: 
	System
▼ TargetSite: 
	Boolean StartWithShellExecuteEx(System.Diagnostics.ProcessStartInfo)
♠ StackTrace: 
   em System.Diagnostics.Process.StartWithShellExecuteEx(ProcessStartInfo startInfo)
   em System.Diagnostics.Process.Start()
   em System.Diagnostics.Process.Start(ProcessStartInfo startInfo)
   em System.Diagnostics.Process.Start(String fileName)
   em WindowsFormsApp15.Form1.button1_Click(Object sender, EventArgs e) na C:\Users\h_z20\source\repos\WindowsFormsApp15\Form1.cs:linha 28
   em System.Windows.Forms.Control.OnClick(EventArgs e)
   em System.Windows.Forms.Button.OnClick(EventArgs e)
   em System.Windows.Forms.Button.OnMouseUp(MouseEventArgs mevent)
   em System.Windows.Forms.Control.WmMouseUp(Message& m, MouseButtons button, Int32 clicks)
   em System.Windows.Forms.Control.WndProc(Message& m)
   em System.Windows.Forms.ButtonBase.WndProc(Message& m)
   em System.Windows.Forms.Button.WndProc(Message& m)
   em System.Windows.Forms.Control.ControlNativeWindow.OnMessage(Message& m)
   em System.Windows.Forms.Control.ControlNativeWindow.WndProc(Message& m)
   em System.Windows.Forms.NativeWindow.DebuggableCallback(IntPtr hWnd, Int32 msg, IntPtr wparam, IntPtr lparam)
   em System.Windows.Forms.UnsafeNativeMethods.DispatchMessageW(MSG& msg)
   em System.Windows.Forms.Application.ComponentManager.System.Windows.Forms.UnsafeNativeMethods.IMsoComponentManager.FPushMessageLoop(IntPtr dwComponentID, Int32 reason, Int32 pvLoopData)
   em System.Windows.Forms.Application.ThreadContext.RunMessageLoopInner(Int32 reason, ApplicationContext context)
   em System.Windows.Forms.Application.ThreadContext.RunMessageLoop(Int32 reason, ApplicationContext context)
   em System.Windows.Forms.Application.Run(Form mainForm)
   em WindowsFormsApp15.Program.Main() na C:\Users\h_z20\source\repos\WindowsFormsApp15\Program.cs:linha 23

-----------
20/09/2022 21:37:14.881 ► Informacao: <<<<<<<<<<<<Finalizou o Programa>>>>>>>>>>>>>

*/

//using Logger;
namespace Logger
{



    #region Classe SimpleLogger Cria um arquivo de log e permite o desenvolvedor escrever mensagens neste arquivo de forma simples. (Exemplo de uso da biblioteca)
    /*
                // Inicializa a Classe
                Logger.SimpleLogger logger = new Logger.SimpleLogger(); // Will create a fresh new log file if it doesn't exist.

            // Escrevendo Mensagens de Log
            logger.Limpo("Uma mensagem sem qualquer pré texto");
            logger.Trace("--> Trace in message here...");
            logger.Info("MAC Wifi: 74:D0:2B:CC:65:F2");
            logger.Warning("Temperatura: 42,375");
            logger.Error("Error message...");
            logger.Fatal("Fatal error message...");
    
            logger.DebugEnable = true;  // habilita o modo debug para que o método logger.Debug seja escrito no log
            logger.Debug("Something to debug..."); // imprime no log apenas se o debug estiver habilitado.

            * exemplo de como ficaria o arquivo de log na saída: **************************
                2019-10-20 09:12:24.256 Info_Temperatura_Etecnet.log is created.
                Uma mensagem sem qualquer pré texto
                2019-10-20 09:12:24.407 [TRACE]    --> Trace in message here...
                2019-10-20 09:12:24.766 [INFO]    MAC Wifi: 74:D0:2B:CC:65:F2
                2019-10-20 09:12:25.583 [WARNING]    Temperatura: 42,375
                2019-10-20 09:12:30.242 [ERROR]    Error message...
                2019-10-20 09:12:34.547 [FATAL]    Fatal error message...
                2019-10-20 09:12:25.056 [DEBUG]    Something to debug... 

            */


    public class SimpleLogger
    {


        private const string FILE_EXT = ".log";
        private readonly string datetimeFormat;
        private readonly string logFilename;
        public bool DebugEnable = false;

        /// <summary>
        /// Initiate an instance of SimpleLogger class constructor.
        /// If log file does not exist, it will be created automatically.
        /// </summary>
        public SimpleLogger(string ExtensaoArquivo = FILE_EXT, bool CriarHeader = true, string PastaDoPrograma = "")
        {
            datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            logFilename = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ExtensaoArquivo;
            if (!System.IO.Directory.Exists(PastaDoPrograma))
            {
                PastaDoPrograma = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location); //"C:\\Users\\h_z20\\source\\repos\\ConsoleApp8\\bin\\Debug" 
            }
            logFilename = (PastaDoPrograma + "\\" + logFilename);
            // Log file header line
            string logHeader = logFilename + " is created.";
            if (!System.IO.File.Exists(logFilename))
            {
                if (CriarHeader)
                {
                    WriteLine(System.DateTime.Now.ToString(datetimeFormat) + " " + logHeader, false);
                }
            }
        }

        /// <summary>
        /// Log a DEBUG message
        /// </summary>
        /// <param name="text">Message</param>
        public void Debug(string text)
        {
            if (DebugEnable == true)
            {
                WriteFormattedLog(LogLevel.DEBUG, text);
            }
        }

        /// <summary>
        /// Log an ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public void Error(string text)
        {
            WriteFormattedLog(LogLevel.ERROR, text);
        }

        /// <summary>
        /// Log a FATAL ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public void Fatal(string text)
        {
            WriteFormattedLog(LogLevel.FATAL, text);
        }

        /// <summary>
        /// Log a FATAL ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public void Limpo(string text)
        {
            WriteFormattedLog(LogLevel.LIMPO, text);
        }

        /// <summary>
        /// Log an INFO message
        /// </summary>
        /// <param name="text">Message</param>
        public void Info(string text)
        {
            WriteFormattedLog(LogLevel.INFO, text);
        }

        /// <summary>
        /// Log a TRACE message
        /// </summary>
        /// <param name="text">Message</param>
        public void Trace(string text)
        {
            WriteFormattedLog(LogLevel.TRACE, text);
        }

        /// <summary>
        /// Log a WARNING message
        /// </summary>
        /// <param name="text">Message</param>
        public void Warning(string text)
        {
            WriteFormattedLog(LogLevel.WARNING, text);
        }

        private void WriteLine(string text, bool append = true)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(logFilename, append, System.Text.Encoding.UTF8))
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        writer.WriteLine(text);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void WriteFormattedLog(LogLevel level, string text)
        {
            string pretext;
            switch (level)
            {
                case LogLevel.TRACE:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [TRACE]   ";
                    break;
                case LogLevel.INFO:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [INFO]    ";
                    break;
                case LogLevel.DEBUG:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [DEBUG]   ";
                    break;
                case LogLevel.WARNING:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [WARNING] ";
                    break;
                case LogLevel.ERROR:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [ERROR]   ";
                    break;
                case LogLevel.FATAL:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [FATAL]   ";
                    break;
                case LogLevel.LIMPO:
                    pretext = "";
                    break;
                default:
                    pretext = "";
                    break;
            }

            WriteLine(pretext + text);
        }

        [System.Flags]
        private enum LogLevel
        {
            TRACE,
            INFO,
            DEBUG,
            WARNING,
            ERROR,
            FATAL,
            LIMPO
        }
    }

    #endregion


    //-----------------------------------------------------------------------------------------------------------


    #region Classe LogWriter Cria um arquivo de log com data no nome e permite o desenvolvedor escrever mensagens de erro detalhada neste arquivo de forma simples. (Exemplo de uso da biblioteca)

    /* Biblioteca que gera um arquivo de log com descrição detelhada de 
     * todos os erros que ocorrerem. assim como nome no método que gerou o erro e etc.
     * Para inserir o Snippet de forma rápida, selecione a regiao que vai ficar dentro de try, use o atalho <Ctrl + K, S> então escreva: log
     * o snnipt está no caminho: C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC#\Snippets\1046\Visual C#\log.snippet
     * Exemplo de uso ******************************     
        
        public Form1()
        {
            InitializeComponent();
            Logger.LogWriter.VerificaSeHabilitaDebug();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Logger.LogWriter.InfoLogIfDebugModeOnly("teste");
            Logger.LogWriter.InfoLogIfDebugModeOnly("Iniciou o método: " + System.Reflection.MethodBase.GetCurrentMethod().ToString() + " --> Da classe: " + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
            try
            {
                System.Diagnostics.Process.Start("programa");
            }
            catch (Exception erro)
            {
                Logger.LogWriter.LogErroDetalhado(erro, "InfoAdicional");
            }
            Logger.LogWriter.InfoLogIfDebugModeOnly("Finalizou o método: " + System.Reflection.MethodBase.GetCurrentMethod().ToString() + " --> Da classe: " + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

      
      ** Exemplo de como ficaria o arquivo de log de erro ****
      
            -----------
            20/09/2022 21:01:06.512 ► Informacao: teste

            -----------
            20/09/2022 21:01:06.566 ► Informacao: Iniciou o método: Void button1_Click(System.Object, System.EventArgs) --> Da classe: WindowsFormsApp15.Form1

            -----------
            20/09/2022 21:01:06.636 ♦ Version: 1.0.0.0
            ▬ Erro: 
	            O sistema não pode encontrar o arquivo especificado
            ► Info: 
	            informações adicionais
            ○ Tipo: 
	            System.ComponentModel.Win32Exception
            ▲ Source: 
	            System
            ▼ TargetSite: 
	            Boolean StartWithShellExecuteEx(System.Diagnostics.ProcessStartInfo)
            ♠ StackTrace: 
               em System.Diagnostics.Process.StartWithShellExecuteEx(ProcessStartInfo startInfo)
               em System.Diagnostics.Process.Start()
               em System.Diagnostics.Process.Start(ProcessStartInfo startInfo)
               em System.Diagnostics.Process.Start(String fileName)
               em WindowsFormsApp15.Form1.button1_Click(Object sender, EventArgs e) na C:\Users\h_z20\source\repos\WindowsFormsApp15\Form1.cs:linha 30

            -----------
            20/09/2022 21:01:06.655 ► Informacao: Finalizou o método: Void button1_Click(System.Object, System.EventArgs) --> Da classe: WindowsFormsApp15.Form1    
      */

    /// <summary>
    /// Log Writer Class
    /// </summary>
    public static class LogWriter
    {

#if (DEBUG == true)
        public static bool DebugEnable = true;
#else
        public static bool? DebugEnable = false;
#endif

     #region Metodos que escrevem no arquivo de log informações personalizadas




        /// <summary>
        /// escreve no arquivo de log, uma mensagem de informação qualquer, e támbém escreve essa mesma mensagem no console (se a aplicação for console) e na Janela Debug do Visual Studio, (apenas se a aplicação tiver no modo Debug)
        /// exemplo de uso:
        ///     Logger.LogWriter.InfoLogIfDebugModeOnly("teste");
        /// </summary>
        /// <param name="Mensagem">nome personalizado a ser exibido</param>
        /// <param name="isFallback">Coloca o Log na pasta Meus Documentos</param>
        public static void InfoLogIfDebugModeOnly(string Mensagem, bool isFallback = false)
        {
            if (DebugEnable == true)
            {
                InfoLog(Mensagem, isFallback);
            }
            else
            {
                return;
            }

        }



        /// <summary>
        /// escreve no arquivo de log, uma mensagem de informação qualquer, e támbém escreve essa mesma mensagem no console (se a aplicação for console) e na Janela Debug do Visual Studio,
        /// Exemplo de uso:
        /// Logger.LogWriter.InfoLog("Mensagem");
        /// </summary>
        /// <param name="Mensagem">nome personalizado a ser exibido</param>
        /// <param name="isFallback">Coloca o Log na pasta Meus Documentos</param>
        public static void InfoLog(string Mensagem, bool isFallback = false)
        {

            try
            {
#region Output folder

                var documents = isFallback ? "LogsIfDebug" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name : ".";
                var folder = Path.Combine(documents, "Logs" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

#endregion

#region Creates the file

                var date = Path.Combine(folder, DateTime.Now.ToString("yyyy_MM_dd") + ".log");
                var dateTime = Path.Combine(folder, DateTime.Now.ToString("yyyy_MM_dd HH_mm_ss_fff") + ".log");

                FileStream fs = null;
                var inUse = false;

                try
                {
                    fs = new FileStream(date, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                }
                catch (Exception)
                {
                    inUse = true;
                    fs = new FileStream(dateTime, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                }

                fs.Dispose();

#endregion

#region Append the exception information

                using (var fileStream = new FileStream(inUse ? dateTime : date, FileMode.Append, FileAccess.Write))
                {
                    using (var writer = new StreamWriter(fileStream))
                    {
                        writer.WriteLine("-----------");
                        writer.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + $" ► Informacao: {Mensagem}");
                        writer.WriteLine();

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("-----------");
                        Console.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff")} => {Mensagem}");
                        Console.WriteLine("");
                        Console.ResetColor();

                        Debug.WriteLine($"-----------"); // http:// serve para deixar essa saída com uma cor destacada
                        Debug.WriteLine($"http://---DEBUG-MSG---D  {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff")} - {Mensagem}"); // http:// serve para deixar essa saída com uma cor destacada
                        Debug.WriteLine($""); // http:// serve para deixar essa saída com uma cor destacada
                    }
                }

#endregion
            }
            catch (Exception erro)
            {
                //One last trial.
                if (!isFallback)
                    LogErroDetalhado(erro, Mensagem);
            }
        }

    #endregion


    #region Metodos que escrevem no arquivo de log informações detalhadas de erro que ocorreram



        /// <summary>
        /// escreve no arquivo de log o erro detalhado, com informações como o tipo do erro, o método que gerou o erro e a identificação da linha que gerou o erro. (apenas se a aplicação tiver no modo Debug)
        /// </summary>
        /// <param name="ex">The Exception to write.</param>
        /// <param name="title">[Opcional] nome personalizado a ser exibido</param>
        /// <param name="aditional">Aditional information.</param>
        /// <param name="isFallback">Coloca o Log na pasta Meus Documentos</param>
        public static void LogErroDetalhadoIfDebugModeOnly(Exception ex, string title = "", object aditional = null, bool isFallback = false)
        {
            if (DebugEnable == true)
            {
                LogErroDetalhado(ex, title, aditional, isFallback);
            }
            else
            {
                return;
            }

        }


        /// <summary>
        /// escreve no arquivo de log o erro detalhado, com informações como o tipo do erro, o método que gerou o erro e a identificação da linha que gerou o erro.
        /// </summary>
        /// <param name="ex">The Exception to write.</param>
        /// <param name="title">[Opcional] nome personalizado a ser exibido</param>
        /// <param name="aditional">Aditional information.</param>
        /// <param name="isFallback">Coloca o Log na pasta Meus Documentos</param>
        public static void LogErroDetalhado(Exception ex, string title = "", object aditional = null, bool isFallback = false)
        {
            try
            {
#region Output folder

                var documents = isFallback ? "LogsIfDebug" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name : ".";
                var folder = Path.Combine(documents, "Logs" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

#endregion

#region Creates the file

                var date = Path.Combine(folder, DateTime.Now.ToString("yyyy_MM_dd") + ".log");
                var dateTime = Path.Combine(folder, DateTime.Now.ToString("yyyy_MM_dd HH_mm_ss_fff") + ".log");

                FileStream fs = null;
                var inUse = false;

                try
                {
                    fs = new FileStream(date, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                }
                catch (Exception)
                {
                    inUse = true;
                    fs = new FileStream(dateTime, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                }

                fs.Dispose();

#endregion

#region Append the exception information

                using (var fileStream = new FileStream(inUse ? dateTime : date, FileMode.Append, FileAccess.Write))
                {
                    using (var writer = new StreamWriter(fileStream))
                    {
                        writer.WriteLine("-----------");
                        writer.WriteLine(FormattableString.Invariant($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff")} ♦ Version: {Assembly.GetExecutingAssembly().GetName().Version}"));
                        writer.WriteLine($"▬ Erro: {Environment.NewLine}\t{ex.Message}");
                        writer.WriteLine($"► Info: {Environment.NewLine}\t{title}");
                        writer.WriteLine($"○ Tipo: {Environment.NewLine}\t{ex.GetType()}");
                        //writer.WriteLine(FormattableString.Invariant($"♦ [Version] Date/Hour: {Environment.NewLine}\t[{Assembly.GetExecutingAssembly().GetName().Version}] {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff")}"));
                        writer.WriteLine($"▲ Source: {Environment.NewLine}\t{ex.Source}");
                        writer.WriteLine($"▼ TargetSite: {Environment.NewLine}\t{ex.TargetSite}");

                        var bad = ex as BadImageFormatException;

                        if (bad != null)
                            writer.WriteLine($"► Fuslog: {Environment.NewLine}\t{bad.FusionLog}");

                        if (aditional != null)
                            writer.WriteLine($"◄ Aditional: {Environment.NewLine}\t{aditional}");

                        writer.WriteLine($"♠ StackTrace: {Environment.NewLine}{ex.StackTrace}");

                        if (ex.InnerException != null)
                        {
                            writer.WriteLine();
                            writer.WriteLine($"▬▬ Message: {Environment.NewLine}\t{ex.InnerException.Message}");
                            writer.WriteLine($"○○ Type: {Environment.NewLine}\t{ex.InnerException.GetType()}");
                            writer.WriteLine($"▲▲ Source: {Environment.NewLine}\t{ex.InnerException.Source}");
                            writer.WriteLine($"▼▼ TargetSite: {Environment.NewLine}\t{ex.InnerException.TargetSite}");
                            writer.WriteLine($"♠♠ StackTrace: {Environment.NewLine}{ex.InnerException.StackTrace}");

                            if (ex.InnerException.InnerException != null)
                            {
                                writer.WriteLine();
                                writer.WriteLine($"▬▬▬ Message: {Environment.NewLine}\t{ex.InnerException.InnerException.Message}");
                                writer.WriteLine($"○○○ Type: {Environment.NewLine}\t{ex.InnerException.InnerException.GetType()}");
                                writer.WriteLine($"▲▲▲ Source: {Environment.NewLine}\t{ex.InnerException.InnerException.Source}");
                                writer.WriteLine($"▼▼▼ TargetSite: {Environment.NewLine}\t{ex.InnerException.InnerException.TargetSite}");
                                writer.WriteLine($"♠♠♠ StackTrace: {Environment.NewLine}\t{ex.InnerException.InnerException.StackTrace}");

                                if (ex.InnerException.InnerException.InnerException != null)
                                {
                                    writer.WriteLine();
                                    writer.WriteLine($"▬▬▬▬ Message: {Environment.NewLine}\t{ex.InnerException.InnerException.InnerException.Message}");
                                    writer.WriteLine($"○○○○ Type: {Environment.NewLine}\t{ex.InnerException.InnerException.InnerException.GetType()}");
                                    writer.WriteLine($"▲▲▲▲ Source: {Environment.NewLine}\t{ex.InnerException.InnerException.InnerException.Source}");
                                    writer.WriteLine($"▼▼▼▼ TargetSite: {Environment.NewLine}\t{ex.InnerException.InnerException.InnerException.TargetSite}");
                                    writer.WriteLine($"♠♠♠♠ StackTrace: {Environment.NewLine}\t{ex.InnerException.InnerException.InnerException.StackTrace}");
                                }
                            }
                        }

                        writer.WriteLine();
                    }
                }

#endregion
            }
            catch (Exception)
            {
                //One last trial.
                if (!isFallback)
                    LogErroDetalhado(ex, title, aditional, true);
            }
        }


    #endregion

        


        /// <summary>
        /// verifica se o programa foi carregado com algum parametro de debug válido, se sim habilita o debug
        /// exemplo de uso:
        /// Logger.LogWriter.VerificaSeHabilitaDebug();
        /// </summary>
        public static void VerificaSeHabilitaDebug()
        {
            try
            {
                if (Environment.GetCommandLineArgs().Length > 1)
                {
                    var ArgsArrayA = Environment.GetCommandLineArgs();
                    if (ArgsArrayA.Contains("-debug"))
                    {
                        DebugEnable = true;
                    }
                    else if (ArgsArrayA.Contains("/debug"))
                    {
                        DebugEnable = true;
                    }
                    else if (ArgsArrayA.Contains("-d"))
                    {
                        DebugEnable = true;
                    }
                    else
                    {
                        DebugEnable = false;
                    }
                }
            }
            catch
            {

            }

        }

#endregion

    }

}







