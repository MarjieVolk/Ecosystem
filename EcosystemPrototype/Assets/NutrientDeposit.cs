using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;

public class NutrientDeposit : MonoBehaviour {
	
	private static Dictionary<Nutrient, Vector3> vectors;
	private const float MAX = 100f;
	private const float MIN = 10f;

	static NutrientDeposit() {
		vectors = new Dictionary<Nutrient, Vector3>();
		vectors[Nutrient.Sugar] = new Vector3(0.25f, 0, 0.25f);
		vectors[Nutrient.Oxygen] = new Vector3(0.25f, 0, -0.25f);
		vectors[Nutrient.Gold] = new Vector3(-0.25f, 0, 0.25f);
		vectors[Nutrient.Rum] = new Vector3(-0.25f, 0, -0.25f);
	}
	
	public Nutrient nutrient;
    public IntegerResourceStore Store;

	private GameObject rend;

    public NutrientDeposit()
    {
        Store = new IntegerResourceStore(int.MaxValue, 0);
    }

	// Use this for initialization
	void Start () {
		rend = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		rend.transform.position = this.transform.position + (this.transform.localScale.x * vectors[nutrient]) + new Vector3(0, 0.3f, 0);
		rend.renderer.material.mainTexture = (Texture2D) Resources.Load(nutrient.ToString(), typeof(Texture2D));
	}
	
	// Update is called once per frame
	void Update () {
		float scale = Mathf.Min(Store.Amount, MAX) / MAX;
		if (scale != 0)
			scale = 0.2f + (0.8f * scale);
		rend.transform.localScale = scale * new Vector3(this.transform.localScale.x, this.transform.localScale.x, this.transform.localScale.z) / 2;
	}

	public void setShow(bool isShow) {
		((MeshRenderer) rend.GetComponent(typeof(MeshRenderer))).enabled = isShow;
	}
}
