using System.Drawing;

namespace Dyes.Commands
{
    [Usage("convert <notation> <color>", "Converts color to different notation (hex, rgb, hsl, hsluv, hpluv)",
        "convert hsl #fff")]
    public class ConvertCmd : ICommand
    {
        public Color Color { get; }

        public ColorNotation ColorNotation { get; }

        public ConvertCmd(Color color, ColorNotation colorNotation)
        {
            Color = color;
            ColorNotation = colorNotation;
        }

        public void Run(IWriter writer, bool isOutputRedirected)
        {
            writer.WriteLine(ColorNotation.Stringify(Color));
        }
    }
}
