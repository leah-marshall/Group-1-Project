using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject[] points;
    [SerializeField] private float speed = 2f;
    int current_destination = 0;
    [SerializeField] private const float WAIT_TIME = 2f;
    private float waited_for = 0f;
    private bool is_waiting = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, points[current_destination].transform.position) < 0.5f)
        {
            if(!is_waiting)
            {
                waited_for = 0f;
                is_waiting = true;

                if (current_destination < points.Length - 1)
                {
                    current_destination++;
                }
                else
                {
                    current_destination = 0;
                }
            }
        
        }

        if(is_waiting)
        {
            waited_for += Time.deltaTime;
            if (waited_for >= WAIT_TIME)
            {
                is_waiting = false;
            }
        }

        if (!is_waiting)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[current_destination].transform.position, speed * Time.deltaTime);
        }
    }
}
