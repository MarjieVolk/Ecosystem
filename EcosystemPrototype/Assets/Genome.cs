using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Genome : MonoBehaviour {

	private Dictionary<Gene, GeneObj> alleles;

	// Use this for initialization
	void Start () {
		alleles = new Dictionary<Gene, GeneObj>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void init(Dictionary<Gene, Allele> halfOne, Dictionary<Gene, Allele> halfTwo) {
		foreach (Gene type in halfOne.Keys) {
			Allele alleleOne = halfOne[type];
			Allele alleleTwo = halfTwo[type];

			GeneObj gene = new GeneObj(alleleOne, alleleTwo);
			alleles[type] = gene;
		}
	}

	public Dictionary<Gene, Allele> getHalfGenome() {
		Dictionary<Gene, Allele> ret = new Dictionary<Gene, Allele>();

		foreach (Gene type in alleles.Keys) {
			ret[type] = alleles[type].getRandom();
		}

		return ret;
	}

    public Allele GetActiveAllele(Gene type)
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
