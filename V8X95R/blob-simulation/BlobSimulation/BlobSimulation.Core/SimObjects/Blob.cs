using System;
using System.Collections.Generic;
using System.Numerics;

namespace BlobSimulation.Core.SimObjects
{

    public class BlobTraits {
        public Trait<float> Speed = new Trait<float>(4f);
        public Trait<float> Size = new Trait<float>(1.5f);
        public Trait<float> VisionRange = new Trait<float>(10f);


        //size **3 + speed **2 + visionRange
        public float EnergyLoss() {
            float sizeEnergy = Size.Value * Size.Value * Size.Value;
            float speedEnergy = Speed.Value * Speed.Value;
            float visionEnergy = VisionRange.Value;

            return sizeEnergy + speedEnergy + visionEnergy;
        }
    }


    public class Blob : SimObject {


        #region --- Stats ---

        BlobTraits Traits = new BlobTraits();


        #endregion

        private float _energy;

        public float Energy {
            get {
                return _energy;
            }
            set {
                _energy = Math.Max(0, value);
                if (_energy <= 0) {
                    Die();
                }
            }
        }

        public bool IsAlive { get; private set; } = true;



        public enum State {
            Idle,
            GoingForFood,
            JustAte,
            Dead
        }

        public State state;


        public int decisionTime = 20;
        private int decisionTimer = 0;


        private Vector2 Target;
        private Food TargetFood = null;


        public override void OnSimulationStart() {
            Traits.Speed.Value = sim.Config.BlobSpeed;
        }


        public override void OnSpawn()
        {
            sim.Logger.Log($"{this} spawned at {FormatPosition()}");
            sim.blobCount++;

            Energy = sim.Config.InitialEnergy;

            //TODO inherited traits, with mutation
            Traits.Speed.Value = sim.Config.BlobSpeed;
        }

        public override void OnDestroy()
        {
            //sim.Logger.Log($"{this}: Im ded!");
            sim.blobCount--;
        }

        public override void TickUpdate()
        {
            if (state == State.JustAte)
            {
                EnterState(State.Idle);
            }

            if (state == State.GoingForFood)
            {
                if (TargetFood != null && TargetFood.isEaten)
                {
                    TargetFood = null;
                }

                if (TargetFood == null) EnterState(State.Idle);
            }

            if (state == State.Idle)
            {
                if (decisionTimer <= 0)
                {
                    Think();
                    decisionTimer = decisionTime;
                }
                decisionTimer--;
            }

            MoveTowards(Target);
            TryReproduce();


            float energyLoss = Traits.EnergyLoss() * sim.DeltaTime * sim.Config.EnergyLossFactor;
            Energy -= energyLoss;

            sim.Logger.Log($"{this} lost {energyLoss:0.0} energy, remaining: {Energy:0.00}", includeInHistory: false);
        }


        #region --- Movement ---
        public void MoveTowards(Vector2 targetPosition) {
            float step = Traits.Speed.Value * sim.DeltaTime;

            if (Vector2.DistanceSquared(Position, targetPosition) <= step * step) {
                Position = targetPosition;
                return;
            }
            Vector2 toTarget = targetPosition - Position;
            Position += Vector2.Normalize(toTarget) * step;
        }

        public void MoveInDirection(Vector2 direction) {
            float step = Traits.Speed.Value * sim.DeltaTime;

            Position += Vector2.Normalize(direction) * step;
        }
        #endregion


        public bool CanReproduce()
        {
            return Energy >= sim.Config.ReproduceEnergyThreshold && sim.random.NextDouble() < sim.Config.ReproduceChance;
        }

        /// <summary>
        /// If reproduction conditions are met,
        /// queues a birth in the simulation,
        /// and removes reproduction energy.
        /// </summary>
        private void TryReproduce() {
            if (CanReproduce()) {
                Energy -= sim.Config.ReproduceEnergyCost;

                sim.QueueBirth(this, this.Position);

                sim.Logger.Log(this + " had a baby");
            }
        }

        private void EnterState(State newState) {
            if (state == newState) return;

            state = newState;
            switch (newState) {
                case State.Idle:
                    decisionTimer = 0;
                    break;
                case State.GoingForFood:
                    break;
            }
            sim.Logger.Log(this + " entered state " + newState, includeInHistory: false);
        }

        private void Think() {
            bool hungry = Energy <= sim.Config.HungerThreshold;

            if (hungry) {
                Food food = FindClosestFood(sim.World);

                if (food != null) {
                    Target = food.Position;
                    TargetFood = food;
                    EnterState(State.GoingForFood);
                }
            }
            else {
                Target = sim.World.RandomPosition(sim.random);
            }
        }

        public Food? FindClosestFood(World world) {
            //TODO
            //spatial grid alapján beazonosítani a közeli foodokat, csak azokat összehasonlítani
            //spatial grid tárolja a cellákban lévő blobokat, és foodokat

            Food? closest = null;
            double closestDistanceSqr = double.MaxValue;

            foreach (var food in world.GetFoods) {
                float distSqr = Vector2.DistanceSquared(Position, food.Position);

                if (distSqr < Traits.VisionRange.Value * Traits.VisionRange.Value && distSqr < closestDistanceSqr) {
                    closest = food;
                    closestDistanceSqr = distSqr;
                }
            }

            return closest;
        }

        public void Eat() {
            Energy += 100;
            EnterState(State.JustAte);
        }

        private void Die()
        {
            sim.Logger.Log(this + " died");
            IsAlive = false;
            EnterState(State.Dead);
        }

        public override string ToString() {
            return $"Blob{Id}";
        }
    }
}
