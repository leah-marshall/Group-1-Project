Shader "Unlit/Goal"
{
    Properties {
		_Color ("Color" , Color) = (1,1,1,1)
		_RimThreshold("Rim Threshold" , Range(0 , 5)) = 1 
		_MainTex("Main Tex" ,  2D) = "white" {}
        _ArrowTex("Goal Tex" ,  2D) = "white" {}
		_Speed("Speed" ,Range(0 , 2)) = 1.0
	}

	SubShader {
		Pass{
			Tags { "LightMode"="ForwardBase" }	
			Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "DisableBatching"="True"}
			
			Blend One One
			ZWrite Off
			Cull Off
			
			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			#define UNITY_PASS_FORWARDBASE
			#pragma multi_compile_fwdbase

			float4 _Color;
			float4 _HighlightColor;
			sampler2D _CameraDepthTexture;
			float _EdgePow;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _MaskTex;
			float _Speed;
			float _RimThreshold;
			float3 worldPos:POSITION1;

			struct a2v{
				float4 vertex:POSITION;
				float3 normal:NORMAL;
				float2 tex:TEXCOORD0;
				float2 uv : TEXCOORD5;
			};

			struct v2f{
				float4 pos:POSITION;
				float4 scrPos:TEXCOORD0;
				half3 worldNormal:TEXCOORD1;
				half3 worldViewDir:TEXCOORD2;
				float2 uv:TEXCOORD3;
				float3 worldPos:POSITION1;
			};

			v2f vert (a2v v )
			{
				v2f o;
				o.pos = UnityObjectToClipPos ( v.vertex );
				o.scrPos = ComputeScreenPos ( o.pos );
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; 
				o.worldNormal = UnityObjectToWorldNormal(v.normal); 
				o.uv = v.uv;
				return o;
			}
		
			fixed4 frag ( v2f i ) : SV_TARGET
			{	
				//Get view direction
				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);          
				fixed tex = tex2D(_MainTex , i.uv + float2(-(_Time.y)*_Speed , -(_Time.y)*_Speed)).r; //Noise texture move vertically
				fixed4 col2 = _Color * tex;
				fixed4 finalColor = fixed4(0,0,0,0);
				half rim = pow(1 - abs(dot(normalize(i.worldNormal),normalize(viewDir))), _RimThreshold); //Rim calculation, since both faces will be rendered, abs() is added.
				finalColor = lerp(finalColor, col2, rim); // The field color
				return finalColor;
			}

			ENDCG
			}

			//For Goal, rotate around
			Pass
			{ 
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
					
				float _Cutoff;
				float4 _Offset;
				sampler2D _ArrowTex;
				float4 _ArrowTex_ST;
				float _Speed;


				v2f vert (appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _ArrowTex);
					return o;
				}

				fixed4 frag (v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = tex2D(_ArrowTex, half2(i.uv.x-_Time.y*_Speed, i.uv.y)); // Move texture
					clip(col.a - 0.5); //Clip according to the texture's alpha channel
					
					return col;
				}
				ENDCG
			}
    }
}
