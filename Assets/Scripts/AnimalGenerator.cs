using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGenerator : MonoBehaviour
{

    public Element[] elements;
    public int animalsSize = 241; // Overall size of the forest (a square of animalsSize X animalsSize).
    public int elementSpacing = 10; // The spacing between element placements. Basically grid size.
    public Material[] materials;

    List<GameObject> objectList;
    List<int> idleTimeList;
    List<int> typeList; 

    // Start is called before the first frame update
    void Start()
    {
    }

    void FixedUpdate(){
        System.Random r = new System.Random();
        for (int i=0;i<objectList.Count;i++){
            if(typeList[i]!=0){
                continue;
            }
            GameObject o = objectList[i];
            if(idleTimeList[i]==0){
                if(r.Next(1,1001)<5){
                    idleTimeList[i] = r.Next(180,600);
                    o.transform.Rotate(0,r.Next(-150,150),0);
                }
            }else{
                if(i==0){
                    Debug.Log(o.transform.position);
                }
                // o.transform.Translate(Vector3.forward * 0.1f);
                o.GetComponent<Rigidbody>().MovePosition(o.GetComponent<Transform>().position + o.GetComponent<Transform>().forward * 0.1f);
                idleTimeList[i] = idleTimeList[i] - 1;
                // o.transform.Translate(Vector3.down, Space.World);
                if(i==0){
                    Debug.Log(o.transform.position);
                }
            }
            
        }
    }

    public void GenerateAnimals(ref float[,] heightMap){
        objectList = new List<GameObject>();
        idleTimeList = new List<int>();
        typeList = new List<int>();
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);


        for (int x = 0; x < (animalsSize - 1) * 10; x += elementSpacing) {
            for (int z = 0; z < (animalsSize - 1) * 10; z += elementSpacing) {

                // For each position, loop through each element...
                for (int i = 0; i < elements.Length; i++) {

                    // Get the current element.
                    Element element = elements[i];
                    // Debug.Log(element.name);
                    
                    // Check if the element can be placed.
                    if (element.CanPlace()) {

                        int xMin = Mathf.FloorToInt(x/10f);
                        int xMax = Mathf.Min(animalsSize - 1, Mathf.CeilToInt(x/10f));
                        int zMin = Mathf.FloorToInt(z/10f);
                        int zMax = Mathf.Min(animalsSize - 1, Mathf.CeilToInt(z/10f));

                        float currentHeight_11 = heightMap [xMin, animalsSize-zMin-1];
                        float currentHeight_12 = heightMap [xMin, animalsSize-zMax-1];
                        float currentHeight_21 = heightMap [xMax, animalsSize-zMin-1];
                        float currentHeight_22 = heightMap [xMax, animalsSize-zMax-1];
                    
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
                        idleTimeList.Add(0);
                        typeList.Add(i);
                        newElement.transform.SetParent(transform);
                        newElement.transform.localScale = Vector3.one * scale * 100;
                        if(i!=0){
                            newElement.transform.localScale = Vector3.one * scale;
                        }
                        newElement.transform.eulerAngles = rotation;
                        newElement.transform.position = position + offset;
                        newElement.tag = "Animal";

                    }

                }
            }
        }

        // setScale(scaleSlider);
        Debug.Log(objectList[1].transform.localScale);
    }
}

// [System.Serializable]
// public class Element {

//     public string name;
//     [Range(1, 100)]
//     public int density;

//     public GameObject[] prefabs;

//     public bool CanPlace () {

//         // // Validation check to see if element can be placed. More detailed calculations can go here, such as checking perlin noise.
//         // if(currentHeight == -Mathf.Infinity){
//         //     return false;
//         // }

//         if (Random.Range(0, 1000) < density)
//             return true;
//         else
//             return false;

//     }

//     public GameObject GetRandom() {

//         // Return a random GameObject prefab from the prefabs array.

//         return prefabs[Random.Range(0, prefabs.Length)];

//     }

// }