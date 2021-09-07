using TextCopy;

namespace Dyes.Commands
{
    [Usage("copy <color>", "Copies given color to clipboard", "copy hsl(120, 50%, 50%)")]
    public class CopyCmd : ICommand
    {
        public CopyCmd(string textToCopy)
        {
            TextToCopy = textToCopy;
        }

        public string TextToCopy { get; }

        public void Run(IWriter writer, bool isOutputRedirected)
        {
            ClipboardService.SetText(TextToCopy);
            writer.WriteLine($"Copied {TextToCopy}");
        }
    }
}
