﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles.FunctionalAlleles
{
    public class DoubleResourceStoreAllele : Allele
    {
        public DoubleResourceStore Store { get; private set; }

        [GeneticallyInheritable]
        public double Capacity, InitialAmount;

        void Awake()
        {
            Store = new DoubleResourceStore(Capacity, InitialAmount);
        }

		void Start() {
			Store.Capacity = Capacity;
			Store.Amount = InitialAmount;
		}
    }
}
