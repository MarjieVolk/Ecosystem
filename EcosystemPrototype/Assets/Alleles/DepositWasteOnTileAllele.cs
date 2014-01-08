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
        private IntegerResourceStore nutrientStore;

        void Start()
        {
            closestTile = TileManager.instance.getTileClosestTo(transform.position);
            nutrientStore = ((IntegerResourceStoreAllele)gameObject.GetComponent<Genome>().GetActiveAllele(Gene.NUTRIENTSTORE)).Store;
        }

        void Update()
        {
            if (!IsActive) return;
            if (CanMove)
            {
                closestTile = TileManager.instance.getTileClosestTo(transform.position);
            }

            int amountAvailable = nutrientStore.Amount;

            int amountToConsume = Math.Max(amountAvailable, MaxDepositRate);
            nutrientStore.removeResource(amountToConsume);
            closestTile.addNutrient(nutrient, amountToConsume);
        }

        public override Allele clone()
        {
            return new DepositWasteOnTileAllele() { CanMove = CanMove, nutrient = nutrient, MaxDepositRate = MaxDepositRate };
        }
    }
}
