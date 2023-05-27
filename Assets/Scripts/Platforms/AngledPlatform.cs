using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngledPlatform : MonoBehaviour
{
    private Rigidbody playerBody;
    private ballcontroller player;
    private float StopTimeSave;
    private float StopTimeTimer;
    private bool StopTimeIncrease;
    [SerializeField] private bool vertical;
    [SerializeField] private float rebound;

    
    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.Find("Camera").transform.parent.parent.GetComponent<Rigidbody>();
        player = GameObject.Find("Camera").transform.parent.parent.GetComponent<ballcontroller>();
        StopTimeSave = player.StopTime;
        StopTimeTimer = 60;
        StopTimeIncrease = false;
    }

    void Update(){
        if (StopTimeIncrease){
            StopTimeTimer--;
        }
        if (StopTimeTimer <= 0){
            StopTimeIncrease = false;
            StopTimeTimer = 60;
            player.StopTime = StopTimeSave;
        }
    }
    
    void OnCollisionEnter(Collision other){
        if (other.collider.tag == "Player"){
            player.StopTime = 0.5f;
            StopTimeIncrease = true;
            if (!vertical){
                playerBody.AddForce(gameObject.transform.forward * rebound * Mathf.Sign(playerBody.velocity.y), ForceMode.Impulse);
            } else {
                playerBody.AddForce(gameObject.transform.forward * rebound, ForceMode.Impulse);
            }
        }
    } 
}
