using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Stopwatch stopwatch;
    // Start is called before the first frame update
    void Start()
    {
        stopwatch = GameObject.Find("TimeText").GetComponent<Stopwatch>();
    }

    void OnTriggerEnter(Collider other){
        if (other.name == "Player"){
            stopwatch.StopStopwatch();
        }
    }

    void OnTriggerStay(Collider other){
        if (other.name == "Player"){
            stopwatch.StopStopwatch();
        }
    }
}
