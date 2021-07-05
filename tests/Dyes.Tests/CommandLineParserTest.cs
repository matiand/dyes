using System;
using Dyes.Commands;
using Xunit;

namespace Dyes.Tests
{
    public class CommandLineParserTest
    {
        public class VersionCmd
        {
            [Theory]
            [InlineData("version")]
            [InlineData("--version")]
            [InlineData("-v")]
            public void GivenMatchingInput_ReturnsVersionCmd(params string[] args)
            {
                var parser = new CommandLineParser();

                var cmd = parser.Parse(args);

                Assert.IsType<Commands.VersionCmd>(cmd);
            }

            [Theory]
            [InlineData("verion")]
            [InlineData("--v")]
            [InlineData("-ver")]
            [InlineData("-version")]
            public void GivenWrongInput_ThrowsArgumentException(params string[] args)
            {
                var parser = new CommandLineParser();

                Assert.Throws<ArgumentException>(() => parser.Parse(args));
            }
        }

        public class HelpCmd
        {
            [Theory]
            [InlineData("help")]
            [InlineData("--help")]
            [InlineData("-h")]
            public void GivenMatchingInput_ReturnsHelpCmd(params string[] args)
            {
                var parser = new CommandLineParser();

                var cmd = parser.Parse(args);

                Assert.IsType<Commands.HelpCmd>(cmd);
            }

            [Theory]
            [InlineData("hel")]
            [InlineData("--h")]
            [InlineData("-help")]
            public void GivenWrongInput_ThrowsArgumentException(params string[] args)
            {
                var parser = new CommandLineParser();

                Assert.Throws<ArgumentException>(() => parser.Parse(args));
            }
        }

        public class CheckTrueColorSupportCmd
        {
            [Theory]
            [InlineData("check-truecolor-support")]
            public void GivenMatchingInput_ReturnsCheckTrueColorSupportCmd(params string[] args)
            {
                var parser = new CommandLineParser();

                var cmd = parser.Parse(args);

                Assert.IsType<CheckTrueColorSupport>(cmd);
            }
        }


        public class ViewCmd
        {
            [Theory]
            [InlineData("view")]
            public void GivenMatchingInput_ReturnsViewCmd(params string[] args)
            {
                var parser = new CommandLineParser();

                var cmd = parser.Parse(args);

                Assert.IsType<Commands.ViewCmd>(cmd);
            }
        }

        public class CopyCmd
        {
            [Theory]
            [InlineData("copy")]
            public void GivenMatchingInput_ReturnsCopyCmd(params string[] args)
            {
                var parser = new CommandLineParser();

                var cmd = parser.Parse(args);

                Assert.IsType<Commands.CopyCmd>(cmd);
            }
        }

        public class ConvertCmd
        {
            [Theory]
            [InlineData("convert")]
            public void GivenMatchingInput_ReturnsConvertCmd(params string[] args)
            {
                var parser = new CommandLineParser();

                var cmd = parser.Parse(args);

                Assert.IsType<Commands.ConvertCmd>(cmd);
            }
        }
    }
}
