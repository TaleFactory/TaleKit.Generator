using System.Text;
using TaleKit.Generator.Generator;

namespace TaleKit.Generator;

public static class Program
{
    public static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
        if (!Directory.Exists(SharedPath.OutputDirectory))
        {
            Directory.CreateDirectory(SharedPath.OutputDirectory);
        }
        
        MapGenerator.Generate();
        MonsterGenerator.Generate();
        SkillGenerator.Generate();
        ItemGenerator.Generate();
        
        TranslationGenerator.Generate();
    }
}
