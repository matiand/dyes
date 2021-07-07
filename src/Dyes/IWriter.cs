using System;
using Spectre.Console;
using Color = System.Drawing.Color;

namespace Dyes
{
    public interface IWriter
    {
        void Write(object input);
        void WriteLine(object input);
        void WriteLine();
        void WriteColor(Color color, int width);
        void WriteColorLine(Color color, int width);
    }
}
