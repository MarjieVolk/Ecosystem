using UnityEngine;
using System.Collections;

public class EnergyStoreAllele : Allele {

	public int StartingEnergy;
	public int MaxEnergy;

	public int Energy{ get; }
	private int _energy;

	/// <summary>
	/// Adds the energy.
	/// </summary>
	/// <returns>The energy that could not be added.</returns>
	/// <param name="toBeAdded">The energy to be added.</param>
	public int addEnergy(int toBeAdded)
	{
		int nextEnergy = _energy + toBeAdded;
		int overflowEnergy = 0;
		if (nextEnergy > MaxEnergy) {
			overflowEnergy = nextEnergy - MaxEnergy;
			nextEnergy = MaxEnergy;
		}

		return overflowEnergy;
	}

	/// <summary>
	/// Removes the energy.
	/// </summary>
	/// <returns><c>true</c>, if energy was removed, <c>false</c> otherwise.</returns>
	/// <param name="toBeRemoved">The energy to be removed.</param>
	public bool removeEnergy(int toBeRemoved)
	{
		int nextEnergy = _energy - toBeRemoved;
		if (nextEnergy < 0) {
			return false;
		}

		_energy = nextEnergy;
		return true;
	}

	// Use this for initialization
	void Start () {
		_energy = StartingEnergy;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
