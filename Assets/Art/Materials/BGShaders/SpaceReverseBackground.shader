// Made with Amplify Shader Editor v1.9.3.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Background/SpaceReverseBackground"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		_MainTex("MainTex", 2D) = "white" {}
		_PlanetColor("PlanetColor", Color) = (0,0.5799811,1,1)
		_PlanetMaskOffset("PlanetMaskOffset", Vector) = (0,-1.75,0,0)
		_PlanetMaskTiling("PlanetMaskTiling", Vector) = (0.9,2.03,0,0)
		_PlanetRotateSpeed("PlanetRotateSpeed", Range( 0 , 0.1)) = 0.06065502
		_PlanetNoiseScale("PlanetNoiseScale", Float) = 30.39
		_PlanetNoiseIntensity("PlanetNoiseIntensity", Range( 0 , 3)) = 1.359349
		_PlanetDustLength("PlanetDustLength", Float) = -8.69
		_FogColor("FogColor", Color) = (0,0.4678721,1,0)
		_FogSpeed("FogSpeed", Range( 0.001 , 0.1)) = 0.02
		_FogScale("FogScale", Range( 20 , 90)) = 90
		_FogLength("FogLength", Range( 0.1 , 1)) = 0.2
		_FogIntensity("FogIntensity", Range( 0 , 3)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		[HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

		Cull Off
		HLSLINCLUDE
		#pragma target 2.0
		#pragma prefer_hlslcc gles
		// ensure rendering platforms toggle list is visible

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"
		ENDHLSL

		
		Pass
		{
			Name "Sprite Unlit"
			Tags { "LightMode"="Universal2D" }

			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM

			#define ASE_SRP_VERSION 140008


			#pragma vertex vert
			#pragma fragment frag

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS SHADERPASS_SPRITEUNLIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"

			

			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _PlanetColor;
			float4 _FogColor;
			float2 _PlanetMaskTiling;
			float2 _PlanetMaskOffset;
			float _PlanetDustLength;
			float _PlanetRotateSpeed;
			float _PlanetNoiseScale;
			float _PlanetNoiseIntensity;
			float _FogLength;
			float _FogSpeed;
			float _FogScale;
			float _FogIntensity;
			CBUFFER_END


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float3 positionWS : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if ETC1_EXTERNAL_ALPHA
				TEXTURE2D( _AlphaTex ); SAMPLER( sampler_AlphaTex );
				float _EnableAlphaTexture;
			#endif

			float4 _RendererColor;

			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			

			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.positionOS.xyz );

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.positionCS = vertexInput.positionCS;
				o.positionWS = vertexInput.positionWS;

				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texCoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 texCoord25 = IN.texCoord0.xy * _PlanetMaskTiling + _PlanetMaskOffset;
				float2 texCoord12 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult46 = (float2(_PlanetMaskOffset.x , ( _PlanetMaskOffset.y * -1.0 )));
				float cos18 = cos( 3.14 );
				float sin18 = sin( 3.14 );
				float2 rotator18 = mul( texCoord12 - appendResult46 , float2x2( cos18 , -sin18 , sin18 , cos18 )) + appendResult46;
				float2 temp_output_34_0_g12 = ( rotator18 - float2( 0.5,0.5 ) );
				float2 break39_g12 = temp_output_34_0_g12;
				float2 appendResult50_g12 = (float2(( 1.0 * ( length( temp_output_34_0_g12 ) * 2.0 ) ) , ( ( atan2( break39_g12.x , break39_g12.y ) * ( 1.0 / TWO_PI ) ) * _PlanetDustLength )));
				float mulTime21 = _TimeParameters.x * _PlanetRotateSpeed;
				float2 appendResult22 = (float2(0.0 , mulTime21));
				float simpleNoise9 = SimpleNoise( (appendResult50_g12*1.0 + appendResult22)*_PlanetNoiseScale );
				float4 lerpResult52 = lerp( tex2D( _MainTex, uv_MainTex ) , _PlanetColor , ( ( saturate( ( 1.0 - length( texCoord25 ) ) ) * saturate( (0.0 + (simpleNoise9 - 0.26) * (1.0 - 0.0) / (0.75 - 0.26)) ) ) * _PlanetNoiseIntensity ));
				float2 appendResult77 = (float2(_FogLength , 1.0));
				float mulTime63 = _TimeParameters.x * ( _FogSpeed * -1.0 );
				float2 appendResult82 = (float2(mulTime63 , 0.0));
				float2 texCoord67 = IN.texCoord0.xy * appendResult77 + appendResult82;
				float simpleNoise62 = SimpleNoise( texCoord67*_FogScale );
				float2 texCoord70 = IN.texCoord0.xy * float2( 1,3.33 ) + float2( 0,0 );
				float4 lerpResult78 = lerp( lerpResult52 , _FogColor , ( (0.0 + (simpleNoise62 - 0.18) * (1.0 - 0.0) / (1.0 - 0.18)) * saturate( ( 1.0 - ( texCoord70.y + 0.0 ) ) ) * _FogIntensity ));
				
				float4 Color = lerpResult78;

				#if ETC1_EXTERNAL_ALPHA
					float4 alpha = SAMPLE_TEXTURE2D( _AlphaTex, sampler_AlphaTex, IN.texCoord0.xy );
					Color.a = lerp( Color.a, alpha.r, _EnableAlphaTexture );
				#endif

				#if defined(DEBUG_DISPLAY)
				SurfaceData2D surfaceData;
				InitializeSurfaceData(Color.rgb, Color.a, surfaceData);
				InputData2D inputData;
				InitializeInputData(IN.positionWS.xy, half2(IN.texCoord0.xy), inputData);
				half4 debugColor = 0;

				SETUP_DEBUG_DATA_2D(inputData, IN.positionWS);

				if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
				{
					return debugColor;
				}
				#endif

				Color *= IN.color * _RendererColor;
				return Color;
			}

			ENDHLSL
		}
		
		Pass
		{
			
			Name "Sprite Unlit Forward"
            Tags { "LightMode"="UniversalForward" }

			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM

			#define ASE_SRP_VERSION 140008


			#pragma vertex vert
			#pragma fragment frag

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS SHADERPASS_SPRITEFORWARD

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"

			

			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _PlanetColor;
			float4 _FogColor;
			float2 _PlanetMaskTiling;
			float2 _PlanetMaskOffset;
			float _PlanetDustLength;
			float _PlanetRotateSpeed;
			float _PlanetNoiseScale;
			float _PlanetNoiseIntensity;
			float _FogLength;
			float _FogSpeed;
			float _FogScale;
			float _FogIntensity;
			CBUFFER_END


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float3 positionWS : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if ETC1_EXTERNAL_ALPHA
				TEXTURE2D( _AlphaTex ); SAMPLER( sampler_AlphaTex );
				float _EnableAlphaTexture;
			#endif

			float4 _RendererColor;

			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			

			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.positionOS.xyz );

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.positionCS = vertexInput.positionCS;
				o.positionWS = vertexInput.positionWS;

				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texCoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 texCoord25 = IN.texCoord0.xy * _PlanetMaskTiling + _PlanetMaskOffset;
				float2 texCoord12 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult46 = (float2(_PlanetMaskOffset.x , ( _PlanetMaskOffset.y * -1.0 )));
				float cos18 = cos( 3.14 );
				float sin18 = sin( 3.14 );
				float2 rotator18 = mul( texCoord12 - appendResult46 , float2x2( cos18 , -sin18 , sin18 , cos18 )) + appendResult46;
				float2 temp_output_34_0_g12 = ( rotator18 - float2( 0.5,0.5 ) );
				float2 break39_g12 = temp_output_34_0_g12;
				float2 appendResult50_g12 = (float2(( 1.0 * ( length( temp_output_34_0_g12 ) * 2.0 ) ) , ( ( atan2( break39_g12.x , break39_g12.y ) * ( 1.0 / TWO_PI ) ) * _PlanetDustLength )));
				float mulTime21 = _TimeParameters.x * _PlanetRotateSpeed;
				float2 appendResult22 = (float2(0.0 , mulTime21));
				float simpleNoise9 = SimpleNoise( (appendResult50_g12*1.0 + appendResult22)*_PlanetNoiseScale );
				float4 lerpResult52 = lerp( tex2D( _MainTex, uv_MainTex ) , _PlanetColor , ( ( saturate( ( 1.0 - length( texCoord25 ) ) ) * saturate( (0.0 + (simpleNoise9 - 0.26) * (1.0 - 0.0) / (0.75 - 0.26)) ) ) * _PlanetNoiseIntensity ));
				float2 appendResult77 = (float2(_FogLength , 1.0));
				float mulTime63 = _TimeParameters.x * ( _FogSpeed * -1.0 );
				float2 appendResult82 = (float2(mulTime63 , 0.0));
				float2 texCoord67 = IN.texCoord0.xy * appendResult77 + appendResult82;
				float simpleNoise62 = SimpleNoise( texCoord67*_FogScale );
				float2 texCoord70 = IN.texCoord0.xy * float2( 1,3.33 ) + float2( 0,0 );
				float4 lerpResult78 = lerp( lerpResult52 , _FogColor , ( (0.0 + (simpleNoise62 - 0.18) * (1.0 - 0.0) / (1.0 - 0.18)) * saturate( ( 1.0 - ( texCoord70.y + 0.0 ) ) ) * _FogIntensity ));
				
				float4 Color = lerpResult78;

				#if ETC1_EXTERNAL_ALPHA
					float4 alpha = SAMPLE_TEXTURE2D( _AlphaTex, sampler_AlphaTex, IN.texCoord0.xy );
					Color.a = lerp( Color.a, alpha.r, _EnableAlphaTexture );
				#endif


				#if defined(DEBUG_DISPLAY)
				SurfaceData2D surfaceData;
				InitializeSurfaceData(Color.rgb, Color.a, surfaceData);
				InputData2D inputData;
				InitializeInputData(IN.positionWS.xy, half2(IN.texCoord0.xy), inputData);
				half4 debugColor = 0;

				SETUP_DEBUG_DATA_2D(inputData, IN.positionWS);

				if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
				{
					return debugColor;
				}
				#endif

				Color *= IN.color * _RendererColor;
				return Color;
			}

			ENDHLSL
		}
		
        Pass
        {
			
            Name "SceneSelectionPass"
            Tags { "LightMode"="SceneSelectionPass" }

            Cull Off

            HLSLPROGRAM

			#define ASE_SRP_VERSION 140008


			#pragma vertex vert
			#pragma fragment frag

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS SHADERPASS_DEPTHONLY
			#define SCENESELECTIONPASS 1


            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			

			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _PlanetColor;
			float4 _FogColor;
			float2 _PlanetMaskTiling;
			float2 _PlanetMaskOffset;
			float _PlanetDustLength;
			float _PlanetRotateSpeed;
			float _PlanetNoiseScale;
			float _PlanetNoiseIntensity;
			float _FogLength;
			float _FogSpeed;
			float _FogScale;
			float _FogIntensity;
			CBUFFER_END


            struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};


            int _ObjectId;
            int _PassValue;

			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			

			VertexOutput vert(VertexInput v )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);
				float3 positionWS = TransformObjectToWorld(v.positionOS);
				o.positionCS = TransformWorldToHClip(positionWS);

				return o;
			}

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				float2 uv_MainTex = IN.ase_texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 texCoord25 = IN.ase_texcoord.xy * _PlanetMaskTiling + _PlanetMaskOffset;
				float2 texCoord12 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult46 = (float2(_PlanetMaskOffset.x , ( _PlanetMaskOffset.y * -1.0 )));
				float cos18 = cos( 3.14 );
				float sin18 = sin( 3.14 );
				float2 rotator18 = mul( texCoord12 - appendResult46 , float2x2( cos18 , -sin18 , sin18 , cos18 )) + appendResult46;
				float2 temp_output_34_0_g12 = ( rotator18 - float2( 0.5,0.5 ) );
				float2 break39_g12 = temp_output_34_0_g12;
				float2 appendResult50_g12 = (float2(( 1.0 * ( length( temp_output_34_0_g12 ) * 2.0 ) ) , ( ( atan2( break39_g12.x , break39_g12.y ) * ( 1.0 / TWO_PI ) ) * _PlanetDustLength )));
				float mulTime21 = _TimeParameters.x * _PlanetRotateSpeed;
				float2 appendResult22 = (float2(0.0 , mulTime21));
				float simpleNoise9 = SimpleNoise( (appendResult50_g12*1.0 + appendResult22)*_PlanetNoiseScale );
				float4 lerpResult52 = lerp( tex2D( _MainTex, uv_MainTex ) , _PlanetColor , ( ( saturate( ( 1.0 - length( texCoord25 ) ) ) * saturate( (0.0 + (simpleNoise9 - 0.26) * (1.0 - 0.0) / (0.75 - 0.26)) ) ) * _PlanetNoiseIntensity ));
				float2 appendResult77 = (float2(_FogLength , 1.0));
				float mulTime63 = _TimeParameters.x * ( _FogSpeed * -1.0 );
				float2 appendResult82 = (float2(mulTime63 , 0.0));
				float2 texCoord67 = IN.ase_texcoord.xy * appendResult77 + appendResult82;
				float simpleNoise62 = SimpleNoise( texCoord67*_FogScale );
				float2 texCoord70 = IN.ase_texcoord.xy * float2( 1,3.33 ) + float2( 0,0 );
				float4 lerpResult78 = lerp( lerpResult52 , _FogColor , ( (0.0 + (simpleNoise62 - 0.18) * (1.0 - 0.0) / (1.0 - 0.18)) * saturate( ( 1.0 - ( texCoord70.y + 0.0 ) ) ) * _FogIntensity ));
				
				float4 Color = lerpResult78;

				half4 outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				return outColor;
			}

            ENDHLSL
        }

		
        Pass
        {
			
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }

            Cull Off

            HLSLPROGRAM

			#define ASE_SRP_VERSION 140008


			#pragma vertex vert
			#pragma fragment frag

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS SHADERPASS_DEPTHONLY
			#define SCENEPICKINGPASS 1


            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        	

			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _PlanetColor;
			float4 _FogColor;
			float2 _PlanetMaskTiling;
			float2 _PlanetMaskOffset;
			float _PlanetDustLength;
			float _PlanetRotateSpeed;
			float _PlanetNoiseScale;
			float _PlanetNoiseIntensity;
			float _FogLength;
			float _FogSpeed;
			float _FogScale;
			float _FogIntensity;
			CBUFFER_END


            struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

            float4 _SelectionID;

			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			

			VertexOutput vert(VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);
				float3 positionWS = TransformObjectToWorld(v.positionOS);
				o.positionCS = TransformWorldToHClip(positionWS);

				return o;
			}

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				float2 uv_MainTex = IN.ase_texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 texCoord25 = IN.ase_texcoord.xy * _PlanetMaskTiling + _PlanetMaskOffset;
				float2 texCoord12 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult46 = (float2(_PlanetMaskOffset.x , ( _PlanetMaskOffset.y * -1.0 )));
				float cos18 = cos( 3.14 );
				float sin18 = sin( 3.14 );
				float2 rotator18 = mul( texCoord12 - appendResult46 , float2x2( cos18 , -sin18 , sin18 , cos18 )) + appendResult46;
				float2 temp_output_34_0_g12 = ( rotator18 - float2( 0.5,0.5 ) );
				float2 break39_g12 = temp_output_34_0_g12;
				float2 appendResult50_g12 = (float2(( 1.0 * ( length( temp_output_34_0_g12 ) * 2.0 ) ) , ( ( atan2( break39_g12.x , break39_g12.y ) * ( 1.0 / TWO_PI ) ) * _PlanetDustLength )));
				float mulTime21 = _TimeParameters.x * _PlanetRotateSpeed;
				float2 appendResult22 = (float2(0.0 , mulTime21));
				float simpleNoise9 = SimpleNoise( (appendResult50_g12*1.0 + appendResult22)*_PlanetNoiseScale );
				float4 lerpResult52 = lerp( tex2D( _MainTex, uv_MainTex ) , _PlanetColor , ( ( saturate( ( 1.0 - length( texCoord25 ) ) ) * saturate( (0.0 + (simpleNoise9 - 0.26) * (1.0 - 0.0) / (0.75 - 0.26)) ) ) * _PlanetNoiseIntensity ));
				float2 appendResult77 = (float2(_FogLength , 1.0));
				float mulTime63 = _TimeParameters.x * ( _FogSpeed * -1.0 );
				float2 appendResult82 = (float2(mulTime63 , 0.0));
				float2 texCoord67 = IN.ase_texcoord.xy * appendResult77 + appendResult82;
				float simpleNoise62 = SimpleNoise( texCoord67*_FogScale );
				float2 texCoord70 = IN.ase_texcoord.xy * float2( 1,3.33 ) + float2( 0,0 );
				float4 lerpResult78 = lerp( lerpResult52 , _FogColor , ( (0.0 + (simpleNoise62 - 0.18) * (1.0 - 0.0) / (1.0 - 0.18)) * saturate( ( 1.0 - ( texCoord70.y + 0.0 ) ) ) * _FogIntensity ));
				
				float4 Color = lerpResult78;
				half4 outColor = _SelectionID;
				return outColor;
			}

            ENDHLSL
        }
		
	}
	CustomEditor "ASEMaterialInspector"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=19303
