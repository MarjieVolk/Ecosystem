using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(NutrientDeposit))]
public class NutrientDepositEditor : Editor {

	private NutrientDeposit deposit;

	public override void OnEnable() {
		deposit = (NutrientDeposit) this.target;
	}

	public override void OnInspectorGUI() {

	}
}
