using Newtonsoft.Json;
using TaleKit.Generator.Parser;

namespace TaleKit.Generator.Generator;

public class SkillGenerator
{
    public static void Generate()
    {
        var sourcePath = Path.Combine(SharedPath.SourceDirectory, "Skills.txt");  
        var outputPath = Path.Combine(SharedPath.OutputDirectory, "Skills.json");
        
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
            var typeLine = region.GetLine(x => x.StartWith("TYPE"));
            var dataLine = region.GetLine(x => x.StartWith("DATA"));
            var targetLine = region.GetLine(x => x.StartWith("TARGET"));

            var vnum = vnumLine.GetValue<int>(1);
            var name = nameLine.GetValue(1);

            output[vnum] = new
            {
                NameKey = name,
                SkillType = typeLine.GetValue<int>(1),
                CastId = typeLine.GetValue<int>(2),
                CastTime = dataLine.GetValue<int>(5),
                Cooldown = dataLine.GetValue<int>(6),
                MpCost = dataLine.GetValue<int>(7),
                TargetType = targetLine.GetValue<int>(1),
                HitType = targetLine.GetValue<int>(2),
                Range = targetLine.GetValue<short>(3),
                ZoneRange = targetLine.GetValue<short>(4)
            };
        }
        
        var serialized = JsonConvert.SerializeObject(output, Formatting.Indented);

        File.WriteAllText(outputPath, serialized);
    }
}