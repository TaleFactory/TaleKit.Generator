using System.Text;
using Newtonsoft.Json;
using TaleKit.Generator.Parser;

namespace TaleKit.Generator.Generator;

public static class TranslationGenerator
{
    public static void Generate()
    {
        var sourcePath = Path.Combine(SharedPath.SourceDirectory, "Translation");  
        var outputPath = Path.Combine(SharedPath.OutputDirectory, "Translation");

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
    
        if (!Directory.Exists(sourcePath))
        {
            throw new InvalidOperationException();
        }

        var directories = Directory.EnumerateDirectories(sourcePath);
        foreach (var directory in directories)
        {
            var directoryName = Path.GetFileName(directory.TrimEnd(Path.DirectorySeparatorChar));
            
            var files = Directory.EnumerateFiles(directory);
            foreach (var file in files)
            {
                var name = Path.GetFileName(file).Replace(".txt", "");

                var content = FileReader.FromFile(file)
                    .SkipEmptyLines()
                    .SkipCommentedLines("#")
                    .TrimLines()
                    .SplitLineContent('\t')
                    .GetContent();

                var output = new Dictionary<string, string>();
                foreach (var line in content.Lines)
                {
                    var key = line.GetValue(0);
                    var value = line.GetValue(1);

                    output[key] = value.Replace("^", " ");
                }

                var path = Path.Combine(outputPath, directoryName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                var serialized = JsonConvert.SerializeObject(output, Formatting.Indented);

                File.WriteAllText(Path.Combine(path, $"{name}.json"), serialized);
            }
        }
    }
}