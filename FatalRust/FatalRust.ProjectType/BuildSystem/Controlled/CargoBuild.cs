using Bearded.Monads;
using FatalRust.Core;
using FatalRust.External;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FatalRust.Build
{
    public class CargoBuild : Microsoft.Build.Utilities.Task
    {
        
        [Required]
        public string ProjectDirectory { get; set; }

        [Required]
        public string PlatformName { get; set; }
        

        public override bool Execute()
        {
            return Rustup.Instance
                .WhenSuccess(rustup => {
                    Log.LogMessage("Found rustup: " + rustup.Version.ToString(), new object[0]);
                    Log.LogMessage("Toolchain path: " + rustup.ToolchainPath, new object[0]); })
                .Select(rustup => rustup.GetCorrectCargos())
                //.WhenSuccess(cargos => Log.LogMessage("Found cargos: " + String.Join("\n\n ", showCargos(cargos)), new object[0]))
                .SelectMany(cargos => 
                    cargos.Where(cargo => cargo.Toolchain.ToString() == PlatformName)
                          .FirstOrNone()
                          .AsEither(new Error<String>("No toolchain was found with the platform name '" + PlatformName + "'.")))

                .WhenSuccess(foundCargo => Log.LogMessage("correct cargo: " + foundCargo.ToString()))

                .SelectMany(cargo => cargo.Build(ProjectDirectory, std => Log.LogMessage("build: " + std)))

                .WhenSuccess(output => Log.LogMessage("cargo success: " + output))
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
