using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles
{
    public class EatFromTileAllele : Allele
    {
        [GeneticallyInheritable]
        public bool CanMove;
        [GeneticallyInheritable]
        public Nutrient nutrient;
        [GeneticallyInheritable]
        public int MaxConsumptionRate;

        private Tile closestTile;
        private IntegerResourceStore nutrientStore;

        void Start()
        {
            closestTile = TileManager.instance.getTileClosestTo(transform.position);
            //TODO specify the nutrient store being fetched properly
            nutrientStore = ((IntegerResourceStoreAllele)gameObject.GetComponent<Genome>().GetActiveAllele(Gene.NUTRIENTSTORE)).Store;
        }

        void Update()
        {
            if (!IsActive) return;
            if (CanMove)
            {
                closestTile = TileManager.instance.getTileClosestTo(transform.position);
            }

            int amountAvailable = closestTile.getNutrientDeposit(nutrient).Store.Amount;
            int spaceAvailable = nutrientStore.RemainingSpace;

            int amountToConsume = Math.Max(amountAvailable, Math.Max(spaceAvailable, MaxConsumptionRate));
            closestTile.removeNutrient(nutrient, amountToConsume);
            nutrientStore.addResource(amountToConsume);
        }

        public override Allele clone()
        {
            return new EatFromTileAllele() { CanMove = CanMove, nutrient = nutrient, MaxConsumptionRate = MaxConsumptionRate };
        }
    }
}
