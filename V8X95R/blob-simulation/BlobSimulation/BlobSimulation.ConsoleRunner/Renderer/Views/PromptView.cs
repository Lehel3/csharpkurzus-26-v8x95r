using BlobSimulation.ConsoleRunner.Renderer.Primitives;

namespace BlobSimulation.ConsoleRunner.Renderer.Views {
    public class PromptView : RenderBlock {

        public override int Width { get; set; } = 50;
        public override int Height { get; set; } = 1;

        private readonly TextBoxBlock textBox;

        public PromptView() {
            textBox = new TextBoxBlock {
                WrapText = false,
                OverflowFromEnd = false,
                PadToWidth = false,
                Text = "> "
            };
        }

        public override void Draw(ConsoleRenderContext ctx) {
            ConsoleDrawUtils.WriteAt(X, Y, string.Empty, Width);
            textBox.X = X;
            textBox.Y = Y;
            textBox.Width = textBox.Text.Length;
            textBox.Height = Height;
            textBox.Draw(ctx);
        }
    }
}
