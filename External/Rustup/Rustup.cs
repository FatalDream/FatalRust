using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bearded.Monads;
using FatalRust.Core;

namespace FatalRust.External
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

                   from path in Communication.ReadProcess("rustup.exe", "which cargo")
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

        public List<EitherSuccessOrError<Cargo,Error<String>>> GetCargos()
        {
            Regex pathRegex = new Regex(
                "([a-z]+)-([a-z0-9_]+)-([a-z]+)-([a-z]+)-([a-z]+).bin");


            return Directory.EnumerateDirectories(toolchainPath)
                .SelectMany(Directory.EnumerateDirectories)
                .Where(toolchain_bin => new DirectoryInfo(toolchain_bin).Name == "bin")
                .SelectMany(Directory.EnumerateFiles)
                .Where(toolchain_bin_file => Path.GetFileName(toolchain_bin_file) == "cargo.exe")
                .Select(path => new ProcessOutput(path)
                                    .ParseRegex(pathRegex)
                                    .SelectMany(m => from channel in ERelaseChannelImpl.FromString(m.Groups[1].Value)
                                                     from arch in ETargetArchitectureImpl.FromString(m.Groups[2].Value)
                                                     from abi in ERustABIImpl.FromString(m.Groups[5].Value)
                                                     from cargo in Cargo.New(path, new ToolchainId(channel, arch, abi))
                                                     select cargo))
                .ToList();
        }

        public List<Cargo> GetCorrectCargos()
        {
            return GetCargos()
                .SelectMany(c => c.Unify(
                    cargo => new Cargo[] { cargo },
                    error => new Cargo[] { }))
                .ToList();
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
