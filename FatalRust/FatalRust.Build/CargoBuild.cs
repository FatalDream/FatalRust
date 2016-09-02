using Microsoft.Build;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FatalRust.Core;
using FatalRust.External;
using Bearded.Monads;
//using FatalRust.External.Cargo;

namespace FatalRust.Build
{
    public class CargoBuild : Microsoft.Build.Utilities.Task
    {
        public override bool Execute()
        {
            return Rustup.Instance
                .WhenSuccess(rustup => {
                    Log.LogMessage("Found rustup: " + rustup.Version.ToString(), new object[0]);
                    Log.LogMessage("Toolchain path: " + rustup.ToolchainPath, new object[0]); })
                .Map(rustup => rustup.GetCargos())
                .WhenSuccess(cargos => Log.LogMessage("Found cargos: " + String.Join("\n\n ", showCargos(cargos)), new object[0]))

                .WhenError(error => Log.LogError(error.ToString(), new object[0]))

                .IsSuccess;
        }

        public List<String> showCargos(List<EitherSuccessOrError<Cargo,Error<String>>> list)
        {
            return list.Select<EitherSuccessOrError<Cargo, Error<String>>, String>(c => c.Unify(
                  cargo => cargo.ToString(),
                  error => error.ToString()))
                .ToList();
        }
    }
}
