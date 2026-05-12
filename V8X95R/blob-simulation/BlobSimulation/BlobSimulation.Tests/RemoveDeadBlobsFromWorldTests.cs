using Xunit;
using BlobSimulation.Core;
using BlobSimulation.Core.Logger;
using System.Linq;
using System.Collections.Generic;

namespace BlobSimulation.Tests
{
    public class RemoveDeadBlobsFromWorldTests
    {
        public class DummyLogger : ILogger
        {
            public List<string> Messages { get; } = new List<string>();
            public void Log(string message, LogLevel level = LogLevel.Info, string category = "", bool includeInHistory = true)
            {
                Messages.Add(message);
            }
        }

        // halott blob torlodik egy tick utan (0 energia)
        [Theory]
        [InlineData(0f)]
        public void RemoveDeadBlobs_WithZeroEnergy_RemovesBlob(float deathEnergy)
        {
            var config = new Config { StartBlobCount = 1, StartFoodCount = 0, WorldSizeX = 10, WorldSizeY = 10 };
            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);

            var blob = sim.World.GetBlobs.First();
            blob.Energy = deathEnergy;
            Assert.Single(sim.World.GetBlobs);

            sim.Step();

            Assert.Empty(sim.World.GetBlobs);
        }

        // elo blobok nem befolyasolodnak
        [Fact]
        public void RemoveDeadBlobs_AliveBlob_RemainsInWorldUnchanged()
        {
            var config = new Config { StartBlobCount = 1, StartFoodCount = 0, WorldSizeX = 10, WorldSizeY = 10 };
            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);

            var blob = sim.World.GetBlobs.First();

            blob.Energy = 100f;

            sim.Step();

            Assert.Single(sim.World.GetBlobs);
            Assert.True(blob.IsAlive);
        }

        // minden blob meghal, ha 0 energy
        [Fact]
        public void RemoveDeadBlobs_AllBlobsDie_WorldIsEmpty()
        {
            var config = new Config { StartBlobCount = 5, StartFoodCount = 0, WorldSizeX = 10, WorldSizeY = 10 };
            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);

            foreach (var blob in sim.World.GetBlobs)
            {
                blob.Energy = 0;
            }

            sim.Step();

            Assert.Empty(sim.World.GetBlobs);
        }


        [Fact]
        public void RemoveDeadBlobs_MixedAliveAndDead_OnlyRemovesDead()
        {
            // EDGE CASE: Kulonbozo allapotu blobok vannak a listaban
            var config = new Config { StartBlobCount = 3, StartFoodCount = 0, WorldSizeX = 10, WorldSizeY = 10 };
            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);

            var blobs = sim.World.GetBlobs.ToList();

            blobs[0].Energy = 100f; // alive
            blobs[1].Energy = 0f;   // dead
            blobs[2].Energy = -10f; // dead

            sim.Step();

            Assert.Single(sim.World.GetBlobs);
            Assert.Equal(blobs[0].Id, sim.World.GetBlobs.First().Id);
        }

        [Fact]
        public void RemoveDeadBlobs_DoD_LogsRemovalEvent()
        {
            var config = new Config { StartBlobCount = 1, StartFoodCount = 0, WorldSizeX = 10, WorldSizeY = 10 };
            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);

            var blob = sim.World.GetBlobs.First();
            blob.Energy = 0;

            sim.Step();

            bool hasDeathLog = logger.Messages.Any(m => m.Contains("died") || m.Contains("ded"));
            Assert.True(hasDeathLog, "A Blob halálát és törlését logolni kell a rendszerben!");
        }
    }
}
