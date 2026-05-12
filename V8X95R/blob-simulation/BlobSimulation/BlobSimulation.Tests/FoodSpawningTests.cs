using BlobSimulation.Core;
using System.Linq;
using Xunit;

namespace BlobSimulation.Tests
{
    public class FoodSpawningTests
    {
        [Fact]
        public void Food_SpawnsConfiguredRate_PerTick()
        {
            var config = new Config
            {
                WorldSizeX = 20,
                WorldSizeY = 20,
                StartBlobCount = 0,
                StartFoodCount = 0,
                FoodSpawnRate = 3f,
                MaxFoodCount = 100
            };

            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);

            sim.Step();

            Assert.Equal(3, sim.World.GetFoods.Count());
        }

        [Fact]
        public void Food_DoesNotSpawn_WhenWorldAlreadyAtMaxFoodCount()
        {
            var config = new Config
            {
                WorldSizeX = 20,
                WorldSizeY = 20,
                StartBlobCount = 0,
                StartFoodCount = 5,
                FoodSpawnRate = 3f,
                MaxFoodCount = 5
            };

            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);

            sim.Step();

            Assert.Equal(5, sim.World.GetFoods.Count());
        }

        [Fact]
        public void Food_Spawn_IsLogged_WithTickAndPosition()
        {
            var config = new Config
            {
                WorldSizeX = 20,
                WorldSizeY = 20,
                StartBlobCount = 0,
                StartFoodCount = 0,
                FoodSpawnRate = 1f,
                MaxFoodCount = 100
            };

            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);

            for (int i = 0; i < 5; i++)
            {
                sim.Step();
            }

            Assert.Contains(logger.Messages, m => m.Contains("Tick 5 started"));
            Assert.Contains(logger.Messages, m => m.Contains("Food spawned at"));
        }
    }
}
