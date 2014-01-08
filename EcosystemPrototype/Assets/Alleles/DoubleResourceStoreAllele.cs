using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles
{
    public class DoubleResourceStoreAllele<T> : Allele
    {
        public DoubleResourceStore<T> Store { get; private set; }

        public T Type;
        public double Capacity, InitialAmount;

        void Start()
        {
            Store = new DoubleResourceStore<T>(Type, Capacity, InitialAmount);
        }

        public override Allele clone()
        {
            return new DoubleResourceStoreAllele<T>() { Type = Type, Capacity = Capacity, InitialAmount = InitialAmount };
        }
    }
}
