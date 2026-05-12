using Xunit;
using BlobSimulation.Core;
using BlobSimulation.Core.Logger;
using BlobSimulation.ConsoleRunner;
using BlobSimulation.ConsoleRunner.ConsoleCommander;
using BlobSimulation.ConsoleRunner.ConsoleCommander.Commands;
using BlobSimulation.ConsoleRunner.Renderer;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BlobSimulation.Tests
{
    public class StepByStepSimulationTests
    {

        [Fact]
        public void Engine_Step_IncrementsTickExactlyOnce()
        {
            var config = new Config { WorldSizeX = 10, WorldSizeY = 10, StartBlobCount = 0, StartFoodCount = 0 };
            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);

            Assert.Equal(0, sim.Tick);
            sim.Step();
            Assert.Equal(1, sim.Tick);

            sim.Step(); sim.Step(); sim.Step();
            Assert.Equal(4, sim.Tick);
        }

        [Fact]
        public void Engine_Step_ExecutesEntityTickUpdates()
        {
            // test, h a sim.Step() tenyleg meghivja a Blobok TickUpdate()-jet
            // A Blob energiaja alapbol 100, lepesenkent csokkennie kell
            var config = new Config { WorldSizeX = 10, WorldSizeY = 10, StartBlobCount = 1, StartFoodCount = 0 };
            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);
            var blob = sim.World.GetBlobs.First();

            float initialEnergy = blob.Energy;

            sim.Step();

            Assert.True(blob.Energy < initialEnergy, "A blob energiájának csökkennie kell egy lépés után a mozgás/létezés miatt!");
        }

        [Fact]
        public void Engine_Step_AccumulatesFoodSpawnCorrectly()
        {
            var config = new Config { WorldSizeX = 10, WorldSizeY = 10, StartBlobCount = 0, StartFoodCount = 0, FoodSpawnRate = 0.5f, MaxFoodCount = 10 };
            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);

            Assert.Empty(sim.World.GetFoods);

            sim.Step();
            // Tick 1 (accumulator 0 -> 0.5)
            Assert.Empty(sim.World.GetFoods); 


            sim.Step();
            // Tick 2 (accumulator 0.5 -> 1.0, Spawn -> 0)
            Assert.Single(sim.World.GetFoods); 
        }


        [Fact]
        public void Handler_StepCommand_AtMaxSteps_IgnoresRequest()
        {
            var config = new Config { MaxSteps = 5, WorldSizeX = 10, WorldSizeY = 10, StartBlobCount = 0, StartFoodCount = 0 };
            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);
            var camera = new ConsoleCamera(0, 0, 10, 10);
            var context = new CommandContext(sim, camera, logger);

            for (int i = 0; i < 5; i++) sim.Step(); // elerjuk a MaxSteps-t

            var command = new StepCommand(1);
            command.Execute(context);

            Assert.Equal(5, sim.Tick);
        }

        [Fact]
        public void Handler_StepCommand_UnlimitedSteps_WorksCorrectly()
        {
            // A -1 jelenti a vegtelen szimulaciot a config alapjan.
            var config = new Config { MaxSteps = -1, WorldSizeX = 10, WorldSizeY = 10, StartBlobCount = 0, StartFoodCount = 0 };
            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);
            var camera = new ConsoleCamera(0, 0, 10, 10);
            var context = new CommandContext(sim, camera, logger);

            for (int i = 0; i < 1000; i++) sim.Step();

            var command = new StepCommand(10);
            command.Execute(context);

            Assert.Equal(1009, sim.Tick);
        }

        [Fact]
        public void Engine_Step_Uninitialized_ThrowsException()
        {
            var config = new Config { WorldSizeX = 10, WorldSizeY = 10 };
            var logger = new DummyLogger();

            var sim = new SimulationEngine(config, logger, initialize: false);

            Assert.Throws<InvalidOperationException>(() => sim.Step());
        }
    }
}
