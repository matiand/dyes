using System;

namespace Dyes.Commands
{
    public class VersionCmd : ICommand
    {
        public void Run(IWriter writer)
        {
            var version = typeof(Program).Assembly.GetName().Version;
            writer.WriteLine(version);
        }
    }
}
