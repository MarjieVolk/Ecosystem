using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles.FunctionalAlleles
{
    public class DepositNutrientsOnTileOnDeathAllele : Allele
    {
        [GeneticallyInheritable]
        public string NutrientStoreGeneBase;

        public void OnStart()
        {
        }

        public void OnDestroy()
        {
            Tile closestTile = TileManager.instance.getTileClosestTo(transform.position);
            foreach (Nutrient nutrient in Enum.GetValues(typeof(Nutrient)))
            {
                IntegerResourceStoreAllele storeAllele = ((IntegerResourceStoreAllele)genome.GetActiveAllele(NutrientStoreGeneBase + Genome.GENE_DELIMITER + nutrient));
                if(storeAllele == null ) continue;
                IntegerResourceStore store = storeAllele.Store;
                int amount = store.Amount;
                store.removeResource(amount);
                closestTile.addNutrient(nutrient, amount);
            }
        }
    }
}
