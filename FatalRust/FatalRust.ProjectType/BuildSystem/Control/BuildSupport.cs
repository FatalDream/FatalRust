using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using System.Windows;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ProjectSystem.Designers;
using Microsoft.VisualStudio.ProjectSystem.Utilities;
using Microsoft.VisualStudio.ProjectSystem.Build;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ProjectSystem;
using System.IO;
using System.Reflection;
using System.Threading;

namespace FatalRust.BuildSystem.Control
{
    [Export(typeof(IBuildSupport))]
    [AppliesTo("ProjectConfigurationsFromRustupToolchains")]
    public class BuildSupport : IBuildSupport
    {
        public BuildStatus Status
        {
            get
            {
                return BuildStatus.Idle;
            }
        }

        public Task<string> GetTargetForBuildAsync(BuildAction buildAction)
        {
            return Task.FromResult("Build");
        }

        public Task<bool> IsBuildActionSupportedAsync(BuildAction buildAction, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsBuildTargetSupportedAsync(string target, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(true);
        }
    }
}
