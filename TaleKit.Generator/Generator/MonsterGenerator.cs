using Newtonsoft.Json;
using TaleKit.Generator.Parser;

namespace TaleKit.Generator.Generator;

public class MonsterGenerator
{
    public static void Generate()
    {
        var sourcePath = Path.Combine(SharedPath.SourceDirectory, "Monsters.txt");
        var outputPath = Path.Combine(SharedPath.OutputDirectory, "Monsters.json");

        var content = FileReader.FromFile(sourcePath)
            .SkipEmptyLines()
            .SkipCommentedLines("#")
            .TrimLines()
            .SplitLineContent('\t')
            .GetContent();

        var regions = content.GetRegions("VNUM");

        var output = new Dictionary<int, object>();
        foreach (var region in regions)
        {
            var vnumLine = region.GetLine(x => x.StartWith("VNUM"));
            var nameLine = region.GetLine(x => x.StartWith("NAME"));
            var levelLine = region.GetLine(x => x.StartWith("LEVEL"));

            var vnum = vnumLine.GetValue<int>(1);
            var name = nameLine.GetValue(1);
            var level = levelLine.GetValue<int>(1);

            output[vnum] = new
            {
                NameKey = name,
                Level = level
            };
        }

        var serialized = JsonConvert.SerializeObject(output, Formatting.Indented);

        File.WriteAllText(outputPath, serialized);
    }
}