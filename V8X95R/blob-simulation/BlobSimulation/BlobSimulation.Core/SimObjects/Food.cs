using System.Numerics;

namespace BlobSimulation.Core.SimObjects
{
    public class Food : SimObject {

        public bool isEaten = false;

        public override void OnSpawn() {
            base.OnSpawn();
            sim.Logger.Log($"Food spawned at {FormatPosition()}");
        }
    }
}
