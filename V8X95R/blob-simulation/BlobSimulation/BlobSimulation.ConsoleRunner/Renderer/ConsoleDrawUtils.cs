namespace BlobSimulation.ConsoleRunner.Renderer {
    
    public static class ConsoleDrawUtils {
        public static void DrawFrame(int x, int y, int width, int height, string title = "", int titleOffset = 0) {
            Console.SetCursorPosition(x, y);
            Console.Write("┌" + new string('─', titleOffset) + title.PadRight(width-titleOffset, '─') + "┐");

            for (int row = 0; row < height; row++) {
                Console.SetCursorPosition(x, y + 1 + row);
                Console.Write("│" + new string(' ', width) + "│");
                /*Console.SetCursorPosition(X, Y + 1 + y);
                Console.Write("|");

                Console.SetCursorPosition(X + width + 1, Y + 1 + y);
                Console.Write("|");
                */
            }

            Console.SetCursorPosition(x, y + height + 1);
            Console.Write("└" + new string('─', width) + "┘");
        }

        public static void WriteAt(int x, int y, string text, int width) {
            Console.SetCursorPosition(x, y);

            if (text.Length > width)
                text = text[..width];

            Console.Write(text.PadRight(width));
        }
    }


}