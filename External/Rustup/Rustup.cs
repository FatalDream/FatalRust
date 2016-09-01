using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bearded.Monads;
using FatalRust.Core;

namespace FatalRust.External.Rustup
{
    public class Rustup
    {
        public static EitherSuccessOrError<Rustup,Error<String>> Instance
        {
            get
            {
                if (instance == null)
                {
                    return Communication.ReadProcess("rustup.exe", "--version")
                        .Map(v => {
                                instance = new Rustup(v);
                                return instance; });
                }
                else
                {
                    return EitherSuccessOrError<Rustup, Error<String>>.Create(
                        instance);
                }
            }
        }

        private Rustup(String version)
        {
            this.version = version;
        }

        


        
        public String Version
        {
            get { return version; }
        }
        private String version;

        private static Rustup instance;
    }
}
