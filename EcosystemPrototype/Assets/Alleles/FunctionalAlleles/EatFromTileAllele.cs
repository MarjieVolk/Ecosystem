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
        [GeneticallyInheritable]
        public double ConsumptionProbability;
        [GeneticallyInheritable]
        public string NutrientStoreGeneBase;

        private Tile closestTile;
        private IntegerResourceStore nutrientStore;

        private System.Random gen;

        public EatFromTileAllele()
        {
            gen = new System.Random();
        }

        void Start()
        {
            closestTile = TileManager.instance.getTileClosestTo(transform.position);
            nutrientStore = ((IntegerResourceStoreAllele)gameObject.GetComponent<Genome>().GetActiveAllele(NutrientStoreGeneBase + Genome.GENE_DELIMITER + nutrient)).Store;
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
            if (gen.NextDouble() < ConsumptionProbability)
            {
                closestTile.removeNutrient(nutrient, amountToConsume);
                nutrientStore.addResource(amountToConsume);
            }
        }
    }
}
