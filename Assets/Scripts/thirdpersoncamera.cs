using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdpersoncamera : MonoBehaviour
{
    private Transform playerSphere;
    private ballcontroller playerController;
    private float currentRoll; // stores starting y position of the mouse, then tracks current position via change in y position

    // Start is called before the first frame update
    void Start()
    {
        playerSphere = GameObject.Find("Player").GetComponent<Transform>(); // using separate script to control local rotation of camera (do not want to roll player's body)
        playerController = playerSphere.GetComponent<ballcontroller>();
        currentRoll = Input.mousePosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion camRoll = Quaternion.identity; // quaternion to store camera rotation on x axis
        float deltaY = Input.GetAxis("Mouse Y");
        currentRoll += -deltaY * playerController.sensitivity; // rather than setting camera roll directly with mouse y pos, track change in positon and multiply by adjustable sensitivity so that cursor can be locked
        currentRoll = Mathf.Clamp(currentRoll, -90, 90);
        camRoll.eulerAngles = new Vector3(currentRoll, playerSphere.rotation.y, playerSphere.rotation.z); // set camera roll to keep player's y and z rotation, set rotation about x to mouse y pos
        transform.localRotation = camRoll; // only change local rotation to prevent yaw overwrite
    }
}
