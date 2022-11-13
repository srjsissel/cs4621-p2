using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")) {
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.playerControl = PlayerController.Control.Swim;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")) {
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.playerControl = PlayerController.Control.Walk;
        }
    }
}
