using System.Collections.Generic;

namespace BlobSimulation.Core
{
    public record class BlobState
    {
        public int Id { get; init; }
        public float X { get; init; }
        public float Y { get; init; }
        public float Energy { get; init; }
        public bool IsAlive { get; init; }

        public BlobState(int id, float x, float y, float energy, bool isAlive)
        {
            Id = id;
            X = x;
            Y = y;
            Energy = energy;
            IsAlive = isAlive;
        }
    }

    public record class FoodState
    {
        public int Id { get; init; }
        public float X { get; init; }
        public float Y { get; init; }

        public FoodState(int id, float x, float y)
        {
            Id = id;
            X = x;
            Y = y;
        }
    }

    public record class SimulationSnapshot
    {
        public int Tick { get; init; }
        public int Seed { get; init; }
        public List<BlobState> Blobs { get; init; }
        public List<FoodState> Foods { get; init; }

        public SimulationSnapshot(int tick, int seed, List<BlobState> blobs, List<FoodState> foods)
        {
            Tick = tick;
            Seed = seed;
            Blobs = blobs;
            Foods = foods;
        }
    }
}
