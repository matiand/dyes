using Moq;
using Xunit;

namespace Dyes.Tests
{
    public class ConvertCmdTests
    {
        [Theory]
        [InlineData("hsl(12, 75%, 50%)", "#DF4620")]
        [InlineData("rgb(30, 60, 90)", "#1E3C5A")]
        [InlineData("hsluv(300, 100%, 50%)", "#CD00E2")]
        public void ConvertsToHexNotation(string color, string expected)
        {
            var writerMock = new Mock<IWriter>();
            var parser = new CommandLineParser();
            var cmd = parser.Parse(new[] { "convert", "hex", color });

            cmd.Run(writerMock.Object);
            var actual = writerMock.Invocations[index: 0].Arguments[index: 0];

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("#1E3C5A", "rgb(30, 60, 90)")]
        [InlineData("hsl(12, 75%, 50%)", "rgb(223, 70, 32)")]
        [InlineData("hsluv(100, 100%, 50%)", "rgb(106, 128, 0)")]
        public void ConvertsToRgbNotation(string color, string expected)
        {
            var writerMock = new Mock<IWriter>();
            var parser = new CommandLineParser();
            var cmd = parser.Parse(new[] { "convert", "rgb", color });

            cmd.Run(writerMock.Object);
            var actual = writerMock.Invocations[index: 0].Arguments[index: 0];

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("#DF4620", "hsl(12, 75%, 50%)")]
        [InlineData("rgb(120, 60, 90)", "hsl(330, 33%, 35%)")]
        [InlineData("hsluv(50, 100%, 50%)", "hsl(41, 100%, 31%)")]
        public void ConvertsToHslNotation(string color, string expected)
        {
            var writerMock = new Mock<IWriter>();
            var parser = new CommandLineParser();
            var cmd = parser.Parse(new[] { "convert", "hsl", color });

            cmd.Run(writerMock.Object);
            var actual = writerMock.Invocations[index: 0].Arguments[index: 0];

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("#DF4620", "hsluv(17, 92%, 52%)")]
        [InlineData("rgb(200, 100, 50)", "hsluv(26, 83%, 53%)")]
        [InlineData("hsl(150, 50%, 50%)", "hsluv(144, 86%, 69%)")]
        public void ConvertsToHsluvNotation(string color, string expected)
        {
            var writerMock = new Mock<IWriter>();
            var parser = new CommandLineParser();

            var cmd = parser.Parse(new[] { "convert", "hsluv", color });

            cmd.Run(writerMock.Object);
            var actual = writerMock.Invocations[index: 0].Arguments[index: 0];

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("#DF4620", "hpluv(17, 314, 52%)")]
        [InlineData("rgb(200, 100, 50)", "hpluv(26, 212, 53%)")]
        [InlineData("hsl(150, 50%, 50%)", "hpluv(144, 118, 69%)")]
        public void ConvertsToHpluvNotation(string color, string expected)
        {
            var writerMock = new Mock<IWriter>();
            var parser = new CommandLineParser();

            var cmd = parser.Parse(new[] { "convert", "hpluv", color });

            cmd.Run(writerMock.Object);
            var actual = writerMock.Invocations[index: 0].Arguments[index: 0];

            Assert.Equal(expected, actual);
        }
    }
}
