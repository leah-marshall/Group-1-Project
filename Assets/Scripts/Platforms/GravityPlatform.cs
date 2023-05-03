using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlatform : MonoBehaviour
{
    private Transform player;
    private Rigidbody playerBody;
    private ballcontroller playerController;
    [SerializeField] private Vector3 gravityDirection;
    private bool inGravityArea = false;
    private Renderer playerMat;
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
        if (inGravityArea){  
            playerBody.AddForce(gravityDirection);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            playerMat.material = playerController.red;
            Physics.gravity = new Vector3(0, 0, 0);
            float yToZero = playerBody.velocity.y;
            while (yToZero > 0.0f){
                playerBody.velocity = new Vector3(playerBody.velocity.x, yToZero, playerBody.velocity.z);
                yToZero -= 20.0f;
            }
            playerController.downDirection = -gameObject.transform.up;
            playerController.onGravityPlatform = true;
            inGravityArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            playerMat.material = playerController.blue;
            playerController.downDirection = Vector3.down;
            Physics.gravity = new Vector3(0, -12f, 0);
            playerController.onGravityPlatform = false;
            inGravityArea = false;
        }
    }
}
