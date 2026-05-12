using BlobSimulation.Core;
using BlobSimulation.Core.Logger;

namespace BlobSimulation.ConsoleRunner.Renderer {

    public class ConsoleRenderContext {
        public SimulationEngine Simulation { get; }
        public ConsoleCamera Camera { get; }

        public ConsoleRenderContext(SimulationEngine simulation, ConsoleCamera camera) {
            Simulation = simulation;
            Camera = camera;
        }
    }
}

