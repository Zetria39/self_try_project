Shader "Unlit/MyShader_Xray"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	    _XRayColor("XRay Color", Color) = (1,0,0,0.5)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        CGINCLUDE
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _XRayColor;
        ENDCG
        
        Pass
        {
            //模板测试；
            // Ref表示参考值，0-255；
            // Comp Always表示默认通过，可写判定逻辑；Comp Greater表示参考大于缓冲，则过
            // Pass Replace表示若通过，则用参考值替换缓冲值，可写逻辑；
            // ZFail Keep表示若没通过，则保持不变，可写逻辑
            Stencil {
              Ref 3
              Comp Greater
              ZFail Keep
              Pass Replace
            }


            Tags { "Queue" = "Transparent" }
            //关闭ZWrite
            // ZWrite Off
            //默认为ZTest都通过
            // ZTest Always
            //透明物体用，混合模式
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _XRayColor;
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
