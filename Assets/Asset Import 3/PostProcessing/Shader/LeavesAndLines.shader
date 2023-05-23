Shader "Hidden/Leaves"
{
    Properties
    {
        [Header(Screen)]
        _MainTex ("Texture", 2D) = "white" {}

        [Header(Leaves)]
        _LeavesTex ("Leaves Texture", 2D) = "white" {}
        _SpdX ("Xspeed", Range(-2,2)) = 0.3
        _SpdY ("Yspeed", Range(-2,2)) = 0.1
        _Color ("Color Tone", Color) = (1,1,1,1)

        [Header(Lines)]
        _Center ("Center", Vector) = (0.5,0.5,0,0)
        _RotateSpd ("Rotate Speed", Range(0.0, 100.0)) = 0.5 
        _Index ("Index", Range(0.0, 2.0)) = 2
        _Radius ("Radius", Range(0.0,10.0))= 6
        _ColorL ("Lines Color", Color) = (1,1,1,1)
        _Lines ("Lines Texture", 2D) = "white"{}
 
    }
    SubShader
    {
        // No culling or depth
 
   
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
 
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _Lines;

            float4 _Center;
            float _RotateSpd;
            float _Radius;
            float _Index;
            fixed4 _ColorL;


            
            fixed4 frag(v2f i) : SV_Target
            {
                //Speed Line

                float2 UV1 =  i.uv - _Center.xy; // For distance 
                float2 UV = normalize(i.uv - _Center.xy); // For rotation

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

                fixed4 color = col * _ColorL + maintex;
                 
                return color; 
            }
            ENDCG
        }  

        Pass
        {
            Blend OneMinusDstColor One
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
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            fixed4 _Color;
            sampler2D _LeavesTex;
            float _SpdX;
            float _SpdY;

            fixed4 frag (v2f i) : SV_Target
            {
               
                fixed4 col2 = tex2D(_LeavesTex, half2(i.uv.x + _SpdX *_Time.y, i.uv.y + (_SpdY*_Time.y)));
                col2 *= _Color;
                clip(col2.a - 0.5f);

                return col2;
            }
            ENDCG
        }
    }
}
