using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Rigidbody rb1;
    [SerializeField] Rigidbody rb2;
    [SerializeField] Rigidbody rb3;
    [SerializeField] Rigidbody rb4;
    [SerializeField] Rigidbody rb5;
 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        rb1.isKinematic = false;
        rb2.isKinematic = false;
        rb3.isKinematic = false;
        rb4.isKinematic = false;
        rb5.isKinematic = false;
    }
}
