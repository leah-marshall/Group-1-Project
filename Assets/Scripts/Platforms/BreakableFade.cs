using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableFade : MonoBehaviour
{
    public bool fading;
    [SerializeField] private Material glassMat;
    private Renderer shardMat;
    private float clipAmnt;
    // Start is called before the first frame update
    void Start()
    {
        clipAmnt = 0;
        shardMat = gameObject.GetComponent<Renderer>();
        shardMat.material = glassMat;
        fading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fading){
            clipAmnt = Mathf.Lerp(clipAmnt, 1f, 1f * Time.deltaTime);
            shardMat.material.SetFloat("_Clip", clipAmnt);
        }

        if (clipAmnt >= 0.9f){
            Destroy(this.gameObject);
        }
        Debug.Log(clipAmnt);
    }
}
