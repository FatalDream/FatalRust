using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Bearded.Monads;

namespace FatalRust.Core
{
    public static class Communication
    {
        public static EitherSuccessOrError<ProcessOutput, Error<String>> ReadProcess(String filename, String arguments, String workingDirectory = ".")
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                };
                return EitherSuccessOrError<String, Error<String>>.Create(
                    Process.Start(startInfo)
                           .StandardOutput
                           .ReadToEnd())
                    .Map(output => new ProcessOutput(output));
            }
            catch (Exception e)
            {
                return EitherSuccessOrError<ProcessOutput, Error<String>>.Create(
                    new Error<String>("When trying to call " + filename + ": " + e.Message));
            }
        }

        public static EitherSuccessOrError<Success, Error<String>> StartProcess(
            String filename,
            String arguments,
            String workingDirectory,
            Action<String> StdHandler,
            Action<String> ErrHandler)
        {
            try
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                };

                p.Start();

                p.OutputDataReceived += (o, a) => StdHandler(a.Data);
                p.BeginOutputReadLine();
                p.ErrorDataReceived += (o, a) => ErrHandler(a.Data);
                p.BeginErrorReadLine();

                p.WaitForExit();

                return EitherSuccessOrError<Success, Error<String>>.Create(new Success());
            }
            catch (Exception e)
            {
                return EitherSuccessOrError<Success, Error<String>>.Create(
                    new Error<String>("When trying to call " + filename + ": " + e.Message));
            }
        }
    }
}
