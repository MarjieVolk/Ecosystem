using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	private PlantStuff plant;
	public int i;
	public int j;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int nutrientAmount(NutrientDeposit.Nutrient nutrient) {
		return getNutrientDeposit (nutrient).amount;
	}

	public void addNutrient(NutrientDeposit.Nutrient nutrient, int amount) {
		NutrientDeposit d = getNutrientDeposit(nutrient);

		if (d != null) {
			d.amount += amount;
		}

		d = (NutrientDeposit) this.gameObject.AddComponent("NutrientDeposit");
		d.amount = amount;
		d.nutrient = nutrient;
	}

	public NutrientDeposit getNutrientDeposit(NutrientDeposit.Nutrient nutrient) {
		Component[] current = this.gameObject.GetComponents(typeof(NutrientDeposit));
		NutrientDeposit d;
		
		foreach (Component c in current) {
			d = (NutrientDeposit) c;
			if (d.nutrient == nutrient) {
				return d;
			}
		}

		return null;
	}

	public bool removeNutrient(NutrientDeposit.Nutrient nutrient, int amount) {
		NutrientDeposit d = getNutrientDeposit(nutrient);

		if (d == null || d.amount < amount) {
			return false;
		}

		d.amount -= amount;
		return true;
	}

	public bool hasPlant() {
		return plant != null;
	}

	public void setPlant(PlantStuff p) {
		plant = p;
	}
}
