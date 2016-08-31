using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Bearded.Monads;

namespace FatalRust.External.Cargo
{
    public class Cargo
    {
        public Cargo(String pathToExecutable)
        {
            this.pathToExecutable = pathToExecutable;
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

        private String pathToExecutable;
    }
}
