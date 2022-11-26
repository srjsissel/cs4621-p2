using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public AudioSource music;
    public AudioClip swimAudio;    
    private Animator anim;

    private void Start(){
        music = this.gameObject.AddComponent<AudioSource>();
        music.playOnAwake = false;
        swimAudio = Resources.Load<AudioClip>("music/swim"); 
    }
    
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")) {
            PlayerController controller = other.GetComponent<PlayerController>();
            anim = other.gameObject.GetComponentsInChildren<Animator>()[0];

            controller.playerControl = PlayerController.Control.Swim;   
            if (!anim.GetBool("swim")) {anim.SetTrigger("swim");}

            music.clip = swimAudio;
            music.loop = true;
            music.Play();
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")) {
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.playerControl = PlayerController.Control.Walk;
            anim = other.gameObject.GetComponentsInChildren<Animator>()[0];
            anim.SetTrigger("normal");
            music.Stop();
        }
    }
}
