Shader "Unlit/NewUnlitShader"
{
    Properties
    {
 
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _offsetIndex ("offsetIndex", Range(0,0.1)) = 0 
        
        
        _RampStart ("RampStart", Range(0.1, 1)) = 0.3
        _RampSize ("RampSize", Range(0, 1)) = 0.1
        [IntRange] _RampStep("RampStep", Range(1,10)) = 1
        _RampSmooth ("RampSmooth", Range(0.01, 1)) = 0.1
        _DarkColor ("DarkColor", Color) = (0.4, 0.4, 0.4, 1)
        _LightColor ("LightColor", Color) = (0.8, 0.8, 0.8, 1)
        
        _RimColor ("RimColor", Color) = (1.0, 1.0, 1.0, 1)
        _RimThreshold("RimThreshold", Range(0, 1)) = 0.45
        _RimSmooth("RimSmooth", Range(0, 0.5)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" 
                }
        LOD 100

        Pass // Elastic
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
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"

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
                float3 normal : NORMAL;
                float3 worldPos: TEXCOORD1;
                float4 shadowPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainPos, _FollowPos;
            float _MeshH, _W_Bottom;
 

            v2f vert (appdata v)
            {
                v2f o;
                float3 mainPos = mul(unity_WorldToObject, _MainPos).xyz; 
                float3 follow = mul(unity_WorldToObject, _FollowPos).xyz; 
                float3 shadowPos = v.vertex.xyz - mainPos;
                float3 offDir = follow - mainPos; 
                float3 followVert = v.vertex.xyz + offDir; 
                float3 wPos = mul(unity_ObjectToWorld, v.vertex).xyz; 
                float mask = (wPos.y - _W_Bottom) / max(0.00001, _MeshH); 
                v.vertex.xyz = lerp(v.vertex.xyz, followVert, mask); 
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(UnityObjectToWorldNormal(v.normal));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = v.uv;
                o.shadowPos = float4(shadowPos, 1.0);
                return o;
            }



            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = fixed4(0,0,0,1);
                return c;
            }
            ENDCG
        }
 
        Pass  // stroke
        {
            Cull Front // Cull the faces toward the camera
                Tags{
                "LightMode" = "ForwardBase"
                "Queue"="Geometry"

            }
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
            
            float4 _MainPos, _FollowPos;
            float _MeshH, _W_Bottom;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                
                // fix the gap at the edge of UVs
                fixed xDist = min(v.uv.x, 1 - v.uv.x); 
                fixed yDist = min(v.uv.y, 1 - v.uv.y); 
                fixed4 weight = min(xDist, yDist);

                weight = smoothstep(0.2, 1.0, weight);

                float3 mainPos = mul(unity_WorldToObject, _MainPos).xyz; 
                float3 follow = mul(unity_WorldToObject, _FollowPos).xyz; 
                float3 shadowPos = v.vertex.xyz - mainPos;
                float3 offDir = follow - mainPos; 
                float3 followVert = v.vertex.xyz + offDir; 
                float3 wPos = mul(unity_ObjectToWorld, v.vertex).xyz; 
                float mask = (wPos.y - _W_Bottom) / max(0.00001, _MeshH); 
                v.vertex.xyz = lerp(v.vertex.xyz, followVert, mask); 
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                v.vertex.xyz += v.normal * (0.001); // Expand vertices in the direction of normals
                 
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

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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
                float3 worldPos:TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _RampStart;
            float _RampSize;
            float _RampStep;
            float _RampSmooth;
            float3 _DarkColor;
            float3 _LightColor;

            float _SpecPow;
            float3 _SpecularColor;
            float _SpecIntensity;
            float _SpecSmooth;

            float3 _RimColor;
            float _RimThreshold;
            float _RimSmooth;

                        float4 _MainPos, _FollowPos;
            float _MeshH, _W_Bottom;
            float4 _Color;

            float linearstep (float min, float max, float t)
            {
                return saturate((t - min) / (max - min));
            }

            v2f vert (appdata v)
            {
                v2f o;
                                float3 mainPos = mul(unity_WorldToObject, _MainPos).xyz; 
                float3 follow = mul(unity_WorldToObject, _FollowPos).xyz; 
                float3 shadowPos = v.vertex.xyz - mainPos;
                float3 offDir = follow - mainPos; 
                float3 followVert = v.vertex.xyz + offDir; 
                float3 wPos = mul(unity_ObjectToWorld, v.vertex).xyz; 
                float mask = (wPos.y - _W_Bottom) / max(0.00001, _MeshH); 
                v.vertex.xyz = lerp(v.vertex.xyz, followVert, mask); 
                
             
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                

                o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                // sample the texture
 
                float4 col = tex2D(_MainTex, i.uv);
                
                //------------------------ 漫反射 ------------------------
                // 得到顶点法线
                float3 normal = normalize(i.worldNormal);
                // 得到光照方向
                float3 worldLightDir = UnityWorldSpaceLightDir(i.worldPos);
                // NoL代表表面接受的能量大小
                float NoL = dot(i.worldNormal, worldLightDir);
                // 计算half-lambert亮度值
                float halfLambert = NoL * 0.5 + 0.5;

                // 得到视向量
                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);

                //------------------------ 边缘光 ------------------------
                // 计算NoV用于计算边缘光
                float NoV = dot(i.worldNormal, viewDir);
                // 计算边缘光亮度值
                float rim = (1 - max(0, NoV)) * NoL;
                // 计算边缘光颜色
                float3 rimColor = smoothstep(_RimThreshold - _RimSmooth / 2, _RimThreshold + _RimSmooth / 2, rim) * _RimColor;

                //------------------------ 色阶 ------------------------
                // 通过亮度值计算线性ramp
                float ramp = linearstep(_RampStart, _RampStart + _RampSize, halfLambert);
                float step = ramp * _RampStep;  // 使每个色阶大小为1, 方便计算
                float gridStep = floor(step);   // 得到当前所处的色阶
                float smoothStep = smoothstep(gridStep, gridStep + _RampSmooth, step) + gridStep;
                ramp = smoothStep / _RampStep;  // 回到原来的空间
                // 得到最终的ramp色彩
                float3 rampColor = lerp(_DarkColor, _LightColor, ramp);
                rampColor *= col;

                 


                
                // 混合颜色
                float3 finalColor = saturate(rampColor + rimColor);


                return float4(finalColor,1);
            }
            ENDCG
        }
        









































        // The pass below is referred to Unity-Built-in-Shaders/DefaultResourcesExtra/AlphaTest-VertexLit.shader
        // Author: TwoTailsGame
        // Link: https://github.com/TwoTailsGames/Unity-Built-in-Shaders/blob/master/DefaultResourcesExtra/AlphaTest-VertexLit.shader
        // Accessed 19/10/2022
        Pass 
        { // Shadow caster to generate custom clipped object's shadows
            Name "Caster"
            Tags { "LightMode" = "ShadowCaster" }
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_instancing // allow instanced shadow pass for most of the shaders
            #include "UnityCG.cginc"

            struct v2f {
                V2F_SHADOW_CASTER;
                float2  uv : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO  
            };

            uniform float4 _MainTex_ST;
            float4 _MainPos, _FollowPos;
            float _MeshH, _W_Bottom;
            float4 _Color;

            v2f vert( appdata_full v )
            {
 
                v2f o;
                float3 mainPos = mul(unity_WorldToObject, _MainPos).xyz; 
                float3 follow = mul(unity_WorldToObject, _FollowPos).xyz; 
                float3 shadowPos = v.vertex.xyz - mainPos;
                float3 offDir = follow - mainPos; 
                float3 followVert = v.vertex.xyz + offDir; 
                float3 wPos = mul(unity_ObjectToWorld, v.vertex).xyz; 
                float mask = (wPos.y - _W_Bottom) / max(0.00001, _MeshH); 
           
                v.vertex.xyz = lerp(v.vertex.xyz, followVert, mask); 
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            uniform sampler2D _MainTex;
            uniform fixed _Cutoff;
   

            float4 frag( v2f i ) : SV_Target
            {
                
                fixed4 texcol = tex2D( _MainTex, i.uv );
                clip( texcol.a*_Color.a - _Cutoff);    
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
