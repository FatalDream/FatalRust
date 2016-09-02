using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bearded.Monads;

namespace FatalRust.Core
{
    public enum EReleaseChannel
    {
        Stable,
        Beta,
        Nightly
    }

    public static class ERelaseChannelImpl
    {
        public static EitherSuccessOrError<EReleaseChannel,Error<String>> FromString(String s)
        {
            switch (s)
            {
                case "stable":
                    return EitherSuccessOrError<EReleaseChannel, Error<String>>.Create(EReleaseChannel.Stable);
                case "beta":
                    return EitherSuccessOrError<EReleaseChannel, Error<String>>.Create(EReleaseChannel.Beta);
                case "nightly":
                    return EitherSuccessOrError<EReleaseChannel, Error<String>>.Create(EReleaseChannel.Nightly);
                default:
                    return EitherSuccessOrError<EReleaseChannel, Error<String>>.Create(new Error<String>("Tried to get relase channel, but '" + s + "' does not match any."));
            }
        }

        public static String ToString(this EReleaseChannel channel)
        {
            switch (channel)
            {
                case EReleaseChannel.Stable:
                    return "stable";
                case EReleaseChannel.Beta:
                    return "beta";
                case EReleaseChannel.Nightly:
                    return "nightly";
                default:
                    return "";
            }
        }
    }
}
