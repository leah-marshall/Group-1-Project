using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highSpeedOff : MonoBehaviour
{
    private ballcontroller player;
    [SerializeField] private bool on;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<ballcontroller>();
    }


    void OnTriggerEnter(Collider other){
        if (other.name == "Player"){
            player.highSpeed = on;
        }
    }

     void OnTriggerStay(Collider other){
        if (other.name == "Player"){
            player.highSpeed = on;
        }
    }
}
