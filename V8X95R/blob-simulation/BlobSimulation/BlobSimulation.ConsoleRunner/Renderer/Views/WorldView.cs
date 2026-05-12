using BlobSimulation.ConsoleRunner.Renderer.Primitives;
using BlobSimulation.Core;
using BlobSimulation.Core.SimObjects;

namespace BlobSimulation.ConsoleRunner.Renderer.Views {
    public class WorldView : RenderBlock {

        public override int Width => Camera.Width * 2 + 2;
        public override int Height => Camera.Height + 2;

        public ConsoleCamera Camera { get; }

        private readonly WorldMapBlock worldMap;
        private readonly PaddingBlock padded;
        private readonly FrameBlock framed;

        public WorldView(ConsoleCamera camera) {
            Camera = camera;

            worldMap = new WorldMapBlock(Camera);
            padded = new PaddingBlock(worldMap);
            framed = new FrameBlock(padded) {
                Title = "World",
                TitleOffset = 6
            };
        }

        public override void Draw(ConsoleRenderContext ctx) {
            padded.Left = 0;
            padded.Right = 0;
            padded.Top = 0;
            padded.Bottom = 0;

            framed.X = X;
            framed.Y = Y;
            framed.Width = Camera.Width * 2;
            framed.Height = Camera.Height;

            framed.Draw(ctx);
        }
    }
}

