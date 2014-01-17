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
        [GeneticallyInheritable]
        public string EnergyStoreGene;

        private DoubleResourceStore energy; 


        public void Start()
        {
            Genome genome = this.gameObject.GetComponent<Genome>();
            energy = ((DoubleResourceStoreAllele)genome.GetActiveAllele(EnergyStoreGene)).Store;
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
