using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    CharacterController controller;
    Vector2 rotation = Vector2.zero;

    bool isFlying = true;
	public float speed = 3f;
    public float gravity = 100.0f;

    void Start(){
        controller = gameObject.GetComponent<CharacterController>();
    }

	void Update () 
    {
        if (!Input.GetKey(KeyCode.LeftControl)){
            rotation.y += Input.GetAxis("Mouse X");
		    rotation.x += -Input.GetAxis("Mouse Y");
        }
		controller.transform.eulerAngles = (Vector2) rotation * speed;
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
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            controller.Move(transform.TransformDirection(new Vector3(speed * 100 * Time.deltaTime, 0, 0)));
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            controller.Move(transform.TransformDirection(new Vector3(-speed * 100 * Time.deltaTime, 0, 0)));
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            controller.Move(transform.TransformDirection(new Vector3(0, 0, -speed * 100 * Time.deltaTime)));
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            controller.Move(transform.TransformDirection(new Vector3(0, 0, speed * 100 * Time.deltaTime)));
    }

    void walkControl(){
        controller.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            controller.Move(transform.TransformDirection(new Vector3(speed * 10 * Time.deltaTime, 0, 0)));
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            controller.Move(transform.TransformDirection(new Vector3(-speed * 10 * Time.deltaTime, 0, 0)));
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            controller.Move(transform.TransformDirection(new Vector3(0, 0, -speed * 10 * Time.deltaTime)));
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            controller.Move(transform.TransformDirection(new Vector3(0, 0, speed * 10 * Time.deltaTime)));
    }
}

