using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ballcontroller : MonoBehaviour
{
    private Transform playerSphere; // stores player sphere object
    private Rigidbody playerBody; // stores player's rigidbody component for physics-based movement
    public Material red, blue, peak;
    private Transform TPCam; // stores player camera
    private bool grounded;
    public bool isDiving;
    [SerializeField] private float groundedDist;
    [HideInInspector] public bool onGravityPlatform;
    [HideInInspector] public Vector3 downDirection; // changes depending on gravity
    public Image spacebar;
    private float currentX; // stores starting x position of the mouse, then tracks current position via change in x position
    public float sensitivity; // public so can be accessed by roll script
    [SerializeField] private float MaxSpeed;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float BounceSpeed;
    [SerializeField] private float BounceHeightLimit;
    [SerializeField] private float StopTime;
    private Collider bounceCollider; 
    private float startHeight, maxHeight, heightGain;

    // Start is called before the first frame update
    void Start()
    {
        spacebar.enabled = false;
        playerSphere = gameObject.GetComponent<Transform>();
        playerBody = playerSphere.GetComponent<Rigidbody>();
        TPCam = GameObject.Find("Camera").GetComponent<Transform>();
        onGravityPlatform = false;
        downDirection = -Vector3.up;
        currentX = Input.mousePosition.x;
        Cursor.lockState = CursorLockMode.Locked; // locks mouse to centre of screen
        bounceCollider = gameObject.GetComponent<Collider>();
        startHeight = playerBody.position.y;
        maxHeight = 0;
        isDiving = false;
    }

    void Update(){ 
        highlightPeak();
        camControl();
    }

    void FixedUpdate()
    {
        groundCheck();
        updateMaxHeight();
        minimumBounce();
        move();
    }

    void minimumBounce(){
        if (grounded && heightGain < 5.0f){
            bounceCollider.material.bounciness = 0.965f;
        } else {
            bounceCollider.material.bounciness = 0.80f;
        }
    }

    void updateMaxHeight(){
        if (playerBody.velocity.y > 0){
            maxHeight = playerBody.position.y;
        }
        if (grounded){
            startHeight = playerBody.position.y;
        }
            heightGain = maxHeight - startHeight;
        /*
            Debug.Log("start height: " + startHeight);
            Debug.Log("max height: " + maxHeight);
            Debug.Log("height gain: " + heightGain);
        */
    }

    
    void move() {
        float forwardMotion = Input.GetAxisRaw("Vertical");
        float horizontalMotion = Input.GetAxisRaw("Horizontal");
        float bounce = 0;
        if (Input.GetKey("space")){
            spacebar.enabled = true;
            bounce = 1;
            isDiving = true;
        } else {
            spacebar.enabled = false;
            bounce = 0;
        }
            Vector3 localVelocity = transform.InverseTransformDirection(playerBody.velocity); // referenced for limiting local velocity on a 3d rigidbody https://answers.unity.com/questions/404420/rigidbody-constraints-in-local-space.html
                localVelocity.x = Mathf.Clamp(localVelocity.x, -MaxSpeed, MaxSpeed);
                localVelocity.z = Mathf.Clamp(localVelocity.z, -MaxSpeed, MaxSpeed);
                localVelocity.y = Mathf.Clamp(localVelocity.y, -BounceHeightLimit*2, BounceHeightLimit);
            playerBody.velocity = transform.TransformDirection(localVelocity);  
       
          Vector3 velocityRef = Vector3.zero; // referenced unity docs https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html + scriptkid's comment https://forum.unity.com/threads/stopping-rigidbody-on-a-dime.263743/
            if (forwardMotion == 0){
                playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, new Vector3(playerBody.velocity.x, playerBody.velocity.y, 0), ref velocityRef, StopTime); 
            } else if (horizontalMotion == 0){
                playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, new Vector3(0, playerBody.velocity.y, playerBody.velocity.z), ref velocityRef, StopTime); // reference ends here
            }
            playerBody.AddForce((playerSphere.forward * forwardMotion * MoveSpeed)
            + (playerSphere.right * horizontalMotion * MoveSpeed)
            + (downDirection * bounce * BounceSpeed));
    }

    void highlightPeak(){
        if (!grounded && !onGravityPlatform && playerBody.velocity.y < 3f){
            playerSphere.GetComponent<Renderer>().material = peak;
        } else if (!onGravityPlatform){
            playerSphere.GetComponent<Renderer>().material = blue;
        }
    }

    void groundCheck(){ // looked at previous script
        LayerMask groundMask = LayerMask.GetMask("Ground"); // referenced layer masking doc for optimization https://docs.unity3d.com/ScriptReference/LayerMask.html
        // Practical Reference Begins Here
        Ray groundCheck = new Ray(playerSphere.position, downDirection); // referenced practical 5-2, changed from Camera.main.ViewportPointToRay because ray relative to player not camera (ask how to reference practicals)
        RaycastHit hitInfo; // as in practical
        // as in practical + Unity Doc for layer masking, only check objects within groundedDist and on ground layer
        // referenced for 1 line raycast if statement https://forum.unity.com/threads/solved-checking-if-raycasthit-null-not-working.370595/#:~:text=RaycastHit%20is%20a%20struct%2C%20structs,RaycastHit%20info%20has%20been%20set.
        // author lordofduct
        // Thread Reference Begins Here
        if (Physics.Raycast(groundCheck, out hitInfo, groundedDist, groundMask)) // check if object on ground layer was hit to see if player is touching ground
        {
        // Thread Reference Ends Here
            grounded = true;
        } 
        else
        {
            grounded = false;
        }
        // Practical Reference Ends Here
    }

    void camControl()
    {
        if (playerBody.velocity.magnitude >= 20.0f){
            TPCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(TPCam.GetComponent<Camera>().fieldOfView, 60 + playerBody.velocity.magnitude * 1.15f, 0.05f);
        } else {
            TPCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(TPCam.GetComponent<Camera>().fieldOfView, 60, 0.1f);
        }
        TPCam.GetComponent<Camera>().fieldOfView = Mathf.Clamp(TPCam.GetComponent<Camera>().fieldOfView, 60, 90);
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

    private void OnCollisionEnter(Collision collision)
    {
        isDiving = false;
    }
}
