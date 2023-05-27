using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolinePlus : MonoBehaviour
{
    private Rigidbody playerBody;
    [SerializeField] private float rebound;
    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.Find("Camera").transform.parent.parent.GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision other){
        if (other.collider.tag == "Player"){
            playerBody.AddForce(gameObject.transform.up * rebound * Mathf.Sign(playerBody.velocity.y), ForceMode.Impulse);
        }
    } 
}
