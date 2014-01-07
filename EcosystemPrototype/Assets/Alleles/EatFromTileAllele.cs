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

            for (int i = 0; i < MaxConsumptionRate; i++)
            {
                if (!eat()) break;
            }
        }

        private bool eat()
        {
            if (!closestTile.removeNutrient(nutrient, 1))
            {
                return false;
            }

            nutrientStore.AddNutrients(nutrient, 1);
            return true;
        }
    }
}
