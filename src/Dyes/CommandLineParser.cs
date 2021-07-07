using System;
using System.Drawing;
using Dyes.Commands;

namespace Dyes
{
    public class CommandLineParser : IParser<string[], ICommand>
    {
        private readonly IParser<string, Color> _colorParser;

        public CommandLineParser(IParser<string, Color> colorParser)
        {
            _colorParser = colorParser;
        }

        public CommandLineParser()
        {
            _colorParser = new ColorParser();
        }

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
                {
                    var color = _colorParser.Parse(args[1]);
                    return new ViewCmd(color);
                }
                case "copy":
                {
                    var color = _colorParser.Parse(args[1]);
                    return new CopyCmd(color);
                }
                case "convert":
                {
                    var notation = ParseColorNotation(args[1]);
                    var color = _colorParser.Parse(args[2]);
                    return new ConvertCmd(color, notation);
                }
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
