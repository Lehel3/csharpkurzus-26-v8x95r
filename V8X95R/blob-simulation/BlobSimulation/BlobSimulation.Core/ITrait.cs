using System;

namespace BlobSimulation.Core
{
    public interface ITrait
    {
        public abstract void Evolve(Random random);
    }
}