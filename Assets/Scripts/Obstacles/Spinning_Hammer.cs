using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning_Hammer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(200f * Time.deltaTime, 0f, 0f, Space.Self);
    }
}
