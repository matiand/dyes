using System.Drawing;
using Ardalis.SmartEnum;

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
            public HexType() : base("hex", 1)
            {
            }

            public override string Stringify(Color color)
            {
                throw new System.NotImplementedException();
            }
        }

        private class RgbType : ColorNotation
        {
            public RgbType() : base("rgb", 2)
            {
            }

            public override string Stringify(Color color)
            {
                throw new System.NotImplementedException();
            }
        }

        private class HslType : ColorNotation
        {
            public HslType() : base("hsl", 3)
            {
            }

            public override string Stringify(Color color)
            {
                throw new System.NotImplementedException();
            }
        }

        private class HsluvType : ColorNotation
        {
            public HsluvType() : base("hsluv", 4)
            {
            }

            public override string Stringify(Color color)
            {
                throw new System.NotImplementedException();
            }
        }

        private class HpluvType : ColorNotation
        {
            public HpluvType() : base("hpluv", 5)
            {
            }

            public override string Stringify(Color color)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
