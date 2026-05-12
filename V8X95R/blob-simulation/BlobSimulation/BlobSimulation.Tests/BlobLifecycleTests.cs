using BlobSimulation.Core;
using System.Linq;
using Xunit;

namespace BlobSimulation.Tests
{
    public class BlobLifecycleTests
    {
        [Fact]
        public void Blob_Reproduces_WhenEnergyMeetsThreshold()
        {
            var config = new Config
            {
                WorldSizeX = 10,
                WorldSizeY = 10,
                StartBlobCount = 1,
                StartFoodCount = 0,
                InitialEnergy = 120f,
                ReproduceEnergyThreshold = 100f,
                ReproduceEnergyCost = 30f,
                ReproduceChance = 1f,
                EnergyLossFactor = 0f
            };

            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);
            var parent = sim.World.GetBlobs.Single();

            sim.Step();

            var blobs = sim.World.GetBlobs.ToList();
            Assert.Equal(2, blobs.Count);
            Assert.Equal(90f, parent.Energy);

            var child = blobs.Single(b => b.Id != parent.Id);
            Assert.Equal(parent.Position, child.Position);
        }

        [Fact]
        public void Blob_DoesNotReproduce_WhenEnergyBelowThreshold()
        {
            var config = new Config
            {
                WorldSizeX = 10,
                WorldSizeY = 10,
                StartBlobCount = 1,
                StartFoodCount = 0,
                InitialEnergy = 99f,
                ReproduceEnergyThreshold = 100f,
                ReproduceEnergyCost = 30f,
                ReproduceChance = 1f,
                EnergyLossFactor = 0f
            };

            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);

            sim.Step();

            Assert.Single(sim.World.GetBlobs);
        }

        [Fact]
        public void Blob_EnergyLossFactor_AppliesPerTickDecay()
        {
            var config = new Config
            {
                WorldSizeX = 10,
                WorldSizeY = 10,
                StartBlobCount = 1,
                StartFoodCount = 0,
                InitialEnergy = 20f,
                TickRate = 1,
                BlobSpeed = 1f,
                ReproduceChance = 0f,
                EnergyLossFactor = 0.1f
            };

            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);
            var blob = sim.World.GetBlobs.Single();

            sim.Step();

            // loss = (size^3 + speed^2 + vision) * deltaTime * factor
            // size=1.5, speed=1, vision=10, deltaTime=1, factor=0.1 -> 1.4375
            Assert.Equal(18.5625f, blob.Energy, 4);
        }

        [Fact]
        public void Blob_EnergyFloorsAtZero_AndBlobIsRemoved()
        {
            var config = new Config
            {
                WorldSizeX = 10,
                WorldSizeY = 10,
                StartBlobCount = 1,
                StartFoodCount = 0,
                InitialEnergy = 1f,
                TickRate = 1,
                BlobSpeed = 5f,
                ReproduceChance = 0f,
                EnergyLossFactor = 1f
            };

            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);
            var blob = sim.World.GetBlobs.Single();

            sim.Step();

            Assert.Equal(0f, blob.Energy);
            Assert.False(blob.IsAlive);
            Assert.Empty(sim.World.GetBlobs);
        }

    }
}
