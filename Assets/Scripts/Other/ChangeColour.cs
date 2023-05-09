using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{

    [SerializeField] private float rValue;
    [SerializeField] private float gValue;
    [SerializeField] private float bValue;
    int frames;

    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();

        rValue = sprite.color.r;
        gValue = sprite.color.g;
        bValue = sprite.color.b;

        sprite.color = new Color(rValue, bValue, gValue, 1);
        frames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        frames++;
        if(frames % 400 == 0)
        {
            float randomNumR = (Random.value - 0.5f) / 5;
            float randomNumG = (Random.value - 0.5f) / 5;
            float randomNumB = (Random.value - 0.5f) / 5;
            Debug.Log(rValue + " __ " + randomNumR);
            sprite.color = new Color(rValue + randomNumR, gValue + randomNumG, bValue + randomNumB, 1);
            Debug.Log(sprite.color.r);
        }
    }

}
