using System;
using System.Collections.Generic;
using System.Globalization;

namespace Hsluv
{
    // This code comes from hsluv-csharp project
    // It works on .NET 6.0 instead of .NET Framework 4.0
    // I needed to do this, because the 'dotnet pack' command was failing
    public class HsluvConverter
    {
        protected static double[][] M =
        {
            new[] { 3.240969941904521, -1.537383177570093, -0.498610760293 },
            new[] { -0.96924363628087, 1.87596750150772, 0.041555057407175 },
            new[] { 0.055630079696993, -0.20397695888897, 1.056971514242878 },
        };

        protected static double[][] MInv =
        {
            new[] { 0.41239079926595, 0.35758433938387, 0.18048078840183 },
            new[] { 0.21263900587151, 0.71516867876775, 0.072192315360733 },
            new[] { 0.019330818715591, 0.11919477979462, 0.95053215224966 },
        };

        protected static double RefX = 0.95045592705167;
        protected static double RefY = 1.0;
        protected static double RefZ = 1.089057750759878;

        protected static double RefU = 0.19783000664283;
        protected static double RefV = 0.46831999493879;

        protected static double Kappa = 903.2962962;
        protected static double Epsilon = 0.0088564516;

        protected static IList<double[]> GetBounds(double L)
        {
            var result = new List<double[]>();

            var sub1 = Math.Pow(L + 16, y: 3) / 1560896;
            var sub2 = sub1 > Epsilon ? sub1 : L / Kappa;

            for (var c = 0; c < 3; ++c)
            {
                var m1 = M[c][0];
                var m2 = M[c][1];
                var m3 = M[c][2];

                for (var t = 0; t < 2; ++t)
                {
                    var top1 = (284517 * m1 - 94839 * m3) * sub2;
                    var top2 = (838422 * m3 + 769860 * m2 + 731718 * m1) * L * sub2 - 769860 * t * L;
                    var bottom = (632260 * m3 - 126452 * m2) * sub2 + 126452 * t;

                    result.Add(new[] { top1 / bottom, top2 / bottom });
                }
            }

            return result;
        }

        protected static double IntersectLineLine(IList<double> lineA,
            IList<double> lineB)
        {
            return (lineA[index: 1] - lineB[index: 1]) / (lineB[index: 0] - lineA[index: 0]);
        }

        protected static double DistanceFromPole(IList<double> point)
        {
            return Math.Sqrt(Math.Pow(point[index: 0], y: 2) + Math.Pow(point[index: 1], y: 2));
        }

        protected static bool LengthOfRayUntilIntersect(double theta,
            IList<double> line,
            out double length)
        {
            length = line[index: 1] / (Math.Sin(theta) - line[index: 0] * Math.Cos(theta));

            return length >= 0;
        }

        protected static double MaxSafeChromaForL(double L)
        {
            var bounds = GetBounds(L);
            var min = double.MaxValue;

            for (var i = 0; i < 2; ++i)
            {
                var m1 = bounds[i][0];
                var b1 = bounds[i][1];
                var line = new[] { m1, b1 };

                var x = IntersectLineLine(line, new[] { -1 / m1, 0 });
                var length = DistanceFromPole(new[] { x, b1 + x * m1 });

                min = Math.Min(min, length);
            }

            return min;
        }

        protected static double MaxChromaForLH(double L, double H)
        {
            var hrad = H / 360 * Math.PI * 2;

            var bounds = GetBounds(L);
            var min = double.MaxValue;

            foreach (var bound in bounds)
            {
                double length;

                if (LengthOfRayUntilIntersect(hrad, bound, out length))
                {
                    min = Math.Min(min, length);
                }
            }

            return min;
        }

        protected static double DotProduct(IList<double> a,
            IList<double> b)
        {
            double sum = 0;

            for (var i = 0; i < a.Count; ++i) sum += a[i] * b[i];

            return sum;
        }

        protected static double Round(double value, int places)
        {
            var n = Math.Pow(x: 10, places);

            return Math.Round(value * n) / n;
        }

