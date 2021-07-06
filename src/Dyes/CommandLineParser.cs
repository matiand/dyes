using System;
using System.Drawing;
using Dyes.Commands;

namespace Dyes
{
    public class CommandLineParser
    {
        public ICommand Parse(string[] args)
        {
            var cmd = args[0];

            switch (cmd)
            {
                case "help":
                case "--help":
                case "-h":
                    return new HelpCmd();
                case "version":
                case "--version":
                case "-v":
                    return new VersionCmd();
                case "check-truecolor-support":
                    return new CheckTrueColorSupport();
                case "view":
                    return new ViewCmd();
                case "copy":
                    return new CopyCmd();
                case "convert":
                    return new ConvertCmd();
            }

            throw new ArgumentException("Wrong command");
        }

        private ColorNotation ParseColorNotation(string input) =>
            input.ToLowerInvariant() switch
            {
                "hex" => ColorNotation.Hex,
                "rgb" => ColorNotation.Rgb,
                "hsl" => ColorNotation.Hsl,
                "hsluv" => ColorNotation.Hsluv,
                "hpluv" => ColorNotation.Hpluv,
                _ => throw new ArgumentException("Wrong color notation keyword")
            };
    }
}
