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

	private class GeneObj {

		private static System.Random gen = new System.Random();
		private Allele dominant;
		private Allele recessive;

		public GeneObj(Allele one, Allele two) {
			//TODO: decide which is dominant
			dominant = one;
			recessive = two;
			//TODO: tell dominant it is dominant
		}

		public Allele getDominant() {
			return dominant;
		}

		public Allele getRecessive() {
			return recessive;
		}

		public Allele getRandom() {
			if (gen.Next(2) == 0) {
				return dominant;
			} else {
				return recessive;
			}
		}
	}
}
