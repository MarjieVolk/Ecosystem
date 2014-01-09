using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles
{
    public class DoubleResourceStoreAllele : Allele
    {
        public DoubleResourceStore Store { get; private set; }

        [GeneticallyInheritable]
        public double Capacity, InitialAmount;

        void Start()
        {
            Store = new DoubleResourceStore(Capacity, InitialAmount);
        }

        public override Allele clone()
        {
            return new DoubleResourceStoreAllele() { Capacity = Capacity, InitialAmount = InitialAmount };
        }
    }
}
