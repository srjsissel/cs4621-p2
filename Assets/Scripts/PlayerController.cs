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

    void Start(){
        controller = gameObject.GetComponent<CharacterController>();
        player = this.gameObject;
    }

	void Update () 
    {
        if (!Input.GetKey(KeyCode.LeftControl)){
            // rotation.y += Input.GetAxis("Mouse X");
            // rotation.y = cam.transform.rotation.y * 10;
		    // rotation.x += -Input.GetAxis("Mouse Y");
        }
		// controller.transform.eulerAngles = (Vector2) rotation * speed;
        if(Input.GetKeyDown(KeyCode.Space)){
            isFlying = !isFlying;
            if (isFlying) {
                controller.Move(new Vector3(0, 5f, 0));
            }
        }

        if (isFlying)
            flyControl();
        else
            walkControl();
    }

    void flyControl(){

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector3 dir = cam.transform.forward * vInput + cam.transform.right * hInput;
        player.transform.forward = Vector3.Slerp(player.transform.forward, dir, Time.deltaTime * 20);
        if(dir != Vector3.zero){
            controller.Move(player.transform.forward.normalized * speed * 20 * Time.deltaTime);
        }
    }

    void walkControl(){
        controller.Move(new Vector3(0, -gravity * Time.deltaTime, 0));

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector3 dir = cam.transform.forward * vInput + cam.transform.right * hInput;
        player.transform.forward = Vector3.Slerp(player.transform.forward, dir, Time.deltaTime * 20);
        if(dir != Vector3.zero){
            player.transform.eulerAngles = Vector3.Scale(player.transform.eulerAngles, new Vector3(0, 1, 1));
            controller.Move(player.transform.forward.normalized * speed * 10 * Time.deltaTime);
        }
    }
}

