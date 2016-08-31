using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Bearded.Monads;

namespace FatalRust.External.Rustup
{
    public class Rustup
    {
        public static EitherSuccessOrError<Rustup,String> Instance
        {
            get
            {
                if (instance == null)
                {
                    return CallRustup("--version")
                        .Map(v => {
                                instance = new Rustup(v);
                                return instance;
                    });
                }
                else
                {
                    return instance
                        .AsOption()
                        .AsEither("Rustup instance is null");
                }
            }
        }

        private Rustup(String version)
        {
            this.version = version;
        }

        private static EitherSuccessOrError<String,String> CallRustup(String arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "rustup.exe",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            using (Process p = Process.Start(startInfo))
            {
                return p.StandardOutput.ReadToEnd().AsOption().AsEither("Could not find rustup.exe");
            }
        }


        
        public String Version
        {
            get { return version; }
        }
        private String version;

        private static Rustup instance;
    }
}
