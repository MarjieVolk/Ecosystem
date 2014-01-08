using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class DoubleResourceStore<T>
    {
        public T StoredResource { get; private set; }
        public double Capacity { get; private set; }
        public double Amount { get; private set; }

        public DoubleResourceStore(T type, int capacity, int initalAmount)
        {
            StoredResource = type;
            Capacity = capacity;
            Amount = initalAmount;
        }

        public bool addResource(double amount)
        {
            double nextAmount = Amount + amount;
            if (nextAmount > Capacity) return false;
            Amount = nextAmount;
            return true;
        }

        public bool removeResource(double amount)
        {
            double nextAmount = Amount - amount;
            if (nextAmount < 0) return false;
            Amount = nextAmount;
            return true;
        }
    }
}