        protected static double FromLinear(double c)
        {
            if (c <= 0.0031308)
            {
                return 12.92 * c;
            }

            return 1.055 * Math.Pow(c, 1 / 2.4) - 0.055;
        }

        protected static double ToLinear(double c)
        {
            if (c > 0.04045)
            {
                return Math.Pow((c + 0.055) / (1 + 0.055), y: 2.4);
            }

            return c / 12.92;
        }

        protected static IList<int> RgbPrepare(IList<double> tuple)
        {
            for (var i = 0; i < tuple.Count; ++i) tuple[i] = Round(tuple[i], places: 3);

            for (var i = 0; i < tuple.Count; ++i)
            {
                var ch = tuple[i];

                if (ch < -0.0001 || ch > 1.0001)
                {
                    throw new Exception("Illegal rgb value: " + ch);
                }
            }

            var results = new int[tuple.Count];

            for (var i = 0; i < tuple.Count; ++i) results[i] = (int)Math.Round(tuple[i] * 255);

            return results;
        }

        public static IList<double> XyzToRgb(IList<double> tuple)
        {
            return new[]
            {
                FromLinear(DotProduct(M[0], tuple)),
                FromLinear(DotProduct(M[1], tuple)),
                FromLinear(DotProduct(M[2], tuple)),
            };
        }

        public static IList<double> RgbToXyz(IList<double> tuple)
        {
            var rgbl = new[]
            {
                ToLinear(tuple[index: 0]),
                ToLinear(tuple[index: 1]),
                ToLinear(tuple[index: 2]),
            };

            return new[]
            {
                DotProduct(MInv[0], rgbl),
                DotProduct(MInv[1], rgbl),
                DotProduct(MInv[2], rgbl),
            };
        }

        protected static double YToL(double Y)
        {
            if (Y <= Epsilon)
            {
                return Y / RefY * Kappa;
            }

            return 116 * Math.Pow(Y / RefY, 1.0 / 3.0) - 16;
        }

        protected static double LToY(double L)
        {
            if (L <= 8)
            {
                return RefY * L / Kappa;
            }

            return RefY * Math.Pow((L + 16) / 116, y: 3);
        }

        public static IList<double> XyzToLuv(IList<double> tuple)
        {
            var X = tuple[index: 0];
            var Y = tuple[index: 1];
            var Z = tuple[index: 2];

            var varU = 4 * X / (X + 15 * Y + 3 * Z);
            var varV = 9 * Y / (X + 15 * Y + 3 * Z);

            var L = YToL(Y);

            if (L == 0)
            {
                return new double[] { 0, 0, 0 };
            }

            var U = 13 * L * (varU - RefU);
            var V = 13 * L * (varV - RefV);

            return new[] { L, U, V };
        }

        public static IList<double> LuvToXyz(IList<double> tuple)
        {
            var L = tuple[index: 0];
            var U = tuple[index: 1];
            var V = tuple[index: 2];

            if (L == 0)
            {
                return new double[] { 0, 0, 0 };
            }

            var varU = U / (13 * L) + RefU;
            var varV = V / (13 * L) + RefV;

            var Y = LToY(L);
            var X = 0 - 9 * Y * varU / ((varU - 4) * varV - varU * varV);
            var Z = (9 * Y - 15 * varV * Y - varV * X) / (3 * varV);

            return new[] { X, Y, Z };
        }

        public static IList<double> LuvToLch(IList<double> tuple)
        {
            var L = tuple[index: 0];
            var U = tuple[index: 1];
            var V = tuple[index: 2];

            var C = Math.Pow(Math.Pow(U, y: 2) + Math.Pow(V, y: 2), y: 0.5);
            var Hrad = Math.Atan2(V, U);

            var H = Hrad * 180.0 / Math.PI;

            if (H < 0)
            {
                H = 360 + H;
            }

            return new[] { L, C, H };
        }

        public static IList<double> LchToLuv(IList<double> tuple)
        {
            var L = tuple[index: 0];
            var C = tuple[index: 1];
            var H = tuple[index: 2];

            var Hrad = H / 360.0 * 2 * Math.PI;
            var U = Math.Cos(Hrad) * C;
            var V = Math.Sin(Hrad) * C;

            return new[] { L, U, V };
        }

