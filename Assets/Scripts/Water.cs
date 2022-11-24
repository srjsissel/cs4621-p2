using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public AudioSource music;
    public AudioClip swimAudio;    

    private void Start(){
        music = this.gameObject.AddComponent<AudioSource>();
        music.playOnAwake = false;
        swimAudio = Resources.Load<AudioClip>("music/swim"); 
    }
    
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")) {
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.playerControl = PlayerController.Control.Swim;   
            
            music.clip = swimAudio;
            music.loop = true;
            music.Play();
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")) {
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.playerControl = PlayerController.Control.Walk;
            music.Stop();
        }
    }
}
