using BlobSimulation.ConsoleRunner.Renderer.Layout;

namespace BlobSimulation.ConsoleRunner.Renderer.Primitives {
    public class TextBoxBlock : RenderBlock {
        public string Text { get; set; } = string.Empty;

        public bool WrapText { get; set; } = true;
        public bool OverflowFromEnd { get; set; } = true;
        public bool PadToWidth { get; set; } = true;

        public override void Draw(ConsoleRenderContext ctx) {
            int contentWidth = Math.Max(1, Width);
            int contentHeight = Math.Max(0, Height);

            var sourceLines = Text
                .Replace("\r\n", "\n")
                .Replace('\r', '\n')
                .Split('\n');

            var wrapped = TextLayout.WrapLines(sourceLines, contentWidth, WrapText);
            var visibleLines = TextLayout.FitHeight(wrapped, contentHeight, OverflowFromEnd);

            for (int row = 0; row < contentHeight; row++) {
                string line = row < visibleLines.Count ? visibleLines[row] : string.Empty;
                if (PadToWidth) {
                    ConsoleDrawUtils.WriteAt(X, Y + row, line, contentWidth);
                }
                else {
                    Console.SetCursorPosition(X, Y + row);
                    if (line.Length > contentWidth)
                        line = line[..contentWidth];
                    Console.Write(line);
                }
            }
        }
    }
}
