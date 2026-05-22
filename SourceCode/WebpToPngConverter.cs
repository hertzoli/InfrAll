using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GerenciadorSistemas
{
    internal static class WebpToPngConverter
    {
        private const int TimeoutMilliseconds = 30000;
        private const string ConverterFileName = "dwebp.exe";

        public static byte[] ConvertToPngBytes(byte[] webpBytes)
        {
            string converterPath = GetConverterPath();
            string tempBasePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            string webpPath = tempBasePath + ".webp";
            string pngPath = tempBasePath + ".png";

            try
            {
                File.WriteAllBytes(webpPath, webpBytes);
                RunConverter(converterPath, webpPath, pngPath);

                if (!File.Exists(pngPath))
                {
                    throw new InvalidOperationException("O conversor WEBP nao gerou o arquivo PNG esperado.");
                }

                return File.ReadAllBytes(pngPath);
            }
            finally
            {
                TryDelete(webpPath);
                TryDelete(pngPath);
            }
        }

        private static string GetConverterPath()
        {
            string converterPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConverterFileName);

            if (!File.Exists(converterPath))
            {
                throw new FileNotFoundException(
                    "Para abrir WEBP, coloque dwebp.exe na mesma pasta do executavel do GerenciadorSistemas.",
                    converterPath);
            }

            return converterPath;
        }

        private static void RunConverter(string converterPath, string webpPath, string pngPath)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = converterPath,
                Arguments = "-quiet " + QuoteArgument(webpPath) + " -o " + QuoteArgument(pngPath),
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            using (Process process = Process.Start(startInfo))
            {
                if (process == null)
                {
                    throw new InvalidOperationException("Nao foi possivel iniciar o conversor WEBP.");
                }

                if (!process.WaitForExit(TimeoutMilliseconds))
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }

                    throw new TimeoutException("A conversao WEBP demorou demais e foi cancelada.");
                }

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (process.ExitCode != 0)
                {
                    string details = (output + Environment.NewLine + error).Trim();
                    if (details.Length == 0)
                    {
                        details = "Codigo de saida: " + process.ExitCode;
                    }

                    throw new InvalidOperationException("Falha ao converter WEBP para PNG. " + details);
                }
            }
        }

        private static string QuoteArgument(string value)
        {
            var builder = new StringBuilder();
            builder.Append('"');
            int backslashCount = 0;

            foreach (char character in value)
            {
                if (character == '\\')
                {
                    backslashCount++;
                    continue;
                }

                if (character == '"')
                {
                    builder.Append('\\', backslashCount * 2 + 1);
                    builder.Append('"');
                    backslashCount = 0;
                    continue;
                }

                builder.Append('\\', backslashCount);
                backslashCount = 0;
                builder.Append(character);
            }

            builder.Append('\\', backslashCount * 2);
            builder.Append('"');
            return builder.ToString();
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch
            {
            }
        }
    }
}
