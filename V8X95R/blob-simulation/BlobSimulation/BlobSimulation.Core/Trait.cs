using System;
using System.Collections.Generic;
using System.Text;

namespace BlobSimulation.Core
{
    public class Trait<T>: ITrait
    {
        public Trait(T defaultValue)
        {
            Value = defaultValue;
        }


        private T _value;
        public T Value {
            get
            {
                return _value; 
            }
            set
            {
                _value = value;
            }
        }

        public virtual void Evolve(Random random)
        {
        
        }
    }

    public class FloatTrait: Trait<float>
    {


        public FloatTrait(float defaultValue) : base(defaultValue)
        {
        }
        public override void Evolve(Random random)
        {
            float mutationAmount = (float)(random.NextDouble() * 0.2 - 0.1);
            Value += mutationAmount;
            if(Value < 0) Value = 0;
        }
    }
}
