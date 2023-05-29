using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour // referenced this video for stopwatch script https://www.youtube.com/watch?v=HLz_k6DSQvU&ab_channel=CocoCode
{
    [HideInInspector] public TMP_Text timeText;
    public bool stopwatchActive;
    private float currentTime;
    private loadingScreen loadManager;
    [SerializeField] private GameObject resultsScreen;

    // Start is called before the first frame update
    void Start()
    {
        timeText = gameObject.GetComponent<TMP_Text>();
        stopwatchActive = false;
        currentTime = 0;
        loadManager = GameObject.Find("LoadingManager").GetComponent<loadingScreen>();
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if(SceneManager.GetActiveScene().buildIndex != 6){
            loadManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        } else {
            resultsScreen.SetActive(true);
            TMP_Text level1Text = GameObject.Find("Level1Text").GetComponent<TMP_Text>();
            TMP_Text level2Text = GameObject.Find("Level2Text").GetComponent<TMP_Text>();
            TMP_Text level3Text = GameObject.Find("Level3Text").GetComponent<TMP_Text>();
            StartCoroutine(results(level1Text, level2Text, level3Text));
        }
    }

    IEnumerator results(TMP_Text _level1Text, TMP_Text _level2Text, TMP_Text _level3Text){
        yield return new WaitForSeconds(1f);
        _level1Text.text += Goal.level1Time;
        yield return new WaitForSeconds(1f);
        _level2Text.text += Goal.level2Time;
        yield return new WaitForSeconds(1f);
        _level3Text.text += Goal.level3Time;
    }
}
