using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	private GameObject plant;
	public int i;
	public int j;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int nutrientAmount(Nutrient nutrient) {
		return getNutrientDeposit (nutrient).Store.Amount;
	}

	public void addNutrient(Nutrient nutrient, int amount) {
		NutrientDeposit d = getNutrientDeposit(nutrient);

		if (d != null) {
            d.Store.addResource(amount);
		} else {
				d = (NutrientDeposit)this.gameObject.AddComponent ("NutrientDeposit");
				d.Store.addResource(amount);
				d.nutrient = nutrient;
		}
	}

	public NutrientDeposit getNutrientDeposit(Nutrient nutrient) {
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

	public bool removeNutrient(Nutrient nutrient, int amount) {
		NutrientDeposit d = getNutrientDeposit(nutrient);

        return d != null && d.Store.removeResource(amount);
	}

	public bool hasPlant() {
		return plant != null;
	}

	public void setPlant(GameObject p) {
		plant = p;
	}
}
