using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutofBounds : MonoBehaviour
{
    private Transform playerPos;
    private Rigidbody playerBody;
    private ballcontroller player;
    private checkPointTracker respawnPoint;
    private Stopwatch stopwatch;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>(); 
        playerBody = GameObject.Find("Player").GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<ballcontroller>();
        respawnPoint = GameObject.Find("Player").GetComponent<checkPointTracker>();
        stopwatch = GameObject.Find("TimeText").GetComponent<Stopwatch>();
    }

    void OnTriggerEnter(Collider other){
        if (other.name == "Player"){
            playerBody.velocity = new Vector3 (0, 0, 0);
            player.highSpeed = false;
            playerPos.position = respawnPoint.currentCheckPoint;
            stopwatch.StopStopwatch();
        }

        if (other.tag == "Box"){
            Destroy(other.gameObject);
        }
    }
}
