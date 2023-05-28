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
        playerBody = GameObject.Find("Camera").transform.parent.parent.GetComponent<Rigidbody>();
        player = GameObject.Find("Camera").transform.parent.parent.GetComponent<ballcontroller>();
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){
            Vector3 velocityRef = Vector3.zero; // referenced unity docs https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html + scriptkid's comment https://forum.unity.com/threads/stopping-rigidbody-on-a-dime.263743/
            playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, new Vector3(0, playerBody.velocity.y, 0), ref velocityRef, 0.1f); 
            player.movementEnabled = false;
            stopwatch.StopStopwatch();
        }
    }

    void OnTriggerStay(Collider other){
        if (other.tag == "Player"){
            Vector3 velocityRef = Vector3.zero; // referenced unity docs https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html + scriptkid's comment https://forum.unity.com/threads/stopping-rigidbody-on-a-dime.263743/
            playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, new Vector3(0, playerBody.velocity.y, 0), ref velocityRef, 0.1f); 
            if (playerBody.velocity.magnitude < 1){
                playerBody.velocity = velocityRef;
            }
            stopwatch.StopStopwatch();
        }
    }

    void OnTriggerExit(Collider other){
        if (other.tag == "Player"){
            player.movementEnabled = true;
        }
    }
}
