using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets;
using Assets.Alleles.FunctionalAlleles;

public class TileManager : MonoBehaviour {


	public static TileManager instance;

	private const float TILE_SCALE = 0.9f;

	public int width;
	public int height;
	public float tileSize;

	private GameObject[,] tiles;
	private Tile highlighted = null;
	private Material highlightedMaterialOld = null;

	private Material highlightMaterial;

	void Awake() {
		instance = this;
		highlightMaterial = (Material) Resources.Load("Materials/HighlightedTile", typeof(Material));

		float totalWidth = tileSize * width;
		float totalHeight = tileSize * height;
		
		float startX = this.transform.position.x - (totalWidth / 2);
		float startZ = this.transform.position.z + (totalHeight / 2);
		
		float x = startX;
		float z = startZ;
		
		tiles = new GameObject[width, height];
		
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				tiles[i,j] = createTile(x + (tileSize / 2), z - (tileSize / 2), i, j);
				z -= tileSize;
			}
			
			z = startZ;
			x += tileSize;
		}
		
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				addNutrient(i, j, 40, Nutrient.Sugar);
				addNutrient(i, j, 40, Nutrient.Gold);
				addNutrient(i, j, 40, Nutrient.Oxygen);
				addNutrient(i, j, 40, Nutrient.Rum);
			}
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (highlighted != null) {
			highlighted.renderer.material = highlightedMaterialOld;
			highlighted = null;
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit)) {
			highlighted = getTileClosestTo(hit.point);
			highlightedMaterialOld = highlighted.renderer.material;
			highlighted.renderer.material = highlightMaterial;
		}
	}

	void OnGUI() {
		if (highlighted != null) {
			GUI.Label(new Rect(0, 0, 5000, 100), "Sugar: " + highlighted.nutrientAmount(Nutrient.Sugar) +
			          "\nRum: " + highlighted.nutrientAmount(Nutrient.Rum) +
			          "\nGold: " + highlighted.nutrientAmount(Nutrient.Gold) +
			          "\nOxygen: " + highlighted.nutrientAmount(Nutrient.Oxygen));
            if (highlighted.hasPlant())
            {
                DoubleResourceStore store = ((DoubleResourceStoreAllele)highlighted.Plant.GetComponent<Genome>().GetActiveAllele("energystore")).Store;
                GUI.Label(new Rect(0, 100, 5000, 100), "Energy: " + store.Amount);
            }
		}
	}

	public Tile[] getTilesInRange(Vector3 position, int radius) {
		Tile tile = getTileClosestTo(position);

		List<Tile> tilesInRange = new List<Tile>();
		for (int i = Math.Max(tile.i - radius, 0); i <= Math.Min(tile.i + radius, tiles.GetUpperBound(0)); i++) {
			for (int j = Math.Max(tile.j - radius, 0); j <= Math.Min(tile.j + radius, tiles.GetUpperBound(1)); j++) {
				tilesInRange.Add(getTileComp(i, j));
			}
		}

		return tilesInRange.ToArray();
	}

	public Tile getTileClosestTo(Vector3 position) {
		float x = position.x;
		float z = position.z;

		int i;

		if (x <= transform.position.x - (width * tileSize / 2.0f)) {
			i = 0;
		} else if (x >= transform.position.x + (width * tileSize / 2.0f)) {
			i = width - 1;
		} else {
			float percent = (x - (transform.position.x - (width * tileSize / 2.0f))) / (width * tileSize);
			i = (int) Mathf.Floor(percent * width);
		}

		int j;
		
		if (z <= transform.position.z - (height * tileSize / 2.0f)) {
			j = height - 1;
		} else if (z >= transform.position.z + (height * tileSize / 2.0f)) {
			j = 0;
		} else {
			float percent = (z - (transform.position.z - (height * tileSize / 2.0f))) / (height * tileSize);
			j = height - ((int) Mathf.Ceil(percent * height));
		}

		return getTileComp(i, j);
	}

	private GameObject createTile(float x, float z, int i, int j) {
		GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
		float size = tileSize * TILE_SCALE;
		tile.transform.localScale = new Vector3(size, 0.5f, size);
		tile.transform.position = new Vector3(x, 0, z);
		tile.renderer.material.mainTexture = (Texture2D) Resources.Load("GoodDirt", typeof(Texture2D));
		Tile t = (Tile) tile.AddComponent(typeof(Tile));
		t.i = i;
		t.j = j;
		return tile;
	}

	private void addNutrient(int i, int j, int amount, Nutrient nutrient) {
		Tile tile = getTileComp(i, j);
		tile.addNutrient(nutrient, amount);
	}

	private Tile getTileComp(int i, int j) {
		GameObject tile = tiles[i,j];
		Component[] current = tile.GetComponents(typeof(Tile));
		return (Tile) current[0];
	}
}
