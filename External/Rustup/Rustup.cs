using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bearded.Monads;
using FatalRust.Core;
using System.Text.RegularExpressions;

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
                    return CreateRustup()
                            .WhenSuccess(rustup => instance = rustup);
                }
                else
                {
                    return EitherSuccessOrError<Rustup, Error<String>>.Create(
                        instance);
                }
            }
        }

        static private EitherSuccessOrError<Rustup, Error<String>> CreateRustup()
        {
            Regex versionRegex = new Regex(
                "([0-9]+)"
                + Regex.Escape(".")
                + "([0-9]+)"
                + Regex.Escape(".")
                + "([0-9]+)");


            return from version in Communication.ReadProcess("rustup.exe", "--version")
                                            .SelectMany(o => o.ParseRegex(versionRegex))
                                            .Map(m => new BinaryVersion(
                                                            Int32.Parse(m.Groups[1].Value),
                                                            Int32.Parse(m.Groups[2].Value),
                                                            Int32.Parse(m.Groups[3].Value)))

                   from path    in Communication.ReadProcess("rustup.exe", "which cargo")
                                            .Map(o => o.Raw().Trim(new char[] { '\n', ' ' }))
                                            .Map(p => Path.GetDirectoryName(p))
                                            .Map(d => Directory.GetParent(d).FullName)
                                            .Map(d => Directory.GetParent(d).FullName)
                                            .Where(d => new DirectoryInfo(d).Name == "toolchains",
                                                   () => new Error<String>("Could not locate rustup toolchain folder"))

                   select new Rustup(version, path);
        }

        private Rustup(BinaryVersion v, String toolchainPath)
        {
            this.version = v;
            this.toolchainPath = toolchainPath;
        }

        
        public BinaryVersion Version
        {
            get { return version; }
        }
        private BinaryVersion version;
        
        public String ToolchainPath
        {
            get { return toolchainPath; }
        }
        private string toolchainPath;

        private static Rustup instance;
    }
}
