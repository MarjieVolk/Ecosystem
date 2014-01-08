using UnityEngine;
using System.Collections;
using Assets.Alleles;
using Assets;

public class DigestionAllele : Allele {

    public Nutrient Input;
    public Nutrient Output;
    public double EnergyInput;
    public double EnergyOutput;
    public int MaxDigestionRate;

    private IntegerResourceStore inputStore;
    private IntegerResourceStore outputStore;
    private DoubleResourceStore energy;

	// Use this for initialization
	void Start () {
	    Genome genome = this.gameObject.GetComponent<Genome>();
        energy = ((DoubleResourceStoreAllele)genome.GetActiveAllele(Gene.ENERGYSTORE)).Store;

        //TODO look up nutrient storage properly
        inputStore = ((IntegerResourceStoreAllele)genome.GetActiveAllele(Gene.NUTRIENTSTORE)).Store;
        inputStore = ((IntegerResourceStoreAllele)genome.GetActiveAllele(Gene.NUTRIENTSTORE)).Store;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (!IsActive) return;
        int numDigestions = computeNumDigestions();

        energy.removeResource(numDigestions * EnergyInput);
        energy.addResource(numDigestions * EnergyOutput);
        inputStore.removeResource(numDigestions);
        outputStore.addResource(numDigestions);
	}

    private int computeNumDigestions()
    {
        int availableInputs = inputStore.Amount;
        int availableNutrientSpace = outputStore.RemainingSpace;
        int availableEnergy = (int) (energy.Amount / EnergyInput);
        int availableEnergySpace = (int) (energy.RemainingSpace / (EnergyOutput - EnergyInput));

        return Mathf.Max(availableInputs, availableNutrientSpace, availableEnergy, availableEnergySpace);
    }

    public override Allele clone()
    {
        return new DigestionAllele() { Input = Input, Output = Output, EnergyInput = EnergyInput, EnergyOutput = EnergyOutput, MaxDigestionRate = MaxDigestionRate };
    }
}
