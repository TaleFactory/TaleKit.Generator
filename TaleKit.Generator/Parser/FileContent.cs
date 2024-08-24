namespace TaleKit.Generator.Parser;

public class FileContent : FileRegion
{
    public FileContent(IEnumerable<FileLine> lines) : base(lines)
    {
    }
}