// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/MyShader_camera2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color1", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float depth : DEPTH;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color1;

            v2f vert (appdata v)
            {
                v2f o;
                // o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.depth = -UnityObjectToViewPos(v.vertex).z * _ProjectionParams.w;
				// o.depth = -UnityObjectToViewPos(v.vertex).z;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float invert = 1 - i.depth;
                
                //�����Ǳ��滻��shader���ж�Ӧ��_MainTex����Կ�����ȡ������
                //������shader��û��_MainTex,�����Լ���Ĭ��ֵ��
                //fixed4 color = tex2D(_MainTex, i.uv);
				// return color;
                //�����Ǳ��滻��shader���ж�Ӧ��_Color1����Կ�����ȡ������
                //������shader��û��_Color1,�����Լ���Ĭ��ֵ��
				return float4(invert, invert, invert, 1) * _Color1;
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }/*
    SubShader
    {
		ZWrite Off   // �ر����д��
		Blend SrcAlpha OneMinusSrcAlpha  // �������ģʽ
		Tags { "RenderType" = "Transparent" }
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float depth : DEPTH;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color1;

            v2f vert (appdata v)
            {
                v2f o;
                // o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.depth = -UnityObjectToViewPos(v.vertex).z * _ProjectionParams.w;
				// o.depth = -UnityObjectToViewPos(v.vertex).z;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float invert = 1 - i.depth;
                
                //�����Ǳ��滻��shader���ж�Ӧ��_MainTex����Կ�����ȡ������
                //������shader��û��_MainTex,�����Լ���Ĭ��ֵ��
                //fixed4 color = tex2D(_MainTex, i.uv);
				// return color;
                //�����Ǳ��滻��shader���ж�Ӧ��_Color1����Կ�����ȡ������
                //������shader��û��_Color1,�����Լ���Ĭ��ֵ��
				return float4(invert, invert, invert, 1) * _Color1;
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    */
    SubShader
    { 
		ZWrite Off   // �ر����д��
        Cull Off
		Blend SrcAlpha OneMinusSrcAlpha  // �������ģʽ
		Tags { "RenderType" = "Transparent" }

		Pass{
				
		    CGPROGRAM
		    #pragma vertex vert
		    #pragma fragment frag

                    #include "UnityCG.cginc"

		    struct appdata
		    {
			    float4 vertex : POSITION;
		    };

		    struct v2f
		    {
			    float4 vertex : SV_POSITION;
		    };

		    v2f vert(appdata v)
		    {
			    v2f o;
			    o.vertex = UnityObjectToClipPos(v.vertex);
			    return o;
		    }
		
            half4 _Color;

		    float4 frag(v2f i) : SV_Target
		    {
			    return  _Color;  // ��������������ɫ
		    }
		    ENDCG
	    }
    }
}
