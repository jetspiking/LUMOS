using System;
using System.Diagnostics;
using System.Text;

namespace LUMOS.Misc
{
    public static class CommandInvoker
    {
        public static void ExecuteCommand(String command, Action<String> onOutput, Action<String> onError)
        {
            String shell, shellOption;
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                shell = "cmd.exe";
                shellOption = "/c";
            }
            else
            {
                shell = "bash";
                shellOption = "-c";
            }

            ProcessStartInfo processStartInfo = new()
            {
                FileName = shell,
                Arguments = $"{shellOption} {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true, // Enable input redirection.
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            Process process = new()
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true // Enable event raising.
            };

            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                    onOutput(args.Data);
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                    onError(args.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }

    }
}
