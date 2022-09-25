using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		MapGenerator mapGen = (MapGenerator)target;
		float[,] noiseMap;
		ForestGenerator forestGenerator = GameObject.Find("ForestGenerator").GetComponent<ForestGenerator>();
		

		if (DrawDefaultInspector ()) {
			if (mapGen.autoUpdate) {
				noiseMap = mapGen.GenerateMap ();
			}
		}

		if (GUILayout.Button ("Generate")) {
			noiseMap = mapGen.GenerateMap ();
			// forestGenerator.GenerateForest(ref noiseMap);
		}
	}
}