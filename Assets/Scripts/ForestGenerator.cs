using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator : MonoBehaviour {

    public int forestSize = 241; // Overall size of the forest (a square of forestSize X forestSize).
    public int elementSpacing = 10; // The spacing between element placements. Basically grid size.

    public Element[] elements;

    public Material[] materials;

    private void Start() {
    }

    public void GenerateForest(ref float[,] heightMap){
        // int nForest = Mathf.FloorToInt(Random.Range(50f, 100f));
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        // for (int i=0;i<nForest;i++){
            
        // }

        // Loop through all the positions within our forest boundary.
        for (int x = 0; x < forestSize; x += elementSpacing) {
            for (int z = 0; z < forestSize; z += elementSpacing) {

                // For each position, loop through each element...
                for (int i = 0; i < elements.Length; i++) {

                    // Get the current element.
                    Element element = elements[i];
                    
                    
                    float currentHeight = heightMap [x, forestSize-z-1];
                    // Check if the element can be placed.
                    if (element.CanPlace(currentHeight)) {
                        
                        // Debug.Log(currentHeight);
                        // Add random elements to element placement.
                        
                        Vector3 position = new Vector3((x-width/2)*10, (currentHeight-0.1f)*10, (z-height/2)*10);
                        Vector3 offset = new Vector3(0f, 0f, 0f);
                        Vector3 rotation = new Vector3(Random.Range(0, 5f), Random.Range(0, 360f), Random.Range(0, 5f));
                        Vector3 scale = Vector3.one * Random.Range(3f, 4f);

                        // Instantiate and place element in world.
                        GameObject newElement = Instantiate(element.GetRandom());
                        newElement.transform.SetParent(transform);
                        newElement.transform.eulerAngles = rotation;
                        newElement.transform.localScale = scale;
                        newElement.transform.position = position + offset;

                        Renderer renderer = newElement.GetComponent<Renderer>();
                        renderer.sharedMaterial = materials[0];
                        // Break out of this for loop to ensure we don't place another element at this position.
                        break;

                    }

                }
            }
        }
    }

}

[System.Serializable]
public class Element {

    public string name;
    [Range(1, 10)]
    public int density;

    public GameObject[] prefabs;

    public bool CanPlace (float currentHeight) {

        // Validation check to see if element can be placed. More detailed calculations can go here, such as checking perlin noise.
        if(currentHeight == -Mathf.Infinity){
            return false;
        }

        if (Random.Range(0, 10) < density)
            return true;
        else
            return false;

    }

    public GameObject GetRandom() {

        // Return a random GameObject prefab from the prefabs array.

        return prefabs[Random.Range(0, prefabs.Length)];

    }

}