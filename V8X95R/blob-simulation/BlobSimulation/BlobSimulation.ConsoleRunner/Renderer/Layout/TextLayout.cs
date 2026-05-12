namespace BlobSimulation.ConsoleRunner.Renderer.Layout {
    public static class TextLayout {
        public static IEnumerable<string> WrapLines(IEnumerable<string> lines, int maxWidth, bool wrap) {
            foreach (var line in lines) {
                if (!wrap) {
                    yield return line ?? string.Empty;
                    continue;
                }

                foreach (var wrapped in WrapText(line ?? string.Empty, maxWidth))
                    yield return wrapped;
            }
        }

        public static List<string> FitHeight(IEnumerable<string> lines, int maxHeight, bool fromEnd) {
            if (maxHeight <= 0)
                return new List<string>();

            return fromEnd
                ? lines.TakeLast(maxHeight).ToList()
                : lines.Take(maxHeight).ToList();
        }

        private static IEnumerable<string> WrapText(string text, int maxWidth) {
            if (maxWidth <= 0)
                yield break;

            if (string.IsNullOrEmpty(text)) {
                yield return string.Empty;
                yield break;
            }

            int index = 0;
            while (index < text.Length) {
                int remaining = text.Length - index;
                if (remaining <= maxWidth) {
                    yield return text.Substring(index);
                    yield break;
                }

                int breakAt = text.LastIndexOf(' ', index + maxWidth - 1, maxWidth);
                if (breakAt < index)
                    breakAt = index + maxWidth;

                yield return text.Substring(index, breakAt - index).TrimEnd();

                index = breakAt;
                while (index < text.Length && text[index] == ' ')
                    index++;
            }
        }
    }
}

