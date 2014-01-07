using UnityEngine;
using System.Collections;

public class DigestionAllele : Allele {

    public Nutrient Input;
    public Nutrient Output;
    public int EnergyInput;
    public int EnergyOutput;
    public int MaxDigestionRate;

    private NutrientStoreAllele nutrients;
    private EnergyStoreAllele energy;

	// Use this for initialization
	void Start () {
	    Genome genome = this.gameObject.GetComponent<Genome>();
        energy = (EnergyStoreAllele)genome.GetActiveAllele(Gene.ENERGYSTORE);
        nutrients = (NutrientStoreAllele)genome.GetActiveAllele(Gene.NUTRIENTSTORE);
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < MaxDigestionRate; i++)
        {
            if (!digest()) break;
        }
	}

    //Digest one unit of the input nutrient into one unit of the output nutrient
    //Cancel the digestion if there's not enough input or if there's not enough room for the outputs
    private bool digest()
    {
        if (!energy.removeEnergy(EnergyInput)) return false;
        if (!nutrients.RemoveNutrients(Input, 1))
        {
            energy.addEnergy(EnergyInput);
            return false;
        }

        int energyOverflow = energy.addEnergy(EnergyOutput);
        int nutrientOverflow = nutrients.AddNutrients(Output, 1);
        if (energyOverflow != 0 || nutrientOverflow != 0)
        {
            energy.removeEnergy(EnergyOutput - energyOverflow);
            energy.addEnergy(EnergyInput);
            nutrients.AddNutrients(Input, 1);
            return false;
        }

        return true;
    }
}
