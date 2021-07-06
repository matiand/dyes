using System.Drawing;

namespace Dyes.Commands
{
    public class ConvertCmd:ICommand
    {
        public Color Color { get; }
        public ColorNotation ColorNotation { get; }

        public ConvertCmd(Color color, ColorNotation colorNotation)
        {
            Color = color;
            ColorNotation = colorNotation;
        }

        public void Run()
        {
            throw new System.NotImplementedException();
        }
    }
}
