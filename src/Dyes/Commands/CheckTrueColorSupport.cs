using System;
using ColorMine.ColorSpaces;
using Spectre.Console;
using Color = System.Drawing.Color;

namespace Dyes.Commands
{
    public class CheckTrueColorSupport : ICommand
    {
        public void Run(IWriter writer, bool isOutputRedirected)
        {
            writer.WriteLine("Your terminal supports TrueColor if these colors are smooth");
            for (int i = 0; i < 360; i++)
            {
                var rgbColor = new Hsl() {H = i, S = 0.75, L = 0.5}.To<Rgb>();
                var color = Color.FromArgb((int) rgbColor.R, (int) rgbColor.G, (int) rgbColor.B);
                writer.WriteColor(color, 1);
                if ((i + 1) % 60 == 0)
                {
                    writer.WriteLine();
                }
            }
        }
    }
}
