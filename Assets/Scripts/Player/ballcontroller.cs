using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ballcontroller : MonoBehaviour
{
    private Transform playerSphere; // stores player sphere object
    private Rigidbody playerBody; // stores player's rigidbody component for physics-based movement
    public Material red, blue;
    private Transform TPCam; // stores player camera
    [HideInInspector] public bool onGravityPlatform;
    [HideInInspector] public Vector3 downDirection; // changes depending on gravity
    public Image spacebar;
    private float currentX; // stores starting x position of the mouse, then tracks current position via change in x position
    public float sensitivity; // public so can be accessed by roll script
    [SerializeField] private float MaxSpeed;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float BounceSpeed;

    // Start is called before the first frame update
    void Start()
    {
        spacebar.enabled = false;
        playerSphere = gameObject.GetComponent<Transform>();
        playerBody = playerSphere.GetComponent<Rigidbody>();
        TPCam = GameObject.Find("Camera").GetComponent<Transform>();
        onGravityPlatform = false;
        downDirection = Vector3.down;
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
        float forwardMotion = Input.GetAxisRaw("Vertical");
        float horizontalMotion = Input.GetAxisRaw("Horizontal");
        float bounce = 0;
        if (Input.GetKey("space")){
            spacebar.enabled = true;
            bounce = 1;
        } else {
            spacebar.enabled = false;
            bounce = 0;
        }
        if (!onGravityPlatform){
            Vector3 localVelocity = transform.InverseTransformDirection(playerBody.velocity); // referenced for limiting local velocity on a 3d rigidbody https://answers.unity.com/questions/404420/rigidbody-constraints-in-local-space.html
            if (Mathf.Abs(playerBody.velocity.x) > MaxSpeed){
                localVelocity.x = MaxSpeed * horizontalMotion;
            }
            if (Mathf.Abs(playerBody.velocity.z) > MaxSpeed){
                localVelocity.z = MaxSpeed * forwardMotion;
            }
            playerBody.velocity = transform.TransformDirection(localVelocity);
        }
        
        if (playerBody.velocity.magnitude <= 0.0001f)
        {
            playerBody.AddForce(-downDirection * bounce * 1250);
        }
        else
        {
            Vector3 velocityRef = Vector3.zero; // referenced unity docs https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html + scriptkid's comment https://forum.unity.com/threads/stopping-rigidbody-on-a-dime.263743/
            if (forwardMotion == 0){
                playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, new Vector3(playerBody.velocity.x, playerBody.velocity.y, 0), ref velocityRef, 0.35f); 
            } else if (horizontalMotion == 0){
                playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, new Vector3(0, playerBody.velocity.y, playerBody.velocity.z), ref velocityRef, 0.35f); // reference ends here
            }
            playerBody.AddForce((playerSphere.forward * forwardMotion * MoveSpeed)
            + (playerSphere.right * horizontalMotion * MoveSpeed)
            + (downDirection * bounce * BounceSpeed));
        }
    }

    void camControl()
    {
        if (playerBody.velocity.magnitude >= 5.0f){
            TPCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(TPCam.GetComponent<Camera>().fieldOfView, 60 + playerBody.velocity.magnitude * 1.15f, 0.2f);
        } else {
            TPCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(TPCam.GetComponent<Camera>().fieldOfView, 60, 0.2f);
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
}
