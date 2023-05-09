using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning_Hammer : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(spinSpeed * Time.deltaTime, 0f, 0f, Space.Self);
    }
}
