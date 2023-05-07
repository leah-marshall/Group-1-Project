using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    // Script not currently used (redundant)
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("enter");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.SetParent(transform);
            Debug.Log("enter");
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Debug.Log("exit");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.SetParent(null);
            Debug.Log("exit");
        }
    }
}
