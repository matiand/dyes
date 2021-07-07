using System;

namespace Dyes.Commands
{
    public class VersionCmd : ICommand
    {
        public void Run(IWriter writer, bool isOutputRedirected)
        {
            var version = typeof(Program).Assembly.GetName().Version;
            writer.WriteLine(version);
        }
    }
}
