using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForestGenerator : MonoBehaviour {

    [Range(0, 1)]
	public float scaleSlider = 1f;

    public int forestSize = 241; // Overall size of the forest (a square of forestSize X forestSize).
    public int elementSpacing = 10; // The spacing between element placements. Basically grid size.

    public Element[] elements;

    public AudioSource music;
    public AudioClip growTree;

    List<GameObject> objectList;

    List<int> objectTypeList;

    List<float> objectOriginalScaleList;

    List<float> objectGrowScaleList;

    public Material[] materials;

    private void Start() {
        // objectList = new List<GameObject>();
        // Debug.Log(" is null? " + (objectList == null));
        // objectTypeList = new List<int>();
        // objectOriginalScaleList = new List<float>();
        // objectGrowScaleList = new List<float>();
        music = this.gameObject.AddComponent<AudioSource>();
        music.playOnAwake = false;
        growTree = Resources.Load<AudioClip>("music/click_tree");
    }

    public void GrowTree(Vector3 position){
        GameObject randomElement, newElement;
        // float elementHeight = randomElement.gameObject.transform.localScale.y;
        Vector3 rotation = new Vector3(Random.Range(0, 5f), Random.Range(0, 360f), Random.Range(0, 5f));
        float growScale = Random.Range(1.3f, 4f);
        float scale = Random.Range(3f, 4f);
        if (scale <= 0.1f){
            randomElement = elements[1].GetRandom();
            newElement = Instantiate(randomElement);
            objectList.Add(newElement);
            objectTypeList.Add(0);
            objectOriginalScaleList.Add(5f);
            objectGrowScaleList.Add(10f);
            newElement.transform.SetParent(transform);
            newElement.transform.localScale = Vector3.zero;
            newElement.transform.eulerAngles = rotation;
            newElement.transform.position = position;
            newElement.AddComponent<MeshCollider>();
            newElement.tag = "Plant";

        } else {
            randomElement = elements[0].GetRandom();
            newElement = Instantiate(randomElement);
            objectList.Add(newElement);
            objectTypeList.Add(0);
            objectOriginalScaleList.Add(scale);
            objectGrowScaleList.Add(growScale);
            newElement.transform.SetParent(transform);
            newElement.transform.localScale = Vector3.zero;
            newElement.transform.eulerAngles = rotation;
            newElement.transform.position = position;
            newElement.AddComponent<MeshCollider>();
            newElement.tag = "Plant";
        } 

        // Instantiate and place element in world.

        StartCoroutine(Scale(newElement, getTreeScale(growScale, scaleSlider) * scale));
        music.clip = growTree;
        music.Play();
    }

    public void RemoveTree(GameObject tree){
        tree.tag = "Removed";
        StartCoroutine(Shrink(tree));
    }

    IEnumerator Shrink(GameObject element){
        while(element.transform.localScale.x > 0)
        {
            element.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * 1.5f;
            yield return null;
        }
    }


    IEnumerator Scale(GameObject element, float maxSize){
        float timer = 0;
 
        // we scale all axis, so they will have the same value, 
        // so we can work with a float instead of comparing vectors
        while(maxSize > element.transform.localScale.x)
        {
            timer += Time.deltaTime;
            element.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * 1.5f;
            yield return null;
        }
    }

    public void GenerateForest(ref float[,] heightMap){
        objectList = new List<GameObject>();
        objectTypeList = new List<int>();
        objectOriginalScaleList = new List<float>();
        objectGrowScaleList = new List<float>();
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
                    // Debug.Log(element.name);
                    
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
                        float scale = Random.Range(3f, 4f);

                        // Instantiate and place element in world.
                        GameObject newElement = Instantiate(randomElement);
                        objectList.Add(newElement);
                        objectTypeList.Add(i);
                        objectOriginalScaleList.Add(scale);
                        objectGrowScaleList.Add(Random.Range(1.3f, 4f));
                        newElement.transform.SetParent(transform);
                        newElement.transform.localScale = Vector3.one * scale;
                        newElement.transform.eulerAngles = rotation;
                        newElement.transform.position = position + offset;
                        if (i == 0) // newElement is tree
                            newElement.AddComponent<MeshCollider>();
                        newElement.tag = "Plant";

                        // Renderer renderer = newElement.GetComponent<Renderer>();
                        // renderer.sharedMaterial = materials[0];
                        // Break out of this for loop to ensure we don't place another element at this position.
                        // break;

                    }

                }
            }
        }

        setScale(scaleSlider);


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

    float getTreeScale(float growScale, float scaleSlider){
        float tempScale = growScale * scaleSlider - 1f;
        if (tempScale <= 0){
            tempScale = 0f;
        }
        if (tempScale > 1){
            tempScale = 1f;
        }
        return tempScale;
    }

    public void setScale(float newScale){
        scaleSlider = newScale;
        for (int i=0; i<objectList.Count; ++i){
            GameObject o = objectList[i];
            if (o.tag == "Removed")
                continue;
            int oType = objectTypeList[i];
            float tempScale = newScale;
            float growScale = objectGrowScaleList[i];
            // o.transform.localScale = o.transform.localScale * newScale / scaleSlider;
            if (oType == 0){
                // o is Tree
                tempScale = getTreeScale(growScale, tempScale);
            }
            o.transform.localScale = Vector3.one * objectOriginalScaleList[i] * tempScale;
        }
        // foreach (GameObject o in objectList){
        //     o.transform.localScale = o.transform.localScale * newScale / scaleSlider;
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