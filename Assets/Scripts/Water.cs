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
            Animator anim = other.gameObject.GetComponentsInChildren<Animator>()[0];
            controller.playerControl = PlayerController.Control.Swim;   
            if (!anim.GetBool("swim")) {anim.SetTrigger("swim");}
            Debug.Log("set to swim");
            music.clip = swimAudio;
            music.loop = true;
            music.Play();
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")) {
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.playerControl = PlayerController.Control.Walk;
            Animator anim = other.gameObject.GetComponentsInChildren<Animator>()[0];
            if (!anim.GetBool("normal")) {anim.SetTrigger("normal");}
            Debug.Log("set to normal");
            music.Stop();
        }
    }
}
