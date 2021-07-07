using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using ColorMine.ColorSpaces;
using Hsluv;

namespace Dyes
{
    public class ColorParser : IParser<string, Color>
    {
        private readonly Regex _hexPattern = new(@"^#?(?:[0-9a-fA-F]{3}){1,2}$");
        private readonly Regex _rgbPattern = new(@"^rgb\((?<red>\d+)[,\s]+(?<green>\d+)[,\s]+(?<blue>\d+)\)$");

        private readonly Regex _hslPattern =
            new(@"^(hsl|hpluv|hsluv)\((?<hue>.+?)[,\s]+(?<sat>.+?)%?[,\s]+(?<light>.+?)%\)$");

        public Color Parse(string input)
        {
            if (_hexPattern.IsMatch(input))
            {
                var hexCode = input.Length is 3 or 6 ? $"#{input}" : input;
                return ColorTranslator.FromHtml(hexCode);
            }

            if (_rgbPattern.IsMatch(input))
            {
                return ParseRgbColor(input);
            }

            if (_hslPattern.IsMatch(input))
            {
                return ParseHslBasedColor(input);
            }

            throw new ArgumentException("Unrecognized color");
        }

        private Color ParseHslBasedColor(string input)
        {
            var match = _hslPattern.Match(input);
            var hue = double.Parse(match.Groups["hue"].Value);
            var saturation = double.Parse(match.Groups["sat"].Value);
            var lightness = double.Parse(match.Groups["light"].Value);

            if (hue is < 0 or > 360) throw new ArgumentException("Hue has wrong value");
            if (saturation is < 0 or > 100) throw new ArgumentException("Saturation has wrong value");
            if (lightness is < 0 or > 100) throw new ArgumentException("Lightness has wrong value");

            if (input.Contains("hsluv"))
            {
                var hexString = HsluvConverter.HsluvToHex(new List<double> {hue, saturation, lightness});
                return Parse(hexString);
            }
            else if (input.Contains("hpluv"))
            {
                var hexString = HsluvConverter.HpluvToHex(new List<double> {hue, saturation, lightness});
                return Parse(hexString);
            }

            var hexColor = new Hsl() {H = hue, S = saturation / 100.0, L = lightness / 100.0}.To<Hex>();
            return Parse(hexColor.Code);
        }

        private Color ParseRgbColor(string input)
        {
            var match = _rgbPattern.Match(input);
            var red = int.Parse(match.Groups["red"]
                .Value);
            var green = int.Parse(match.Groups["green"]
                .Value);
            var blue = int.Parse(match.Groups["blue"]
                .Value);

            return Color.FromArgb(255, red, green, blue);
        }
    }
}
