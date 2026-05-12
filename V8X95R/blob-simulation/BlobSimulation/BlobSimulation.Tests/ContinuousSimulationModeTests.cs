using BlobSimulation.ConsoleRunner;
using BlobSimulation.ConsoleRunner.ConsoleCommander;
using BlobSimulation.ConsoleRunner.ConsoleCommander.Commands;
using BlobSimulation.ConsoleRunner.Renderer;
using BlobSimulation.ConsoleRunner.Renderer.Views;
using BlobSimulation.Core;
using System.Reflection;
using Xunit;

namespace BlobSimulation.Tests {
    public class ContinuousSimulationModeTests {
        [Fact]
        public void Parser_GoCommand_ReturnsGoCommand() {
            var parser = new CommandParser(new CommandProvider());

            var result = parser.Parse("go");

            Assert.True(result.Success);
            Assert.NotNull(result.Command);
            Assert.IsType<GoCommand>(result.Command);
        }

        [Fact]
        public void ContinuousMode_WhenAllBlobsDead_ReturnsTrue() {
            var config = new Config {
                WorldSizeX = 10,
                WorldSizeY = 10,
                StartBlobCount = 0,
                StartFoodCount = 0,
                MaxSteps = -1,
                TickRate = 1000
            };

            var logger = CreateConsoleLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);
            var camera = new ConsoleCamera(5, 5, 10, 10);
            var worldView = new WorldView(camera);

            bool shouldStop = InvokeRunContinuousMode(sim, camera, worldView, logger, config.TickRate);

            Assert.True(shouldStop);
        }

        [Fact]
        public void ContinuousMode_WhenMaxStepsReached_ReturnsFalse() {
            var config = new Config {
                WorldSizeX = 10,
                WorldSizeY = 10,
                StartBlobCount = 0,
                StartFoodCount = 0,
                MaxSteps = 1,
                TickRate = 1000
            };

            var logger = CreateConsoleLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);
            var camera = new ConsoleCamera(5, 5, 10, 10);
            var worldView = new WorldView(camera);

            sim.Step();
            Assert.Equal(1, sim.Tick);

            bool shouldStop = InvokeRunContinuousMode(sim, camera, worldView, logger, config.TickRate);

            Assert.False(shouldStop);
        }

        private static ConsoleLogger CreateConsoleLogger() {
            string path = Path.Combine(Path.GetTempPath(), $"blobsim-test-{Guid.NewGuid():N}.log");
            var fileSink = new FileLogSink(path);
            var consoleSink = new ConsoleLogSink();
            var logger = new ConsoleLogger(fileSink, consoleSink);
            logger.Mode = LogMode.Buffered;
            return logger;
        }

        private static bool InvokeRunContinuousMode(
            SimulationEngine sim,
            ConsoleCamera camera,
            WorldView worldView,
            ConsoleLogger logger,
            int tickRate) {
            MethodInfo? method = typeof(Program).GetMethod(
                "RunContinuousMode",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(method);

            object? result = method!.Invoke(null, new object[] { sim, camera, worldView, logger, tickRate });
            Assert.IsType<bool>(result);
            return (bool)result!;
        }
    }
}

