using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Vector2 rotation = Vector2.zero;
	public float speed = 3;

	void Update () 
    {
		rotation.y += Input.GetAxis("Mouse X");
		rotation.x += -Input.GetAxis("Mouse Y");
		transform.eulerAngles = (Vector2) rotation * speed;
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            transform.Translate(new Vector3(speed * 100 * Time.deltaTime, 0, 0));
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            transform.Translate(new Vector3(-speed * 100 * Time.deltaTime, 0, 0));
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            transform.Translate(new Vector3(0, 0, -speed * 100 * Time.deltaTime));
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            transform.Translate(new Vector3(0, 0, speed * 100 * Time.deltaTime));
    }
}
