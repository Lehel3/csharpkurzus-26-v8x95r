using Xunit;
using BlobSimulation.Core;
using BlobSimulation.ConsoleRunner.ConsoleCommander;
using BlobSimulation.ConsoleRunner.Renderer;
using System.Collections.Generic;
using BlobSimulation.ConsoleRunner;
using BlobSimulation.ConsoleRunner.ConsoleCommander.Commands;

namespace BlobSimulation.Tests
{

    public class StepAheadTests
    {
        [Fact]
        public void Parser_ValidStepCount_ReturnsStepCommand()
        {
            var parser = new CommandParser(new CommandProvider());

            var result = parser.Parse("step 10");

            Assert.True(result.Success);
            Assert.NotNull(result.Command);
            Assert.IsType<StepCommand>(result.Command);
            var stepCommand = (StepCommand)result.Command!;
            Assert.Equal(10, stepCommand.Count);
        }

        [Fact]
        public void Parser_NegativeStepCount_ReturnsCommandError()
        {
            var parser = new CommandParser(new CommandProvider());

            var result = parser.Parse("step -5");

            Assert.False(result.Success);
            Assert.Equal("Invalid step count.", result.ErrorMessage);
        }

        // nulla ertek kezelese
        [Fact]
        public void Parser_ZeroStepCount_ReturnsCommandError()
        {
            var parser = new CommandParser(new CommandProvider());

            var result = parser.Parse("step 0");

            Assert.False(result.Success);
            Assert.Equal("Invalid step count.", result.ErrorMessage);
        }

        [Fact]
        public void Handler_StepCommand_RespectsMaxStepsLimit()
        {
            var config = new Config { MaxSteps = 100, WorldSizeX = 10, WorldSizeY = 10, StartBlobCount = 0, StartFoodCount = 0 };
            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);
            var camera = new ConsoleCamera(0, 0, 10, 10);
            var context = new CommandContext(sim, camera, logger);

            for (int i = 0; i < 95; i++)
            {
                sim.Step();
            }

            var command = new StepCommand(10);
            command.Execute(context);

            Assert.Equal(99, sim.Tick);
        }
    }
}
