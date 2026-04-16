Shader "Unlit/MyShader_camera"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	    _DisplacementTex ("Displacement Texture", 2D) = "white" {}
        _Magnitude ("_Magnitude", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
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
            sampler2D _DisplacementTex;
            float4 _DisplacementTex_ST;
            float _Magnitude;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {/*
                //颜色滤镜
                float4 color = tex2D(_MainTex, i.uv);
                color *= float4(i.uv.r, i.uv.g, 0, 1);
                
                //纹理滤镜
                //x对应red，y对应green
                float2 disp = tex2D(_DisplacementTex, i.uv).xy; 
                //纹理图中red代表u+1，v-1；green代表u-1，v+1；_Magnitude代表扭曲程度
                disp = ((disp * 2) - 1) * _Magnitude;  
	            float4 color = tex2D(_MainTex, i.uv + disp);
                */
                //纹理滤镜(随时间变化)
                float2 distuv = float2(i.uv.x + _Time.x * 2, i.uv.y + _Time.x * 2);
                //x对应red，y对应green
	            float2 disp = tex2D(_DisplacementTex, distuv).xy;
                //纹理图中red代表u+1，v-1；green代表u-1，v+1；_Magnitude代表扭曲程度
                disp = ((disp * 2) - 1) * _Magnitude;  
	            float4 color = tex2D(_MainTex, i.uv + disp);

	            return color;
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
