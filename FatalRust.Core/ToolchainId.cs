using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatalRust.Core
{
    public class ToolchainId
    {
        private EReleaseChannel ReleaseChannel;
        private ETargetArchitecture TargetArchitecture;
        private ERustABI RustABI;

        public ToolchainId(EReleaseChannel channel, ETargetArchitecture architecture, ERustABI abi)
        {
            this.ReleaseChannel = channel;
            this.TargetArchitecture = architecture;
            this.RustABI = abi;
        }

        public override String ToString()
        {
            return ReleaseChannel.ToString() + "-" + TargetArchitecture.ToString() + "-" + RustABI.ToString();
        }
    }
}
