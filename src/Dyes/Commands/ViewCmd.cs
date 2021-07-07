using System;
using System.Collections.Generic;
using ColorMine.ColorSpaces;
using Spectre.Console;
using Color = System.Drawing.Color;

namespace Dyes.Commands
{
    public class ViewCmd : ICommand
    {
        public Color Color { get; }

        public ViewCmd(Color color)
        {
            Color = color;
        }

        public void Run(IWriter writer)
        {
            var notations = new List<ColorNotation>
            {
                ColorNotation.Hex,
                ColorNotation.Rgb,
                ColorNotation.Hsl,
                ColorNotation.Hsluv,
                ColorNotation.Hpluv
            };
            
            foreach (var colorNotation in notations)
            {
                writer.WriteColor(Color, 12);
                writer.WriteLine($"\t{colorNotation.Stringify(Color)}");
            }
        }
    }
}
