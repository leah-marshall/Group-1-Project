Shader "Hidden/NewImageEffectShader1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Center ("Center", Vector) = (0.5,0.5,0,0)
        _RotateSpd ("Rotate Speed", Range(0.0, 100.0)) = 0.5 
        _Index ("Index", Range(0.0, 1000.0)) = 2
        _Radius ("Radius", Range(0.0,1000.0))= 6
        _Color ("Color", Color) = (1,1,1,1)
        _Lines ("Lines Texture", 2D) = "white"{}
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
                float4 uvHalf : TEXCOORD1; // Contains two sets of UV, XY will be regular uv, wz will be UV for rotation
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

				o.uvHalf.xy = v.uv; //Regular
				
				float aspect = _ScreenParams.x / _ScreenParams.y;
				v.uv.x*= aspect;
				
				o.uvHalf.wz = mul(float2x2(0.707, -0.707, 0.707, 0.707) , v.uv); //Rotate 
 
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

            fixed4 frag(v2f i) : SV_Target
            {
                //Speed Line

                float2 UV1 =  i.uv - _Center.xy; // For distance 
                float2 UV = normalize(i.uv - _Center.xy); // For rotation

                float2 offset = 0.01 * float2(cos(_Time.y), sin(_Time.y)); // circular offset

               
                // Split channels to allow offset in r and b channel
                fixed4 colr = tex2D(_MainTex, i.uv + offset); 
                fixed4 colb = tex2D(_MainTex, i.uv + offset);
                fixed4 colg = tex2D(_MainTex, i.uv);

                //fixed4 maintex = (colr,colg,colb,1);
                fixed4 maintex = tex2D(_MainTex, i.uv); // Screen

                

                half angle = radians(_RotateSpd * _Time.y*2);

                half sinAngle, cosAngle;// Components for rotation matrix
                sinAngle = sin(angle);
                cosAngle = cos(angle);

                half2x2 rotateMatrix = half2x2(cosAngle, -sinAngle, sinAngle, cosAngle); //Rotation matrix
                half2 RotatedUV = normalize(mul(rotateMatrix, UV));

                half2x2 rotateMatrix1 = half2x2(cosAngle, sinAngle, -sinAngle, cosAngle);//Rotation matrix in another direction
                half2 RotatedUV1 = normalize(mul(rotateMatrix1, UV));

                half col = tex2D(_Lines, RotatedUV).r * tex2D(_Lines, RotatedUV1).r; // Blend two lines in different rotation direction

                half uvMask = pow(_Index * length(UV1), _Radius); //A mask which controls the display area of speed line
                col *= uvMask;

                fixed4 color = col *_Color + fixed4(colr.r, colg.g, colb.b, 1);
                 
                return color; 
            }
            ENDCG
        }      

 
    }
}
  
 
