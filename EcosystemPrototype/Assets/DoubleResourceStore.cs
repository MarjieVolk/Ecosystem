using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class DoubleResourceStore
    {
        public double Capacity { get; set; }
        public double Amount { get; set; }
        public double RemainingSpace { get { return Capacity - Amount; } }

        public DoubleResourceStore(double capacity, double initalAmount)
        {
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
