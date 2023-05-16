using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    Rigidbody rb;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player"){
            if(player.GetComponent<ballcontroller>().isDiving)
            {
                Destroy(gameObject, 0f);
            }  
        }  
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player"){
            if(player.GetComponent<ballcontroller>().isDiving)
            {
                Destroy(gameObject, 0f);
            }  
        }  
    }
}
