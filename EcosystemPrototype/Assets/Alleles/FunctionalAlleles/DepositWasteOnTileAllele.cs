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
		private Dictionary<IntegerResourceStore, Nutrient> wasteNutrients;

        private const string NUTRIENT_STORE = "nutrientstore";

        void Start()
        {
            closestTile = TileManager.instance.getTileClosestTo(transform.position);

			wasteNutrients = new Dictionary<IntegerResourceStore, Nutrient>();
			NutrientValueAllele nutrientVals = (NutrientValueAllele) genome.GetActiveAllele(nutrientValueGene);
			foreach (object o in Enum.GetValues(typeof(Nutrient))) {
				Nutrient n = (Nutrient) o;
				if (nutrientVals.getNutrientPriority(n) == 0) {
					Genome g = gameObject.GetComponent<Genome>();
					String gene = NUTRIENT_STORE + Genome.GENE_DELIMITER + n.ToString();
					IntegerResourceStoreAllele storeAllele = (IntegerResourceStoreAllele) g.GetActiveAllele(gene);
					IntegerResourceStore store = storeAllele.Store;
					wasteNutrients[store] = n;
				}
			}
        }

        void Update()
        {
            if (!IsActive) return;
            if (CanMove)
            {
                closestTile = TileManager.instance.getTileClosestTo(transform.position);
            }

			int amountDeposited = 0;

			foreach (IntegerResourceStore store in wasteNutrients.Keys) {
				int amountAvailable = store.Amount;
				
				int amountToConsume = Math.Min(amountAvailable, MaxDepositRate - amountDeposited);
				store.removeResource(amountToConsume);
				closestTile.addNutrient(wasteNutrients[store], amountToConsume);
				amountDeposited += amountToConsume;
			}

        }
    }
}
