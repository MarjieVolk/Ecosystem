using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles.FunctionalAlleles
{
    public class IntegerResourceStoreAllele : Allele
    {
        public IntegerResourceStore Store { get; private set; }

        [GeneticallyInheritable]
        public int Capacity, InitialAmount;

        void Awake()
        {
            Store = new IntegerResourceStore(Capacity, InitialAmount);
        }
    }
}
