using System;

namespace Dyes.Commands
{
    [Usage("-v, --version, version", "Print version of this program")]
    public class VersionCmd : ICommand
    {
        public void Run(IWriter writer, bool isOutputRedirected)
        {
            var version = typeof(Program).Assembly.GetName().Version;
            writer.WriteLine(version);
        }
    }
}
