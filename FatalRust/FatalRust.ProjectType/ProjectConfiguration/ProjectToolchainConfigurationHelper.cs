using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.ProjectSystem;
using System.Collections.Immutable;
using Bearded.Monads;
using FatalRust.Core;
using FatalRust.External;

namespace FatalRust
{
    static class ProjectToolchainConfigurationHelper
    {
        public static EitherSuccessOrError<IList<ProjectConfiguration>,Error<String>> GetConfigurations()
        {
            return Rustup.Instance
                .Select<Rustup,Error<String>,IList<ProjectConfiguration>> (
                    rustup => rustup.GetCorrectCargos()
                                     .Select(c => (ProjectConfiguration) new ToolchainProjectConfiguration(c.Toolchain))
                                     .ToList());
        }
    }
}