        public static IList<double> HsluvToLch(IList<double> tuple)
        {
            var H = tuple[index: 0];
            var S = tuple[index: 1];
            var L = tuple[index: 2];

            if (L > 99.9999999)
            {
                return new[] { 100, 0, H };
            }

            if (L < 0.00000001)
            {
                return new[] { 0, 0, H };
            }

            var max = MaxChromaForLH(L, H);
            var C = max / 100 * S;

            return new[] { L, C, H };
        }

        public static IList<double> LchToHsluv(IList<double> tuple)
        {
            var L = tuple[index: 0];
            var C = tuple[index: 1];
            var H = tuple[index: 2];

            if (L > 99.9999999)
            {
                return new[] { H, 0, 100 };
            }

            if (L < 0.00000001)
            {
                return new[] { H, 0, 0 };
            }

            var max = MaxChromaForLH(L, H);
            var S = C / max * 100;

            return new[] { H, S, L };
        }

        public static IList<double> HpluvToLch(IList<double> tuple)
        {
            var H = tuple[index: 0];
            var S = tuple[index: 1];
            var L = tuple[index: 2];

            if (L > 99.9999999)
            {
                return new[] { 100, 0, H };
            }

            if (L < 0.00000001)
            {
                return new[] { 0, 0, H };
            }

            var max = MaxSafeChromaForL(L);
            var C = max / 100 * S;

            return new[] { L, C, H };
        }

        public static IList<double> LchToHpluv(IList<double> tuple)
        {
            var L = tuple[index: 0];
            var C = tuple[index: 1];
            var H = tuple[index: 2];

            if (L > 99.9999999)
            {
                return new[] { H, 0, 100 };
            }

            if (L < 0.00000001)
            {
                return new[] { H, 0, 0 };
            }

            var max = MaxSafeChromaForL(L);
            var S = C / max * 100;

            return new[] { H, S, L };
        }

        public static string RgbToHex(IList<double> tuple)
        {
            var prepared = RgbPrepare(tuple);

            return string.Format("#{0}{1}{2}",
                prepared[index: 0].ToString("x2"),
                prepared[index: 1].ToString("x2"),
                prepared[index: 2].ToString("x2"));
        }

        public static IList<double> HexToRgb(string hex)
        {
            return new[]
            {
                int.Parse(hex.Substring(startIndex: 1, length: 2), NumberStyles.HexNumber) / 255.0,
                int.Parse(hex.Substring(startIndex: 3, length: 2), NumberStyles.HexNumber) / 255.0,
                int.Parse(hex.Substring(startIndex: 5, length: 2), NumberStyles.HexNumber) / 255.0,
            };
        }

        public static IList<double> LchToRgb(IList<double> tuple)
        {
            return XyzToRgb(LuvToXyz(LchToLuv(tuple)));
        }

        public static IList<double> RgbToLch(IList<double> tuple)
        {
            return LuvToLch(XyzToLuv(RgbToXyz(tuple)));
        }

        // Rgb <--> Hsluv(p)

        public static IList<double> HsluvToRgb(IList<double> tuple)
        {
            return LchToRgb(HsluvToLch(tuple));
        }

        public static IList<double> RgbToHsluv(IList<double> tuple)
        {
            return LchToHsluv(RgbToLch(tuple));
        }

        public static IList<double> HpluvToRgb(IList<double> tuple)
        {
            return LchToRgb(HpluvToLch(tuple));
        }

        public static IList<double> RgbToHpluv(IList<double> tuple)
        {
            return LchToHpluv(RgbToLch(tuple));
        }

        // Hex

        public static string HsluvToHex(IList<double> tuple)
        {
            return RgbToHex(HsluvToRgb(tuple));
        }

        public static string HpluvToHex(IList<double> tuple)
        {
            return RgbToHex(HpluvToRgb(tuple));
        }

        public static IList<double> HexToHsluv(string s)
        {
            return RgbToHsluv(HexToRgb(s));
        }

        public static IList<double> HexToHpluv(string s)
        {
            return RgbToHpluv(HexToRgb(s));
        }
    }
}
