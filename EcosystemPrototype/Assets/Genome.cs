using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Alleles;
using System;
using System.Reflection;
using Assets;
using System.Linq;

public class Genome : MonoBehaviour {

	private Dictionary<string, GeneObj> alleles;
    public const string GENE_DELIMITER = ".";

	void Awake () {
        //Assumption:
        //Every editor-defined allele will be present at this time
        //No non-editor-defined alleles will be present at this time
		alleles = new Dictionary<string, GeneObj>();

        generateDataAlleles();
        //generateNumericDataAlleles();
        registerAlleleComponents();
	}

    private void generateDataAlleles()
    {
        Allele[] allAlleles = this.gameObject.GetComponents<Allele>();
        foreach (Allele functionalAllele in allAlleles)
        {
            DataAllele.generateDataAlleles(gameObject, functionalAllele);
        }
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
                throw new UnityException("There must be exactly 2 alleles corresponding to each gene, but there is only one allele for the gene " + gene);
            }
            alleles[gene] = new GeneObj(geneAlleles[0], geneAlleles[1]);
        }
    }

	void Update () {
	
	}

	public void init(Dictionary<string, Allele> halfOne, Dictionary<string, Allele> halfTwo) {
        //copy so they can be mutated freely
        halfOne = new Dictionary<string, Allele>(halfOne);
        halfTwo = new Dictionary<string, Allele>(halfTwo);

        DataAllele.GenerateDefaultDataAlleles(gameObject, halfOne, halfTwo);

        //for each gene in either organism's genotype
		foreach (string type in halfOne.Keys) {
            //fetch the alleles for that gene from each organism
            Allele alleleOne = halfOne[type];
            Allele alleleTwo = halfTwo[type];

			Allele newOne = (Allele) gameObject.AddComponent(alleleOne.GetType().Name);
			newOne.clone(alleleOne);
			Allele newTwo = (Allele) gameObject.AddComponent(alleleTwo.GetType().Name);
			newTwo.clone(alleleTwo);
			
			newOne.setGenome(this);
			newTwo.setGenome(this);

			GeneObj gene = new GeneObj(newOne, newTwo);
			alleles[type] = gene;
		}

        populateSeparatelyInheritedFields();
	}

    /// <summary>
    /// Take data from the data alleles and inject them into the functional alleles they belong to.
    /// </summary>
    private void populateSeparatelyInheritedFields()
    {
        //for each gene (that might be a functional gene with corresponding data genes)
        foreach (string gene in alleles.Keys)
        {
            GeneObj genotype = alleles[gene];
            Allele active = genotype.active;
            Allele inactive = genotype.inactive;

            //for each field in the active allele (the value of which might be inherited separately in a data allele)
            foreach (FieldInfo field in active.GetType().GetFields())
            {
                InheritSeparatelyAttribute[] attributes = (InheritSeparatelyAttribute[])field.GetCustomAttributes(typeof(InheritSeparatelyAttribute), false);
                //if this field is in fact inherited separately
                if (attributes.Length == 1)
                {
                    //inject the value in the data allele into the field
                    string dataAlleleGene = DataAllele.nameDataAllele(active, attributes[0].InheritanceGroup);
                    DataAllele data = (DataAllele)alleles[dataAlleleGene].active;
                    field.SetValue(active, data.Data[field]);
                }
            }
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
