using Xunit;
using BlobSimulation.Core;
using BlobSimulation.Core.Logger;
using System;
using System.Collections.Generic;

namespace BlobSimulation.Tests
{
    public class ConfigureWorldParametersTests
    {
        public class DummyLogger : ILogger
        {
            public List<string> Messages { get; } = new List<string>();
            public void Log(string message, LogLevel level = LogLevel.Info, string category = "", bool includeInHistory = true)
            {
                Messages.Add(message);
            }
        }

        // default ertekek, ha nincsen config adva
        [Fact]
        public void Config_NoParametersProvided_AppliesDefaultValues()
        {
            var config = new Config();

            Assert.Equal(20, config.WorldSizeX);
            Assert.Equal(20, config.WorldSizeY);
            Assert.Equal(5, config.StartBlobCount);
            Assert.Equal(20, config.TickRate);
            Assert.Equal(5f, config.BlobSpeed);

            var exception = Record.Exception(() => config.Validate());
            Assert.Null(exception);
        }

        // user beallitja a world size, step count-ot
        [Fact]
        public void Engine_WithCustomConfig_CreatesCorrectSizedWorld()
        {
            var config = new Config
            {
                WorldSizeX = 35,
                WorldSizeY = 40,
                MaxSteps = 100,
                StartBlobCount = 0,
                StartFoodCount = 0
            };
            var logger = new DummyLogger();

            var sim = new SimulationEngine(config, logger, initialize: true);

            Assert.Equal(35, sim.World.Width);
            Assert.Equal(40, sim.World.Height);
        }

        // invalid parameterek
            [Theory]
            [InlineData(-5, 20)] // X negatív
            [InlineData(20, -10)] // Y negatív
        public void Config_Validate_NegativeWorldSize_ThrowsArgumentException(int sizeX, int sizeY)
        {
            var config = new Config { WorldSizeX = sizeX, WorldSizeY = sizeY };

            var ex = Assert.Throws<ArgumentException>(() => config.Validate());
            Assert.Contains("world_size must be positive", ex.Message);
        }

        // (Edge Cases)
        [Fact]
        public void Config_Validate_ZeroWorldSize_ThrowsArgumentException()
        {
            var config = new Config { WorldSizeX = 0, WorldSizeY = 20 };

            var ex = Assert.Throws<ArgumentException>(() => config.Validate());
            Assert.Contains("world_size must be positive", ex.Message);
        }

        [Fact]
        public void Config_Validate_MultipleInvalidParameters_AggregatesErrors()
        {
            var config = new Config
            {
                WorldSizeX = -1,
                StartBlobCount = -5,
                InitialEnergy = -10f,
                TickRate = 0
            };

            var ex = Assert.Throws<ArgumentException>(() => config.Validate());

            Assert.Contains("world_size must be positive", ex.Message);
            Assert.Contains("blob_count must be positive", ex.Message);
            Assert.Contains("initial_energy must be positive", ex.Message);
            Assert.Contains("tick_rate must be positive", ex.Message);
        }
    }
}
