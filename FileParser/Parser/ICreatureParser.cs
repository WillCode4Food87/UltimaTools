using FileParser.Models;

namespace FileParser.Parser
{
    public interface ICreatureParser
    {
        CreatureData? Parse(string filePath);
    }
}
