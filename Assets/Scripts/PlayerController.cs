using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Vector2 rotation = Vector2.zero;

    public enum Control {Fly, Walk, Swim};
    public Control playerControl = Control.Walk;


    public float speed = 3f;
    public float swimSpeed = 2f;

    public float gravity = 100.0f;

    public Camera cam;
    GameObject player;
    GameObject waterPlane;
    GameObject rayPoint;
    ForestGenerator forestGenerator;

    public GameObject thirdPersonCam;
    public GameObject flyCam;


    void Start()
    {
        thirdPersonCam.SetActive(true);
        flyCam.SetActive(false);
        controller = gameObject.GetComponent<CharacterController>();
        forestGenerator = GameObject.Find("ForestGenerator").GetComponent<ForestGenerator>();
        waterPlane = GameObject.Find("Water Plane");
        rayPoint = GameObject.Find("RayPoint");
        player = this.gameObject;
        rayPoint.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl)){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            thirdPersonCam.SetActive(false);
            flyCam.SetActive(false);
        } else {
            if (Input.GetKey(KeyCode.Q)){
                setPlayerVisible(false);
                Ray ray = new Ray(player.transform.position, cam.transform.forward);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo)){
                    Debug.Log("hit");
                    Debug.Log(hitInfo.point);
                    if (hitInfo.collider.gameObject != waterPlane){
                        rayPoint.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                        rayPoint.SetActive(true);
                    }
                    if (Input.GetKeyDown(KeyCode.Mouse0)){
                        forestGenerator.GrowTree(rayPoint.transform.position);
                    }
                    
                }

                
            } else {
                setPlayerVisible(true);
                rayPoint.SetActive(false);
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (Input.GetKeyDown(KeyCode.Space)){
                if (playerControl == Control.Fly){
                    playerControl = Control.Walk;
                } else {
                    playerControl = Control.Fly;
                    controller.Move(new Vector3(0, 5f, 0));
                }
            }
            thirdPersonCam.SetActive(playerControl == Control.Walk || playerControl == Control.Swim);
            flyCam.SetActive(playerControl == Control.Fly);
            if (playerControl == Control.Fly)
                flyControl();
            else if (playerControl == Control.Walk)
                walkControl();
            else if (playerControl == Control.Swim)
                swimControl();
        }

    }
    void swimControl(){
        controller.Move(new Vector3(0, -gravity * Time.deltaTime, 0));

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        
        Vector3 dir = cam.transform.forward * vInput + cam.transform.right * hInput;
        player.transform.forward = Vector3.Slerp(player.transform.forward, dir, Time.deltaTime * 20);
        if (dir != Vector3.zero)
        {
            // TODO: Modify swim control after model is chosen
            player.transform.eulerAngles = Vector3.Scale(player.transform.eulerAngles, new Vector3(0, 1, 1));
            controller.Move(player.transform.forward.normalized * swimSpeed * 10 * Time.deltaTime);
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

    void setPlayerVisible(bool isVisible) {
        foreach (Renderer r in player.GetComponentsInChildren<Renderer>()){
            r.enabled = isVisible;
        }
    }
}

