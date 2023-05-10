using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointTracker : MonoBehaviour
{
    public Vector3 currentCheckPoint;
    // Start is called before the first frame update
    void Start()
    {
        currentCheckPoint = GameObject.Find("Player").transform.position;
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "Checkpoint" && other.gameObject.transform.position != currentCheckPoint){
            currentCheckPoint = other.gameObject.transform.position;
            Destroy(other.gameObject);
        }
    }
}
