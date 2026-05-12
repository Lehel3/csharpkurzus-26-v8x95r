using BlobSimulation.Core;
using BlobSimulation.Core.SimObjects;
using System.Linq;
using System.Numerics;
using Xunit;

namespace BlobSimulation.Tests
{
    public class BlobDecisionLogicTests
    {
        [Fact]
        public void HungryBlob_PrioritizesNearestFood_AndMovesTowardIt()
        {
            var config = new Config
            {
                WorldSizeX = 20,
                WorldSizeY = 20,
                StartBlobCount = 1,
                StartFoodCount = 0,
                InitialEnergy = 10f,
                HungerThreshold = 50f,
                TickRate = 1,
                BlobSpeed = 1f,
                ReproduceChance = 0f,
                EnergyLossFactor = 0f
            };

            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);
            var blob = sim.World.GetBlobs.Single();

            blob.Position = new Vector2(0, 0);
            sim.Spawn(new Food(), new Vector2(5, 0));
            sim.Spawn(new Food(), new Vector2(8, 0));

            float distanceBefore = Vector2.Distance(blob.Position, new Vector2(5, 0));

            sim.Step();

            float distanceAfter = Vector2.Distance(blob.Position, new Vector2(5, 0));
            Assert.True(distanceAfter < distanceBefore);
        }

        [Fact]
        public void EnergeticBlob_AttemptsReproduction()
        {
            var config = new Config
            {
                WorldSizeX = 20,
                WorldSizeY = 20,
                StartBlobCount = 1,
                StartFoodCount = 0,
                InitialEnergy = 200f,
                ReproduceEnergyThreshold = 100f,
                ReproduceEnergyCost = 30f,
                ReproduceChance = 1f,
                EnergyLossFactor = 0f
            };

            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);

            sim.Step();

            Assert.Equal(2, sim.World.GetBlobs.Count());
        }
    }
}
