using System;
using Spectre.Console;
using Color = System.Drawing.Color;

namespace Dyes
{
    public class ConsoleWriter : IWriter
    {
        public void Write(object input)
        {
            Console.Write(input);
        }

        public void WriteLine(object input)
        {
            Console.WriteLine(input);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteColor(Color color, int width)
        {
            var hexColor = ColorNotation.Hex.Stringify(color);
            var spaceBlock = new string(c: ' ', width);

            AnsiConsole.Markup($"[default on {hexColor}]{spaceBlock}[/]");
        }

        public void WriteColorLine(Color color, int width)
        {
            var hexColor = ColorNotation.Hex.Stringify(color);
            var spaceBlock = new string(c: ' ', width);

            AnsiConsole.MarkupLine($"[default on {hexColor}]{spaceBlock}[/]");
        }
    }
}
