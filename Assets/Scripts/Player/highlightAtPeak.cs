using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highlightAtPeak : MonoBehaviour
{
    private ballcontroller player;
    private Rigidbody playerBody;
    private ParticleSystem highlightShine;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<ballcontroller>();   
        playerBody = gameObject.GetComponent<Rigidbody>();
        highlightShine = GameObject.Find("sparkle").GetComponent<ParticleSystem>();
        highlightShine.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        highlightPeak();
    }

    void highlightPeak(){
        if (!player.grounded && !player.onGravityPlatform 
            && playerBody.velocity.y < 10f && playerBody.velocity.y > -10f && player.heightGain > 4.5f
            && (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0))){
            highlightShine.Play();
        } else if (player.grounded && !player.onGravityPlatform){
            highlightShine.Stop();
        }
    }
}
