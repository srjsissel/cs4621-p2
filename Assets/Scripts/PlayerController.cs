using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Vector2 rotation = Vector2.zero;

    bool isFlying = false;
    public float speed = 3f;
    public float gravity = 100.0f;

    public Camera cam;
    GameObject player;

    public GameObject thirdPersonCam;
    public GameObject flyCam;

    void Start()
    {
        thirdPersonCam.SetActive(true);
        flyCam.SetActive(false);
        controller = gameObject.GetComponent<CharacterController>();
        player = this.gameObject;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl)){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            thirdPersonCam.SetActive(false);
            flyCam.SetActive(false);
        }else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (Input.GetKeyDown(KeyCode.Space)){
                isFlying = !isFlying;
                if (isFlying){
                    controller.Move(new Vector3(0, 5f, 0));
                }
            }
            thirdPersonCam.SetActive(!isFlying);
            flyCam.SetActive(isFlying);
            if (isFlying)
                flyControl();
            else
                walkControl();
        }

    }

    void flyControl()
    {

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector3 dir = cam.transform.forward * vInput + cam.transform.right * hInput;
        player.transform.forward = Vector3.Slerp(player.transform.forward, dir, Time.deltaTime * 20);
        if (dir != Vector3.zero)
        {
            controller.Move(player.transform.forward.normalized * speed * 20 * Time.deltaTime);
        }
    }

    void walkControl()
    {
        controller.Move(new Vector3(0, -gravity * Time.deltaTime, 0));

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector3 dir = cam.transform.forward * vInput + cam.transform.right * hInput;
        player.transform.forward = Vector3.Slerp(player.transform.forward, dir, Time.deltaTime * 20);
        if (dir != Vector3.zero)
        {
            player.transform.eulerAngles = Vector3.Scale(player.transform.eulerAngles, new Vector3(0, 1, 1));
            controller.Move(player.transform.forward.normalized * speed * 10 * Time.deltaTime);
        }
    }
}

