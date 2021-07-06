namespace Dyes
{
    public interface IParser<in TInput, out TOutput>
    {
        public TOutput Parse(TInput input);
    }
}
