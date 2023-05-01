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
            Quaternion onPlatformRotation = Quaternion.identity;
            onPlatformRotation.eulerAngles = new Vector3(90.0f, 0, 90.0f);
            player.rotation = onPlatformRotation;
            Physics.gravity = new Vector3(0, 0, 0);
            playerBody.velocity = new Vector3(playerBody.velocity.x, 0, playerBody.velocity.z);
            playerController.downDirection = -gameObject.transform.up;
            inGravityArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            playerMat.material = playerController.blue;
            playerController.downDirection = Vector3.down;
            Physics.gravity = new Vector3(0, -9.81f, 0);
            inGravityArea = false;
        }
    }
}
