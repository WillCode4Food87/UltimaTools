namespace FileParser.Services.Parse
{
    public interface IParser<T>
    {
        List<T> Parse(IEnumerable<string> filePaths);
    }
}
