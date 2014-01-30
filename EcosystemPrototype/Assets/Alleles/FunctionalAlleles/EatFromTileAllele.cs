using System;
using System.Collections.Generic;
using System.Collections;
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
        public int MaxConsumptionRate;
		[GeneticallyInheritable]
		public string nutrientValueGene;

        private Tile closestTile;
		private NutrientValueAllele nutrientVals;

        private string nutrientStoreGene = "nutrientstore";
		private System.Random gen;

        void Start()
        {
			closestTile = TileManager.instance.getTileClosestTo(transform.position);
			nutrientVals = (NutrientValueAllele) genome.GetActiveAllele(nutrientValueGene);
			gen = new System.Random();
		}

        void Update()
        {
            if (!IsActive) return;
            if (CanMove)
            {
                closestTile = TileManager.instance.getTileClosestTo(transform.position);
            }

			Nutrient? n = getNutrientToEat();

			if (n != null && gen.Next(100) < 10) {
				Nutrient nutrient = (Nutrient) n;
				IntegerResourceStore nutrientStore = ((IntegerResourceStoreAllele)gameObject.GetComponent<Genome>().GetActiveAllele(nutrientStoreGene + Genome.GENE_DELIMITER + nutrient)).Store;

	            int amountAvailable = closestTile.getNutrientDeposit(nutrient).Store.Amount;
	            int spaceAvailable = nutrientStore.RemainingSpace;

	            int amountToConsume = Mathf.Min(amountAvailable, spaceAvailable, MaxConsumptionRate);

	            closestTile.removeNutrient(nutrient, amountToConsume);
	            nutrientStore.addResource(amountToConsume);
			}
        }

		/// <summary>
		/// Returns the type of nutrient this organism decided to eat on this tick.
		/// Chooses randomly from nutrients that are both available and desired.
		/// Returns -1 if no desired nutrients are available;
		/// </summary>
		/// <returns>The nutrient to eat.</returns>
		private Nutrient? getNutrientToEat() {
			List<Nutrient> possibleNutrients = new List<Nutrient>();
			List<int> priorities = new List<int>();
			int sum = 0;

			foreach (object o in Enum.GetValues(typeof(Nutrient))) {
				Nutrient n = (Nutrient) o;
				if (closestTile.nutrientAmount(n) > 0) {
					int priority = nutrientVals.getNutrientPriority(n);
					possibleNutrients.Add(n);
					priorities.Add(priority);
					sum += priority;
				}
			}

			int val = gen.Next(sum);
			for (int i = 0; i < priorities.Count; i++) {
				if (val < priorities[i]) {
					return possibleNutrients[i];
				}
				val -= priorities[i];
			}

			return null;
		}
    }
}











































































