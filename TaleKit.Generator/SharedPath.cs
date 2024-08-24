namespace TaleKit.Generator;

public class SharedPath
{
    public static string SourceDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Source");
    public static string OutputDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");
}