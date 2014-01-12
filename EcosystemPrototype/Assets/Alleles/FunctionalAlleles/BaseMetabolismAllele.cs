using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles.FunctionalAlleles
{
    public class BaseMetabolismAllele : Allele
    {
        [GeneticallyInheritable]
        public double MetabolicRate;

        private DoubleResourceStore energy; 

        private const string ENERGY_STORE = "energystore";

        public void Start()
        {
            Genome genome = this.gameObject.GetComponent<Genome>();
            energy = ((DoubleResourceStoreAllele)genome.GetActiveAllele(ENERGY_STORE)).Store;
        }

        public void Update()
        {
            if (!energy.removeResource(MetabolicRate))
            {
                Destroy(gameObject);
            }
        }
    }
}
