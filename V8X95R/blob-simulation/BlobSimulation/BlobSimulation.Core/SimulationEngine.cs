using BlobSimulation.Core.Logger;
using BlobSimulation.Core.SimObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BlobSimulation.Core
{
    public class SimulationEngine {

        #region --- Events ---
        public event Action<SimObject> OnSimObjectCreated;
        public event Action<int> OnSimObjectDestroyed;

        public event Action<Blob> OnBlobCreated;
        public event Action<int> OnBlobDestroyed;
        
        public event Action<Food> OnFoodCreated;
        public event Action<int> OnFoodDestroyed;
        
        #endregion

        public World World { get; private set; }

        public int Tick { get; private set; }
        public float DeltaTime => 1f / Config.TickRate;


        //testing
        public int blobCount = 0;

        public bool Initialized { get; private set; } = false;

        private float foodSpawnAccumulator;


        public readonly Config Config;
        public readonly ILogger Logger;


        public SimulationEngine(Config config, ILogger logger, bool initialize = true)
        {
            this.Config = config;
            this.Logger = logger;
            if (initialize) Initialize();
        }


        #region --- Seed & Random---
        public int Seed { get; private set; }
        public Random random;    


        private void SetRandomSeed() {
            SetSeed(new Random().Next());
        }

        private void SetSeed(int seed) {
            Seed = seed;
            random = new Random(Seed);
        }

        #endregion


        public void Initialize()
        {
            if (Initialized) return;


            // Itt a World méreteit egyaránt a WorldSize adja (négyzetes grid)
            World = new World(this, Config.WorldSizeX, Config.WorldSizeY);

            // Seed
            if (Config.UseRandomSeed) SetRandomSeed();
            else SetSeed(Config.Seed);


            Logger.Log($"Simulation started. Using Seed: {Seed}", LogLevel.Success, "Sim");

            // Blobok lehelyezése
            for (int i = 0; i < Config.StartBlobCount; i++)
            {
                Spawn(new Blob(), World.RandomPosition(random));
            }
            for (int i = 0; i < Config.StartFoodCount; i++)
            {
                Spawn(new Food(), World.RandomPosition(random));
            }

            foreach (SimObject simObj in World.SimObjects)
            {
                simObj.OnSimulationStart();
            }

            Initialized = true;
        }


        #region --- Step ---

        public void Step() {
            if (!Initialized || World == null) {
                throw new InvalidOperationException("Simulation must be initialized before stepping.");
            }

            Tick++;
            Logger.Log($"Tick {Tick} started. Blob count: {World.GetBlobs.Count()}, Food count: {World.GetFoods.Count()}");


            foodSpawnAccumulator += Config.FoodSpawnRate;
            FoodSpawning();


            foreach (var simObject in World.SimObjects)
            {
                simObject.TickUpdate();
            }




            RemoveDeadBlobs();
            HandleEating();
            ApplyBirths();


            var avgEnergy = World.GetBlobs.Any() ? World.GetBlobs.Average(b => b.Energy) : 0f;
            Logger.Log($"Avg energy: {avgEnergy:0.00}");



            //TODO - Save state
            //lementeni minden fontos állapotot ami alapján visszajátszható/betölthető a szimuláció
        }
        #endregion

        private void FoodSpawning() {
            if (World.GetFoods.Count() >= Config.MaxFoodCount) return;

            while (foodSpawnAccumulator >= 1f) {

                Vector2 spawnPos = World.RandomPosition(random);
                Spawn(new Food(), spawnPos);
                foodSpawnAccumulator -= 1f;
            }
        }

        #region --- Spawning/Despawning ---

        public SimObject Spawn(SimObject simObject)
        {
            return Spawn(simObject, Vector2.Zero);
        }

        public SimObject Spawn(SimObject simObject, Vector2 pos)
        {
            simObject.sim = this;
            simObject.Position = pos;

            World.SimObjects.Add(simObject);
            OnSimObjectCreated?.Invoke(simObject);

            simObject.OnSpawn();

            return simObject;
        }

        public void Despawn(SimObject simObject)
        {
            World.SimObjects.Remove(simObject);
            OnSimObjectDestroyed?.Invoke(simObject.Id);

            simObject.OnDestroy();
        }

        #endregion



        // Queue birth so babies are not included in the update loop of current tick
        #region --- Birth and Death ---

        public struct Birth {
            public Blob parent; // TODO two parent births (not in scope)
            public Vector2 position;
        }

        List<Birth> pendingBirths = new List<Birth>();

        public void QueueBirth(Blob parent, Vector2 birthPos)
        {
            QueueBirth(new Birth
            {
                parent = parent,
                position = birthPos
            });
        }

        public void QueueBirth(Birth newBirth)
        {
            pendingBirths.Add(newBirth);
        }

        private void ApplyBirths() {
            foreach (var birth in pendingBirths) {
                Spawn(new Blob(), birth.position);
            }
            if (pendingBirths.Count > 0) {
                Logger.Log("Applied " + pendingBirths.Count + " births", LogLevel.Info, "Sim");
            }
            pendingBirths.Clear();
        }

        private void RemoveDeadBlobs()
        {
            foreach (var blob in World.GetBlobs.Where(b => !b.IsAlive).ToList())
            {
                Despawn(blob);
            }
        }

        #endregion


        private void HandleEating() {
            //OPTIMIZATION spacial grid for food distance checks, only check foods in vision range
            //TODO - handle multiple blobs eating the same food in the same tick (currently the blob with lower id gets it)
            foreach (var blob in World.GetBlobs.OrderBy(b => b.Id).ToList()) {
                foreach (var food in World.GetFoods.ToList()) {

                    if (Vector2.Distance(blob.Position, food.Position) < 0.1f) {
                        blob.Eat();
                        food.isEaten = true;
                        Despawn(food);
                    }
                }
            }
        }

        public void SaveSimulation(string path) {
            //TODO - save simulation state to json
            throw new NotImplementedException("SaveSimulation is not implemented.");
        }

        public void LoadSimulation(string path) {
            //TODO - load simulation state from json
            throw new NotImplementedException("LoadSimulation is not implemented.");
        }
    }

}
