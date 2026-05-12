using BlobSimulation.ConsoleRunner.Renderer.Primitives;
using BlobSimulation.Core.Logger;
using System.Text;

namespace BlobSimulation.ConsoleRunner.Renderer.Views {
    public class LogHistoryView : RenderBlock {

        public override int Width { get; set; } = 50;
        public override int Height { get; set; } = 20;

        public int PaddingX { get; set; } = 1;
        public int PaddingY { get; set; } = 0;

        private readonly TextBoxBlock textBox;
        private readonly PaddingBlock padded;
        private readonly FrameBlock framed;

        public LogHistoryView() {
            textBox = new TextBoxBlock {
                WrapText = true,
                OverflowFromEnd = true
            };

            padded = new PaddingBlock(textBox);
            framed = new FrameBlock(padded) {
                Title = "Logs",
                TitleOffset = 6
            };
        }

        public override void Draw(ConsoleRenderContext ctx) {
            padded.Left = PaddingX;
            padded.Right = PaddingX;
            padded.Top = PaddingY;
            padded.Bottom = PaddingY;

            framed.X = X;
            framed.Y = Y;
            framed.Width = Width;
            framed.Height = Height;

            framed.Draw(ctx);
        }

        public void SetLogs(IEnumerable<LogEntry> logs) {
            var sb = new StringBuilder();

            foreach (LogEntry log in logs) {
                string line = string.IsNullOrEmpty(log.Category)
                    ? log.Message
                    : $"[{log.Category}] {log.Message}";
                sb.Append(line);
                sb.Append('\n');
            }

            textBox.Text = sb.ToString().TrimEnd('\n');
        }
    }
}
