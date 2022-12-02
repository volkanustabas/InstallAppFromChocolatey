using System;
using System.Diagnostics;
using System.IO;

namespace InstallAppFromChocolatey
{
    internal class Program
    {
        public const string ChocoPath = @"%ALLUSERSPROFILE%\chocolatey\choco.exe";
        public const string CommandInstall7Zip = @"choco install 7zip -y";
        public const string CommandInstallFirefox = @"choco install firefox -y";

        public const string CommandInstallChoco =
            @"@powershell -NoProfile -ExecutionPolicy Bypass -Command ""iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1'))"" && SET PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin";

        private static void Main(string[] args)
        {
            Console.ReadKey();

            if (File.Exists(ChocoPath))
            {
                ExecuteCommandSync(CommandInstallFirefox);
                ExecuteCommandSync(CommandInstall7Zip);
            }
            else
            {
                ExecuteCommandSync(CommandInstallChoco);
                ExecuteCommandSync(CommandInstallFirefox);
                ExecuteCommandSync(CommandInstall7Zip);
            }

            Console.ReadKey();
        }

        public static void ExecuteCommandSync(object command)
        {
            try
            {
                var procStartInfo =
                    new ProcessStartInfo("cmd", "/c " + command)
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        Verb = "runas"
                    };

                var proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
                var result = proc.StandardOutput.ReadToEnd();
                Console.WriteLine(result);
            }
            catch (Exception objException)
            {
                Console.WriteLine(objException.ToString());
            }
        }
    }
}