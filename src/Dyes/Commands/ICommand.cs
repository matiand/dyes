namespace Dyes.Commands
{
    public interface ICommand
    {
        public void Run(IWriter writer, bool isOutputRedirected = false);
    }
}
