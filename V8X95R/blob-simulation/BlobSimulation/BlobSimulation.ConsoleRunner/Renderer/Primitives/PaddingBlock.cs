namespace BlobSimulation.ConsoleRunner.Renderer.Primitives {
    public class PaddingBlock : RenderBlock {
        public RenderBlock Child { get; }

        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        public PaddingBlock(RenderBlock child, int left = 0, int right = 0, int top = 0, int bottom = 0) {
            Child = child;
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public override void Draw(ConsoleRenderContext ctx) {
            Child.X = X + Left;
            Child.Y = Y + Top;
            Child.Width = Math.Max(0, Width - Left - Right);
            Child.Height = Math.Max(0, Height - Top - Bottom);
            Child.Draw(ctx);
        }
    }
}

