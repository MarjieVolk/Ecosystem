using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class IntegerResourceStore
    {
        public int Capacity { get; private set; }
        public int Amount { get; private set; }
        public int RemainingSpace { get { return Capacity - Amount; } }

        public IntegerResourceStore(int capacity, int initalAmount)
        {
            Capacity = capacity;
            Amount = initalAmount;
        }

        public bool addResource(int amount)
        {
            int nextAmount = Amount + amount;
            if (nextAmount > Capacity) return false;
            Amount = nextAmount;
            return true;
        }

        public bool removeResource(int amount)
        {
            int nextAmount = Amount - amount;
            if (nextAmount < 0) return false;
            Amount = nextAmount;
            return true;
        }
    }
}
