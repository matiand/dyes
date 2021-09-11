using System.Diagnostics;
using System.Reflection;

namespace Dyes.Commands
{
    [Usage("-v, --version, version", "Print version of this program")]
    public class VersionCmd : ICommand
    {
        public void Run(IWriter writer, bool isOutputRedirected)
        {
            var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            writer.WriteLine(version);
        }
    }
}
