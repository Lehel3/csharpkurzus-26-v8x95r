using BlobSimulation.Core;
using BlobSimulation.Core.SimObjects;
using System.Linq;
using System.Numerics;
using Xunit;

namespace BlobSimulation.Tests
{
    public class BlobMovementTests
    {
        [Fact]
        public void Blob_MovesEachTick_WhenFoodTargetExists()
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

            blob.Position = new Vector2(5, 5);
            sim.Spawn(new Food(), new Vector2(8, 5));

            var before = blob.Position;
            sim.Step();
            var after = blob.Position;

            Assert.NotEqual(before, after);
        }

        [Fact]
        public void Blob_MoveDistance_PerTick_MatchesSpeedTimesDeltaTime()
        {
            var config = new Config
            {
                WorldSizeX = 20,
                WorldSizeY = 20,
                StartBlobCount = 1,
                StartFoodCount = 0,
                InitialEnergy = 10f,
                HungerThreshold = 50f,
                TickRate = 2,
                BlobSpeed = 4f,
                ReproduceChance = 0f,
                EnergyLossFactor = 0f
            };

            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);
            var blob = sim.World.GetBlobs.Single();

            blob.Position = new Vector2(0, 0);
            sim.Spawn(new Food(), new Vector2(9, 0));

            var before = blob.Position;
            sim.Step();
            var after = blob.Position;

            float movedDistance = Vector2.Distance(before, after);
            Assert.Equal(config.BlobSpeed * sim.DeltaTime, movedDistance, 4);
        }

        [Fact]
        public void Blob_StaysWithinWorldBoundaries_AfterTick()
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
            sim.Spawn(new Food(), new Vector2(1, 0));

            sim.Step();

            Assert.InRange(blob.Position.X, 0f, config.WorldSizeX - 1);
            Assert.InRange(blob.Position.Y, 0f, config.WorldSizeY - 1);
        }
    }
}
