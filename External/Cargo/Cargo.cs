using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Bearded.Monads;
using FatalRust.Core;
using System.Text.RegularExpressions;

namespace FatalRust.External
{
    public class Cargo
    {
        public static EitherSuccessOrError<Cargo,Error<String>> New(String pathToCargo, ToolchainId toolchain)
        {
            Regex versionRegex = new Regex(
                "([0-9]+)"
                + Regex.Escape(".")
                + "([0-9]+)"
                + Regex.Escape(".")
                + "([0-9]+)"
                + "-([a-z]+)");

            return Communication.ReadProcess(pathToCargo, "--version")
                .SelectMany(o => o.ParseRegex(versionRegex))
                .SelectMany(m => from channel in ERelaseChannelImpl.FromString(m.Groups[4].Value)
                                 select new CargoVersion(
                                    Int32.Parse(m.Groups[1].Value),
                                    Int32.Parse(m.Groups[2].Value),
                                    Int32.Parse(m.Groups[3].Value),
                                    channel))
                .Select(v => new Cargo(pathToCargo, toolchain, v));
        }

        private Cargo(String pathToExecutable, ToolchainId toolChain, CargoVersion version)
        {
            this.pathToExecutable = pathToExecutable;
            this.toolChain = toolChain;
            this.version = version;
        }

        public EitherSuccessOrError<CargoResult, String> Call()
        {
            String cargo = Path.Combine(pathToExecutable, "cargo.exe");
            var process = System.Diagnostics.Process.Start(cargo);
            return process.StandardOutput.ReadToEnd()
                .AsOption()
                .Map(text => new CargoResult(text))
                .AsEither("could not read cargo...");
        }
        
        public override string ToString()
        {
            return toolChain.ToString() + "\n" + version.ToString() + "\n@" + pathToExecutable;
        }

        public ToolchainId Toolchain
        {
            get { return toolChain; }
        }

        private String pathToExecutable;
        private ToolchainId toolChain;
        private CargoVersion version;

    }
}
