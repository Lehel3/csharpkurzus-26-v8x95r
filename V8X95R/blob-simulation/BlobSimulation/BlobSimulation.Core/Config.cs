using System;
using System.Collections.Generic;


namespace BlobSimulation.Core
{
    public class Config
    {
        #region --- Properties ---

        // Seed
        public int Seed { get; set; } = 0;
        public bool UseRandomSeed { get; set; } = false;


        // Engine
        public int TickRate { get; set; } = 20;
        public int MaxSteps { get; set; } = -1;


        // World
        public int WorldSizeX { get; set; } = 20;
        public int WorldSizeY { get; set; } = 20;


        // FOOD
        public int StartFoodCount { get; set; } = 5;
        public float FoodSpawnRate { get; set; } = 0.1f;
        public int MaxFoodCount { get; set; } = 50;


        // Blob Parameters
        public int StartBlobCount { get; set; } = 5;


        public float InitialEnergy { get; set; } = 100f;
        public float BlobSpeed { get; set; } = 5f;


        // Reproduction
        public float ReproduceChance { get; set; } = 0.1f;
        public float ReproduceEnergyThreshold { get; set; } = 100f;
        public float ReproduceEnergyCost { get; set; } = 30f;


        public float HungerThreshold { get; set; } = 100f;

        public float EnergyLossFactor { get; set; } = 0.1f;

        #endregion


        public Config() {}


        public void Validate()
        {
            List<string> errors = new List<string>();

            if (WorldSizeX <= 0 || WorldSizeY <= 0) errors.Add("world_size must be positive");
            if (StartBlobCount < 0) errors.Add("blob_count must be positive");
            if (InitialEnergy <= 0) errors.Add("initial_energy must be positive");
            if (BlobSpeed <= 0) errors.Add("blob_speed must be positive");
            if (TickRate <= 0) errors.Add("tick_rate must be positive");


            if (errors.Count > 0)
            {
                throw new ArgumentException("Invalid configuration:\n" + string.Join("\n", errors));
            }
        }


        #region --- Args Override ---
        /*
        // from file with args override
        public static Config LoadAndParse(string[] args)
        {
            Config config = new Config(); // Alapértelmezett értékek
            string configPath = ".\\Configs\\config.json";
            bool customConfigRequested = false;

            // searching for --config arg
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--config" && i + 1 < args.Length)
                {
                    configPath = args[i + 1];
                    customConfigRequested = true;
                    break;
                }
            }

            // config loading
            if (File.Exists(configPath))
            {
                string jsonString = File.ReadAllText(configPath);
                config = JsonSerializer.Deserialize<Config>(jsonString) ?? config;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[Rendszer] Config fájl sikeresen betöltve: {configPath}");
                Console.ResetColor();
            }
            else if (customConfigRequested)
            {
                // Ha a user kért egy fájlt, de nincs ott, állítsuk meg a programot
                string fullPath = Path.GetFullPath(configPath);
                throw new FileNotFoundException($"A megadott config fájl nem található! Keresett útvonal:\n{fullPath}");
            }


            // DINAMIKUS FELÜLÍRÁS
            OverrideFromArgs(config, args);

            config.Validate();
            return config;
        }

        private static void OverrideFromArgs(Config config, string[] args)
        {
            foreach (var arg in args)
            {
                // Elvárjuk a --ParamName=Érték formátumot
                if (arg.StartsWith("--") && arg.Contains("="))
                {
                    var parts = arg.Substring(2).Split('=');
                    var propName = parts[0];
                    var value = parts[1];

                    var prop = typeof(Config).GetProperty(propName,
                        System.Reflection.BindingFlags.IgnoreCase |
                        System.Reflection.BindingFlags.Public |
                        System.Reflection.BindingFlags.Instance);

                    if (prop != null)
                    {
                        try
                        {
                            var typedValue = Convert.ChangeType(value, prop.PropertyType);
                            prop.SetValue(config, typedValue);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"[Rendszer] CLI Felülírás: {propName} = {value}");
                            Console.ResetColor();
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"[Hiba] Nem sikerült konvertálni: {propName} = {value}");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        // parameter nincs classban, --config-ot kihagyni hibauzenet
                        if (propName.ToLower() != "config")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"[Figyelem] Ismeretlen paraméter: {propName} (Ellenőrizd az írásmódot!)");
                            Console.ResetColor();
                        }
                    }
                }
            }
        }
        */
        #endregion


    }
}
