Shader "BadDog/BGStandard"
{
    Properties
    {
        _MainTex ("MainTex, Albedo (RGB), Emission (A)", 2D) = "white" {}

        _MixTex("MixTex, Metallic (R), IsSkin (G), Skin Curvature (B), Smoothness (A)", 2D) = "white" {}
		_SmoothnessScale("Smoothness Scale", Range(0.0, 2.0)) = 1.0
        _MetallicScale("Metallic Scale", Range(0.0, 2.0)) = 1.0

        _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Bump Scale", Float) = 1.0

		[Header(Emission)]
		_EmissionStrength("Emission Strength", Range(0, 20)) = 1
		_EmissionColor ("Emission Color", Color) = (0,0,0,0)

		[Header(Skin)]
        _SSSLut("SSS Lut", 2D) = "white" {}
        _SkinNormalBias("Skin Normal Bias", Range(0, 8)) = 0
		_SkinBlurStrength("Skin Blur Strength", Range(0, 1)) = 0
		_SkinCurvatureOffset("Skin Curvature Offset", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        LOD 200

        CGPROGRAM

        #pragma surface surf BGStandard fullforwardshadows addshadow

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MixTex;
        sampler2D _BumpMap;
        sampler2D _SSSLut;

		fixed4 _EmissionColor;

        half _SmoothnessScale;
        half _MetallicScale;
		half _BumpScale;

		half _SkinNormalBias;
		half _SkinBlurStrength;
		half _SkinCurvatureOffset;

		half _EmissionStrength;

		#include "UnityCG.cginc"
		#include "./BGStandardBase.cginc"

        struct Input
        {
            float2 uv_MainTex;
			float3 worldNormal;
			float3 worldPos;
			INTERNAL_DATA
        };

        void surf (Input IN, inout SurfaceOutputBGStandard o)
        {
            fixed4 mainColor = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 mixColor = tex2D (_MixTex, IN.uv_MainTex);

            o.Albedo = mainColor.rgb;
            o.Metallic = mixColor.r * _MetallicScale;
            o.Smoothness = mixColor.a * _SmoothnessScale;
			o.Emission = _EmissionColor * (1-mainColor.a) * _EmissionStrength;
			o.Normal = UnpackScaleNormal(tex2D (_BumpMap, IN.uv_MainTex), _BumpScale);
			o.Occlusion = 1;
            o.Alpha = 1;
			o.IsSkin = step(0.5, mixColor.g);
			o.SkinCurvature = saturate(mixColor.b + _SkinCurvatureOffset);

			float3 skinNormal = UnpackNormal(tex2Dbias(_BumpMap, float4 (IN.uv_MainTex, 0, _SkinNormalBias)));
			skinNormal = normalize(lerp(o.Normal, skinNormal, _SkinBlurStrength));
			o.SkinNormal = WorldNormalVector(IN, skinNormal);
        }
        ENDCG
    }

    FallBack "Diffuse"
}
