using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    private ballcontroller player;
    private float MaxSpeedSave;
    [SerializeField] private float speedModifier;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<ballcontroller>();
        MaxSpeedSave = player.MaxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other){
        if (other.name == "Player"){
            player.MaxSpeed *= speedModifier;
        }
    }

    void OnTriggerExit(Collider other){
        if (other.name == "Player"){
            player.MaxSpeed = MaxSpeedSave;
        }
    }
}
