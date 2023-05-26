Shader "Unlit/NewUnlitShader"
{
    Properties
    {
 
        _MainTex ("Texture", 2D) = "white" {}

        _StrokeWeight("StrokeWeight", Range(0.0001, 1)) = 0.0001 
        _RampStart ("RampStart", Range(0.1, 1)) = 0.3
        _RampSize ("RampSize", Range(0, 1)) = 0.1
        [IntRange] _RampStep("RampStep", Range(1,10)) = 1
        _RampSmooth ("RampSmooth", Range(0.01, 1)) = 0.1
        _DarkColor ("DarkColor", Color) = (0.4, 0.4, 0.4, 1)
        _LightColor1 ("LightColor", Color) = (0.8, 0.8, 0.8, 1)
        
        _RimColor ("RimColor", Color) = (1.0, 1.0, 1.0, 1)
        _RimThreshold("RimThreshold", Range(0, 1)) = 0.45
        _RimSmooth("RimSmooth", Range(0, 0.5)) = 0.1
    }
    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            }
        LOD 100

        /* Stroke Pass from Zewuzi's MPIE project
        Author: Zewuzi Meng
        Locaiton: MPIE Assessment
        Access: 25/05/2023*/
 
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
    
            float4 _MainPos, _FollowPos;
            float4 _Color;
            fixed _StrokeWeight;

            v2f vert (appdata v)
            {
                v2f o;
                
            
                v.vertex.xyz += v.normal * _StrokeWeight  ;  
                 
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex.z -= 0.01 ;
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return 0.0;
            }
            ENDCG
        }

        /* Ramp based on half Lambert
           Author: Huc_Boy
           Location: https://blog.csdn.net/liu_xueping/article/details/127909153
           Access: 25/05/2023*/

        Pass
        {
            Tags{
                "LightMode" = "ForwardBase"
                "Queue"="Geometry"
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal: NORMAL;  
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal: TEXCOORD1;
                float3 viewDir : TEXCOORD4;
                float3 worldPos:TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _RampStart;
            float _RampSize;
            float _RampStep;
            float _RampSmooth;
            float3 _DarkColor;
            float3 _LightColor1;

            float _SpecPow;
            float3 _SpecularColor;
            float _SpecIntensity;
            float _SpecSmooth;

            float3 _RimColor;
            float _RimThreshold;
            float _RimSmooth;

            // For ramp calculations later. Simplified smoothstep() but linear.
            // This function will return a value between 0 to 1. This value indicates the current half Lamber's diffusion in the ramp.
            float linearstep (float min, float max, float t)
            {
                return saturate((t - min) / (max - min));
            }

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                // sample the texture
 
                float4 col = tex2D(_MainTex, i.uv);
                
                // Diffuse
                // Get normal direction
                float3 normal = normalize(i.worldNormal);
                // Get light direction
                float3 worldLightDir = normalize(_WorldSpaceLightPos0);
                // Lambert
                float NoL = dot(i.worldNormal, worldLightDir);
                // Half lambert
                float halfLambert = NoL * 0.5 + 0.5;

                // Get view direction
                float3 viewDir = normalize(i.viewDir);

                // Rim 
                // Angle between the view and normal
                float NoV = dot(i.worldNormal, viewDir);
                // Closer to edge, the brighter it is
                float rim = (1 - max(0, NoV)) * NoL;

                // _RimThreshold is the mid value, when the smoothstep result nears to 1, the rim color will show.
                float3 rimColor = smoothstep(_RimThreshold - _RimSmooth / 2, _RimThreshold + _RimSmooth / 2, rim) * _RimColor;

                // Color Ramp
                // Calculate color ramp based on halfLmabert's diffusion, bright or dark
                float ramp = linearstep(_RampStart, _RampStart + _RampSize, halfLambert);
                float step = ramp * _RampStep;  // RampStep = 1 initially, find the specific area in the ramp
                float gridStep = floor(step);   // Round to the integer
                float smoothStep = smoothstep(gridStep, gridStep + _RampSmooth, step) + gridStep; //Returns a value between 0 to 1.
                ramp = smoothStep / _RampStep;  // When ramp = 0, it means at the start point of the ramp, it should be dark. When ramp = 1, vice versa. 
                // Final color ramp
                float3 rampColor = lerp(_DarkColor, _LightColor1, ramp); // Customised colors in ramp can prevent a complete balck in dark area
                rampColor *= col;

                

                //fixed phong = pow(max(0, dot(i.worldNormal, )))
                
                // Final Color output
                float3 finalColor = saturate(rampColor + rimColor);


                return float4(finalColor,1);
            }
            ENDCG
        }

        /* Point Light Pass from Zewuzi's MPIE project
           Author: Zewuzi Meng
           Location: MPIE project
           Accessed: 25/05/2023
        */

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
                    float4 vertex : SV_POSITION;
                    float4 pass_color : COLOR;
                    float3 worldNormal : NORMAL;
                    fixed3 viewDir : TEXCOORD2;
                    float3 worldPos : TEXCOORD6;
                    LIGHTING_COORDS(4, 5)
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _RampStart;
                float _RampSize;
                float _RampStep;
                float _RampSmooth;
                float3 _DarkColor;
                float3 _LightColor1;

                float _SpecPow;
                float3 _SpecularColor;
                float _SpecIntensity;
                float _SpecSmooth;

                float3 _RimColor;
                float _RimThreshold;
                float _RimSmooth;

                v2f vert (appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);           
                    o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    return o;
                }

                fixed4 new_frag (v2f i) : SV_Target
                {
                    UNITY_LIGHT_ATTENUATION(attenuation, 0, i.worldPos); // Refers to the Unity-Built-in-Shaders/AutoLight.cginc
                    float3 lightDir;
                    if (_WorldSpaceLightPos0.w == 0) // directional light's direction
                    {
                        lightDir = normalize(_WorldSpaceLightPos0);
                    }
                    else // other light's direction
                    {
                        lightDir = normalize(_WorldSpaceLightPos0 - i.worldPos);
                    }

                    float4 col = tex2D(_MainTex, i.uv);
                      
                    float3 finalColor = col * _LightColor0 * attenuation;
                    return float4(finalColor, 1);
                }
            ENDCG
        }   
    }
    FallBack "VertexLit"
}
