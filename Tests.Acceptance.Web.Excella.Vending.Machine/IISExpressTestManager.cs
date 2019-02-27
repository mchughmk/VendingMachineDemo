using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Tests.Acceptance.Web.Excella.Vending.Machine
{
    public static class IISExpressTestManager
    {
        private const string APPLICATION_NAME = "Excella.Vending.Web.UI";

        public static void StopIISExpress()
        {
            var localIISExpressProcesses = GetIISProcesses();
            foreach (var iisExpressProcess in localIISExpressProcesses)
            {
                if (!iisExpressProcess.HasExited)
                {
                    iisExpressProcess.Kill();
                }
            }
        }

        public static bool IsIISExpressRunning()
        {
            var localIISExpressProcesses = GetIISProcesses();

            return localIISExpressProcesses.Any(x => x.HasExited == false);
        }

        public static void StartIISExpress()
        {
            var applicationPath = GetApplicationPath(APPLICATION_NAME);
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            var startInfoFileName = programFiles + @"\IIS Express\iisexpress.exe";
            var startInfoArguments = string.Format("/config:\"{0}\\..\\..\\..\\.vs\\config\\applicationhost.config\"", applicationPath);
            Debug.WriteLine("Application Path: " + applicationPath);
            Debug.WriteLine("Program Files: " + programFiles);
            Debug.WriteLine("startInfoFileName: " + startInfoFileName);
            Debug.WriteLine("startInfoArguments: " + startInfoArguments);

            var iisProcess = new Process
            {
                StartInfo =
                {
                    FileName = startInfoFileName,
                    Arguments = startInfoArguments
                }
            };

            iisProcess.Start();
        }

        private static Process[] GetIISProcesses()
        {
            return Process.GetProcessesByName("iisexpress");
        }

        private static string GetApplicationPath(string applicationName)
        {
            var solutionFolder = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            return Path.Combine(solutionFolder, applicationName);
        }
    }
}