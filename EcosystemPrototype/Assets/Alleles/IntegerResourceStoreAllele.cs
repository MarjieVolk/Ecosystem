using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles
{
    public class IntegerResourceStoreAllele<T> : Allele
    {
        public IntegerResourceStore<T> Store { get; private set; }

        public readonly T Type;
        public readonly int Capacity, InitialAmount;

        void Start()
        {
            Store = new IntegerResourceStore<T>(Type, Capacity, InitialAmount);
        }
    }
}
