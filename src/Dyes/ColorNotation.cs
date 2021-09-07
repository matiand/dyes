using System.Collections.Generic;
using System.Drawing;
using Ardalis.SmartEnum;
using ColorMine.ColorSpaces;
using Hsluv;

namespace Dyes
{
    public abstract class ColorNotation : SmartEnum<ColorNotation>
    {
        public static readonly ColorNotation Hex = new HexType();
        public static readonly ColorNotation Rgb = new RgbType();
        public static readonly ColorNotation Hsl = new HslType();
        public static readonly ColorNotation Hsluv = new HsluvType();
        public static readonly ColorNotation Hpluv = new HpluvType();

        private ColorNotation(string name, int value) : base(name, value)
        {
        }

        public abstract string Stringify(Color color);

        private class HexType : ColorNotation
        {
            public HexType() : base("hex", value: 1)
            {
            }

            public override string Stringify(Color color)
            {
                var hexColor = new Rgb { R = color.R, G = color.G, B = color.B }.To<Hex>();
                return hexColor.Code;
            }
        }

        private class RgbType : ColorNotation
        {
            public RgbType() : base("rgb", value: 2)
            {
            }

            public override string Stringify(Color color)
            {
                return $"rgb({color.R}, {color.G}, {color.B})";
            }
        }

        private class HslType : ColorNotation
        {
            public HslType() : base("hsl", value: 3)
            {
            }

            public override string Stringify(Color color)
            {
                return
                    $"hsl({color.GetHue():N0}, {color.GetSaturation() * 100:N0}%, {color.GetBrightness() * 100:N0}%)";
            }
        }

        private class HsluvType : ColorNotation
        {
            public HsluvType() : base("hsluv", value: 4)
            {
            }

            public override string Stringify(Color color)
            {
                // RgbToHsluv requires rgb values in 0-1 range
                var hslColor = HsluvConverter.RgbToHsluv(new List<double>
                    { color.R / 256.0, color.G / 256.0, color.B / 256.0 });
                return $"hsluv({hslColor[index: 0]:N0}, {hslColor[index: 1]:N0}%, {hslColor[index: 2]:N0}%)";
            }
        }

        private class HpluvType : ColorNotation
        {
            public HpluvType() : base("hpluv", value: 5)
            {
            }

            public override string Stringify(Color color)
            {
                // RgbToHpluv requires rgb values in 0-1 range
                var hplColor = HsluvConverter.RgbToHpluv(new List<double>
                    { color.R / 256.0, color.G / 256.0, color.B / 256.0 });
                return $"hpluv({hplColor[index: 0]:N0}, {hplColor[index: 1]:N0}, {hplColor[index: 2]:N0}%)";
            }
        }
    }
}
