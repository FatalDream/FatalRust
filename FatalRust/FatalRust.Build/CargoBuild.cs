using Microsoft.Build;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FatalRust.External.Rustup;
//using FatalRust.External.Cargo;

namespace FatalRust.Build
{
    public class CargoBuild : Microsoft.Build.Utilities.Task
    {
        public override bool Execute()
        {
            var result = Rustup.Instance;
            result.Do(
                rustup => {
                    Log.LogMessage("Found rustup: " + rustup.Version.ToString(), new object[0]);
                    Log.LogMessage("Toolchain path: " + rustup.ToolchainPath, new object[0]);
                    },
                error => Log.LogError(error.ToString(), new object[0]));

            return result.IsSuccess;
        }
    }
}
