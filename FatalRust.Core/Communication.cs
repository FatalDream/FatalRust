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
        public static EitherSuccessOrError<String, Error<String>> ReadProcess(String filename, String arguments)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                return EitherSuccessOrError<String, Error<String>>.Create(
                    Process.Start(startInfo)
                           .StandardOutput
                           .ReadToEnd());
            }
            catch (Exception e)
            {
                return EitherSuccessOrError<String, Error<String>>.Create(
                    new Error<String>("When trying to call " + filename + ": " + e.Message));
            }
        }
    }
}
