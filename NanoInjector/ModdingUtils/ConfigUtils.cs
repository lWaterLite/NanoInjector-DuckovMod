using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace NanoInjector.ModdingUtils;

public static class ConfigUtils
{
    public static Config? GetConfig()
    {
        string p = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
            "config.json");
        if (!File.Exists(p)) return null;
        using StreamReader file = File.OpenText(p);
        JsonSerializer jsonSerializer = new();
        return jsonSerializer.Deserialize(file, typeof(Config)) as Config;
    }
}

public record Config
{
    public float BuffTime;
}
