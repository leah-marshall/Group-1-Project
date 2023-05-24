using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;

public class Stopwatch : MonoBehaviour 
/*This entire script except from the parts highlighted are from this link
Author: Coco Code
Location: https://www.youtube.com/watch?v=HLz_k6DSQvU&ab_channel=CocoCode
Accessed: 24/05/23
*/
{
    private TMP_Text timeText;
    bool stopwatchActive;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        timeText = gameObject.GetComponent<TMP_Text>();
        stopwatchActive = false;
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopwatchActive){
            currentTime += Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        string zeroInsert = ""; // Added by Quinn
        if (time.Seconds < 10){
            zeroInsert = "0";
        } 
        string zeroInsertMS1 = "";
        if (time.Milliseconds < 10){
            zeroInsertMS1 = "0";
        } 
        string zeroInsertMS2 = "";
        if (time.Milliseconds < 100){
            zeroInsertMS2 = "0";
        } 
        timeText.text = time.Minutes.ToString() + ":" + zeroInsert + time.Seconds.ToString() + ":" + zeroInsertMS1 + zeroInsertMS2 + time.Milliseconds.ToString();
        if (time.Minutes >= 10){ // Added by Quinn
            timeText.text = "TIME OUT";
        }
    }

    public void StartStopwatch(){
        stopwatchActive = true;
    }

    public void StopStopwatch(){
        stopwatchActive = false;
    }
}
