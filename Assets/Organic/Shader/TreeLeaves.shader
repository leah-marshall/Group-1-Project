Shader "Unlit/TreeLeaves"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff ("Cutoff", Range(0,1))= 0.5
        _Color ("Color", Color) = (0,0,0,1)
        _ColorTop ("ColorT", Color) = (0,0,0,1)
        _ColorBot ("ColorB", Color) = (0,0,0,1)
        _Height ("Height", Range(0,1)) = 0.5
        _RimColor ("RimColor", Color) = (1.0, 1.0, 1.0, 1)
        _RimThreshold("RimThreshold", Range(0, 1)) = 0.45
        _RimSmooth("RimSmooth", Range(0, 0.5)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull Off
            Tags {
                "RenderType"="Opaque" 
                "Queue"="Geometry"
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
                    UNITY_FOG_COORDS(1)
                    float4 pos : SV_POSITION;
                    float4 pass_color : COLOR;
                    float3 normal : NORMAL;
                    fixed3 viewDir : TEXCOORD2;
                    LIGHTING_COORDS(4, 5)
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _LightColor;
                fixed4 _Color;
                fixed3 _ColorTop;
                fixed3 _ColorBot;
                float _Height;

                float3 _RimColor;
                float _RimThreshold;
                float _RimSmooth;

                v2f vert (appdata v)
                {
                    v2f o;
            
                    o.pos = UnityObjectToClipPos(v.pos);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.pass_color = v.color;
 
                    //Convert normal from local to world space
                    o.normal = normalize(UnityObjectToWorldNormal(v.normal));

                    TRANSFER_VERTEX_TO_FRAGMENT(o);
                    return o;
                }

                fixed4 new_frag (v2f i) : SV_Target
                {
                    fixed3 normalDir = normalize(i.normal);                            
                    float3 lightDir = normalize(_WorldSpaceLightPos0);   
                    float lighting = dot(lightDir, normalDir); 
                    lighting = (lighting+1) * 0.5;
                    
                    fixed4 col = tex2D(_MainTex, i.uv);
                    clip(col.a - 0.5f); // clip by the alpha channel to produce desired shape

                    half3 Mask = i.normal.y;
                    Mask = Mask /2 + 0.5;
                    Mask *= _Height;
                    Mask = smoothstep(0,1,Mask);

                    float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.pos.xyz);

                    float NoV = dot(i.normal, viewDir);
                    float rim = (1 - max(0, NoV)) * lighting;
                     

                    float3 rimColor = smoothstep(_RimThreshold - _RimSmooth / 2, _RimThreshold + _RimSmooth / 2, rim) * _RimColor; // Controls rim size, sedge smoothness and colors
                     

                    fixed3 final = lerp(_ColorBot, _ColorTop, Mask) * lighting + rimColor;

                    return fixed4(final* lighting, 1);
                    //return lerp(lighting * _Color, 0, col.g); // returns the color with outlines of chillies in green channel
                }
            ENDCG
        }

        Pass // Similar light pass in "line1" shader
        {
            Tags {
                "LightMode"="ForwardAdd"
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
                    float4 tangent : TANGENT;
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
                fixed4 _Color;

                v2f vert (appdata v)
                {
                    v2f o;
            
                    o.pos = UnityObjectToClipPos(v.vertex);
                    //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.uv = v.uv;
                    o.pass_color = v.color;
                    fixed3 viewDir = WorldSpaceViewDir(v.vertex);
                    o.viewDir = viewDir;
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                    //convert normal from local to world space
                    o.normal = normalize(UnityObjectToWorldNormal(v.normal));
                    TRANSFER_VERTEX_TO_FRAGMENT(o);
                    return o;
                }

                fixed4 new_frag (v2f i) : SV_Target 
                {
                    UNITY_LIGHT_ATTENUATION(attenuation, 0, i.worldPos);
                    
                    fixed3 normalDir = normalize(i.normal);                  
                    float3 lightDir;             

                    if (_WorldSpaceLightPos0.w == 0) // directional lights
                    {
                        lightDir = normalize(_WorldSpaceLightPos0);
                    }
                    else // other lights
                    {
                        lightDir = normalize(_WorldSpaceLightPos0 - i.worldPos);
                    }

                
                    float lighting = dot(normalDir, lightDir);
                    lighting = smoothstep(0.4, 1.0, lighting);

                    fixed4 col = tex2D(_MainTex, i.uv);
                    clip(col.a - 0.5f);  // Clip again so the lighting effect will not apply to the whole plane

                    fixed4 baseCol = lerp(lighting * _Color, 0, col.g);  
                    fixed4 final = baseCol * _LightColor0 * attenuation;  
                    return final;
                }
            ENDCG
        }

        // The pass below is referred to Unity-Built-in-Shaders/DefaultResourcesExtra/AlphaTest-VertexLit.shader 
        // Author: TwoTailsGames
        // Link: https://github.com/TwoTailsGames/Unity-Built-in-Shaders/blob/master/DefaultResourcesExtra/AlphaTest-VertexLit.shader   
        // Accessed: 19/10/2022
        Pass { // Shadow caster to generate custom clipped object's shadows
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

            v2f vert( appdata_full v )
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            uniform sampler2D _MainTex;
            uniform fixed _Cutoff;
            uniform fixed4 _Color;

            float4 frag( v2f i ) : SV_Target
            {
                fixed4 texcol = tex2D( _MainTex, i.uv );
                clip( texcol.a*_Color.a - _Cutoff);    
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }        
    }     
}
