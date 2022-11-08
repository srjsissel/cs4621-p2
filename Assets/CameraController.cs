using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject thirdPersonCam;
    public GameObject flyCam;


    // Start is called before the first frame update
    void Start()
    {
        thirdPersonCam.SetActive(true);
        flyCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            thirdPersonCam.SetActive(!thirdPersonCam.activeSelf);
            flyCam.SetActive(!thirdPersonCam.activeSelf);
        }
    }
}
