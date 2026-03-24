using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace OnlineAutoUpdate
{
    /*
		
	Lib que ao iniciar o programa verifica em (etecnet.com.br/EtecnetoTools/NomeDaAplicacao(que deve ser o namespace)/ver(que é um txt sem extenção)) se tem update.
    se sim, e se se clicado em "atualizar agora", então tem que baixar a nova versão, fechar a versão atual, fazer um backup da versão atual, substituir a versão atual, e inicializa a nova versão. (tudo automaticamente)
        
    todo: fazer com que o script que troca o executável funcione mesmo se o caminho tiver em pastas com acênto e espaço.
    todo: permite configurar se a verificação se existe update ocorre automaticamente ou manualmente (apenas se o usuário solicitar) 
    todo: permitir configurar se a verificação de nova versão ocorre no inicio do programa ou na finalização do programa	
	todo: permite configurar se exibe para o usuário a opção de aceitar a ou não.	
	todo: permitir criar uma caixa de dialogo automaticamente que mostras opções e informações do update, (ideia: em vez de uma caixa de dialogo, usar uma região do form igual a barra de status.)
	todo: permitir exibir as alterações da nova versão.	
	todo: Verifica Data e hora do sistema está atualizada (se desatualizado, tenta atualizar)
    todo: criar uma função que verifica se tem update disponível e retorna true se verdadeiro
    todo: fazer toda essa tarefa de update ser async e multitask.



    //----------------------
    Para enviar o novo executável atualizado para o FTP automaticamente (ou seja, colocar a nova atualização online)  
        1) criar um atalho do programa "%OneDrive%\Programação\C#\Tools\AutoSend to FTP.exe" na pasta do projeto. na pasta do projeto
        2) arrastar o executável para dentro do atalho ou abrir o atalho e arrastar o exe para dentro de caminho do arquivo.    


    OBSERVAÇÂO: o nome da pasta do arquivo e o nome do arquivo será o [Nome do assembly] que fica em properties do projeto logo na primeira aba aplicativo
        ficando assim: (ftp)//EtecnetoTools/[Nome do assembly]/[Nome do assembly].exe
            link para download exemplo: https://www.etecnet.com.br/EtecnetTools/Config_Video_Wall/Config_Video_Wall.exe
    
    //---------------------------------------------
    exemplo de uso:
     public Form1()
        {
            InitializeComponent();
         //---------------- Verifica update online    
            OnlineAutoUpdate.OnlineAutoUpdateAsync.OnlineAutoUpdateAsync2();
         //------------------------------------
        }
	
    */

    //OnlineAutoUpdate.OnlineAutoUpdateAsync.OnlineAutoUpdateAsync2();
    public static class OnlineAutoUpdateAsync
    {        
        static public async void OnlineAutoUpdateAsync2()
        {
            await Task.Factory.StartNew(() => {
                try
                {
                    var update = new OnlineAutoUpdate();
                }
                catch (Exception erro)
                {
                    Logger.LogWriter.LogErroDetalhado(erro, " >>Erro ao executar OnlineAutoUpdate()");
                }
            }, TaskCreationOptions.LongRunning); // OK, não bloqueia a GUI

        }
    }

        class OnlineAutoUpdate
    {
        public string urlExecutavelVersaoOnline { get; set; } // "https://www.etecnet.com.br/EtecnetoTools/NomeDaAplicacao(que deve ser o namespace)/nome executável)"; 
        public string urlVerificaVersaoOnline { get; set; } // "https://www.etecnet.com.br/EtecnetoTools/NomeDaAplicacao(que deve ser o namespace)/ver(que é um txt sem extenção))"; 
        public string CaminhoArquivoDeBackup { get; set; } //@"C:\x86\pt-pt\AppDataClassOl.vxl";
        public string caminhoDoArquivoLocal { get; set; } //@"C:\x86\pt-pt\AppDataClass.vxl";
        public string PastaParaSalvarOProgramaBaixado { get; set; } //@"C:\x86\pt-pt\Kernelsys.rom";
        public string NomeDoProgramaAtual { get; set; } //(ex.: MyApp.exe)
        public string NomeDoProgramaAtualSemExtensao { get; set; } //(ex.: MyApp.exe)
        private string VersaoAppLocalAtual = "";
        private string VersaoAppOnlineNovo = "";


        string NomeDoAssembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; // retorna o [Nome do assembly] que fica em properties do projeto logo na primeira aba aplicativo


        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlDoProgramaParaBaixar"></param>
        public OnlineAutoUpdate(string urlDoProgramaParaBaixar = null)
        {
            urlVerificaVersaoOnline = @"https://www.etecnet.com.br/EtecnetTools/" + NomeDoAssembly + "/ver";
            urlExecutavelVersaoOnline = @"https://www.etecnet.com.br/EtecnetTools/" + NomeDoAssembly + "/" + NomeDoAssembly + ".exe";

            caminhoDoArquivoLocal = System.Reflection.Assembly.GetEntryAssembly().Location; //Caminho = "C:\\Users\\SincronizaDataHora\\bin\\Debug\\TimeSyncOS.exe" 
            PastaParaSalvarOProgramaBaixado = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location); //"C:\\Users\\ConsoleApp8\\bin\\Debug"   (retorna a pasta do programa)

            CaminhoArquivoDeBackup = PastaParaSalvarOProgramaBaixado + "\\" + NomeDoAssembly + ".bak.exe"; //Caminho = "C:\\Users\\SincronizaDataHora\\bin\\Debug\\TimeSyncOS.exe" 
            NomeDoProgramaAtual = System.AppDomain.CurrentDomain.FriendlyName;
            NomeDoProgramaAtualSemExtensao = System.Diagnostics.Process.GetCurrentProcess().ProcessName; // Retorna o nome do arquivo sem extensão (ex.: MyApp)

            //Verifica Data e hora do sistema se atualizada

            // verifica se tem versão nova
            if (versaoOnlineEhMaisAtual(System.Reflection.Assembly.GetEntryAssembly().Location, urlVerificaVersaoOnline) == true)
            {
                // pergunta para o usuário se deseja faze a atualização
                if (MessageBox.Show("Nova versão detectada. Deseja atualizar agora?", "Update", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //faz download do novo programa
                    DownloadFile(urlExecutavelVersaoOnline, PastaParaSalvarOProgramaBaixado + "\\" + NomeDoAssembly + "_new.exe", false);

                    //verifica se o arquivo Aupdateet.bat existe e apaga caso existir
                    CheckIfDestinationFileExistAndDelete(PastaParaSalvarOProgramaBaixado + "\\Aupdateet.bat", false);

                    //Cria arquivo de script e Executa:
                    using (System.IO.StreamWriter Updatescript = new System.IO.StreamWriter(PastaParaSalvarOProgramaBaixado + "\\Aupdateet.bat", true))
                    {

                        //@echo off
                        Updatescript.WriteLine("@echo off");

                        //REM finaliza app atual(taskkill /F /PID 1242)
                        //taskkill /IM "App1.0C.exe" /F
                        Updatescript.WriteLine("taskkill /IM \"" + NomeDoProgramaAtual + "\" /F");

                        //REM Deleta Arquivo de backup duplicado se houver
                        //del Aupdateet.bat
                        Updatescript.WriteLine($"del \"{NomeDoProgramaAtualSemExtensao}_v{VersaoAppLocalAtual}.exe.bak\"");

                        //REM faz backup do app atual
                        //ren "D:\Temp\App1.0C.exe" "App1.0C_v1.1.exe.bak"
                        Updatescript.WriteLine($"ren \"{PastaParaSalvarOProgramaBaixado}\\{NomeDoProgramaAtual}\" \"{NomeDoProgramaAtualSemExtensao}_v{VersaoAppLocalAtual}.exe.bak\"");

                        //REM copia novo App baixado para o local do App antigo
                        //copy D:\Temp\App1.0C.exe" "App1.0C.exe"
                        Updatescript.WriteLine($"ren \"{PastaParaSalvarOProgramaBaixado}\\{NomeDoAssembly}_new.exe\" \"{NomeDoProgramaAtual}\"");

                        //REM inicializa o app novo
                        //start "Titulo" "App1.0C.exe"
                        Updatescript.WriteLine($"start \"\" \"{PastaParaSalvarOProgramaBaixado}\\{NomeDoProgramaAtual}\"");

                        //REM Deleta Script Aupdateet.bat
                        //del Aupdateet.bat
                        Updatescript.WriteLine($"del \"{PastaParaSalvarOProgramaBaixado}\\Aupdateet.bat\"");
                    }

                    //Executa o script
                    System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo();                    
                    start.FileName = PastaParaSalvarOProgramaBaixado + "\\Aupdateet.bat"; //start.FileName = @"C:\Windows\System32\Sysprep\sysprep.exe";
                    start.WindowStyle = ProcessWindowStyle.Hidden; //opções System.Diagnostics.ProcessWindowStyle.Hidden ou .maximized  ou .minimized ou .normal
                    start.CreateNoWindow = false; // informa se a janela do aplicativo deve ser criada ou não true = não criar janela
                    start.UseShellExecute = true; // define se o shell do windows deve ser usado para executar o processo. true = usar o shell do windows para executar o processo
                    var per = new Process();
                    per.StartInfo = start;
                    per.Start();

                }
                else
                {

                }
            }

        }



        /// <summary>
        /// 
        /// Exemplo: 
        ///     Version VersaoOnline = OnlineAutoUpdate.OnlineAutoUpdate.PegaVersaoOnline("https://www.etecnet.com.br/etecnet_VDI_OS/Ver");
        /// </summary>
        /// <param name="UrlVersaoOnline"></param>
        /// <returns></returns>
        public Version PegaVersaoOnline(string UrlVersaoOnline)
        {
            try
            {
                Version VersionOut;
                WebClient web = new WebClient();
                System.IO.Stream stream = web.OpenRead(UrlVersaoOnline);
                using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                {
                    VersaoAppOnlineNovo = reader.ReadToEnd();
                    //System.IO.File.ver
                    if (Version.TryParse(VersaoAppOnlineNovo, out VersionOut) == false)
                    {
                        // Debug.Print("PegaVersaoOnline: Erro ao processar a versão: " + reader.ReadToEnd());
                        throw new ArgumentException("PegaVersaoOnline: Erro ao processar a versão: " + reader.ReadToEnd());
                    }
                    else
                    {
                        return VersionOut;
                    }
                }
            }
            catch (Exception erro)
            {
                if (erro.InnerException != null)
                {
                    if (erro.InnerException is System.Security.Authentication.AuthenticationException)
                    {
                        throw new Exception("não foi possível conectar ao servidor por que a data e hora do terminal estão Desatualizada");
                    }
                }

                throw new Exception("Erro ao Verificar a versão disponível Online. >>>" + erro.Message);
            }
        }

        /// <summary>
        /// 
        /// Exemplo: 
        ///     Version VersaoLocal = OnlineAutoUpdate.OnlineAutoUpdate.PegaVersaoLocal(@"C:\x86\pt-pt\AppDataClass.vxl");
        /// </summary>
        /// <param name="CaminhoDoArquivo"></param>
        /// <returns></returns>
        public Version PegaVersaoLocal(string CaminhoDoArquivo)
        {
            Version VersionOut;

            if (System.IO.File.Exists(CaminhoDoArquivo) == true)
            {
                VersaoAppLocalAtual = FileVersionInfo.GetVersionInfo(CaminhoDoArquivo).FileVersion;
                //System.IO.File.ver
                if (Version.TryParse(VersaoAppLocalAtual, out VersionOut) == false)
                {
                    Debug.Print("PegaVersaoLocal: Erro ao processar a versão: " + VersaoAppLocalAtual);
                    throw new ArgumentException("PegaVersaoLocal: Erro ao processar a versão: " + VersaoAppLocalAtual);
                }
                else
                {
                    
                    return VersionOut;
                }
            }
            return new Version("0.0");



        }

        /// <summary>
        /// verifica se a versão online é mais atual e retorna true se verdadeiro, caso contrario retorna false
        /// exemplo: 
        ///     OnlineAutoUpdate.OnlineAutoUpdate.versaoOnlineEhMaisAtual(@"C:\x86\pt-pt\AppDataClass.vxl", "https://www.etecnet.com.br/etecnet_VDI_OS/Ver");
        /// </summary>
        /// <param name="CaminhoDoArquivoLocal"> caminho completo do programa local que se deseja verificar a versão</param>
        /// <param name="UrlVersaoOnline">caminho da url com o arquivo que contém a versão do programa que está online</param>
        /// <returns></returns>
        public bool versaoOnlineEhMaisAtual(string CaminhoDoArquivoLocal, string UrlVersaoOnline)
        {
            Version VersionOnline;
            Version VersionArquivoLocal;


            VersionOnline = PegaVersaoOnline(UrlVersaoOnline);
            VersionArquivoLocal = PegaVersaoLocal(CaminhoDoArquivoLocal);

            if (VersionOnline > VersionArquivoLocal) //se a versão online for mais nova, então...
            {
                return true;
            }
            return false;
        }





        public static bool DownloadCompleto = false;
        public static ProgressBar _barraDeProgresso;
        /// <summary>
        /// 
        /// Exemplo:
        ///     OnlineAutoUpdate.OnlineAutoUpdate.DownloadFile("https://www.etecnet.com.br/etecnet_VDI_OS/Kernelsys.rom", @"C:\x86\pt-pt\Kernelsys.rom");
        /// </summary>
        /// <param name="urlArquivo"></param>
        /// <param name="DestinoArquivo"></param>
        /// <param name="barraDeProgresso"></param>
        public void DownloadFile(string urlArquivo, string DestinoArquivo, bool CreateBackup = true, ProgressBar barraDeProgresso = null)
        {
            // verifica se a pasta de destino existe, se não então cria
            CheckIfDestinationFilePathExist(DestinoArquivo, true);
            // verifica se arquivo de destino existe, se sim então verifica se precisa de backup, se sim faz backup e deleta, caso contrario só deleta
            CheckIfDestinationFileExistAndDelete(DestinoArquivo, CreateBackup);

            using (WebClient wc = new WebClient())
            {
                if (barraDeProgresso != null)
                {
                    _barraDeProgresso = barraDeProgresso;
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    wc.DownloadDataCompleted += wc_DownloadCompleted;
                }
                //todo: verificar a necessidade de criar o evento downloadCompleted
                wc.DownloadFile(new System.Uri(urlArquivo), DestinoArquivo);
                //wc.DownloadFileTaskAsync(new System.Uri(urlArquivo), DestinoArquivo);
            }

        }
        // Event to track the progress
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            _barraDeProgresso.Value = e.ProgressPercentage;
        }

        // Event to track the progress
        void wc_DownloadCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            _barraDeProgresso.Value = 100;
            DownloadCompleto = true;
        }


        /// <summary>
        /// Verifica se as pastas de um determinado caminho de um arquivo existe, podendo opcionalmente já criar as pastas ou não.
        /// exemplo: CheckIfDestinationFilePathExist(@"c:\temp\MyTest.txt", true);
        /// </summary>
        /// <param name="DestinoArquivo"></param>
        /// <param name="CreateDestinationPath"></param>
        /// <returns></returns>
        public bool CheckIfDestinationFilePathExist(string DestinoArquivo, bool CreateDestinationPath = true)
        {
            var ApenasDiretorioDoArquivo = System.IO.Path.GetDirectoryName(DestinoArquivo); // retorna apenas o caminho da pasta do arquivo
            if (System.IO.Directory.Exists(ApenasDiretorioDoArquivo) == false)
            {
                if (CreateDestinationPath == true)
                {
                    System.IO.Directory.CreateDirectory(ApenasDiretorioDoArquivo);
                    return true;
                }
                return false;
            }
            else return true;
        }


        /// <summary>
        /// Verifica se o arquivo de caminho existe, se sim já deleta o mesmo, podendo opcionalmente criar um Backup ou não.
        /// exemplo: CheckIfDestinationFileExistAndDelete(@"c:\temp\MyTest.txt", true);
        /// </summary>
        /// <param name="DestinoArquivo"></param>
        /// <param name="CreateDestinationPath"></param>
        /// <returns></returns>
        public bool CheckIfDestinationFileExistAndDelete(string DestinoArquivo, bool CreateBackup = true)
        {
            try
            {
                if (System.IO.File.Exists(DestinoArquivo) == true)
                {
                    if (CreateBackup == true)
                    {
                        // deleta backup se o mesmo existir
                        System.IO.File.Delete(DestinoArquivo + ".bak");
                        //faz backup
                        System.IO.File.Move(DestinoArquivo, DestinoArquivo + ".bak");  //Renomeia arquivo
                    }
                    // deleta arquivo
                    System.IO.File.Delete(DestinoArquivo);
                    return true;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }


}

/*
 

Ver 1.1
    Corrigido o bug do arquivo bat está dando um erro de não foi possivel encontrar o arquivo ao executar o primeiro comando ren "nome.exe" "nome_ba_v1.1.exe" Causa, o arquivo bat não estava apontando para os caminhos completos, então se o app iniciar a partir de outra pasta qualquer o mesmo dava erro 
    acrescentado a extensão .bak no executável de backup









 
	Passos
        Verifica Data e hora do sistema está atualizada
        se desatualizado, tenta atualizar
        verifica se tem versão nova
        se sim, então
        verifica se faz atualização direto ou se pergunta.
        faz a atualização
            faz download do novo programa
            Cria arquivo de script :
                @echo off
                REM finaliza app atual  (taskkill /F /PID 1242)
                taskkill /IM "App1.0C.exe" /F

                REM faz backup do app atual
                ren "App1.0C.exe" "App1.0C.bak.exe"

                REM copia novo App baixado para o local do App antigo
                copy "App1.1C.exe" "App1.0C.exe"

                REM inicializa o app novo
                start "Titulo" "App1.0C.exe"
            Executa o script


 */
