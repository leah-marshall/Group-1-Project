using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ballcontroller : MonoBehaviour
{
    private Transform playerSphere; // stores player sphere object
    private Rigidbody playerBody; // stores player's rigidbody component for physics-based movement
    private Transform TPCam; // stores player camera
    public Image spacebar;
    private float currentX; // stores starting x position of the mouse, then tracks current position via change in x position
    public float sensitivity; // public so can be accessed by roll script
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float BounceSpeed;

     [SerializeField] Material mat;

    // Start is called before the first frame update
    void Start()
    {
        spacebar.enabled = false;
        playerSphere = gameObject.GetComponent<Transform>();
        playerBody = playerSphere.GetComponent<Rigidbody>();
        TPCam = GameObject.Find("Camera").GetComponent<Transform>();
        currentX = Input.mousePosition.x;
        Cursor.lockState = CursorLockMode.Locked; // locks mouse to centre of screen
    }

    void Update(){
        camControl();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move();
    }

    
    void move() {
        
        float forwardMotion = Input.GetAxis("Vertical");
        float horizontalMotion = Input.GetAxis("Horizontal");
        float bounce = 0;
        if (Input.GetKey("space")){
            spacebar.enabled = true;
            bounce = 1;
            mat.SetFloat("_Index", 2f);
        } else {
            spacebar.enabled = false;
            bounce = 0;
            mat.SetFloat("_Index", 0f);
        }
        Debug.Log("forward: " + forwardMotion + "horizontal: " + horizontalMotion + "bounce: " + bounce);
        Debug.Log(playerBody.velocity.magnitude);
        if (playerBody.velocity.magnitude == 0)
        {
            playerBody.AddForce(Vector3.up * bounce * 1000);
        }
        else
        {
            playerBody.AddForce((playerSphere.forward * forwardMotion * MoveSpeed)
            + (playerSphere.right * horizontalMotion * MoveSpeed)
            + (Vector3.down * bounce * BounceSpeed));
        }
    }

    void camControl()
    {
        Quaternion camYaw = Quaternion.identity; // quaternion to store camera rotation on y axis
        // Thread Reference Begins Here
        float deltaX = Input.GetAxis("Mouse X");
        // https://forum.unity.com/threads/how-can-i-lock-the-cursor-while-detecting-mouse-position.833401/ looked at ThermalFusion's
        // comment to find out that input.getaxis mouse x allows cursor to be locked while tracking change in mouse position
        // Thread Reference Ends Here
        currentX += deltaX * sensitivity; // rather than setting camera yaw directly with mouse x pos, track change in positon and multiply by adjustable sensitivity so that cursor can be locked
        camYaw.eulerAngles = new Vector3(playerSphere.rotation.x, currentX, playerSphere.rotation.z); // set camera yaw to keep player's x and z rotation, set rotation about y to mouse x pos
        playerSphere.rotation = camYaw; // rotate camera
    }
}
