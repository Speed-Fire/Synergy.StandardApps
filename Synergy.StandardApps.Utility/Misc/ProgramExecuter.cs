using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Utility.Misc
{
    public static class ProgramExecuter
    {
        /// <summary>
        /// Executes program with specified arguments.
        /// </summary>
        /// <param name="fileName">Executable name/path.</param>
        /// <param name="isAdminRun">Run as Administrator.</param>
        /// <param name="_output">Execution output.</param>
        /// <param name="_error">Execution error.</param>
        /// <param name="timeout">Timeout (in milliseconds).</param>
        /// <param name="args">Execution arguments.</param>
        /// <returns>Execution exit code or -1 if timed out.</returns>
        public static int Execute(
            string fileName,
            bool isAdminRun,
            out string _output,
            out string _error,
            int timeout,
            params string[] args)
        {
            int exitCode = 0;
            _output = string.Empty; 
            _error = string.Empty;

            using (var process = new System.Diagnostics.Process())
            {

                var startInfo = new System.Diagnostics.ProcessStartInfo();

                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = fileName;

                foreach (var arg in args)
                    startInfo.ArgumentList.Add(arg);

                if (isAdminRun)
                    startInfo.Verb = "runas";

                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;

                process.StartInfo = startInfo;
                process.Start();

                var output = new StringBuilder();
                var error = new StringBuilder();

                using AutoResetEvent outputWaitHandle = new(false);
                using AutoResetEvent errorWaitHandle = new(false);
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        output.AppendLine(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        errorWaitHandle.Set();
                    }
                    else
                    {
                        error.AppendLine(e.Data);
                    }
                };

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                if (process.WaitForExit(timeout) &&
                    outputWaitHandle.WaitOne(timeout) &&
                    errorWaitHandle.WaitOne(timeout))
                {
                    // Process completed. Check process.ExitCode here.

                    exitCode = process.ExitCode;
                    _output = output.ToString();
                    _error = error.ToString();
                }
                else
                {
                    // Timed out.
                    exitCode = -1;
                }
            }

            return exitCode;
        }
    }
}
