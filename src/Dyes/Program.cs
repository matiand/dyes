using System;
using System.Linq;
using System.Reflection;

namespace Dyes
{
    class Program
    {
        static void Main(string[] args)
        {
            var colorParser = new ColorParser();
            var cliParser = new CommandLineParser(colorParser);
            var consoleWriter = new ConsoleWriter();

            try
            {
                var cmd = cliParser.Parse(args);
                cmd.Run(consoleWriter);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Environment.Exit(1);
            }
        }
    }
}
