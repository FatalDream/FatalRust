using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatalRust.Core
{
    public class ProcessOutput
    {
        public ProcessOutput(String raw)
        {
            this.raw = raw;
        }

        public String Raw() => raw;

        private String raw;
    }
}
