﻿using UnityEngine;
using System.Collections;

public abstract class Allele : MonoBehaviour {

	public bool IsActive { get { return _isActive; } set
		{
			_isActive = value;
			SetActive(value);
		}
	}

	public Gene gene;

	private Genome genome;
	private bool _isActive;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected void SetActive(bool active) { }

	public abstract Allele clone();

	public void setGenome(Genome g) {
		this.genome = g;
	}
}
