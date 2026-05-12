namespace BlobSimulation.ConsoleRunner.Renderer {
    public class ConsoleCamera {
        public int CenterX { get; set; }
        public int CenterY { get; set; }

        public int Width { get; set; } = 30;
        public int Height { get; set; } = 20;

        public ConsoleCamera(int centerX, int centerY, int width, int height) {
            CenterX = centerX;
            CenterY = centerY;
            Width = width;
            Height = height;
        }
    }
}

