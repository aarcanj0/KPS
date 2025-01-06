using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace KPSIntegrityServices
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override async void OnStart(string[] args)
        {
            await verifyMemory(true, await searchAVSolutionExecutable());
        }

        protected override void OnStop()
        {
        }

        public static async Task verifyMemory(bool executableExists, string executablePath)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (executableExists)
                    {
                        Process[] ps1 = Process.GetProcessesByName("avp");
                        if (ps1.Length > 0)
                        {
                            //Console.WriteLine("Processo existe");
                        }
                        else
                        {
                            // Console.WriteLine("Iniciando processo");
                            Process.Start(executablePath);
                        }
                    }
                    else
                    {
                        Environment.Exit(1);
                    }

                    await Task.Delay(3000);
                }
            });
        }

        public static Task<string> searchAVSolutionExecutable()
        {
            return Task.Run(() =>
            {
                string directoryPath = @"C:\Program Files (x86)\Kaspersky Lab\";
                string executableName = "avpui.exe";
                string completePathExecutable = string.Empty;
                bool fileFound = false;

                try
                {
                    string[] files = Directory.GetFiles(directoryPath, executableName, SearchOption.AllDirectories);
                    if (files.Length > 0)
                    {
                        fileFound = true;
                        completePathExecutable = files[0];
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Erro ao procurar o executável: {ex.Message}");
                }

                if (!fileFound)
                {
                    //Console.WriteLine("Executável não encontrado.");
                    completePathExecutable = "0x1";
                    Environment.Exit(1);
                    return "0x1";
                }
                return completePathExecutable.ToString();
            });
        }
    }
}