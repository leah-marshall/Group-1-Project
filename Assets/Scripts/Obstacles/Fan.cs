using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] private float fanStrength;
    


    void OnTriggerStay(Collider other){
        if (other.name == "Player" || other.tag == "Box")
        {
            
            other.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * fanStrength);
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.name == "Player" || other.tag == "Box")
        {
            /*For line 33
            Set the velocity to zero based on user ScriptKid's comment
            Author: ScriptKid
            Location: https://forum.unity.com/threads/stopping-rigidbody-on-a-dime.263743/
            Accessed: 24/05/23

            Referenced how to use vector3
            Author: Unity
            Location: https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html
            Accessed: 24/05/23
            */
            Vector3 velocityRef = Vector3.zero;
            other.GetComponent<Rigidbody>().velocity = Vector3.SmoothDamp(other.GetComponent<Rigidbody>().velocity, 
                                                                        new Vector3(other.GetComponent<Rigidbody>().velocity.x, 0, other.GetComponent<Rigidbody>().velocity.z), ref velocityRef, 0.02f); 
            other.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * fanStrength * 1.5f);
        }
    }
}
