﻿using UnityEngine;
using System.Collections;
using Assets.Alleles;
using Assets;

namespace Assets.Alleles.FunctionalAlleles
{
    public class DigestionAllele : Allele
    {
        [GeneticallyInheritable]
        public Nutrient Input;
        [GeneticallyInheritable]
        public Nutrient Output;
        [GeneticallyInheritable]
        public double EnergyInput;
        [GeneticallyInheritable]
        public double EnergyOutput;
        [GeneticallyInheritable]
        public int MaxDigestionRate;
        [GeneticallyInheritable]
        public string EnergyStoreGene;
        [GeneticallyInheritable]
        public string NutrientStoreGeneBase;

        private IntegerResourceStore inputStore;
        private IntegerResourceStore outputStore;
        private DoubleResourceStore energy;


        // Use this for initialization
        void Start()
        {
            Genome genome = this.gameObject.GetComponent<Genome>();
            energy = ((DoubleResourceStoreAllele)genome.GetActiveAllele(EnergyStoreGene)).Store;

            //TODO look up nutrient storage properly
            inputStore = ((IntegerResourceStoreAllele)genome.GetActiveAllele(NutrientStoreGeneBase + Genome.GENE_DELIMITER + Input)).Store;
            outputStore = ((IntegerResourceStoreAllele)genome.GetActiveAllele(NutrientStoreGeneBase + Genome.GENE_DELIMITER + Output)).Store;
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
            int availableEnergy = (int)(energy.Amount / EnergyInput);
            int availableEnergySpace = (int)(energy.RemainingSpace / (EnergyOutput - EnergyInput));

            return Mathf.Min(availableInputs, availableNutrientSpace, availableEnergy, availableEnergySpace);
        }
    }
}
