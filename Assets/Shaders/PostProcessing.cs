using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PostProcessing : MonoBehaviour
{
    public Material PostProcess;
    public float pixelSize = 1;
     
 
    // Start is called before the first frame update
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
     
        Graphics.Blit(source, destination, PostProcess);  
      
    }
}
