using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FatalRust.Core;

namespace FatalRust.External
{
    public class CargoVersion : BinaryVersion
    {
        public CargoVersion(int major, int minor, int patch, EReleaseChannel channel) : base(major, minor, patch)
        {
            this.channel = channel;
        }

        private EReleaseChannel channel;
    }
}
