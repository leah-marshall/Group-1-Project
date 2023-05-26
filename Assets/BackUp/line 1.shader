// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'


// Shader adpated from Zewuzi's MPIE assessment
// Author: Zewuzi Meng
// Location: MPIE Assessment
// Accessed: 25/05/2023

Shader "Unlit/line"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _rampTex ("RampTexture", 2D) = "white" {}

        _RimColor ("RimColor", Color) = (1.0, 1.0, 1.0, 1)
        _RimThreshold("RimThreshold", Range(0, 1)) = 0.45
        _RimSmooth("RimSmooth", Range(0, 0.5)) = 0.1

        _StrokeWeight("strokeWeight", Range(0,0.09)) = 0.09
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" 
               "Queue"="Geometry"
             }
        LOD 100

        Pass  // stroke
        {
            Cull Front // Cull the faces toward the camera
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Noise;
            fixed _offsetIndex;
            float _StrokeWeight;

            v2f vert (appdata v)
            {
                v2f o;
                
                // fix the gap at the edge of UVs
                fixed xDist = min(v.uv.x, 1 - v.uv.x); 
                fixed yDist = min(v.uv.y, 1 - v.uv.y); 
                fixed4 weight = min(xDist, yDist);
                weight = smoothstep(0.2,0.9, weight);

                fixed offset = tex2Dlod(_Noise, fixed4(v.uv, 0, 0)); // Affect the position of vertices by noise texture later
                v.vertex.xyz += v.normal * ( _StrokeWeight + offset * _offsetIndex * weight); // Expand vertices in the direction of normals
                 
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex.z -= 0.01f;
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return 0.03;
            }
            ENDCG
        }

        Pass
        {
            Tags {
                "LightMode"="ForwardBase"
            }
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment new_frag
                #pragma multi_compile_fwdbase

                #include "UnityCG.cginc"
                #include "UnityLightingCommon.cginc"
                #include "AutoLight.cginc"

                struct appdata
                {
                    float4 pos : POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal : NORMAL;
                    float4 color : COLOR;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 pos : SV_POSITION;
                    float4 pass_color : COLOR;
                                     
                    float3 normal : NORMAL;
                    fixed3 viewDir : TEXCOORD2;
                    LIGHTING_COORDS(4, 5)
                };

                sampler2D _MainTex;
                sampler2D _rampTex;
                float4 _MainTex_ST;

                float3 _RimColor;
                float _RimThreshold;
                float _RimSmooth;

                v2f vert (appdata v)
                {
                    v2f o;
            
                    o.pos = UnityObjectToClipPos(v.pos);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex); 
                    o.viewDir = WorldSpaceViewDir(v.pos);
                    //convert normal from local to world space
                    o.normal = normalize(UnityObjectToWorldNormal(v.normal)); 
                    
                    TRANSFER_VERTEX_TO_FRAGMENT(o);
                    return o;
                }

                fixed4 new_frag (v2f i) : SV_Target
                {
                    fixed3 normalDir = normalize(i.normal);                     
                    float3 lightDir = normalize(_WorldSpaceLightPos0); // Directional Light's direction
                    float3 viewDir = normalize(i.viewDir);

                    float NoV = dot(normalDir, viewDir); // Find the rim
                    // Closer to edge, the brighter it is
 

                    float lighting = dot(lightDir, normalDir); 
                    //If the dot product equals to 1 means that this part is pointing to the lightsource and it should be the brightest part
                    //If the dot product equals to -1 means that this part is not facing the lightsource and it should be the darkest part
                    
                    // Constrain the value to 0 - 1 
                    lighting = saturate(lighting);  
                    float rim = (1 - max(0, NoV)) *lighting;

                    float3 rimColor = smoothstep(_RimThreshold - _RimSmooth / 2, _RimThreshold + _RimSmooth / 2, rim) * _RimColor;
                    //lighting = smoothstep(0.2, 0.7, lighting);

                    fixed4 col = tex2D(_MainTex, i.uv); // sample colored texture
                    
                    fixed rampTex = tex2D(_rampTex, fixed2(lighting,0)); // Sample ramp texture black to white
                    // Saturate(lighting) is the value of x-axis, the value of y-axis can be fixed since it will not affect the output of ramp texture sampling
                    fixed4 final = col * rampTex + fixed4(rimColor,1); // Get the final result
                    return final;
                }
            ENDCG
        }

        Pass
        {
            Tags {
                "LightMode"="ForwardAdd" // Additive per-pixel light
            }

            Blend One One // Additive. When there are more light sources, the surface gets more brighter

            CGPROGRAM
                #pragma vertex vert
                #pragma fragment new_frag
                #pragma multi_compile_fwdadd_fullshadows

                //skip variants that are not going to be used
                #pragma skip_variants DIRECTIONAL DIRECTIONAL_COOKIE POINT_COOKIE 

                #include "UnityCG.cginc"
                #include "UnityLightingCommon.cginc"
                #include "AutoLight.cginc"
               

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal : NORMAL;
                    float4 color : COLOR;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 pos : SV_POSITION;
                    float4 pass_color : COLOR;
                    float3 normal : NORMAL;
                    fixed3 viewDir : TEXCOORD2;
                    float3 worldPos : TEXCOORD6;
                    LIGHTING_COORDS(4, 5)
                };

                sampler2D _MainTex;
                sampler2D _Normal;
                sampler2D _Noise;
                sampler2D _rampTex;
                float4 _MainTex_ST;
                float4 _LightColor;

                v2f vert (appdata v)
                {
                    v2f o;
            
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                    //convert normal from local to world space
                    o.normal = normalize(UnityObjectToWorldNormal(v.normal)); 

                    TRANSFER_VERTEX_TO_FRAGMENT(o);
                    return o;
                }

                fixed4 new_frag (v2f i) : SV_Target
                {
                    UNITY_LIGHT_ATTENUATION(attenuation, 0, i.worldPos); // Refers to the Unity-Built-in-Shaders/AutoLight.cginc
                    
                    fixed3 normalDir = normalize(i.normal);                  
                    float3 lightDir;             

                    if (_WorldSpaceLightPos0.w == 0) // directional light's direction
                    {
                        lightDir = normalize(_WorldSpaceLightPos0);
                    }
                    else // other light's direction
                    {
                        lightDir = normalize(_WorldSpaceLightPos0 - i.worldPos);
                    }

                    // Same process in last pass
                    float lighting = dot(normalDir, lightDir);
                    lighting = saturate(lighting);
                    //lighting = smoothstep(0.2, 0.7, lighting);

                    fixed4 col = tex2D(_MainTex, i.uv); 

                    fixed rampTex = tex2D(_rampTex, fixed2(saturate(lighting), 0)); 
                    rampTex = step(0.2, rampTex) * rampTex;
                    
                    fixed4 final = col  * rampTex * _LightColor0  * attenuation;
                    //Times _LightColor allows the meshes to be affected by different color lights
                    return final;
                }
            ENDCG
        }             
    }
    Fallback "VertexLit"
}
