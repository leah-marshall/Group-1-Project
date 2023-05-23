Shader "Unlit/cloud"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color",Color) = (1,1,1,1)
        _NormalTex("Normal Map", 2D) = "white" {} 
        _AO ("ao" ,2D ) = "white"{}
        _Density ("density", Range(0,1)) = 0.05
 
	_Cutoff("Cut off",Range(0,1)) = 0.5 
    }
    SubShader
    {
    
        LOD 100
        Tags {"Queue"="Transparent" "IgnoreProjector"="True"   "DisableBatching"="True"}

        Pass
        {
        Tags
        {            
            "RenderType" = "TransparentCutout"        
        }
            Cull Off
            ZWrite Off
            Blend One OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal: NORMAL;  
                float4 tangent : TANGENT0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD3;
                float3 worldNormal: TEXCOORD1;
                float3 worldPos:TEXCOORD2;
                float3 worldViewDir : TEXCOORD7;

                //Tangent transpose matrix

                float3 tSpace0 : TEXCOORD4;
                float3 tSpace1 : TEXCOORD5;
                float3 tSpace2 : TEXCOORD6;
            };

            sampler2D _MainTex;
            sampler2D _NormalTex;
            sampler2D _AO;
            float4 _MainTex_ST;
            float _Density;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                //Tangent Transpose Matrix Calculation （For normal map shows bumpness correctly）

                //Tangent from object space to world space 
                half3 worldTangent = UnityObjectToWorldDir(v.tangent);
                fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w; // v.tangent.w indicates the direction which the tangent is facing

                half3 worldBinormal = cross(o.worldNormal, worldTangent) * tangentSign; // Cross product produces bitangent/binormal
                o.tSpace0 = float3(worldTangent.x,worldBinormal.x,o.worldNormal.x); // Tangent x
                o.tSpace1 = float3(worldTangent.y,worldBinormal.y,o.worldNormal.y); // Tangent y
                o.tSpace2 = float3(worldTangent.z,worldBinormal.z,o.worldNormal.z); // Tangent z
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 worldNormal = normalize(i.worldNormal);
                float3 worldViewDir = normalize(i.worldViewDir);
                // sample the texture
 
                fixed4 col = tex2D(_MainTex, half2(i.uv.x + 0.05* sin(_Time.y), i.uv.y));
                float4 ao = tex2D(_AO, i.uv);
                clip(col.a - 0.5f); // clip by the alpha channel to produce desired shape

     
                half3 normalTex = UnpackNormal(tex2D(_NormalTex,i.uv));

                // Based on normal map instead of vertex, so it is not the same as the one calculated in vertex shader
                half3 worldNormal1 = half3(dot(i.tSpace0,normalTex),dot(i.tSpace1,normalTex),dot(i.tSpace2,normalTex));
                worldNormal1 = normalize(worldNormal1);

                float NoL = max(0, dot(worldNormal1, _WorldSpaceLightPos0.xyz));
                fixed4 nCol = fixed4(NoL,NoL,NoL,1);
                fixed4 color = (nCol + col +ao) * _Density*_Color;

                return color ;             
            }
            ENDCG
        }
}
}