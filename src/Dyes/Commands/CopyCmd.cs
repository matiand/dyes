using System.Drawing;

namespace Dyes.Commands
{
    public class CopyCmd:ICommand
    {
        public Color Color { get; }

        public CopyCmd(Color color)
        {
            Color = color;
        }

        public void Run()
        {
            throw new System.NotImplementedException();
        }
    }
}
