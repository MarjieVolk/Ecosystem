using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NutrientStoreAllele : Allele {

    public int Capacity;

    private Dictionary<Nutrient, int> nutrientStore;

    void Start()
    {
        nutrientStore = new Dictionary<Nutrient, int>();
    }

    /// <summary>
    /// Adds nutrients to the NutrientStore.
    /// </summary>
    /// <param name="nutrient">
    /// The type of nutrient to be added.
    /// </param>
    /// <param name="amount">
    /// The amount of the nutrient to be added.
    /// </param>
    /// <returns>
    /// The amount of the nutrient that could NOT be added, e.g. 0 if all of the nutrients could be added.
    /// </returns>
    public int AddNutrients(Nutrient nutrient, int amount)
    {
        if (nutrientStore[nutrient] == null)
        {
            nutrientStore[nutrient] = 0;
        }

        int nextAmount = amount + nutrientStore[nutrient];
        int overflow = 0;
        if (nextAmount > Capacity)
        {
            overflow = nextAmount - Capacity;
            nextAmount = Capacity;
        }

        nutrientStore[nutrient] = nextAmount;

        return overflow;
    }

    public int GetNutrients(Nutrient nutrient)
    {
        return nutrientStore[nutrient];
    }

    /// <summary>
    /// Removes amount of nutrient from the nutrient store, or nothing at all if that's impossible.
    /// </summary>
    /// <param name="nutrient">
    /// The type of nutrient to be removed.
    /// </param>
    /// <param name="amount">
    /// The amount of the nutrient desired.
    /// </param>
    /// <returns>
    /// true if all nutrients requested were removed, false otherwise.
    /// </returns>
    public bool RemoveNutrients(Nutrient nutrient, int amount)
    {
        if (nutrientStore[nutrient] == null || nutrientStore[nutrient] < amount)
        {
            return false;
        }

        nutrientStore[nutrient] -= amount;
        return true;
    }
}
