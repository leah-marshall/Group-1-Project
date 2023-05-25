using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdpersoncamera : MonoBehaviour
{
    /* script adapted from Quinn's MPIE project 
    Author: Quinn McMahon
    Location: MPIE Assessment 
    Accessed: 25/05/23    */
    private Transform playerRoll;
    private ballcontroller playerController;
    private float currentRoll; // stores starting y position of the mouse, then tracks current position via change in y position
    [SerializeField] PauseMenu pause_menu;

    // Start is called before the first frame update
    void Start()
    {
        playerRoll = GameObject.Find("Rotator").GetComponent<Transform>(); // using separate script to control local rotation of camera (do not want to roll player's body)
        playerController = playerRoll.parent.GetComponent<ballcontroller>();
        currentRoll = Input.mousePosition.y;
        pause_menu = GameObject.Find("PauseCanvas").GetComponent<PauseMenu>(); // NOT FROM ORIGINAL CODE
    }

    // Update is called once per frame
    void Update()
    {
        if(!pause_menu.is_paused) // NOT FROM ORIGINAL CODE
        {
            Quaternion camRoll = Quaternion.identity; // quaternion to store camera rotation on x axis
            float deltaY = Input.GetAxis("Mouse Y");
            currentRoll += -deltaY * playerController.sensitivity; // rather than setting camera roll directly with mouse y pos, track change in positon and multiply by adjustable sensitivity so that cursor can be locked
            currentRoll = Mathf.Clamp(currentRoll, -90, 90);
            camRoll.eulerAngles = new Vector3(currentRoll, playerRoll.rotation.y, playerRoll.rotation.z); // set camera roll to keep player's y and z rotation, set rotation about x to mouse y pos
            playerRoll.localRotation = camRoll; // only change local rotation to prevent yaw overwrite
        }
        
    }
}
