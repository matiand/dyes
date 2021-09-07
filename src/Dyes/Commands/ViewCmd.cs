using System.Collections.Generic;
using System.Drawing;

namespace Dyes.Commands
{
    [Usage("view <color>", "View color and show its value in other formats", "view rgb(40, 120, 100)")]
    public class ViewCmd : ICommand
    {
        public ViewCmd(Color color)
        {
            Color = color;
        }

        public Color Color { get; }

        public void Run(IWriter writer, bool isOutputRedirected)
        {
            if (isOutputRedirected)
            {
                writer.WriteLine(ColorNotation.Hex.Stringify(Color));
                return;
            }

            var notations = new List<ColorNotation>
            {
                ColorNotation.Hex,
                ColorNotation.Rgb,
                ColorNotation.Hsl,
                ColorNotation.Hsluv,
                ColorNotation.Hpluv,
            };

            foreach (var colorNotation in notations)
            {
                writer.WriteColor(Color, width: 12);
                writer.WriteLine($"\t{colorNotation.Stringify(Color)}");
            }
        }
    }
}
