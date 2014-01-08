using UnityEngine;
using System.Collections;

public class EnergyStoreAllele : Allele {

	public int StartingEnergy;
	public int MaxEnergy;

    public int Energy { get; private set; }

	/// <summary>
	/// Adds the energy.
	/// </summary>
	/// <returns>The energy that could not be added.</returns>
	/// <param name="toBeAdded">The energy to be added.</param>
	public int addEnergy(int toBeAdded)
	{
		int nextEnergy = Energy + toBeAdded;
		int overflowEnergy = 0;
		if (nextEnergy > MaxEnergy) {
			overflowEnergy = nextEnergy - MaxEnergy;
			nextEnergy = MaxEnergy;
		}

        Energy = nextEnergy;

		return overflowEnergy;
	}

	/// <summary>
	/// Removes the energy.
	/// </summary>
	/// <returns><c>true</c>, if energy was removed, <c>false</c> otherwise.</returns>
	/// <param name="toBeRemoved">The energy to be removed.</param>
	public bool removeEnergy(int toBeRemoved)
	{
		int nextEnergy = Energy - toBeRemoved;
		if (nextEnergy < 0) {
			return false;
		}

		Energy = nextEnergy;
		return true;
	}

	// Use this for initialization
	void Start () {
		Energy = StartingEnergy;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
