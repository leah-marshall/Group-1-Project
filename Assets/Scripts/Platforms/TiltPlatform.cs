using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltPlatform : MonoBehaviour
{
    private Rigidbody playerBody;
    [SerializeField] private float rebound;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEenter(Collision other){
        if (other.collider.name == "Player"){
            playerBody.AddForce(gameObject.transform.up * rebound, ForceMode.Impulse);
            Debug.Log("doing it");
        }
    }
    void OnCollisionExit(Collision other){
        if (other.collider.name == "Player"){
            playerBody.AddForce(gameObject.transform.up * rebound, ForceMode.Impulse);
            Debug.Log("doing it");
        }
    }
}
