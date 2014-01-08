using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles
{
    public class IntegerResourceStoreAllele<T> : Allele
    {
        public IntegerResourceStore<T> Store { get; private set; }

        public T Type;
        public int Capacity, InitialAmount;

        void Start()
        {
            Store = new IntegerResourceStore<T>(Type, Capacity, InitialAmount);
        }

        public override Allele clone()
        {
            return new IntegerResourceStoreAllele<T>() { Type = Type, Capacity = Capacity, InitialAmount = InitialAmount };
        }
    }
}
