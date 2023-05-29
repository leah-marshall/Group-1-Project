using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private Stopwatch stopwatch;
    private Rigidbody playerBody;
    private ballcontroller player;
    private AudioSource music;
    [SerializeField] private AudioClip goalClip;
    private Animator timeAnimation;
    static public string level1Time, level2Time, level3Time;
    // Start is called before the first frame update
    void Start()
    {
        stopwatch = GameObject.Find("TimeText").GetComponent<Stopwatch>();
        playerBody = GameObject.Find("Camera").transform.parent.parent.GetComponent<Rigidbody>();
        player = GameObject.Find("Camera").transform.parent.parent.GetComponent<ballcontroller>();
        music = GameObject.Find("MusicTemp").GetComponent<AudioSource>();
        timeAnimation = stopwatch.gameObject.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){
            music.loop = false;
            music.Stop();
            music.PlayOneShot(goalClip);
            Vector3 velocityRef = Vector3.zero; // referenced unity docs https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html + scriptkid's comment https://forum.unity.com/threads/stopping-rigidbody-on-a-dime.263743/
            playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, new Vector3(0, playerBody.velocity.y, 0), ref velocityRef, 0.1f); 
            player.movementEnabled = false;
            stopwatch.StopStopwatch();
            updatePlayerTimes();
            Debug.Log("Level 1: " + level1Time + " Level 2: " + level2Time + " Level 3: " + level3Time);
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

    void updatePlayerTimes(){
        if (SceneManager.GetActiveScene().buildIndex == 2){
            level1Time = stopwatch.timeText.text;
            timeAnimation.Play("timeCentered");
        } else if (SceneManager.GetActiveScene().buildIndex == 4){
            level2Time = stopwatch.timeText.text;
            timeAnimation.Play("timeCentered2");
        } else if (SceneManager.GetActiveScene().buildIndex == 6){
            level3Time = stopwatch.timeText.text;
            timeAnimation.Play("timeCentered3");
        }
    }
}
