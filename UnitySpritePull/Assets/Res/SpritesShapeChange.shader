// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
//形变 //by thinbug.lpj 2022.12
Shader "InkPainter/SpriteShapeChange"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
		_DragBeginSkin("拖拽点位", vector) = (0,0,0,0)
		_DragOffsetSkin("拖拽偏移", vector) = (0,0,0,0)
		_Size("Size", float) = 0.5
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex MySpriteVert
				#pragma fragment SpriteFrag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_local _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
				#include "UnitySprites.cginc"

				uniform float2 _DragOffsetSkin;
		float _Size;
		float4 _DragBeginSkin;

		/*float4x4 Translation(float4 t)
		{
			return float4x4(
				1.0,0.0,0.0,t.x,
				0.0,1.0,0.0,t.y,
				0.0,0.0,1.0,t.z,
				0.0,0.0,0.0,1.0);
		}*/

		float lengthComput(in float2 p, in float2 offset)
		{
			return length(p - offset);
		}

		v2f MySpriteVert(appdata_t IN)
		{
			v2f OUT;

			UNITY_SETUP_INSTANCE_ID(IN);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

			OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
			//放在世界坐标前
			float lenx = lengthComput(OUT.vertex, _DragBeginSkin);
			if (lenx < _Size)
			{
				OUT.vertex.x = OUT.vertex.x + _DragOffsetSkin.x * (1 - lenx / _Size);
				OUT.vertex.y = OUT.vertex.y + _DragOffsetSkin.y * (1 - lenx / _Size);
			}
			OUT.vertex = UnityObjectToClipPos(OUT.vertex);
			
			OUT.texcoord = IN.texcoord;
			OUT.color = IN.color * _Color * _RendererColor;

			#ifdef PIXELSNAP_ON
			OUT.vertex = UnityPixelSnap(OUT.vertex);
			#endif

			return OUT;
		}
	ENDCG
	}
		}
}
