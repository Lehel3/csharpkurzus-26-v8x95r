using BlobSimulation.Core;
using BlobSimulation.Core.SimObjects;

namespace BlobSimulation.ConsoleRunner.Renderer.Primitives {
    public class WorldMapBlock : RenderBlock {
        public ConsoleCamera Camera { get; }

        public WorldMapBlock(ConsoleCamera camera) {
            Camera = camera;
        }

        public override void Draw(ConsoleRenderContext ctx) {
            World world = ctx.Simulation.World;

            int startX = Camera.CenterX - Camera.Width / 2;
            int startY = Camera.CenterY - Camera.Height / 2;

            for (int viewY = 0; viewY < Camera.Height; viewY++) {
                for (int viewX = 0; viewX < Camera.Width; viewX++) {
                    int worldX = startX + viewX;
                    int worldY = startY + (Camera.Height - 1 - viewY);

                    Console.SetCursorPosition(
                        X + viewX * 2,
                        Y + viewY
                    );

                    Console.Write(GetCell(world, worldX, worldY));
                }
            }
        }

        private string GetCell(World world, int x, int y)
        {
            if (x < 0 || y < 0 || x >= world.Width || y >= world.Height)
                return "  ";

            if (world.SimObjects.OfType<Blob>()
                .Any(b => (int)b.Position.X == x && (int)b.Position.Y == y))
                return "B ";

            if (world.SimObjects.OfType<Food>()
                .Any(f => (int)f.Position.X == x && (int)f.Position.Y == y))
                return "F ";

            return ". ";
        }
    }
}
