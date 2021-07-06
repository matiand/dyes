using System.Drawing;

namespace Dyes.Commands
{
    public class ViewCmd : ICommand
    {
        public Color Color { get; }

        public ViewCmd(Color color)
        {
            Color = color;
        }

        public void Run()
        {
            throw new System.NotImplementedException();
        }
    }
}
