using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using FatalRust.Core;

namespace FatalRust
{
    class ToolchainProjectConfiguration : ProjectConfiguration
    {
        public ToolchainProjectConfiguration(ToolchainId toolchain, String name)
        {
            this.toolchain = toolchain;
            this.name = name;
        }

        IImmutableDictionary<string, string> ProjectConfiguration.Dimensions
        {
            get
            {
                var builder = ImmutableDictionary.CreateBuilder<String, String>();
                builder.Add("ReleaseChannel", toolchain.ReleaseChannel.ToString());
                builder.Add("TargetArchitecture", toolchain.TargetArchitecture.ToString());
                builder.Add("RustABI", toolchain.RustABI.ToString());
                builder.Add("Name", name);
                builder.Add("OutputName", "output-" + name);
                builder.Add("AvailablePlatforms", "cargo");
                builder.Add("Configuration", "Debug");
                builder.Add("Platform", "x86");
                return builder.ToImmutableDictionary();
            }
        }

        string ProjectConfiguration.Name
        {
            get
            {
                return toolchain.ToString();
            }
        }

        bool IEquatable<ProjectConfiguration>.Equals(ProjectConfiguration other)
        {
            return other.GetType() == this.GetType()
                && this.toolchain.Equals(((ToolchainProjectConfiguration)other).Toolchain)
                && this.name.Equals(((ToolchainProjectConfiguration)other).Name);
        }

        private ToolchainId toolchain;

        public ToolchainId Toolchain
        {
            get { return toolchain; }
        }

        private String name;
        public String Name
        {
            get { return name; }
        }
    }
}
