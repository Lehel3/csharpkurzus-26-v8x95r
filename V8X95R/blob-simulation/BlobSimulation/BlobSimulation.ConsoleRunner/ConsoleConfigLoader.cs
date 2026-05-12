using BlobSimulation.Core;
using BlobSimulation.Core.Logger;
using System.Text.Json;


namespace BlobSimulation.ConsoleRunner {
    public static class ConsoleConfigLoader {

        public static Config Load(ILogger Logger, string path = ".\\Configs\\config.json") {
            string fullPath = Path.GetFullPath(path);

            if (!File.Exists(path))
                throw new FileNotFoundException($"Config file not found at:\n\"{fullPath}\"");

            string jsonString = File.ReadAllText(path);

            var options = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true
            };

            Config config = JsonSerializer.Deserialize<Config>(jsonString, options)!;

            config.Validate();

            Logger.Log($"Config file loaded from: \"{fullPath}\"", LogLevel.Success, "System");

            return config;
        }

    }
}
