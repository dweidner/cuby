﻿Shader "Custom/yar" {
	Properties {
      _ColorHigh ("Color High", COLOR) = (1,1,1,1)
      _ColorLow ("Color Low", COLOR) = (1,1,1,1)
      _ColorFar ("Color Far", COLOR) = (1,1,1,1)
//      _GlobalIllumination ("Global Light Strength ", Float) = .5
      _yPosLow ("Y Pos Low", Float) = 0
      _yPosHigh ("Y Pos High", Float) = 10
      _zPosNear ("Z Pos Near", Float) = 0
      _zPosFar ("Z Pos Far", Float) = 10
      _GradientStrength ("Graident Strength", Float) = 1
      _EmissiveStrengh ("Emissive Strengh ", Float) = 1
//      _ColorY ("Top", COLOR) = (1,1,1,1)
//      _ColorZ ("Left", COLOR) = (1,1,1,1)
//      _ColorX ("Right", COLOR) = (1,1,1,1)
	}
	
	SubShader {
		Tags { 
      "Queue" = "Geometry"
      "RenderType"="Opaque" 
      }
		
		CGPROGRAM
		#pragma surface surf Lambert
      #define WHITE3 fixed3(1,1,1)
      #define UP float3(0,1,0)
      #define RIGHT float3(1,0,0)
      #define LEFT float3(0,0,-1)

		fixed4 _BodyColor;
      fixed4 _ColorLow;
      fixed4 _ColorHigh;
      fixed4 _ColorFar;
      fixed4 _ColorX;
      fixed4 _ColorY;
      fixed4 _ColorZ;
      half _yPosLow;
      half _yPosHigh;
      half _zPosNear;
      half _zPosFar;
      half _GradientStrength;
      half _EmissiveStrengh;
      half _GlobalIllumination;
      
		struct Input {
			float2 uv_MainTex;
         float3 worldPos;
         float3 normal;
		};

		void surf (Input IN, inout SurfaceOutput o) {
         // gradient color at this height
         half3 gradient = lerp(_ColorHigh, _ColorFar,  smoothstep( _zPosNear, _zPosFar, IN.worldPos.z )).rgb;
         gradient = lerp(gradient, _ColorLow,  smoothstep( _yPosHigh, _yPosLow, IN.worldPos.y )).rgb;
      
         
//         half3 gradient = lerp(_ColorLow, _ColorHigh,  smoothstep( _yPosLow, _yPosHigh, IN.worldPos.y )).rgb;
//         gradient = lerp(gradient, _ColorFar,  smoothstep( _zPosNear, _zPosFar, IN.worldPos.z )).rgb;
      
         // lerp the 
         //gradient = lerp(WHITE3, gradient, _GradientStrength);
         // add ColorX if the normal is facing positive X-ish
         half3 finalColor = _ColorX.rgb * max(0,dot(o.Normal, RIGHT))* _ColorX.a;
//         half3 finalColor = _ColorX.rgb * dot(o.Normal, RIGHT) * _ColorX.a;
         
         // add ColorY if the normal is facing positive Y-ish (up)
         finalColor += _ColorY.rgb * max(0,dot(o.Normal, UP)) * _ColorY.a;
//         finalColor += _ColorY.rgb * dot(o.Normal, UP) * _ColorY.a;
         
         // add ColorY if the normal is facing positive Z-ish
         finalColor += _ColorZ.rgb * max(0,dot(o.Normal, LEFT)) * _ColorZ.a;
//         finalColor += _ColorZ.rgb * dot(o.Normal, LEFT) * _ColorZ.a;
         
         // add the gradient color
         //finalColor = (finalColor + (gradient));
//         finalColor = half4((finalColor.rgb*finalColor.a+gradient.rgb*gradient.a),finalColor.a+gradient.a);
         //finalColor *= _ColorHigh;
         
         finalColor = (gradient * _GlobalIllumination) + ((finalColor * gradient) * (1 - _GlobalIllumination));
//         finalColor = half4((finalColor.rgb*finalColor.a+_BodyColor.rgb*_BodyColor.a),finalColor.a+_BodyColor.a)
         
         // scale down to 0-1 values
         finalColor = saturate(finalColor);
         
         // how much should go to emissive
         o.Emission = lerp(half3(0,0,0), finalColor, _EmissiveStrengh);
         
         // the "color" before lighting is applied
         o.Albedo = finalColor * saturate(1 - _EmissiveStrengh);
         
         // opaque
			o.Alpha = 1;
		}
		ENDCG
	} 
   fallback "Vertex Lit"
}