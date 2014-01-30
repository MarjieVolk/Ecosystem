using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles.FunctionalAlleles
{
    public class DepositWasteOnTileAllele : Allele
    {
        [GeneticallyInheritable]
        public bool CanMove;
        [GeneticallyInheritable]
		public int MaxDepositRate;
		[GeneticallyInheritable]
		public string nutrientValueGene;

        private Tile closestTile;
        private NutrientValueAllele nutrientVals;

        private const string NUTRIENT_STORE = "nutrientstore";

        void Start()
        {
            closestTile = TileManager.instance.getTileClosestTo(transform.position);

			nutrientVals = (NutrientValueAllele) genome.GetActiveAllele(nutrientValueGene);
        }

        void Update()
        {
            if (!IsActive) return;
            if (CanMove)
            {
                closestTile = TileManager.instance.getTileClosestTo(transform.position);
            }

            Dictionary<IntegerResourceStore, Nutrient> wasteNutrients = determineWasteNutrients();

            int amountDeposited = 0;

			foreach (IntegerResourceStore store in wasteNutrients.Keys) {
				int amountAvailable = store.Amount;
				
				int amountToConsume = Math.Min(amountAvailable, MaxDepositRate - amountDeposited);
				store.removeResource(amountToConsume);
				closestTile.addNutrient(wasteNutrients[store], amountToConsume);
				amountDeposited += amountToConsume;
			}

        }

        private Dictionary<IntegerResourceStore, Nutrient> determineWasteNutrients()
        {
            Dictionary<IntegerResourceStore, Nutrient> wasteNutrients = new Dictionary<IntegerResourceStore, Nutrient>();
            foreach (object o in Enum.GetValues(typeof(Nutrient)))
            {
                Nutrient n = (Nutrient)o;
                if (nutrientVals.getNutrientPriority(n) == 0)
                {
                    Genome g = gameObject.GetComponent<Genome>();
                    String gene = NUTRIENT_STORE + Genome.GENE_DELIMITER + n.ToString();
                    IntegerResourceStoreAllele storeAllele = (IntegerResourceStoreAllele)g.GetActiveAllele(gene);
                    IntegerResourceStore store = storeAllele.Store;
                    wasteNutrients[store] = n;
                }
            }
            return wasteNutrients;
        }
    }
}
