Shader "Unlit/Glass 1"
{
    Properties
    {
        _DistortionTex("Distortion Texture", 2D) = "white"{}
        _Color ("Color", Color) = (1,1,1,0.5)
        _DistortIndex("Distort Index", Range(0,1)) = 0
    }
    SubShader
    {
        Tags{"Queue" = "Transparent"}
        Cull Off

        GrabPass{"_GrabTex"}

        Pass
        {
            CGPROGRAM
 
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD;
                 
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos: SV_POSITION;
                float4 screenUV : TEXCOORD1;
            };

       
            sampler2D _GrabTex;
            sampler2D _DistortionTex;
            float4 _DistortionTex_ST;
            fixed4 _Color;
            fixed _DistortIndex;


            v2f vert (appdata v)
            {
                v2f o;
              
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _DistortionTex);
         
                o.screenUV = ComputeScreenPos(o.pos);
        
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                 
                fixed4 distortTex = tex2D(_DistortionTex, i.uv);
                float2 uv = lerp(i.screenUV.xy/i.screenUV.w, distortTex, _DistortIndex); 
                 
                fixed4 grabTex = tex2D(_GrabTex, uv);
                return grabTex*_Color; 
            }
            ENDCG
        }

 
    }
}
