using Newtonsoft.Json;
using TaleKit.Generator.Parser;

namespace TaleKit.Generator.Generator;

public class ItemGenerator
{
    public static void Generate()
    {
        var sourcePath = Path.Combine(SharedPath.SourceDirectory, "Items.txt");  
        var outputPath = Path.Combine(SharedPath.OutputDirectory, "Items.json");
        
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
            var firstLine = region.GetLine(x => x.StartWith("VNUM"));
            var secondLine = region.GetLine(x => x.StartWith("NAME"));
            var indexLine = region.GetLine(x => x.StartWith("INDEX"));
            var dataLine = region.GetLine(x => x.StartWith("DATA"));

            var vnum = firstLine.GetValue<int>(1);
            var name = secondLine.GetValue(1);
            var inventoryType = indexLine.GetValue<int>(1);
            var type = indexLine.GetValue<int>(2);
            var subType = indexLine.GetValue<int>(3);
            var imageId = indexLine.GetValue<int>(5);
            var data = dataLine.GetValues().Skip(1).ToArray();

            switch (inventoryType)
            {
                case 4:
                    inventoryType = 0;
                    break;
                case 8:
                    inventoryType = 0;
                    break;
                case 9:
                    inventoryType = 1;
                    break;
                case 10:
                    inventoryType = 2;
                    break;
            }

            output[vnum] = new
            {
                Id = vnum,
                NameKey = name,
                BagType = inventoryType,
                Type = type,
                SubType = subType,
                ImageId = imageId
            };
        }
        
        var serialized = JsonConvert.SerializeObject(output, Formatting.Indented);

        File.WriteAllText(outputPath, serialized);
    }
}