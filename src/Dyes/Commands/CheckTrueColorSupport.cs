using System.Drawing;
using ColorMine.ColorSpaces;

namespace Dyes.Commands
{
    [Usage("check-truecolor-support", "Checks if this terminal supports TrueColor")]
    public class CheckTrueColorSupportCmd : ICommand
    {
        public void Run(IWriter writer, bool isOutputRedirected)
        {
            writer.WriteLine("Your terminal supports TrueColor if these colors are smooth");
            for (var i = 0; i < 360; i++)
            {
                var rgbColor = new Hsl { H = i, S = 0.75, L = 0.5 }.To<Rgb>();
                var color = Color.FromArgb((int)rgbColor.R, (int)rgbColor.G, (int)rgbColor.B);
                writer.WriteColor(color, width: 1);
                if ((i + 1) % 60 == 0)
                {
                    writer.WriteLine();
                }
            }
        }
    }
}
