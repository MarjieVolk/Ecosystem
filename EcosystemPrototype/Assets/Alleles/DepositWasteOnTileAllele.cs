using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles
{
    public class DepositWasteOnTileAllele : Allele
    {
        public bool CanMove;
        public Nutrient nutrient;
        public int MaxDepositRate;

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

            int amountAvailable = nutrientStore.GetNutrients(nutrient);

            int amountToConsume = Math.Max(amountAvailable, MaxDepositRate);
            nutrientStore.RemoveNutrients(nutrient, amountToConsume);
            closestTile.addNutrient(nutrient, amountToConsume);
        }
    }
}
