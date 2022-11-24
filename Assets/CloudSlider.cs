using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSlider : MonoBehaviour
{
    public static Material cloudMat;
    public float maxVal = 1f;
    // Start is called before the first frame update
    void Start()
    {
        cloudMat = GetComponent<Renderer>().material;
    }

    public void cloudSlider(float slider){
        cloudMat.SetFloat("_CloudPower", maxVal / slider);
    }

    public static void addRain(bool isRain){
        if (isRain){
            cloudMat.SetFloat("_CloudPower", cloudMat.GetFloat("_CloudPower") - 0.5f);
        } else {
            cloudMat.SetFloat("_CloudPower", cloudMat.GetFloat("_CloudPower") + 0.5f);
        }
    }
}
