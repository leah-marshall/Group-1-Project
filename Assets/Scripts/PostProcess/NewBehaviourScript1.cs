using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class NewBehaviourScript1 : MonoBehaviour
{
    public Material PostProcess;

 
    // Start is called before the first frame update
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
     
        Graphics.Blit(source, destination, PostProcess);  

         
    }
}
