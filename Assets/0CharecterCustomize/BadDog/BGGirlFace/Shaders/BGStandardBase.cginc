#ifndef BADDOG_STANDARD_BASE
#define BADDOG_STANDARD_BASE

	#include "UnityPBSLighting.cginc"
	
	struct SurfaceOutputBGStandard
	{
		half3 Albedo;      
		float3 Normal;      
		half3 Emission;
		half Metallic;    
		half Smoothness;  
		half Occlusion;  
		half Alpha;    
		half IsSkin;
		float3 SkinNormal;      
		half SkinCurvature;
	};

	half3 SkinDiffuseTerm(SurfaceOutputBGStandard s, UnityGI gi)
	{	
		float NoL = saturate(dot(s.SkinNormal, gi.light.dir));
		NoL = NoL * 0.5 + 0.5;
		half3 diffuseTerm = tex2D(_SSSLut, float2(NoL, s.SkinCurvature)); 
		return diffuseTerm;
	}

	half4 LightingBGStandard(SurfaceOutputBGStandard s, float3 viewDir, UnityGI gi)
	{
		s.Normal = normalize(s.Normal);

		half oneMinusReflectivity;
		half3 specColor;
		half3 diffColor = DiffuseAndSpecularFromMetallic(s.Albedo, s.Metallic, /*out*/ specColor, /*out*/ oneMinusReflectivity);

		diffColor = lerp(diffColor, s.Albedo, s.IsSkin);

		UnityLight light = gi.light;	

		half perceptualRoughness = SmoothnessToPerceptualRoughness(s.Smoothness);
		half roughness = PerceptualRoughnessToRoughness(perceptualRoughness);

		half3 H = Unity_SafeNormalize (float3(light.dir) + viewDir);
		half NoH = saturate(dot(s.Normal, H));
		half NoV = abs(dot(s.Normal, viewDir)); 
		half LoH = saturate(dot(light.dir, H));
		half NoL = saturate(dot(s.Normal, light.dir));

		half V = SmithJointGGXVisibilityTerm (NoL, NoV, roughness);
		half D = GGXTerm (NoH, roughness);
		
		half specularTerm = (V * D) * UNITY_PI;
		
		#ifdef UNITY_COLORSPACE_GAMMA
			specularTerm = sqrt(max(1e-4h, specularTerm));
		#endif

	    specularTerm = max(0, specularTerm * NoL);

	    half surfaceReduction;
		#ifdef UNITY_COLORSPACE_GAMMA
	        surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;      
		#else
	        surfaceReduction = 1.0 / (roughness*roughness + 1.0); 
		#endif

		half grazingTerm = saturate(s.Smoothness + (1-oneMinusReflectivity));

		half3 diffuseTerm = lerp(half3(NoL, NoL, NoL), SkinDiffuseTerm(s, gi), s.IsSkin);

		half3 color = diffColor * (gi.indirect.diffuse + light.color * diffuseTerm)
						+ specularTerm * light.color * specColor
						+ surfaceReduction * gi.indirect.specular * specColor;

		#if defined (UNITY_PASS_FORWARDBASE)
			color += s.Emission;
		#endif

		return half4(color, 1);
	}

	inline void LightingBGStandard_GI(SurfaceOutputBGStandard s, UnityGIInput data, inout UnityGI gi)
	{
#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
		gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
#else
		Unity_GlossyEnvironmentData g = UnityGlossyEnvironmentSetup(s.Smoothness, data.worldViewDir, s.Normal, lerp(unity_ColorSpaceDielectricSpec.rgb, s.Albedo, s.Metallic));
		gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal, g);
#endif
	}

#endif

