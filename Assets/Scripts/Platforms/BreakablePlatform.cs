using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    Rigidbody rb;
    public GameObject player;
    bool playerIsDiving;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerIsDiving = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            playerIsDiving = player.GetComponent<ballcontroller>().isDiving;
            if(playerIsDiving == true)
            {
                Destroy(gameObject, 0f);
            }    
        }
    }
}
