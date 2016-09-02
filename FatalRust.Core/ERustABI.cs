using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bearded.Monads;

namespace FatalRust.Core
{
    public enum ERustABI
    {
        gnu,
        msvc
    }

    public static class ERustABIImpl
    {
        public static EitherSuccessOrError<ERustABI,Error<String>> FromString(String abi)
        {
            switch (abi)
            {
                case "gnu":
                    return EitherSuccessOrError<ERustABI, Error<String>>.Create(ERustABI.gnu);
                case "msvc":
                    return EitherSuccessOrError<ERustABI, Error<String>>.Create(ERustABI.msvc);
                default:
                    return EitherSuccessOrError<ERustABI, Error<String>>.Create(new Error<String>("Could not parse rust abi from '" + abi + "'"));
            }
        }
    }

}
