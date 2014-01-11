using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Alleles.FunctionalAlleles
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

        private string nutrientStoreGene = "nutrientstore";

        void Start()
        {
            closestTile = TileManager.instance.getTileClosestTo(transform.position);
            //TODO specify the nutrient store being fetched properly
            //nutrientStoreGene += Genome.GENE_DELIMITER + nutrient.ToString();
            nutrientStore = ((IntegerResourceStoreAllele)gameObject.GetComponent<Genome>().GetActiveAllele(nutrientStoreGene)).Store;
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

            int amountToConsume = Mathf.Min(amountAvailable, spaceAvailable, MaxConsumptionRate);
            if (new System.Random().Next(100) < 10)
            {
                closestTile.removeNutrient(nutrient, amountToConsume);
                nutrientStore.addResource(amountToConsume);
            }
        }
    }
}
