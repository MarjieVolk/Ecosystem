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

            for (int i = 0; i < MaxDepositRate; i++)
            {
                if (!deposit()) break;
            }
        }

        private bool deposit()
        {
            if (!nutrientStore.RemoveNutrients(nutrient, 1))
            {
                return false;
            }

            closestTile.addNutrient(nutrient, 1);
            return true;
        }
    }
}
