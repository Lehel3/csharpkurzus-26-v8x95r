using Xunit;
using BlobSimulation.Core;
using BlobSimulation.Core.Logger;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace BlobSimulation.Tests
{
    public class EarlyStopConditionTests
    {
        public class DummyLogger : ILogger
        {
            public List<string> Messages { get; } = new List<string>();
            public void Log(string message, LogLevel level = LogLevel.Info, string category = "", bool includeInHistory = true)
            {
                Messages.Add(message);
            }
        }


        [Fact]
        public void CheckEarlyStopCondition_WithAliveBlobs_ReturnsFalse()
        {
            var config = new Config { StartBlobCount = 1, StartFoodCount = 0, WorldSizeX = 10, WorldSizeY = 10 };
            var sim = new SimulationEngine(config, new DummyLogger(), initialize: true);

            bool isExtinct = Program.CheckEarlyStopCondition(sim);

            Assert.False(isExtinct, "Nem állhat le, amíg van élő Blob.");
        }

        [Fact]
        public void CheckEarlyStopCondition_WhenLastBlobDies_ReturnsTrue()
        {
            var config = new Config { StartBlobCount = 1, StartFoodCount = 0, WorldSizeX = 10, WorldSizeY = 10 };
            var sim = new SimulationEngine(config, new DummyLogger(), initialize: true);

            var theLastBlob = sim.World.GetBlobs.First();
            theLastBlob.Energy = 0; // kovetkezo lepesben meghal

            sim.Step();

            bool isExtinct = Program.CheckEarlyStopCondition(sim);
            Assert.True(isExtinct, "Jeleznie kell a kihalást, ha az utolsó blob is elpusztult.");
        }

        // Edge Cases
        [Fact]
        public void CheckEarlyStopCondition_StartsEmpty_ReturnsTrueImmediately()
        {
            var config = new Config { StartBlobCount = 0, StartFoodCount = 0, WorldSizeX = 10, WorldSizeY = 10 };
            var sim = new SimulationEngine(config, new DummyLogger(), initialize: true);

            bool isExtinct = Program.CheckEarlyStopCondition(sim);

            Assert.True(isExtinct, "Ha 0 blobbal indul a szimuláció, azonnal le kell állnia.");
        }

        [Fact]
        public void CheckEarlyStopCondition_MassExtinction_ReturnsTrueAndMaintainsZeroCount()
        {
            var config = new Config { StartBlobCount = 10, StartFoodCount = 0, WorldSizeX = 10, WorldSizeY = 10 };
            var sim = new SimulationEngine(config, new DummyLogger(), initialize: true);

            foreach (var blob in sim.World.GetBlobs)
            {
                blob.Energy = 0;
            }

            sim.Step();

            Assert.Empty(sim.World.GetBlobs);

            bool isExtinct = Program.CheckEarlyStopCondition(sim);
            Assert.True(isExtinct, "Tömeges kihalás után is pontosan jeleznie kell a leállást.");
        }

        [Fact]
        public void CheckEarlyStopCondition_BirthAndDeathInSameTick_ReturnsFalse()
        {
            var config = new Config { StartBlobCount = 1, StartFoodCount = 0, WorldSizeX = 10, WorldSizeY = 10 };
            var sim = new SimulationEngine(config, new DummyLogger(), initialize: true);

            var parentBlob = sim.World.GetBlobs.First();

            sim.QueueBirth(parentBlob, Vector2.Zero);

            parentBlob.Energy = 0;

            sim.Step();

            Assert.Single(sim.World.GetBlobs);

            bool isExtinct = Program.CheckEarlyStopCondition(sim);
            Assert.False(isExtinct, "Ha meghal a szülő, de születik utód ugyanabban a lépésben, a faj nem halt ki!");
        }
    }
}
