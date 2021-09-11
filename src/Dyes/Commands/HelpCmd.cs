using System;
using System.Linq;
using System.Reflection;

namespace Dyes.Commands
{
    [Usage("-h, --help, help", "Print this message")]
    public class HelpCmd : ICommand
    {
        public void Run(IWriter writer, bool isOutputRedirected)
        {
            PrintUsage(writer);
        }

        public static void PrintUsage(IWriter writer)
        {
            var programName = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyTitleAttribute>()!.Title;
            writer.WriteLine($"Usage of {programName}:");

            foreach (var usage in GetCmdUsages())
            {
                writer.WriteLine($"  {usage.CmdWithArgs}");
                writer.WriteLine($"\t{usage.Description}");
                if (usage.Example.Length > 0) writer.WriteLine($"\tEXAMPLE: {programName} {usage.Example}");
            }

            PrintSupportedColorSyntax(writer);
        }

        private static void PrintSupportedColorSyntax(IWriter writer)
        {
            writer.WriteLine("\n  Supported color syntax:");
            writer.WriteLine("\tHex: #ffee12, #ACF, ffee12, acf");
            writer.WriteLine("\tRgb: rgb(0, 0, 0), rgb(20 40 60)");
            writer.WriteLine("\tHsl: hsl(0, 0%, 0%), hsl(100 75% 50%)");
            writer.WriteLine("\tHsluv: hsluv(0, 0%, 0%), hsluv(100 75% 50%)");
            writer.WriteLine("\tHpluv: hpluv(0, 0%, 0%), hpluv(100 225% 50%)");
        }

        private static IOrderedEnumerable<UsageAttribute> GetCmdUsages()
        {
            var commands = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsAbstract);

            var usages =
                commands.SelectMany(cmd => cmd.GetCustomAttributes())
                    .Select(attr => attr as UsageAttribute)
                    .Where(attr => attr is not null)
                    .OrderBy(attr => attr.CmdWithArgs);

            return usages;
        }
    }
}
