using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FatalRust.BuildSystem.Control
{
    public interface IBuildController
    {
        String Respond();

        Task<String> Start();
    }
}
