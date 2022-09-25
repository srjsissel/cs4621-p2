using UnityEngine;
using System.Collections;
using static ForestGenerator;

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
	public DrawMode drawMode;

	public int mapWidth = 241;
	public int mapHeight = 241;
	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public bool autoUpdate;

	public TerrainType[] regions;

	private void Start() {
		float[,] heightMap = GenerateMap();
		ForestGenerator forestGenerator = GameObject.Find("ForestGenerator").GetComponent<ForestGenerator>();
		forestGenerator.GenerateForest(ref heightMap);
	}

	public float[,] GenerateMap() {
		int noiseSeed = Mathf.FloorToInt(Random.value * float.MaxValue);
		float[,] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
		float[,] addNoise = Noise.GenerateNoiseMap (mapWidth, mapHeight, noiseSeed, noiseScale, octaves, persistance, lacunarity, offset);
		Color[] colourMap = new Color[mapWidth * mapHeight];
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
		
		noiseMap = normalize(noiseMap);

		MapDisplay display = FindObjectOfType<MapDisplay> ();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (noiseMap));
		} else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap (colourMap, mapWidth, mapHeight));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColourMap (colourMap, mapWidth, mapHeight));
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
}

[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}