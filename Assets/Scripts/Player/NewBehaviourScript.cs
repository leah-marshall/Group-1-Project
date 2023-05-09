using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Material mat;
    private Vector3 followPos = Vector3.zero;
    private Vector3 massVelocity = Vector3.zero;
    public float stiffness = 60f;
    public float damping = 2f;

    public float movementSpeed = 15f;
    private float max, min;
    private bool isGrounded = true;
    private Rigidbody rb;
    bool grounded = true;
public float jumpForce = 10f;
    private void Start()
    {
        mat = GetComponent<MeshRenderer>().sharedMaterial;
        followPos = transform.position;

        max = GetComponent<MeshFilter>().sharedMesh.bounds.max.y;
        min = GetComponent<MeshFilter>().sharedMesh.bounds.min.y*5f;

        mat.SetFloat("_MeshH", max - min);
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Vector3 force = GetMainForce(); // elastic force
        force += GetResistanceForce(); // resistance force
        massVelocity += (force) * Time.deltaTime*2; // F = ma
        followPos += massVelocity * Time.deltaTime*2; 

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(hInput, 0f, vInput).normalized;

        transform.position += moveDirection * Time.deltaTime * movementSpeed;
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            if(grounded)
            {
                rb.velocity+= new Vector3(0,5,0);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
             
        }
        SetMatData();
        
 
    }
    private Vector3 GetMainForce()
    {
 
        Vector3 forceDir = transform.position - followPos;
        return forceDir * stiffness;
    }
    private Vector3 GetResistanceForce()
    {
        return -massVelocity * damping;
    }
    private void SetMatData()
    {
        mat.SetVector("_MainPos", transform.position);
        mat.SetVector("_FollowPos", followPos);
        mat.SetFloat("_W_Bottom", transform.position.y + min);
    }
    void OnCollisionEnter(Collision collision)
    {
        grounded = true;
    }
}

 
