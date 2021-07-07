using System;
using System.Linq;
using System.Reflection;
using Dyes.Commands;

namespace Dyes
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Console.IsInputRedirected)
            {
                args = args.Append(Console.ReadLine()).ToArray();
            }

            var colorParser = new ColorParser();
            var cliParser = new CommandLineParser(colorParser);
            var consoleWriter = new ConsoleWriter();
            try
            {
                var cmd = cliParser.Parse(args);
                cmd.Run(consoleWriter, Console.IsOutputRedirected);
            }
            catch (ArgumentException e)
            {
                HelpCmd.PrintUsage(consoleWriter);
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (IndexOutOfRangeException)
            {
                HelpCmd.PrintUsage(consoleWriter);
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("xsel"))
                {
                    Console.WriteLine(
                        "Looks like your are running under Linux. Package 'xsel' is required for copying to work.");
                }
                else
                {
                    Console.WriteLine($"Error: {e.Message}");
                }

                Environment.Exit(1);
            }
        }
    }
}
