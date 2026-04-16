Shader "Unlit/MyShader_learnuse"
{
    // Properties定义了在unity Inspector中可以设置的属性
    Properties
    {
        // 定义一个2D纹理属性_MainTex，在Inspector显示为Texture，默认白色
        
        // [KeywordEnum(Opaque, Cutout, Fade, Transparent)] _renderingMode ("Rendering Mode", float) = 0
        _MainTex ("Texture", 2D) = "white" {}
        _SecondTex  ("Texture2", 2D) = "white" {}
        _WaveAmplitude ("Wave Amplitude", Range(0,1)) = 0.1 
        _WaveFrequency ("Wave Frequency", Range(0,10)) = 1 
        _RotationSpeed ("Rotation Speed", Range(0,10)) = 1 
 
        _Color ("Color1", Color) = (1,0,0,1)
        _Color2 ("Color2", Color) = (0,0,1,1)
        _Speed ("Speed", Range(0,10)) = 1 
        _Scale ("Scale", Range(1,100)) = 5 

        
        // [HideInInspector] _renderingMode ("__mode", Float) = 0.0
        // [HideInInspector] _SrcBlend ("__src", Float) = 1.0
        // [HideInInspector] _DstBlend ("__dst", Float) = 0.0
        // [HideInInspector] _ZWrite ("__zw", Float) = 1.0
    }
    // SubShader块包含shader具体实现
    SubShader
    {
        // Tags用于设置shader渲染类型，这里是不透明物体
        Tags { 
            "RenderType"="Opaque" 
            "LightMode" = "ForwardBase"
        }
        // LOD（Level of Details）值，用于在不同性能设备上选择不同的shader实现
        LOD 100


        CGINCLUDE
            
            // 顶点着色器
            #pragma vertex vert
            // 片元着色器
            #pragma fragment frag
            // make fog work（启用unity的雾效系统）
            #pragma multi_compile_fog
            //光线用
            #include "Lighting.cginc"

            #include "UnityCG.cginc"
            
            // 定义顶点着色器的输入数据结构
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };
            
            // 定义顶点着色器传递到片元着色器的数据结构
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float3 viewDir : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _SecondTex;
            float4 _SecondTex_ST;
            float _WaveAmplitude;
            float _WaveFrequency;
            float _RotationSpeed;
            
            // 简单波浪
            v2f vert_wave (appdata v)
            {
                // 定义输出结构
                v2f o;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float wave = sin(worldPos.x * _WaveFrequency+ _Time.y) * _WaveAmplitude;
                v.vertex.y += wave;
                // 将顶点从模型空间转换到裁剪空间
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 处理纹理的缩放和平移变换到uv坐标
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // 计算雾效坐标
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            
            // 简单旋转
            v2f vert_rotate (appdata v)
            {
                // 定义输出结构
                v2f o;
                float angle = _Time.y * _RotationSpeed;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 selfPos = v.vertex.xyz;
                float3 rotatePos = worldPos;
                rotatePos.x = worldPos.x * cos(angle) - worldPos.z * sin(angle);
                rotatePos.z = worldPos.x * sin(angle) + worldPos.z * cos(angle);
                rotatePos.x = selfPos.x * cos(angle) - selfPos.z * sin(angle)+ worldPos.x-selfPos.x;
                rotatePos.z = selfPos.x * sin(angle) + selfPos.z * cos(angle)+ worldPos.z-selfPos.z;
                // 将顶点从模型空间转换到裁剪空间
                o.vertex = UnityObjectToClipPos(rotatePos);
                // 处理纹理的缩放和平移变换到uv坐标
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // 计算雾效坐标
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            // 简单缩放
            v2f vert_scale (appdata v)
            {
                // 定义输出结构
                v2f o;
                float scale = 1.0+sin(_Time.y * _RotationSpeed)*0.2;
                v.vertex.xyz *=scale;
                // 将顶点从模型空间转换到裁剪空间
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 处理纹理的缩放和平移变换到uv坐标
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // 计算雾效坐标
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            

            float4 _Color;
            float4 _Color2;
            float _Speed;
            float _Scale;

            // 简单渐变色效果
            fixed4 frag_gradient(v2f i) : SV_Target
            {
                float t = (sin(_Time.y * _Speed) + 1) * 0.5;
                fixed4 temp = lerp(_Color, _Color2, t); //插值函数
                return temp;
            }

            // 简单棋盘格效果
            fixed4 frag_chackerboard(v2f i) : SV_Target
            {
                float2 pattern = floor(i.uv * _Scale);
                // float2 pattern = i.uv;
                // float checker = floor((pattern.x) % 1+0.5) * floor((pattern.y) % 1+0.5);
                float checker = (pattern.x+pattern.y)%2;
                fixed4 temp = lerp(_Color, _Color2, checker);
                return temp;
            }

            // 圆形渐变效果
            fixed4 frag_circle(v2f i) : SV_Target
            {
                float2 center = float2(0.5,0.5);
                float dist = distance(i.uv, center);
                // float dir = 0.5*sin(dist * _Scale + _Time.y * _Speed)+0.5;
                float dir = smoothstep(0.5,0.0,dist);
                fixed4 temp = lerp(_Color, _Color2, dir);
                return temp;
            }
            
            // 波浪纹理效果
            fixed4 frag_wave(v2f i) : SV_Target
            {
                float wave = 0.5*sin(_Time.y*_Speed + i.uv.x * _Scale)+0.5;
                fixed4 temp = lerp(_Color, _Color2, wave);
                return temp;
            }
            
            // 彩虹效果
            fixed4 frag_rainbow(v2f i) : SV_Target
            {
                float3 color;
                color.r = sin(_Time.y * _Speed + i.uv.x * 6.28) * 0.5 +0.5;
                color.g = sin(_Time.y * _Speed + i.uv.x * 6.28 + 2.094) * 0.5 +0.5;
                color.b = sin(_Time.y * _Speed + i.uv.x * 6.28 + 4.189) * 0.5 +0.5;
                float4 temp = float4(color, 1);
                return temp;
            }
            
        ENDCG

        // Pass块定义渲染一个物体需要执行的渲染过程
        Pass
        {
            CGPROGRAM

            // 顶点着色器函数
            v2f vert (appdata v)
            {
                // return vert_rotate(v);
                // 定义输出结构
                v2f o;
                // 将顶点从模型空间转换到裁剪空间
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 处理纹理的缩放和平移变换到uv坐标
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // 计算雾效坐标
                UNITY_TRANSFER_FOG(o,o.vertex);
                // 同样将法线从模型空间转换到世界空间
                o.normal = normalize(mul(v.normal,(float3x3)unity_WorldToObject));
                o.normal = UnityObjectToWorldNormal(v.normal);
                // _WorldSpaceCameraPos.xyz 相机的世界坐标 后一个表示object上某一点在世界上的坐标
                o.viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
                return o;
            }

            // 片元着色器函数
            fixed4 frag (v2f i) : SV_Target
            {
                // saturate将负数强行变成0
                // return saturate(dot(i.normal, _WorldSpaceLightPos0));
                // float4 color = float4(i.uv.r, i.uv.g, 0, 1);
                // float4 color = float4((i.normal + 1) *0.5, 1);
                float temp_dot = 1-dot(i.normal,i.viewDir)*2;
                float4 color = float4(temp_dot,temp_dot,temp_dot, 1);
	            return color;
                return frag_chackerboard(i);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

            ENDCG
        }
        
        // Pass块定义渲染一个物体需要执行的渲染过程
        Pass
        {
            Tags { 
            "Queue" = "Transparent"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
           
            // 顶点着色器函数
            v2f vert (appdata v)
            {
                // return vert_rotate(v);
                // 定义输出结构
                v2f o;
                // 将顶点从模型空间转换到裁剪空间
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 处理纹理的缩放和平移变换到uv坐标
                o.uv = TRANSFORM_TEX(v.uv, _SecondTex);
                // 计算雾效坐标
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
                return o;
            }

            // 片元着色器函数
            fixed4 frag (v2f i) : SV_Target
            {
             //    float4 color = float4(i.uv.r, i.uv.g, 0, 1);
	            // return color;
             //    return frag_chackerboard(i);
                // sample the texture
                fixed4 col = tex2D(_SecondTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
        
    }
    
    // CustomEditor "MyShader_learnuseEditor"
}

/*
Shader "Unlit/MyShader_learnuse"
{
    // Properties定义了在unity Inspector中可以设置的属性
    Properties
    {
        // 定义一个2D纹理属性_MainTex，在Inspector显示为Texture，默认白色
        
        // [KeywordEnum(Opaque, Cutout, Fade, Transparent)] _renderingMode ("Rendering Mode", float) = 0
        _MainTex ("Texture", 2D) = "white" {}
        _SecondTex  ("Texture2", 2D) = "white" {}
        _WaveAmplitude ("Wave Amplitude", Range(0,1)) = 0.1 
        _WaveFrequency ("Wave Frequency", Range(0,10)) = 1 
        _RotationSpeed ("Rotation Speed", Range(0,10)) = 1 
 
        _Color ("Color1", Color) = (1,0,0,1)
        _Color2 ("Color2", Color) = (0,0,1,1)
        _Speed ("Speed", Range(0,10)) = 1 
        _Scale ("Scale", Range(1,100)) = 5 

        
        // [HideInInspector] _renderingMode ("__mode", Float) = 0.0
        // [HideInInspector] _SrcBlend ("__src", Float) = 1.0
        // [HideInInspector] _DstBlend ("__dst", Float) = 0.0
        // [HideInInspector] _ZWrite ("__zw", Float) = 1.0
    }
    // SubShader块包含shader具体实现
    SubShader
    {
        // Tags用于设置shader渲染类型，这里是不透明物体
        Tags { "RenderType"="Opaque" }
        // LOD（Level of Details）值，用于在不同性能设备上选择不同的shader实现
        LOD 100

        // Pass块定义渲染一个物体需要执行的渲染过程
        Pass
        {
            CGPROGRAM
            // 顶点着色器
            #pragma vertex vert
            // 片元着色器
            #pragma fragment frag
            // make fog work（启用unity的雾效系统）
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            
            // 定义顶点着色器的输入数据结构
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            // 定义顶点着色器传递到片元着色器的数据结构
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _WaveAmplitude;
            float _WaveFrequency;
            float _RotationSpeed;
            
            // 简单波浪
            v2f vert_wave (appdata v)
            {
                // 定义输出结构
                v2f o;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float wave = sin(worldPos.x * _WaveFrequency+ _Time.y) * _WaveAmplitude;
                v.vertex.y += wave;
                // 将顶点从模型空间转换到裁剪空间
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 处理纹理的缩放和平移变换到uv坐标
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // 计算雾效坐标
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            
            // 简单旋转
            v2f vert_rotate (appdata v)
            {
                // 定义输出结构
                v2f o;
                float angle = _Time.y * _RotationSpeed;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 selfPos = v.vertex.xyz;
                float3 rotatePos = worldPos;
                rotatePos.x = worldPos.x * cos(angle) - worldPos.z * sin(angle);
                rotatePos.z = worldPos.x * sin(angle) + worldPos.z * cos(angle);
                rotatePos.x = selfPos.x * cos(angle) - selfPos.z * sin(angle)+ worldPos.x-selfPos.x;
                rotatePos.z = selfPos.x * sin(angle) + selfPos.z * cos(angle)+ worldPos.z-selfPos.z;
                // 将顶点从模型空间转换到裁剪空间
                o.vertex = UnityObjectToClipPos(rotatePos);
                // 处理纹理的缩放和平移变换到uv坐标
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // 计算雾效坐标
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            // 简单缩放
            v2f vert_scale (appdata v)
            {
                // 定义输出结构
                v2f o;
                float scale = 1.0+sin(_Time.y * _RotationSpeed)*0.2;
                v.vertex.xyz *=scale;
                // 将顶点从模型空间转换到裁剪空间
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 处理纹理的缩放和平移变换到uv坐标
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // 计算雾效坐标
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            // 顶点着色器函数
            v2f vert (appdata v)
            {
                // return vert_rotate(v);
                // 定义输出结构
                v2f o;
                // 将顶点从模型空间转换到裁剪空间
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 处理纹理的缩放和平移变换到uv坐标
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // 计算雾效坐标
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }


            float4 _Color;
            float4 _Color2;
            float _Speed;
            float _Scale;

            // 简单渐变色效果
            fixed4 frag_gradient(v2f i) : SV_Target
            {
                float t = (sin(_Time.y * _Speed) + 1) * 0.5;
                fixed4 temp = lerp(_Color, _Color2, t); //插值函数
                return temp;
            }

            // 简单棋盘格效果
            fixed4 frag_chackerboard(v2f i) : SV_Target
            {
                float2 pattern = floor(i.uv * _Scale);
                // float2 pattern = i.uv;
                // float checker = floor((pattern.x) % 1+0.5) * floor((pattern.y) % 1+0.5);
                float checker = (pattern.x+pattern.y)%2;
                fixed4 temp = lerp(_Color, _Color2, checker);
                return temp;
            }

            // 圆形渐变效果
            fixed4 frag_circle(v2f i) : SV_Target
            {
                float2 center = float2(0.5,0.5);
                float dist = distance(i.uv, center);
                // float dir = 0.5*sin(dist * _Scale + _Time.y * _Speed)+0.5;
                float dir = smoothstep(0.5,0.0,dist);
                fixed4 temp = lerp(_Color, _Color2, dir);
                return temp;
            }
            
            // 波浪纹理效果
            fixed4 frag_wave(v2f i) : SV_Target
            {
                float wave = 0.5*sin(_Time.y*_Speed + i.uv.x * _Scale)+0.5;
                fixed4 temp = lerp(_Color, _Color2, wave);
                return temp;
            }
            
            // 彩虹效果
            fixed4 frag_rainbow(v2f i) : SV_Target
            {
                float3 color;
                color.r = sin(_Time.y * _Speed + i.uv.x * 6.28) * 0.5 +0.5;
                color.g = sin(_Time.y * _Speed + i.uv.x * 6.28 + 2.094) * 0.5 +0.5;
                color.b = sin(_Time.y * _Speed + i.uv.x * 6.28 + 4.189) * 0.5 +0.5;
                float4 temp = float4(color, 1);
                return temp;
            }
            
            // 片元着色器函数
            fixed4 frag (v2f i) : SV_Target
            {
                float4 color = float4(i.uv.r, i.uv.g, 0, 1);
	            return color;
                return frag_chackerboard(i);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
        
    // }
    
    // SubShader
    // {
        // Tags用于设置shader渲染类型，这里是半透明物体
        // Tags { "RenderType" = "Transparent" }
        // LOD（Level of Details）值，用于在不同性能设备上选择不同的shader实现
        // LOD 100
        
        // Pass块定义渲染一个物体需要执行的渲染过程
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            // 顶点着色器
            #pragma vertex vert
            // 片元着色器
            #pragma fragment frag
            // make fog work（启用unity的雾效系统）
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            
            // 定义顶点着色器的输入数据结构
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            // 定义顶点着色器传递到片元着色器的数据结构
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _SecondTex;
            float4 _SecondTex_ST;
            float _WaveAmplitude;
            float _WaveFrequency;
            float _RotationSpeed;
            
            // 简单波浪
            v2f vert_wave (appdata v)
            {
                // 定义输出结构
                v2f o;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float wave = sin(worldPos.x * _WaveFrequency+ _Time.y) * _WaveAmplitude;
                v.vertex.y += wave;
                // 将顶点从模型空间转换到裁剪空间
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 处理纹理的缩放和平移变换到uv坐标
                o.uv = TRANSFORM_TEX(v.uv, _SecondTex);
                // 计算雾效坐标
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            
            // 简单旋转
            v2f vert_rotate (appdata v)
            {
                // 定义输出结构
                v2f o;
                float angle = _Time.y * _RotationSpeed;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 selfPos = v.vertex.xyz;
                float3 rotatePos = worldPos;
                // rotatePos.x = worldPos.x * cos(angle) - worldPos.z * sin(angle);
                // rotatePos.z = worldPos.x * sin(angle) + worldPos.z * cos(angle);
                rotatePos.x = selfPos.x * cos(angle) - selfPos.z * sin(angle)+ worldPos.x-selfPos.x;
                rotatePos.z = selfPos.x * sin(angle) + selfPos.z * cos(angle)+ worldPos.z-selfPos.z;
                // 将顶点从模型空间转换到裁剪空间
                o.vertex = UnityObjectToClipPos(rotatePos);
                // 处理纹理的缩放和平移变换到uv坐标
                o.uv = TRANSFORM_TEX(v.uv, _SecondTex);
                // 计算雾效坐标
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            // 简单缩放
            v2f vert_scale (appdata v)
            {
                // 定义输出结构
                v2f o;
                float scale = 1.0+sin(_Time.y * _RotationSpeed)*0.2;
                v.vertex.xyz *=scale;
                // 将顶点从模型空间转换到裁剪空间
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 处理纹理的缩放和平移变换到uv坐标
                o.uv = TRANSFORM_TEX(v.uv, _SecondTex);
                // 计算雾效坐标
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            // 顶点着色器函数
            v2f vert (appdata v)
            {
                // return vert_rotate(v);
                // 定义输出结构
                v2f o;
                // 将顶点从模型空间转换到裁剪空间
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 处理纹理的缩放和平移变换到uv坐标
                o.uv = TRANSFORM_TEX(v.uv, _SecondTex);
                // 计算雾效坐标
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }


            float4 _Color;
            float4 _Color2;
            float _Speed;
            float _Scale;

            //简单渐变色效果
            fixed4 frag_gradient(v2f i) : SV_Target
            {
                float t = (sin(_Time.y * _Speed) + 1) * 0.5;
                fixed4 temp = lerp(_Color, _Color2, t); //插值函数
                return temp;
            }

            //简单棋盘格效果
            fixed4 frag_chackerboard(v2f i) : SV_Target
            {
                float2 pattern = floor(i.uv * _Scale);
                // float2 pattern = i.uv;
                // float checker = floor((pattern.x) % 1+0.5) * floor((pattern.y) % 1+0.5);
                float checker = (pattern.x+pattern.y)%2;
                fixed4 temp = lerp(_Color, _Color2, checker);
                return temp;
            }

            //圆形渐变效果
            fixed4 frag_circle(v2f i) : SV_Target
            {
                float2 center = float2(0.5,0.5);
                float dist = distance(i.uv, center);
                float dir = 0.5*sin(dist * _Scale + _Time.y * _Speed)+0.5;
                // float dir = smoothstep(0.5,0.0,dist);
                fixed4 temp = lerp(_Color, _Color2, dir);
                return temp;
            }
            
            //波浪纹理效果
            fixed4 frag_wave(v2f i) : SV_Target
            {
                float wave = 0.5*sin(_Time.y*_Speed + i.uv.x * _Scale)+0.5;
                fixed4 temp = lerp(_Color, _Color2, wave);
                return temp;
            }
            
            //彩虹效果
            fixed4 frag_rainbow(v2f i) : SV_Target
            {
                float3 color;
                color.r = sin(_Time.y * _Speed + i.uv.x * 6.28) * 0.5 +0.5;
                color.g = sin(_Time.y * _Speed + i.uv.x * 6.28 + 2.094) * 0.5 +0.5;
                color.b = sin(_Time.y * _Speed + i.uv.x * 6.28 + 4.189) * 0.5 +0.5;
                float4 temp = float4(color, 1);
                return temp;
            }
            
            // 片元着色器函数
            fixed4 frag (v2f i) : SV_Target
            {
             //    float4 color = float4(i.uv.r, i.uv.g, 0, 1);
	            // return color;
             //    return frag_chackerboard(i);
                // sample the texture
                fixed4 col = tex2D(_SecondTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
        
    }
    
    CustomEditor "MyShader_learnuseEditor"
}
*/