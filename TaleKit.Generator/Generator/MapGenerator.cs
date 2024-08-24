using Newtonsoft.Json;
using TaleKit.Generator.Parser;

namespace TaleKit.Generator.Generator;

public class MapGenerator
{
    public static void Generate()
    {
        var sourcePath = Path.Combine(SharedPath.SourceDirectory, "Maps.txt");  
        var outputPath = Path.Combine(SharedPath.OutputDirectory, "Maps.json");
        
        var content = FileReader.FromFile(sourcePath)
            .SkipEmptyLines()
            .SkipCommentedLines("#")
            .SkipLines(x => x.StartsWith("DATA"))
            .TrimLines()
            .SplitLineContent(' ')
            .GetContent();

        var output = new Dictionary<int, object>();
        foreach (var line in content.Lines)
        {
            var firstId = line.GetValue<int>(0);
            var secondId = line.GetValue<int>(1);
            var name = line.GetLastValue();
            
            if (firstId == secondId)
            {
                var file = Path.Combine(SharedPath.SourceDirectory, "Grid", $"{firstId}.bin");
                
                output[firstId] = new
                {
                    NameKey = name,
                    Grid = File.Exists(file) ? File.ReadAllBytes(file) : []
                };
                continue;
            }
            
            for (var i = firstId; i < secondId; i++)
            {
                var file = Path.Combine(SharedPath.SourceDirectory, "Grid", $"{firstId}.bin");
                
                output[firstId] = new
                {
                    NameKey = name,
                    Grid = File.Exists(file) ? File.ReadAllBytes(file) : []
                };
            }
        }
        
        var serialized = JsonConvert.SerializeObject(output, Formatting.Indented);

        File.WriteAllText(outputPath, serialized);
    }
}