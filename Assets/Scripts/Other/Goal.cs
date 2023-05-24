using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Stopwatch stopwatch;
    private Rigidbody playerBody;
    private ballcontroller player;
    // Start is called before the first frame update
    void Start()
    {
        stopwatch = GameObject.Find("TimeText").GetComponent<Stopwatch>();
        playerBody = GameObject.Find("Player").GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<ballcontroller>();
    }
            /*For line 30 and line 39
            Set the velocity to zero based on user ScriptKid's comment
            Author: ScriptKid
            Location: https://forum.unity.com/threads/stopping-rigidbody-on-a-dime.263743/
            Accessed: 24/05/23

            Referenced how to use vector3
            Author: Unity
            Location: https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html
            Accessed: 24/05/23
            */
    void OnTriggerEnter(Collider other){
        if (other.name == "Player"){
            Vector3 velocityRef = Vector3.zero; 
            playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, new Vector3(0, playerBody.velocity.y, 0), ref velocityRef, 0.1f); 
            player.movementEnabled = false;
            stopwatch.StopStopwatch();
        }
    }

    void OnTriggerStay(Collider other){
        if (other.name == "Player"){
            Vector3 velocityRef = Vector3.zero; 
            playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, new Vector3(0, playerBody.velocity.y, 0), ref velocityRef, 0.1f); 
            if (playerBody.velocity.magnitude < 1){
                playerBody.velocity = velocityRef;
            }
            stopwatch.StopStopwatch();
        }
    }

    void OnTriggerExit(Collider other){
        if (other.name == "Player"){
            player.movementEnabled = true;
        }
    }
}
