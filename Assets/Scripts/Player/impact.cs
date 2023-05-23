using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class impact : MonoBehaviour
{
    private ballcontroller player;
    private Transform playerSphere;
    private Rigidbody playerBody;
    private ParticleSystem impactEffect;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<ballcontroller>();   
        playerSphere = gameObject.GetComponent<Transform>();
        playerBody = gameObject.GetComponent<Rigidbody>();
        impactEffect = GameObject.Find("TextPow").GetComponent<ParticleSystem>();
        impactEffect.Stop();
    }

    // Update is called once per frame
    void Update()
    {
    }

    

    void OnTriggerEnter(Collider other){
        if (other.gameObject.layer == 7 && playerBody.velocity.y < -60){
            impactEffect.Emit(1);
        }
    }  
}
