using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles
{
    public class EatFromTileAllele : Allele
    {
        public bool CanMove;
        public Nutrient nutrient;
        public int MaxConsumptionRate;

        private Tile closestTile;
        private NutrientStoreAllele nutrientStore;

        void Start()
        {
            closestTile = TileManager.instance.getTileClosestTo(transform.position);
            nutrientStore = (NutrientStoreAllele)gameObject.GetComponent<Genome>().GetActiveAllele(Gene.NUTRIENTSTORE);
        }

        void Update()
        {
            if (CanMove)
            {
                closestTile = TileManager.instance.getTileClosestTo(transform.position);
            }

            int amountAvailable = closestTile.getNutrientDeposit(nutrient).amount;
            int spaceAvailable = nutrientStore.Capacity - nutrientStore.GetNutrients(nutrient);

            int amountToConsume = Math.Max(amountAvailable, Math.Max(spaceAvailable, MaxConsumptionRate));
            closestTile.removeNutrient(nutrient, amountToConsume);
            nutrientStore.AddNutrients(nutrient, amountToConsume);
        }
    }
}
