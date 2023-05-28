using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    [SerializeField] Rigidbody rb1;
    [SerializeField] Rigidbody rb2;
    [SerializeField] Rigidbody rb3;
    [SerializeField] Rigidbody rb4;
    [SerializeField] Rigidbody rb5;
    [SerializeField] private BreakableFade rb1Fade, rb2Fade, rb3Fade, rb4Fade, rb5Fade;
    // private Rigidbody rb;
    private GameObject player;
    private BounceAudio feedback;
    

    // Combined Zewuzi's New Behaviour Script w/ Leah's Breakable platform scripts
    void Start()
    {
        player = GameObject.Find("Camera").transform.parent.parent.gameObject;
        feedback = player.GetComponent<BounceAudio>();
        // rb = GetComponent<Rigidbody>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player"){
            if(player.GetComponent<ballcontroller>().isDiving)
            {
                feedback.shatterEffect();
                rb1.gameObject.transform.parent = null;
                rb2.gameObject.transform.parent = null;
                rb3.gameObject.transform.parent = null;
                rb4.gameObject.transform.parent = null;
                rb5.gameObject.transform.parent = null;
                
                // Destroy(gameObject, 0f);
                rb1.isKinematic = false;
                rb2.isKinematic = false;
                rb3.isKinematic = false;
                rb4.isKinematic = false;
                rb5.isKinematic = false;
                rb1Fade.fading = true;
                rb2Fade.fading = true;
                rb3Fade.fading = true;
                rb4Fade.fading = true;
                rb5Fade.fading = true;
                Destroy(this.gameObject);
            }  
        }  
    }
}
