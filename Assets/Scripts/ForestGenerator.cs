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

        for (int x = 0; x < (forestSize - 1) * 10; x += elementSpacing) {
            for (int z = 0; z < (forestSize - 1) * 10; z += elementSpacing) {

                // For each position, loop through each element...
                for (int i = 0; i < elements.Length; i++) {

                    // Get the current element.
                    Element element = elements[i];
                    
                    // Check if the element can be placed.
                    if (element.CanPlace()) {

                        int xMin = Mathf.FloorToInt(x/10f);
                        int xMax = Mathf.Min(forestSize - 1, Mathf.CeilToInt(x/10f));
                        int zMin = Mathf.FloorToInt(z/10f);
                        int zMax = Mathf.Min(forestSize - 1, Mathf.CeilToInt(z/10f));

                        float currentHeight_11 = heightMap [xMin, forestSize-zMin-1];
                        float currentHeight_12 = heightMap [xMin, forestSize-zMax-1];
                        float currentHeight_21 = heightMap [xMax, forestSize-zMin-1];
                        float currentHeight_22 = heightMap [xMax, forestSize-zMax-1];
                    
                        float currentHeight_1 = Mathf.Lerp(currentHeight_11, currentHeight_21, x % 10 / 10f + 0.05f);
                        float currentHeight_2 = Mathf.Lerp(currentHeight_12, currentHeight_22, x % 10 / 10f + 0.05f);

                        float currentHeight = Mathf.Lerp(currentHeight_1, currentHeight_2, z % 10 / 10f);

                        if (float.IsNaN(currentHeight) || float.IsInfinity(currentHeight)){
                            continue;
                        
                        }
                        
                        // Add random elements to element placement.
                        GameObject randomElement = element.GetRandom();
                        float elementHeight = randomElement.gameObject.transform.localScale.y;
                        Vector3 position = new Vector3(x, (currentHeight-0.05f*elementHeight)*10, z);
                        Vector3 offset = new Vector3(-(width/2f-0.5f)*10f, 0f, -(height/2f-0.5f)*10f);
                        Vector3 rotation = new Vector3(Random.Range(0, 5f), Random.Range(0, 360f), Random.Range(0, 5f));
                        Vector3 scale = Vector3.one * Random.Range(3f, 4f);

                        // Instantiate and place element in world.
                        GameObject newElement = Instantiate(randomElement);
                        newElement.transform.SetParent(transform);
                        newElement.transform.localScale = scale;
                        newElement.transform.eulerAngles = rotation;
                        newElement.transform.position = position + offset;

                        // Renderer renderer = newElement.GetComponent<Renderer>();
                        // renderer.sharedMaterial = materials[0];
                        // Break out of this for loop to ensure we don't place another element at this position.
                        // break;

                    }

                }
            }
        }



        // Loop through all the positions within our forest boundary.
        // for (int x = 0; x < forestSize; x += elementSpacing) {
        //     for (int z = 0; z < forestSize; z += elementSpacing) {

        //         // For each position, loop through each element...
        //         for (int i = 0; i < elements.Length; i++) {

        //             // Get the current element.
        //             Element element = elements[i];
                    
                    
        //             float currentHeight = heightMap [x, forestSize-z-1];
        //             // Check if the element can be placed.
        //             if (element.CanPlace(currentHeight)) {
                        
        //                 // Debug.Log(currentHeight);
        //                 // Add random elements to element placement.
        //                 float elementHeight = element.GetRandom().gameObject.transform.localScale.y;
        //                 Vector3 position = new Vector3((x-width/2)*10, (currentHeight-0.05f*elementHeight)*10, (z-height/2)*10);
        //                 Vector3 offset = new Vector3(0f, 0f, 0f);
        //                 Vector3 rotation = new Vector3(Random.Range(0, 5f), Random.Range(0, 360f), Random.Range(0, 5f));
        //                 Vector3 scale = Vector3.one * Random.Range(3f, 4f);

        //                 // Instantiate and place element in world.
        //                 GameObject newElement = Instantiate(element.GetRandom());
        //                 newElement.transform.SetParent(transform);
        //                 newElement.transform.localScale = scale;
        //                 newElement.transform.eulerAngles = rotation;
        //                 newElement.transform.position = position + offset;

        //                 // Renderer renderer = newElement.GetComponent<Renderer>();
        //                 // renderer.sharedMaterial = materials[0];
        //                 // Break out of this for loop to ensure we don't place another element at this position.
        //                 // break;

        //             }

        //         }
        //     }
        // }
    }

}

[System.Serializable]
public class Element {

    public string name;
    [Range(1, 100)]
    public int density;

    public GameObject[] prefabs;

    public bool CanPlace () {

        // // Validation check to see if element can be placed. More detailed calculations can go here, such as checking perlin noise.
        // if(currentHeight == -Mathf.Infinity){
        //     return false;
        // }

        if (Random.Range(0, 1000) < density)
            return true;
        else
            return false;

    }

    public GameObject GetRandom() {

        // Return a random GameObject prefab from the prefabs array.

        return prefabs[Random.Range(0, prefabs.Length)];

    }

}