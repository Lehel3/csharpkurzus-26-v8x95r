using BlobSimulation.ConsoleRunner.Renderer.Primitives;

namespace BlobSimulation.ConsoleRunner.Renderer {
    public class ConsoleLayout {
        private readonly List<IRenderBlock> components = new();

        public void Add(IRenderBlock component) {
            components.Add(component);
        }

        public void Draw(ConsoleRenderContext ctx) {
            foreach (var component in components) {
                component.Draw(ctx);
            }
        }
    }
}

