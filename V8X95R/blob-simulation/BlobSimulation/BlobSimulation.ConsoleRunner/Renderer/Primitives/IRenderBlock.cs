namespace BlobSimulation.ConsoleRunner.Renderer.Primitives {
    public interface IRenderBlock {
        int X { get; set; }
        int Y { get; set; }
        int Width { get; }
        int Height { get; }

        void Draw(ConsoleRenderContext ctx);
    }

    public abstract class RenderBlock : IRenderBlock {
        public virtual int X { get; set; }
        public virtual int Y { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public virtual int EndX => X + Width;
        public virtual int EndY => Y + Height;

        public abstract void Draw(ConsoleRenderContext ctx);
    }
}

