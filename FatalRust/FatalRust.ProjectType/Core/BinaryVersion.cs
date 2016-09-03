using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatalRust.Core
{
    public class BinaryVersion
    {
        public BinaryVersion(int major, int minor, int patch)
        {
            this.major = major;
            this.minor = minor;
            this.patch = patch;
        }

        int major, minor, patch;

        public int Major() => major;
        public int Minor() => minor;
        public int Path() => patch;

        public override string ToString()
        {
            return major.ToString() + "." + minor.ToString() + "." + patch.ToString();
        }
    }
}
