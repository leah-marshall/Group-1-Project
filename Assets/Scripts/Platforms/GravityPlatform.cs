using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlatform : MonoBehaviour
{
    private Transform player;
    private Rigidbody playerBody;
    private ballcontroller playerController;
    private Vector3 gravityDirection;
    [SerializeField] private float gravityMultiplier = 1;
    [SerializeField] private float gravitySlow = 0.01f;
    public bool leftGravity, rightGravity, upGravity, forwardGravity, backwardGravity;
    private bool inGravityArea = false;
    private Renderer playerMat;
    // Start is called before the first frame update
    void Start()
    {
        if (leftGravity){
            gravityDirection = new Vector3 (-30*gravityMultiplier, 0, 0);
        } else if (rightGravity){
            gravityDirection = new Vector3 (30*gravityMultiplier, 0, 0);
        } else if (upGravity){
            gravityDirection = new Vector3 (0, 30*gravityMultiplier, 0);
        } else if (forwardGravity){
            gravityDirection = new Vector3 (0, 0, -30*gravityMultiplier);
        } else if (backwardGravity){
            gravityDirection = new Vector3 (0, 0, 30*gravityMultiplier);
        }
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

    /*For line 62
    Set the velocity to zero based on user ScriptKid's comment
    Author: ScriptKid
    Location: https://forum.unity.com/threads/stopping-rigidbody-on-a-dime.263743/
    Accessed: 24/05/23

    Referenced how to use vector3
    Author: Unity
    Location: https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html
    Accessed: 24/05/23
    */

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            playerMat.material = playerController.red;
            Physics.gravity = new Vector3(0, 0, 0);
            Vector3 velocityRef = Vector3.zero; 
            playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, new Vector3(playerBody.velocity.x, 0, playerBody.velocity.z), ref velocityRef, gravitySlow); 
            playerController.downDirection = gameObject.transform.up;
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
            Physics.gravity = new Vector3(0, -30f, 0);
            playerController.onGravityPlatform = false;
            inGravityArea = false;
        }
    }
}
