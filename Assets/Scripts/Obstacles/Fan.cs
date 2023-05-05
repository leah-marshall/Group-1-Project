using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    private Transform player;
    private Rigidbody playerBody;
    private ballcontroller playerController;
    private bool inFanArea = false;
    private Renderer playerMat;
    [SerializeField] private float fanStrength;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        playerBody = player.GetComponent<Rigidbody>();
        playerController = player.GetComponent<ballcontroller>();
        playerMat = player.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inFanArea){  
            playerBody.AddForce(gameObject.transform.forward * fanStrength);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            playerMat.material = playerController.red;
            inFanArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            playerMat.material = playerController.blue;
            inFanArea = false;
        }
    }
}
