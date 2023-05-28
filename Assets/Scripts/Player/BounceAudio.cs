using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAudio : MonoBehaviour
{
    private Rigidbody player;
    private AudioSource audioSound, woosh, shine, gravity, tilt, shatter;
    [SerializeField] private AudioClip[] bounceSounds;
    private int index;
    private AudioClip currentSound;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Camera").transform.parent.parent.GetComponent<Rigidbody>();
        audioSound = player.gameObject.GetComponent<AudioSource>();
        woosh = GameObject.Find("WooshAudio").GetComponent<AudioSource>();
        shine = GameObject.Find("ShineAudio").GetComponent<AudioSource>();
        gravity = GameObject.Find("GravityAudio").GetComponent<AudioSource>();
        tilt = GameObject.Find("TiltAudio").GetComponent<AudioSource>();
        shatter = GameObject.Find("BreakAudio").GetComponent<AudioSource>();
        currentSound = bounceSounds[0];
        index = 0;
    }

    public void cycleSounds(){
        audioSound.volume = player.velocity.magnitude/100;
            if (!audioSound.isPlaying){
                audioSound.PlayOneShot(currentSound);
                index++;
                if (index > bounceSounds.Length -1){
                    index = 0;
                }
                currentSound = bounceSounds[index];
            }
    }

    public void wooshEffect(){
        woosh.pitch = 1 + player.velocity.magnitude/75;
        woosh.volume = Mathf.Lerp(woosh.volume, player.velocity.magnitude/150, 0.1f);
        if (!woosh.isPlaying){
            woosh.Play();
        }
    }

    public void wooshStop(){
        woosh.volume = Mathf.Lerp(woosh.volume, 0, 0.1f);
        woosh.Stop();
    }

    public void shineEffect(){
        shine.volume = Mathf.Lerp(shine.volume, 0.3f, 0.1f);
        if (!shine.isPlaying){
            shine.Play();
        }
    }

    public void shineStop(){
        shine.volume = Mathf.Lerp(shine.volume, 0, 0.1f);
        shine.Stop();
    }

    public void gravityEffect(){
        gravity.volume = Mathf.Lerp(gravity.volume, 0.45f, 0.1f);
        if (!gravity.isPlaying){
            gravity.Play();
        }
    }

    public void gravityStop(){
        gravity.volume = Mathf.Lerp(gravity.volume, 0, 0.1f);
        gravity.Stop();
    }

    public void tiltEffect(){
        if (!tilt.isPlaying){
            tilt.PlayOneShot(tilt.clip);
        }
    }

    public void shatterEffect(){
            shatter.PlayOneShot(shatter.clip);
    }
}
