using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stopwatch : MonoBehaviour // referenced this video for stopwatch script https://www.youtube.com/watch?v=HLz_k6DSQvU&ab_channel=CocoCode
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
        string zeroInsert = ""; // added by me
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
        timeText.text = time.Minutes.ToString() + ":" + zeroInsert + time.Seconds.ToString() + ":" + zeroInsertMS1 + zeroInsertMS2 + time.Milliseconds.ToString(); // from tutorial
        if (time.Minutes >= 10){ // added by me
            timeText.text = "TIME OUT";
        }
    }

    public void StartStopwatch(){
        stopwatchActive = true;
    }

    public void StopStopwatch(){
        stopwatchActive = false;
    }

     public void nextScene(){
        Physics.gravity = new Vector3(0, -30f, 0);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
