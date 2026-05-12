namespace BlobSimulation.ConsoleRunner.Renderer.Primitives {
    public class FrameBlock : RenderBlock {
        public RenderBlock Child { get; }
        public string Title { get; set; } = string.Empty;
        public int TitleOffset { get; set; } = 0;

        public FrameBlock(RenderBlock child) {
            Child = child;
        }

        public override void Draw(ConsoleRenderContext ctx) {
            ConsoleDrawUtils.DrawFrame(X, Y, Width, Height, Title, TitleOffset);

            Child.X = X + 1;
            Child.Y = Y + 1;
            Child.Width = Width;
            Child.Height = Height;
            Child.Draw(ctx);
        }
    }
}

