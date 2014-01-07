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
        int numDigestions = computeNumDigestions();

        energy.removeEnergy(numDigestions * EnergyInput);
        energy.addEnergy(numDigestions * EnergyOutput);
        nutrients.RemoveNutrients(Input, numDigestions);
        nutrients.AddNutrients(Output, numDigestions);
	}

    private int computeNumDigestions()
    {
        int availableInputs = nutrients.GetNutrients(Input);
        int availableNutrientSpace = nutrients.Capacity - nutrients.GetNutrients(Output);
        int availableEnergy = energy.Energy / EnergyInput;
        int availableEnergySpace = (energy.MaxEnergy - energy.Energy) / (EnergyOutput - EnergyInput);

        return Mathf.Max(availableInputs, availableNutrientSpace, availableEnergy, availableEnergySpace);
    }
}
