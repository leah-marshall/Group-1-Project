using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PostProcessingScript : MonoBehaviour
{
    public Material PostProcess;
    public float pixelSize = 1;
    public float spd = 10;
 
    public Camera myCam;
    void OnEnable()
    {
        myCam.depthTextureMode |= DepthTextureMode.Depth; // Allow depth texture to be generated
    }

    // Start is called before the first frame update
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        PostProcess.SetFloat("_RotateSpd", spd); // speed
     
        Graphics.Blit(source, destination, PostProcess);  
    }
}
