using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlantStuff : MonoBehaviour {

	public int visionRange;
	public NutrientDeposit.Nutrient inputNutrient;
	public NutrientDeposit.Nutrient outputNutrient;
	public int reproductionCost;
		
	private int energy = 0;
	private Tile tile;

	private System.Random gen = new System.Random();

	// Use this for initialization
	void Start () {
		tile = TileManager.instance.getTileClosestTo(transform.position);
		tile.setPlant(this);
		transform.position = tile.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (gen.Next(100) > 90) {
			if (!tile.removeNutrient(inputNutrient, 1)) {
				UnityEngine.Object.Destroy(this.gameObject);
				return;
			}
			energy += 1;
			tile.addNutrient(outputNutrient, 1);
		}

		if (gen.Next(100) < (energy - reproductionCost) * 2.0f) {
			Tile[] tiles = TileManager.instance.getTilesInRange(this.transform.position, visionRange);
			List<Tile> options = new List<Tile>();

			foreach (Tile t in tiles) {
				if (!t.hasPlant() && t.nutrientAmount(inputNutrient) > 5) {
					options.Add(t);
				}
			}

			if (options.Count > 0) {
				Tile t = options[gen.Next(options.Count)];

				GameObject child = GameObject.CreatePrimitive(PrimitiveType.Capsule);
				PlantStuff comp = (PlantStuff) child.AddComponent(typeof(PlantStuff));
				comp.visionRange = visionRange;
				comp.inputNutrient = inputNutrient;
				comp.outputNutrient = outputNutrient;
				comp.reproductionCost = reproductionCost;
				child.transform.position = t.transform.position;
				child.transform.localScale = this.transform.localScale;
				child.renderer.material = this.renderer.material;

				comp.tile = t;
				t.setPlant(comp);

				energy -= reproductionCost;
			}
		}
	}
}
