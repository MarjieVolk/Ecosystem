﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Alleles;
using System;
using System.Reflection;
using Assets;
using System.Linq;

public class Genome : MonoBehaviour {

	private Dictionary<string, GeneObj> alleles;
    private const string GENE_DELIMITER = ".";

	void Start () {
        //Assumption:
        //Every editor-defined allele will be present at this time
        //No non-editor-defined alleles will be present at this time
		alleles = new Dictionary<string, GeneObj>();

        registerAlleleComponents();
        generateDataAlleles();
        generateNumericDataAlleles();
	}

    private void generateDataAlleles()
    {
        // Create data alleles for every allele that wants them
        // These data alleles should be in the same half-genome as their parent allele!!

        //for each gene in the genome
        foreach (string gene in alleles.Keys)
        {
            GeneObj genotype = alleles[gene];

            //generate the data alleles for the active and inactive alleles in that gene
            Dictionary<string, DataAllele> activeDataAlleles = generateDataAlleles(genotype.active);
            Dictionary<string, DataAllele> inactiveDataAlleles = generateDataAlleles(genotype.inactive);

            //generate default data alleles where necessary (can occur if genotype.active and genotype.inactive have different types)
            IEnumerable<string> dataGenes = Enumerable.Union<string>(activeDataAlleles.Keys, inactiveDataAlleles.Keys);
            generateDefaultAlleles(dataGenes, activeDataAlleles, inactiveDataAlleles);
            generateDefaultAlleles(dataGenes, inactiveDataAlleles, activeDataAlleles);

            foreach (string dataGene in dataGenes)
            {
                alleles[dataGene] = new GeneObj(activeDataAlleles[dataGene], inactiveDataAlleles[dataGene]);
            }
        }
    }

    private void generateDefaultAlleles(IEnumerable<string> dataGenes, Dictionary<string, DataAllele> target, Dictionary<string, DataAllele> source)
    {
        foreach (string dataGene in dataGenes)
        {
            if (!target.ContainsKey(dataGene))
            {
                DataAllele defaultCopy = gameObject.AddComponent<DataAllele>();
                source[dataGene].GenerateDefaultCopy(defaultCopy);
                target[dataGene] = defaultCopy;
            }
        }
    }

    /// <summary>
    /// Generates the data alleles needed to allow the configuration of the given functional allele to be genetically inherited as intended.
    /// </summary>
    /// <param name="functionalAllele"></param>
    /// <returns>
    /// The data alleles that collectively store the configuration of functionalAllele, organized by their full gene name.
    /// </returns>
    private Dictionary<string, DataAllele> generateDataAlleles(Allele functionalAllele)
    {
        Type alleleType = functionalAllele.GetType();

        Dictionary<string, DataAllele> dataAlleles = new Dictionary<string, DataAllele>();

        //for each inheritance group
        Dictionary<string, List<FieldInfo>> fields = collectFieldsByInheritanceGroup(alleleType);
        foreach (string inheritanceGroup in fields.Keys)
        {
            //create a data allele
            DataAllele dataAllele = gameObject.AddComponent<DataAllele>();
            dataAllele.gene = functionalAllele.gene + GENE_DELIMITER + inheritanceGroup;

            //for each field in the inheritance group
            foreach (FieldInfo field in fields[inheritanceGroup])
            {
                //put that data in the data allele
                dataAllele.Data[field] = field.GetValue(functionalAllele);
            }

            dataAlleles[inheritanceGroup] = dataAllele;
        }
        return dataAlleles;
    }

    private static Dictionary<string, List<FieldInfo>> collectFieldsByInheritanceGroup(Type allele)
    {
        Dictionary<string, List<FieldInfo>> fields = new Dictionary<string, List<FieldInfo>>();
        foreach (FieldInfo field in allele.GetFields())
        {
            InheritSeparatelyAttribute[] attributes = (InheritSeparatelyAttribute[])field.GetCustomAttributes(typeof(InheritSeparatelyAttribute), false);
            if (attributes.Length > 2) throw new InvalidAttributeException();
            foreach (InheritSeparatelyAttribute attribute in attributes)
            {
                string inheritanceGroup = attribute.InheritanceGroup;
                if (!fields.ContainsKey(inheritanceGroup))
                {
                    fields[inheritanceGroup] = new List<FieldInfo>();
                }
                fields[inheritanceGroup].Add(field);
            }
        }
        return fields;
    }

    private void generateNumericDataAlleles()
    {
        // Create numeric data alleles for every allele that wants them

        //for each allele in the genome
            //for each numeric inheritance tagged field
                //generate a set of alleles
    }

    private void registerAlleleComponents()
    {
        // Search for Allele components and register them with old Genome here
        Allele[] allAlleles = this.gameObject.GetComponents<Allele>();
        Dictionary<string, List<Allele>> temp = new Dictionary<string, List<Allele>>();
        foreach (Allele a in allAlleles)
        {
            if (!temp.ContainsKey(a.gene))
            {
                temp[a.gene] = new List<Allele>();
            }

            temp[a.gene].Add(a);
            a.setGenome(this);
        }

        foreach (string gene in temp.Keys)
        {
            List<Allele> geneAlleles = temp[gene];
            if (geneAlleles.Count != 2)
            {
                throw new UnityException("There must be exactly 2 alleles corresponding to each gene!");
            }
            alleles[gene] = new GeneObj(geneAlleles[0], geneAlleles[1]);
        }
    }

	void Update () {
	
	}

	public void init(Dictionary<string, Allele> halfOne, Dictionary<string, Allele> halfTwo) {
		foreach (string type in halfOne.Keys) {
			Allele alleleOne = halfOne[type];
			Allele alleleTwo = halfTwo[type];

			alleleOne.setGenome(this);
			alleleTwo.setGenome(this);

			GeneObj gene = new GeneObj(alleleOne, alleleTwo);
			alleles[type] = gene;
		}
	}

	public Dictionary<string, Allele> getHalfGenome() {
		Dictionary<string, Allele> ret = new Dictionary<string, Allele>();

		foreach (string type in alleles.Keys) {
			ret[type] = alleles[type].getRandom();
		}

		return ret;
	}

    public Allele GetActiveAllele(string type)
    {
        if (!alleles.ContainsKey(type)) return null;
        return alleles[type].active;
    }

	private class GeneObj {

		private static System.Random gen = new System.Random();
		public Allele active {get; private set;}
		public Allele inactive {get; private set;}

		public GeneObj(Allele one, Allele two) {
			//TODO: decide which is dominant
            //NOTE: if this behavior changes it will break generateDataAlleles and generateNumericDataAlleles.  Perhaps make them a different constructor?
			active = one;
			inactive = two;
			one.IsActive = true;
			two.IsActive = false;
		}

		public Allele getRandom() {
			if (gen.Next(2) == 0) {
				return active;
			} else {
				return inactive;
			}
		}
	}
}
