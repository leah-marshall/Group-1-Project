Shader "Hidden/NewImageEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Center ("Center", Vector) = (0.5,0.5,0,0)
        _RotateSpd ("Rotate Speed", Range(0.0, 100.0)) = 0.5 
        _Index ("Index", Range(0.0, 2.0)) = 2
        _Radius ("Radius", Range(0.0,10.0))= 6
        _Color ("Color", Color) = (1,1,1,1)
        _Lines ("Lines Texture", 2D) = "white"{}

        _Density ("Density", float) = 10
		_Radius1 ("Radius1", float) = 0.5
		_HalfToneFactor ("HalfToneFactor", Range(0,1) ) = 0.5
		_SourceFactor("SourceFactor", Range(0,1)) = 0.5
		_Lightness("Lightness", float) = 1
		_Color01("Color01",Color) = (1,1,1,1)
		_Color02("Color02",Color) = (0,0,0,1)
		_SmoothEdge("SmoothEdge", float) = 0.1
    }
    SubShader
    {
        // No culling or depth
        
        Tags { "RenderType" = "Opaque" }
       
        Cull Off ZWrite Off ZTest Always
 

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 uvHalf : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

				o.uvHalf.xy = v.uv;
				
				float aspect = _ScreenParams.x / _ScreenParams.y;
				v.uv.x*= aspect;
				
				o.uvHalf.wz = mul(float2x2(0.707, -0.707, 0.707, 0.707) , v.uv);        
 
                return o;
            }

            sampler2D _MainTex;
            sampler2D _Lines;
            float PixelizedScreenWidth;
            float PixelizedScreenHeight;
            float4 _Center;
            float _RotateSpd;
            float _Radius;
            float _Index;
            fixed4 _Color;
            
			float _Density;
			float _Radius1;
			float _SmoothEdge;
			float _HalfToneFactor;
			float _SourceFactor;
			float _Lightness;
			fixed4 _Color01;
			fixed4 _Color02;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 UV1 =  i.uv - _Center.xy; // For distance 
                float2 UV = normalize(i.uv - _Center.xy); // For rotation

                float2 offset = 0.01 * float2(cos(_Time.y), sin(_Time.y));

                fixed4 col1 = tex2D(_MainTex, i.uv);
                fixed colr = tex2D(_MainTex, i.uv + offset);
                fixed colb = tex2D(_MainTex, i.uv + offset);
                fixed colg = tex2D(_MainTex, i.uv);

                //fixed4 maintex = (colr,colg,colb,1);
                fixed4 maintex = tex2D(_MainTex, i.uv);

                half angle = radians(_RotateSpd * _Time.y*2);

                half sinAngle, cosAngle;
                sinAngle = sin(angle);
                cosAngle = cos(angle);

                half2x2 rotateMatrix = half2x2(cosAngle, -sinAngle, sinAngle, cosAngle);
                half2 RotatedUV = normalize(mul(rotateMatrix, UV));

                half2x2 rotateMatrix1 = half2x2(cosAngle, sinAngle, -sinAngle, cosAngle);
                half2 RotatedUV1 = normalize(mul(rotateMatrix1, UV));

                half col = tex2D(_Lines, RotatedUV).r * tex2D(_Lines, RotatedUV1).r;

                half uvMask = pow(_Index * length(UV1), _Radius);
                col *= uvMask;

                
				fixed4 texColor = tex2D(_MainTex,i.uvHalf.xy);
			
				float lightness = (texColor) * _Lightness;
				
				float radius = 1 - lightness + _Radius;
				
				fixed2 vectorCenter = 2 * frac(_Density * i.uvHalf.zw) - 1;
				
				float distance = length(vectorCenter);
				fixed4 halftoneColor = lerp(_Color01, _Color02, smoothstep(radius, radius +_SmoothEdge, distance));
				
				fixed4 color = texColor * _SourceFactor + texColor * halftoneColor * _HalfToneFactor;

                //return fixed4( col2,  col2,col2 ,1);
                return fixed4(colr,colg,colb,1)* maintex + col + color;
            }
            ENDCG
        }

        // Pass
        // {
        //     CGPROGRAM
        //     #pragma vertex vert
        //     #pragma fragment frag

        //     #include "UnityCG.cginc"

        //     struct appdata
        //     {
        //         float4 vertex : POSITION;
        //         float2 uv : TEXCOORD0;
        //     };

        //     struct v2f
        //     {
        //         float2 uv : TEXCOORD0;
        //         float4 vertex : SV_POSITION;
        //     };

        //     v2f vert (appdata v)
        //     {
        //         v2f o;
        //         o.vertex = UnityObjectToClipPos(v.vertex);
        //         o.uv = v.uv;
        //         return o;
        //     }

        //     sampler2D _MainTex;

        //     fixed4 frag (v2f i) : SV_Target
        //     {
        //         float2 offset = 0.005 * float2(cos(_Time.y), sin(_Time.y));
        //         fixed4 col = tex2D(_MainTex, i.uv);
        //         fixed colr = tex2D(_MainTex, i.uv + offset);
        //         fixed colb = tex2D(_MainTex, i.uv + offset);
        //         fixed colg = tex2D(_MainTex, i.uv);
                
        //         return fixed4(colr,colg,colb,1);
        //     }
        //     ENDCG
        // }
        
    }
}
  
 
