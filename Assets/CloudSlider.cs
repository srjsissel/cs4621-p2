using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSlider: MonoBehaviour
{
    public static Material cloudMat;
    public static float maxVal = 1.5f;
    static bool rain = true;
    static float sliderVal = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        cloudMat = GetComponent<Renderer>().material;
    }

    public static void cloudSlider(float slider){
        sliderVal = slider;
        if (rain)
            cloudMat.SetFloat("_CloudPower", (maxVal) / (slider + 0.7f));
        else 
            cloudMat.SetFloat("_CloudPower", (maxVal + 0.3f) / (slider + 0.7f));
    }

    public static void addRain(bool isRain){
        rain = isRain;
        cloudSlider(sliderVal);
    }
}