Node;AmplifyShaderEditor.Vector2Node;16;-2080,240;Inherit;False;Property;_PlanetMaskOffset;PlanetMaskOffset;2;0;Create;True;0;0;0;False;0;False;0,-1.75;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-1912,480;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1760,688;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;46;-1656,480;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-1824,848;Inherit;False;Constant;_Float0;Float 0;9;0;Create;True;0;0;0;False;0;False;3.14;0;0;3.14;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1440,976;Inherit;False;Property;_PlanetRotateSpeed;PlanetRotateSpeed;4;0;Create;True;0;0;0;False;0;False;0.06065502;0;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;18;-1504,688;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;2.86;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;21;-1376,1072;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1760,944;Inherit;False;Property;_PlanetDustLength;PlanetDustLength;7;0;Create;True;0;0;0;False;0;False;-8.69;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;22;-1168,1040;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;17;-1280,688;Inherit;True;Polar Coordinates;-1;;12;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;-6.46;False;3;FLOAT2;0;FLOAT;55;FLOAT;56
Node;AmplifyShaderEditor.Vector2Node;31;-2096,96;Inherit;False;Property;_PlanetMaskTiling;PlanetMaskTiling;3;0;Create;True;0;0;0;False;0;False;0.9,2.03;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;64;352,1040;Inherit;False;Property;_FogSpeed;FogSpeed;9;0;Create;True;0;0;0;False;0;False;0.02;0;0.001;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;24;-960,688;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-1792,64;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;-0.36,-2.51;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-912,944;Inherit;False;Property;_PlanetNoiseScale;PlanetNoiseScale;5;0;Create;True;0;0;0;False;0;False;30.39;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;144,1136;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;28;-1568,64;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;9;-704,688;Inherit;True;Simple;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-48,1344;Inherit;False;Property;_FogLength;FogLength;11;0;Create;True;0;0;0;False;0;False;0.2;0;0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;63;304,1152;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;57;-448,688;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0.26;False;2;FLOAT;0.75;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;26;-1408,64;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;256,1680;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,3.33;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;77;240,1328;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;82;480,1152;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;58;-176,688;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;59;-1216,64;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;69;512,1600;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;368,1440;Inherit;False;Property;_FogScale;FogScale;10;0;Create;True;0;0;0;False;0;False;90;0;20;90;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;67;416,1312;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.5,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-16,256;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-80,976;Inherit;False;Property;_PlanetNoiseIntensity;PlanetNoiseIntensity;6;0;Create;True;0;0;0;False;0;False;1.359349;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;71;720,1600;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;62;672,1312;Inherit;True;Simple;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;224,496;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;50;752,544;Inherit;False;Property;_PlanetColor;PlanetColor;1;0;Create;True;0;0;0;False;0;False;0,0.5799811,1,1;1,0.4874805,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;896,176;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;-1;93bd6d0c7bbd3bf4cb5a47ad1ea6f7ec;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;72;896,1600;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;74;928,1312;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0.18;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;1088,1712;Inherit;False;Property;_FogIntensity;FogIntensity;12;0;Create;True;0;0;0;False;0;False;1;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;52;1216,528;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;65;1392,944;Inherit;False;Property;_FogColor;FogColor;8;0;Create;True;0;0;0;False;0;False;0,0.4678721,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;1408,1456;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;78;1664,960;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;0,0;Float;False;False;-1;2;ASEMaterialInspector;0;15;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit Forward;0;1;Sprite Unlit Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;True;2;5;False;;10;False;;2;5;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2;0,0;Float;False;False;-1;2;ASEMaterialInspector;0;15;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;SceneSelectionPass;0;2;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3;0,0;Float;False;False;-1;2;ASEMaterialInspector;0;15;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;ScenePickingPass;0;3;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;2000,960;Float;False;True;-1;2;ASEMaterialInspector;0;15;Background/SpaceReverseBackground;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit;0;0;Sprite Unlit;4;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;True;True;2;5;False;;10;False;;2;5;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;3;Vertex Position;1;0;Debug Display;0;0;External Alpha;0;0;0;4;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.CommentaryNode;39;-1040,-32;Inherit;False;111.7;100;mask;0;;1,1,1,1;0;0
WireConnection;44;0;16;2
WireConnection;46;0;16;1
WireConnection;46;1;44;0
WireConnection;18;0;12;0
WireConnection;18;1;46;0
WireConnection;18;2;47;0
WireConnection;21;0;19;0
WireConnection;22;1;21;0
WireConnection;17;1;18;0
WireConnection;17;4;61;0
WireConnection;24;0;17;0
WireConnection;24;2;22;0
WireConnection;25;0;31;0
WireConnection;25;1;16;0
WireConnection;80;0;64;0
WireConnection;28;0;25;0
WireConnection;9;0;24;0
WireConnection;9;1;10;0
WireConnection;63;0;80;0
WireConnection;57;0;9;0
WireConnection;26;0;28;0
WireConnection;77;0;76;0
WireConnection;82;0;63;0
WireConnection;58;0;57;0
WireConnection;59;0;26;0
WireConnection;69;0;70;2
WireConnection;67;0;77;0
WireConnection;67;1;82;0
WireConnection;49;0;59;0
WireConnection;49;1;58;0
WireConnection;71;0;69;0
WireConnection;62;0;67;0
WireConnection;62;1;68;0
WireConnection;33;0;49;0
WireConnection;33;1;34;0
WireConnection;72;0;71;0
WireConnection;74;0;62;0
WireConnection;52;0;6;0
WireConnection;52;1;50;0
WireConnection;52;2;33;0
WireConnection;73;0;74;0
WireConnection;73;1;72;0
WireConnection;73;2;79;0
WireConnection;78;0;52;0
WireConnection;78;1;65;0
WireConnection;78;2;73;0
WireConnection;0;1;78;0
ASEEND*/
//CHKSM=9B6C34D474C545DD646B53D337C0409427F31533