﻿using System;
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
        [GeneticallyInheritable]
        public string NutrientStoreGeneBase;

        private Tile closestTile;
        private IntegerResourceStore nutrientStore;

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

            int amountAvailable = nutrientStore.Amount;

            int amountToConsume = Math.Min(amountAvailable, MaxDepositRate);
            nutrientStore.removeResource(amountToConsume);
            closestTile.addNutrient(nutrient, amountToConsume);
        }
    }
}
