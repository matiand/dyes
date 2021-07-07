using System;
using System.Drawing;
using Xunit;

namespace Dyes.Tests
{
    public class ColorParserTests
    {
        public class HexColors
        {
            [Theory]
            [InlineData("#fafafa")]
            [InlineData("#fff")]
            [InlineData("#ABCDEF")]
            [InlineData("00A3D1")]
            [InlineData("aaa")]
            public void OnGoodInput_ReturnsColorInstance(string input)
            {
                var parser = new ColorParser();

                var actual = parser.Parse(input);

                Assert.IsType<Color>(actual);
            }

            [Theory]
            [InlineData("#-1afde1")]
            [InlineData("#ffeed")]
            [InlineData("#ffed")]
            [InlineData("#ABGG11")]
            [InlineData("#ed221a1")]
            [InlineData("eeff1")]
            [InlineData("egg")]
            public void OnWrongInput_ThrowsArgumentException(string input)
            {
                var parser = new ColorParser();

                Assert.Throws<ArgumentException>(() => parser.Parse(input));
            }

            [Fact]
            public void ReturnedColorHasCorrectRgbValues()
            {
                var parser = new ColorParser();

                var actualLong = parser.Parse("#43afbb");
                var expectedLong = Color.FromArgb(255, 67, 175, 187);
                var actualShort = parser.Parse("#39f");
                var expectedShort = Color.FromArgb(255, 51, 153, 255);

                Assert.Equal(expectedLong, actualLong);
                Assert.Equal(expectedShort, actualShort);
            }
        }

        public class RgbColors
        {
            [Theory]
            [InlineData("rgb(12, 32, 44)")]
            [InlineData("rgb(255,255,255)")]
            [InlineData("rgb(0, 0, 0)")]
            public void OnGoodInput_ReturnsColorInstance(string input)
            {
                var parser = new ColorParser();

                var actual = parser.Parse(input);

                Assert.IsType<Color>(actual);
            }

            [Theory]
            [InlineData("rgb(12, 32)")]
            [InlineData("rgb(256, 0, 0)")]
            [InlineData("rbg(0, 0, 0)")]
            public void OnWrongInput_ThrowsArgumentException(string input)
            {
                var parser = new ColorParser();

                Assert.Throws<ArgumentException>(() => parser.Parse(input));
            }

            [Fact]
            public void ReturnedColorHasCorrectRgbValues()
            {
                var parser = new ColorParser();

                var actual = parser.Parse("rgb(67, 175, 187)");
                var expected = Color.FromArgb(255, 67, 175, 187);

                Assert.Equal(expected, actual);
            }
        }

        public class HueBasedColors
        {
            [Theory]
            [InlineData("hsl(12, 32%, 44%)")]
            [InlineData("hsl(12.12, 32.444%, 44.1%)")]
            [InlineData("hsluv(360, 0%, 100%)")]
            [InlineData("hpluv(44.4, 0, 0%)")]
            public void OnGoodInput_ReturnsColorInstance(string input)
            {
                var parser = new ColorParser();

                var actual = parser.Parse(input);

                Assert.IsType<Color>(actual);
            }

            [Theory]
            [InlineData("hslu(12, 32)")]
            [InlineData("hsl(100, 100, 100)")]
            [InlineData("hsluv(361, 0%, -10%)")]
            [InlineData("hpluv(-0.1, 0, 101%)")]
            [InlineData("hpl(0, 0, 0")]
            public void OnWrongInput_ThrowsArgumentException(string input)
            {
                var parser = new ColorParser();

                Assert.Throws<ArgumentException>(() => parser.Parse(input));
            }

            [Fact]
            public void ReturnedColorHasCorrectRgbValues()
            {
                var parser = new ColorParser();

                var actualHsl = parser.Parse("hsl(30, 50%, 40%)");
                var expectedHsl = Color.FromArgb(255, 153, 102, 51);
                var actualHsluv = parser.Parse("hsluv(30, 50.1%, 39.9%)");
                var expectedHsluv = Color.FromArgb(255, 128, 83, 65);
                var actualHpluv = parser.Parse("hpluv(30, 50, 40%)");
                var expectedHpluv = Color.FromArgb(255, 111, 90, 83);

                Assert.Equal(expectedHsl, actualHsl);
                Assert.Equal(expectedHsluv, actualHsluv);
                Assert.Equal(expectedHpluv, actualHpluv);
            }
        }

        [Theory]
        [InlineData("hsl(13 34% 44.4%)")]
        [InlineData("hsluv(13 34% 44.4%)")]
        [InlineData("hpluv(13 34 44.4%)")]
        [InlineData("rgb(13 80 90)")]
        public void SupportsCssVariationSyntax(string input)
        {
            var parser = new ColorParser();

            var color = parser.Parse(input);

            Assert.IsType<Color>(color);
        }
    }
}
