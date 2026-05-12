using BlobSimulation.ConsoleRunner.Renderer;
using BlobSimulation.ConsoleRunner.Renderer.Primitives;
using BlobSimulation.ConsoleRunner.Renderer.Views;
using BlobSimulation.Core;
using BlobSimulation.Core.SimObjects;
using System.Numerics;
using System.Reflection;
using Xunit;

namespace BlobSimulation.Tests
{
    public class CliWorldGridRenderingTests
    {
        [Fact]
        public void WorldGrid_RespectsConfiguredSize_For10x10View()
        {
            var camera = new ConsoleCamera(centerX: 5, centerY: 5, width: 10, height: 10);
            var worldView = new WorldView(camera);

            Assert.Equal(22, worldView.Width);
            Assert.Equal(12, worldView.Height);
        }

        [Fact]
        public void WorldGrid_UsesDistinctSymbols_ForBlobFoodAndEmptyCell()
        {
            var config = new Config
            {
                WorldSizeX = 10,
                WorldSizeY = 10,
                StartBlobCount = 0,
                StartFoodCount = 0
            };

            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);

            sim.Spawn(new Blob(), new Vector2(1, 1));
            sim.Spawn(new Food(), new Vector2(2, 2));

            var camera = new ConsoleCamera(centerX: 5, centerY: 5, width: 10, height: 10);
            var worldMap = new WorldMapBlock(camera);
            MethodInfo? getCell = typeof(WorldMapBlock).GetMethod("GetCell", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(getCell);

            string blobCell = (string)getCell!.Invoke(worldMap, new object[] { sim.World, 1, 1 })!;
            string foodCell = (string)getCell.Invoke(worldMap, new object[] { sim.World, 2, 2 })!;
            string emptyCell = (string)getCell.Invoke(worldMap, new object[] { sim.World, 3, 3 })!;

            Assert.Equal("B ", blobCell);
            Assert.Equal("F ", foodCell);
            Assert.Equal(". ", emptyCell);
        }

        [Fact]
        public void WorldGrid_OutOfBoundsCell_IsRenderedAsBlank()
        {
            var config = new Config
            {
                WorldSizeX = 10,
                WorldSizeY = 10,
                StartBlobCount = 0,
                StartFoodCount = 0
            };

            var logger = new DummyLogger();
            var sim = new SimulationEngine(config, logger, initialize: true);
            var camera = new ConsoleCamera(centerX: 5, centerY: 5, width: 10, height: 10);
            var worldMap = new WorldMapBlock(camera);
            MethodInfo? getCell = typeof(WorldMapBlock).GetMethod("GetCell", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(getCell);

            string outOfBoundsCell = (string)getCell!.Invoke(worldMap, new object[] { sim.World, -1, 0 })!;
            Assert.Equal("  ", outOfBoundsCell);
        }
    }
}
