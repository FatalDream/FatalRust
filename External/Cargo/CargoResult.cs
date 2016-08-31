using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatalRust.External.Cargo
{
    public class CargoResult
    {
        public CargoResult(String result)
        {
            this.result = result;
        }

        public override String ToString()
        {
            return result;
        }

        private String result;
    }
}
