using TextCopy;

namespace Dyes.Commands
{
    public class CopyCmd : ICommand
    {
        public string TextToCopy { get; }

        public CopyCmd(string textToCopy)
        {
            TextToCopy = textToCopy;
        }

        public void Run(IWriter writer, bool isOutputRedirected)
        {
            ClipboardService.SetText(TextToCopy);
            writer.WriteLine($"Copied {TextToCopy}");
        }
    }
}
