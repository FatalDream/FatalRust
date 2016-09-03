using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatalRust.Core
{
    public class ToolchainId : IEquatable<ToolchainId>
    {
        private EReleaseChannel releaseChannel;
        private ETargetArchitecture targetArchitecture;
        private ERustABI rustABI;

        public EReleaseChannel ReleaseChannel
        {
            get { return releaseChannel; }
        }

        public ETargetArchitecture TargetArchitecture
        {
            get { return targetArchitecture; }
        }

        public ERustABI RustABI
        {
            get { return rustABI; }
        }

        public ToolchainId(EReleaseChannel channel, ETargetArchitecture architecture, ERustABI abi)
        {
            this.releaseChannel = channel;
            this.targetArchitecture = architecture;
            this.rustABI = abi;
        }

        public override String ToString()
        {
            return releaseChannel.ToString() + "-" + targetArchitecture.ToString() + "-" + rustABI.ToString();
        }

        bool IEquatable<ToolchainId>.Equals(ToolchainId other)
        {
            return this.releaseChannel == other.releaseChannel
                && this.targetArchitecture == other.targetArchitecture
                && this.rustABI == other.rustABI;
        }
    }
}
