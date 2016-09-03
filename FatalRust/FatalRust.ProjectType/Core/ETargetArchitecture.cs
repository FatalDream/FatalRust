using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bearded.Monads;

namespace FatalRust.Core
{
    public enum ETargetArchitecture
    {
        i686,
        x86_64
    }

    public static class ETargetArchitectureImpl
    {
        public static EitherSuccessOrError<ETargetArchitecture,Error<String>> FromString(String arch)
        {
            switch (arch)
            {
                case "i686":
                    return EitherSuccessOrError<ETargetArchitecture, Error<String>>.Create(ETargetArchitecture.i686);
                case "x86_64":
                    return EitherSuccessOrError<ETargetArchitecture, Error<String>>.Create(ETargetArchitecture.x86_64);
                default:
                    return EitherSuccessOrError<ETargetArchitecture, Error<String>>.Create(new Error<String>("Could not parse architecture from '" + arch + "'"));
            }
        }

        public static String ToString(this ETargetArchitecture arch)
        {
            switch (arch)
            {
                case ETargetArchitecture.i686:
                    return "i686";
                case ETargetArchitecture.x86_64:
                    return "x86_64";
                default:
                    return "";
            }
        }
    }
}
