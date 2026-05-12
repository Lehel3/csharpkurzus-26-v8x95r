using BlobSimulation.Core.SimObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace BlobSimulation.Core
{
    public class World {
        public SimulationEngine sim;

        public int Width { get; }
        public int Height { get; }


        public List<SimObject> SimObjects { get; } = new List<SimObject>();

        public IEnumerable<Blob> GetBlobs => SimObjects.OfType<Blob>();
        public IEnumerable<Food> GetFoods => SimObjects.OfType<Food>();

        public World(SimulationEngine sim, int width, int height) {
            this.sim = sim;
            Width = width;
            Height = height;
        }

        public Vector2 RandomPosition(Random random) {
            return new Vector2(random.Next(0, Width), random.Next(0, Height));
        }
    }
}
