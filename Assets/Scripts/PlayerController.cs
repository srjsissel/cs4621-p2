using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Vector2 rotation = Vector2.zero;

    public enum Control {Fly, Walk, Swim};
    public Control playerControl = Control.Walk;
    private Animator anim;

    public float speed = 3f;
    public float swimSpeed = 2f;

    public float gravity = 100.0f;

    public Camera cam;
    public GameObject player;
    GameObject waterPlane;
    GameObject rayPoint;
    ForestGenerator forestGenerator;

    public GameObject thirdPersonCam;
    public GameObject flyCam;

    public Material rayPointActive, rayPointInactive;

    public AudioSource music;
    public AudioClip error;

    public ParticleSystem fire;


    void Start()
    {
        thirdPersonCam.SetActive(true);
        flyCam.SetActive(false);
        anim = gameObject.GetComponentsInChildren<Animator>()[0];
        controller = gameObject.GetComponent<CharacterController>();
        forestGenerator = GameObject.Find("ForestGenerator").GetComponent<ForestGenerator>();
        waterPlane = GameObject.Find("Water Plane");
        rayPoint = GameObject.Find("RayPoint");
        player = this.gameObject;
        rayPoint.SetActive(false);
        music = this.gameObject.AddComponent<AudioSource>();
        music.playOnAwake = false;
        error = Resources.Load<AudioClip>("music/error");
        fire.Stop();
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
                    if (hitInfo.collider != null){
                        rayPoint.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                        if (hitInfo.collider.gameObject == waterPlane){
                            if (Input.GetKeyDown(KeyCode.Mouse0) || hitInfo.collider.gameObject.tag == "Animal"){
                                music.clip = error;
                                music.Play();
                            }
                            rayPoint.GetComponent<Renderer>().material = rayPointInactive;
                        } else if (hitInfo.collider.gameObject.tag == "Plant"){
                            if (Input.GetKeyDown(KeyCode.Mouse0)){
                                fire.transform.position = hitInfo.collider.gameObject.transform.position;
                                fire.Play();
                                forestGenerator.RemoveTree(hitInfo.collider.gameObject);
                            }
                            rayPoint.GetComponent<Renderer>().material = rayPointActive;
                        } else {
                            rayPoint.GetComponent<Renderer>().material = rayPointActive;
                            rayPoint.SetActive(true);
                            if (Input.GetKeyDown(KeyCode.Mouse0)){
                                forestGenerator.GrowTree(rayPoint.transform.position);
                            }
                        }
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
                    anim.SetTrigger("normal");
                    playerControl = Control.Walk;
                } else {
                    anim.SetTrigger("fly");
                    playerControl = Control.Fly;
                    controller.Move(new Vector3(0, 10f, 0));
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
            player.transform.eulerAngles = Vector3.Scale(player.transform.eulerAngles, new Vector3(0, 1, 1));
            controller.Move(player.transform.forward.normalized * speed * 10 * Time.deltaTime);
        }

        // player.transform.rotation = Quaternion.Euler(new Vector3(90, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z));
        
        //会出不去？
        // float myY = player.transform.position.y - 3;
        // if(myY<=0){
        //     myY = 0;
        // }
        // player.transform.position = new Vector3(player.transform.position.x, myY, player.transform.position.z);
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

