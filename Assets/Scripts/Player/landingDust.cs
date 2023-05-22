using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class landingDust : MonoBehaviour
{
    private ballcontroller player;
    private Transform playerSphere;
    private Rigidbody playerBody;
    private ParticleSystem dust;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<ballcontroller>();   
        playerSphere = gameObject.GetComponent<Transform>();
        playerBody = gameObject.GetComponent<Rigidbody>();
        dust = GameObject.Find("Landing").GetComponent<ParticleSystem>();
        dust.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        emitDust();
    }

    void emitDust(){
        if (player.grounded){
            dust.Emit((int)(Mathf.Abs(playerBody.velocity.y)/8));
        } else {
            dust.Stop();
        }
    }
}
