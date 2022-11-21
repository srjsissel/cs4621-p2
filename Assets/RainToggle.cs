using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainToggle : MonoBehaviour
{
    ParticleSystem rain;

    public AudioSource music;
    public AudioClip rainSfx;
    // Start is called before the first frame update
    void Start()
    {
        rain = GetComponent<ParticleSystem>();
        music = this.gameObject.AddComponent<AudioSource>();
        music.playOnAwake = false;
        music.loop = true;
        rainSfx = Resources.Load<AudioClip>("music/rain");
        music.clip = rainSfx;
        music.Play();
    }

    public void toggle(bool isRain){
        if (isRain){
            rain.Play();
            music.clip = rainSfx;
            music.Play();

        } else {
            rain.Stop();
            music.Stop();
        }
    }
}
