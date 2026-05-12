using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BlobSimulation.Core.SimObjects
{
    public abstract class SimObject {
        private static int nextId = 0;

        public SimulationEngine sim;

        public int Id { get; }
        public Vector2 Position { get; set; }

        public SimObject() {
            Id = nextId++;
            Position = Vector2.Zero;
        }

        public string FormatPosition() {
            return $"({Position.X.ToString("0.00")}, {Position.Y.ToString("0.00")})";
        }

        public virtual void OnSimulationStart() { }

        public virtual void OnSpawn() { }

        public virtual void TickUpdate() { }

        public virtual void OnDestroy() { }
    }
}