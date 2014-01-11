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
        public Nutrient nutrient;
        [GeneticallyInheritable]
        public int MaxDepositRate;

        private Tile closestTile;
        private IntegerResourceStore nutrientStore;

        private const string NUTRIENT_STORE = "nutrientstore.Rum";

        void Start()
        {
            closestTile = TileManager.instance.getTileClosestTo(transform.position);
            nutrientStore = ((IntegerResourceStoreAllele)gameObject.GetComponent<Genome>().GetActiveAllele(NUTRIENT_STORE)).Store;
        }

        void Update()
        {
            if (!IsActive) return;
            if (CanMove)
            {
                closestTile = TileManager.instance.getTileClosestTo(transform.position);
            }

            int amountAvailable = nutrientStore.Amount;

            int amountToConsume = Math.Min(amountAvailable, MaxDepositRate);
            nutrientStore.removeResource(amountToConsume);
            closestTile.addNutrient(nutrient, amountToConsume);
        }
    }
}
