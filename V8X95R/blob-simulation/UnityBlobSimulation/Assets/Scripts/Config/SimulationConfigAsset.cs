using UnityEngine;
using BlobSimulation.Core;

[CreateAssetMenu(fileName ="Config", menuName = "Simulation/Config")]
public class SimulationConfigAsset : ScriptableObject {

    [Header("Startup Settings")]
    public int seed = 0;
    public bool useRandomSeed = false;

    public int worldSizeX = 20;
    public int worldSizeY = 20;

    public int startFoodCount = 10;
    public int startBlobCount = 5;

    public int maxSteps = -1;


    [Header("Blob Start Settings")]
    public float initialEnergy = 100f;




    [Header("Runtime Settings")]
    public int tickRate = 20;


    public float foodSpawnRate = 0.1f;
    public int maxFoodCount = 50;
    
    //blob stats (until no mutation)
    public float blobSpeed = 1f;

    //energy
    public float reproduceChance = 0.05f;
    public float reproduceEnergyThreshold = 100f;
    public float reproduceEnergyCost = 30f;

    public float hungerThreshold = 100f;
    public float energyLossFactor = 1f;




    public Config ToCoreConfig() {
        var config = new Config {
            Seed = seed,
            UseRandomSeed = useRandomSeed,

            WorldSizeX = worldSizeX,
            WorldSizeY = worldSizeY,

            StartBlobCount = startBlobCount,
            StartFoodCount = startFoodCount,

            InitialEnergy = initialEnergy,

            // Runtime settings
            TickRate = tickRate,
            MaxSteps = maxSteps,

            BlobSpeed = blobSpeed,
            FoodSpawnRate = foodSpawnRate,
            MaxFoodCount = maxFoodCount,

            ReproduceChance = reproduceChance,
            ReproduceEnergyThreshold = reproduceEnergyThreshold,
            ReproduceEnergyCost = reproduceEnergyCost,
            
            HungerThreshold = hungerThreshold,
            EnergyLossFactor = energyLossFactor,

        };

        config.Validate();
        Debug.Log("Config loaded successfully.");
        return config;
    }
}