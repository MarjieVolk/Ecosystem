using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Alleles;

public class Genome : MonoBehaviour {

	private Dictionary<string, GeneObj> alleles;

	void Start () {
		alleles = new Dictionary<string, GeneObj>();

		// Search for Allele components and register them with old Genome here
		Allele[] allAlleles = this.gameObject.GetComponents<Allele>();
		Dictionary<string, List<Allele>> temp = new Dictionary<string, List<Allele>>();
		foreach (Allele a in allAlleles) {
			if (!temp.ContainsKey(a.gene)) {
				temp[a.gene] = new List<Allele>();
			}

			temp[a.gene].Add(a);
			a.setGenome(this);
		}

		foreach (string gene in temp.Keys) {
			List<Allele> geneAlleles = temp[gene];
			if (geneAlleles.Count != 2) {
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
			active = one;
			inactive = two;
			//TODO: tell dominant it is dominant
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
