using UnityEngine;
using System.Collections;
using static ForestGenerator;

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
	public DrawMode drawMode;
	
	[Range(0, 1)]
	public float colorSlider = 0.5f;

	public int mapWidth = 241;
	public int mapHeight = 241;
	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public bool useFixedSeed;
	public int seed;
	public Vector2 offset;

	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public bool autoUpdate;

	public TerrainType[] regions;

	float[,] heightMap, noiseMap, addNoise;
	int noiseSeed;
	Color[] colourMap;
	Color[] finalColorMap;
	MapDisplay display;
	MeshData mesh;
	GameObject waterPlane;

	private void Start() {
		Random.InitState((int)System.DateTime.Now.Ticks);
		noiseSeed = Mathf.FloorToInt(Random.value * int.MaxValue);
		if (!useFixedSeed){
			seed = Mathf.FloorToInt(Random.value * int.MaxValue);
		}
		noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
		addNoise = Noise.GenerateNoiseMap (mapWidth, mapHeight, noiseSeed, noiseScale, octaves, persistance, lacunarity, offset);
		heightMap = GenerateMap();
		ForestGenerator forestGenerator = GameObject.Find("ForestGenerator").GetComponent<ForestGenerator>();
		waterPlane = GameObject.Find("Water Plane");
		forestGenerator.GenerateForest(ref heightMap);
	}

	public float[,] GenerateMap() {
		getMapColor(ref noiseMap, ref addNoise);
		getFinalMapColor();
		noiseMap = normalize(noiseMap);

		display = FindObjectOfType<MapDisplay> ();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (noiseMap));
		} else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap (finalColorMap, mapWidth, mapHeight));
		} else if (drawMode == DrawMode.Mesh) {
			mesh = MeshGenerator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, meshHeightCurve);
			display.DrawMesh (mesh, TextureGenerator.TextureFromColourMap (finalColorMap, mapWidth, mapHeight));
		}

		float[,] ans = new float[mapWidth, mapHeight];
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				ans[x, y] = meshHeightCurve.Evaluate(noiseMap [x, y]) * meshHeightMultiplier;
				if(noiseMap[x,y]<=regions[1].height){
					ans[x,y] = -Mathf.Infinity;
				}
				
			}
		}
		return ans;
	}



	public void setColor(float newColor){
		colorSlider = newColor;	
		getFinalMapColor();
		display = FindObjectOfType<MapDisplay> ();
		//display.DrawTexture(TextureGenerator.TextureFromColourMap (finalColorMap, mapWidth, mapHeight));
		display.DrawMesh (mesh, TextureGenerator.TextureFromColourMap (finalColorMap, mapWidth, mapHeight));
	}

	void getMapColor(ref float[,] noiseMap, ref float[,] addNoise){
		if (colourMap == null)
			colourMap = new Color[mapWidth * mapHeight];
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				float currentHeight = noiseMap [x, y];
				for (int i = 0; i < regions.Length; i++) {
					if (currentHeight <= regions [i].height) {
						Color tempColor = regions [i].colour;
						if(currentHeight >= regions[0].height){
							float rnd = Random.value * 2f - 1f;
							if(rnd < 0.5f){
								tempColor.r = tempColor.r + rnd*(10f/255f);
								tempColor.g = tempColor.g + rnd*(10f/255f);
								tempColor.b = tempColor.b + rnd*(10f/255f);
							}

							// if( addNoise[x,y]>0.7){
							// 	noiseMap[x,y]-=addNoise[x,y]*Random.value;
							// 	noiseMin = Mathf.Min(noiseMap[x,y],noiseMin);
							// }
						}
						colourMap [y * mapWidth + x] = tempColor;
						break;
					}
				}
			}
		}
	}

	void getFinalMapColor(){
		if (finalColorMap == null)
			finalColorMap = new Color[mapWidth * mapHeight];
		float h, s, v;
		float sMultiplier = colorSlider * 0.8f + 0.6f;	// 0.6 ~ 1.4
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				Color.RGBToHSV(colourMap[y * mapWidth + x], out h, out s, out v);
				finalColorMap [y * mapWidth + x] = Color.HSVToRGB(h, s * sMultiplier, v);
					// colourMap [y * mapWidth + x] * (colorSlider + 0.5f);
			}
		}
	}

	public float[,] normalize(float[,] noiseMap){
		float min = 0;
		float max = 1;
		int mapWidth = noiseMap.GetLength(0);
		int mapHeight = noiseMap.GetLength(1);
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				min = Mathf.Min(noiseMap[x,y],min);
				max = Mathf.Max(noiseMap[x,y],max);
			}
		}

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				noiseMap[x,y] =  (noiseMap[x,y] - min)/(max-min);
			}
		}

		return noiseMap;
	}

	void OnValidate() {
		if (mapWidth < 1) {
			mapWidth = 1;
		}
		if (mapHeight < 1) {
			mapHeight = 1;
		}
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
	}

	public void setHeight(float slider){
		regions[0].height = 0.1f + slider * 0.2f;
		// display = FindObjectOfType<MapDisplay> ();
		// display.DrawMesh (mesh, TextureGenerator.TextureFromColourMap (finalColorMap, mapWidth, mapHeight));
		GenerateMap();
	}

	public void setWaterHeight(float slider){
		//0.1f ~ 2.6f
		waterPlane.transform.position = waterPlane.transform.position + 
										new Vector3(0f, -waterPlane.transform.position.y + slider * 5f + 0.5f, 0f);
	}
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}