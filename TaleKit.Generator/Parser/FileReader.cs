using System.Text;

namespace TaleKit.Generator.Parser;

public class FileReader
{
    private readonly string[] _content;
    private readonly List<Predicate<string>> _skipConditions;
    private char _separator;

    private bool _trim;

    private FileReader(string[] content)
    {
        _content = content;
        _skipConditions = new List<Predicate<string>>();
    }

    public static FileReader FromString(string content) => new FileReader(content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
    public static FileReader FromFile(string path) => new FileReader(File.ReadAllLines(path, Encoding.GetEncoding(1252)));

    public FileReader SkipEmptyLines() => SkipLines(string.IsNullOrEmpty);

    public FileReader SkipCommentedLines(string commentTag) => SkipLines(x => x.StartsWith(commentTag));

    public FileReader TrimLines()
    {
        _trim = true;
        return this;
    }

    public FileReader SplitLineContent(char separator)
    {
        _separator = separator;
        return this;
    }

    public FileReader SkipLines(Predicate<string> predicate)
    {
        _skipConditions.Add(predicate);
        return this;
    }

    public FileContent GetContent()
    {
        var lines = new List<FileLine>();
        foreach (var line in _content)
        {
            if (_skipConditions.Any(x => x.Invoke(line)))
            {
                continue;
            }

            var content = line;

            if (_trim)
            {
                content = content.Trim();
            }

            lines.Add(new FileLine(content.Split(_separator), _separator));
        }

        return new FileContent(lines);
    }
}