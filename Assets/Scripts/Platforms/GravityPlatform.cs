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
        player = GameObject.Find("Camera").transform.parent.parent.GetComponent<Transform>();
        playerBody = player.GetComponent<Rigidbody>();
        playerController = player.GetComponent<ballcontroller>();
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
        if (other.tag == "Player")
        {
            Physics.gravity = new Vector3(0, 0, 0);
            Vector3 velocityRef = Vector3.zero; // referenced unity docs https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html + scriptkid's comment https://forum.unity.com/threads/stopping-rigidbody-on-a-dime.263743/
            playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, new Vector3(playerBody.velocity.x, 0, playerBody.velocity.z), ref velocityRef, gravitySlow); 
            playerController.downDirection = gameObject.transform.up;
            playerController.onGravityPlatform = true;
            inGravityArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerController.downDirection = Vector3.down;
            Physics.gravity = new Vector3(0, -30f, 0);
            playerController.onGravityPlatform = false;
            inGravityArea = false;
        }
    }
}
