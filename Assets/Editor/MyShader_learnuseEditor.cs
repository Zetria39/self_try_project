using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;



public class MyShader_learnuseEditor : ShaderGUI
{
    //private bool isSimpleMode = true;
    //private MaterialProperty mainTex;
    //private MaterialProperty secondTex;
    //private MaterialProperty waveAmplitude;
    //private MaterialProperty waveFrequency;
    //private MaterialProperty rotationSpeed;
    //private MaterialProperty color1;
    //private MaterialProperty color2;
    //private MaterialProperty speed;
    //private MaterialProperty scale;
    //private bool useNormalMap = true;

    private MaterialProperty renderingMode_now;
    public enum RenderingMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent,
    }

    public static void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        renderingMode_now = FindProperty("_renderingMode", properties);

        base.OnGUI(materialEditor, properties);
        /*
        mainTex = FindProperty("_MainTex", properties);
        secondTex = FindProperty("_SecondTex", properties);
        waveAmplitude = FindProperty("_WaveAmplitude", properties);
        waveFrequency = FindProperty("_WaveFrequency", properties);
        rotationSpeed = FindProperty("_RotationSpeed", properties);
        color1 = FindProperty("_Color1", properties);
        color2 = FindProperty("_Color2", properties);
        speed = FindProperty("_Speed", properties);
        scale = FindProperty("_Scale", properties);


        GUILayout.BeginHorizontal();
        if (isSimpleMode)
        {
            GUILayout.Box("简单模式", GUILayout.Width(100));
            if (GUILayout.Button("高级模式", GUILayout.Width(100)))
            {
                isSimpleMode = false;
            }
        }
        else
        {
            if (GUILayout.Button("简单模式", GUILayout.Width(100)))
            {
                isSimpleMode = true;
            }
            GUILayout.Box("高级模式", GUILayout.Width(100));
        }
        GUILayout.EndHorizontal();
        
        if (isSimpleMode)
        {
            materialEditor.TexturePropertyTwoLines(new GUIContent("漫反射贴图"), mainTex, baseColor, new GUIContent("环境光强度"), ambientIntensity);
            useNormalMap = EditorGUILayout.BeginToggleGroup("启用法线贴图", useNormalMap);
            if (useNormalMap)
            {
                mat.EnableKeyword("_USENORMALMAP_ON");
                materialEditor.TexturePropertySingleLine(new GUIContent("法线贴图"), normalTex, normalScale);
            }
            else
            {
                mat.DisableKeyword("_USENORMALMAP_ON");
            }
            EditorGUILayout.EndToggleGroup();

            materialEditor.ColorProperty(specColor, "高光颜色");
            materialEditor.ShaderProperty(specIntensity, "高光强度");
            materialEditor.ShaderProperty(shininess, "光泽度");
        }
        else
        {
            GUILayout.Label("效果预览");

            materialEditor.DefaultPreviewGUI(new Rect(20, 50, 200, 200), GUIStyle.none);
            GUILayout.Label("", GUILayout.Height(200));
            materialEditor.TextureProperty(mainTex, "漫反射贴图");
            materialEditor.ColorProperty(baseColor, "漫反射颜色");
            materialEditor.ShaderProperty(ambientIntensity, "环境光强度");
            useNormalMap = EditorGUILayout.BeginToggleGroup("启用法线贴图", useNormalMap);
            if (useNormalMap)
            {
                mat.EnableKeyword("_USENORMALMAP_ON");
                materialEditor.TextureProperty(normalTex, "法线贴图");
                materialEditor.ShaderProperty(normalScale, "法线强度");
            }
            else
            {
                mat.DisableKeyword("_USENORMALMAP_ON");
            }
            EditorGUILayout.EndToggleGroup();
            materialEditor.ColorProperty(specColor, "高光颜色");
            materialEditor.ShaderProperty(specIntensity, "高光强度");
            materialEditor.ShaderProperty(shininess, "光泽度");
        */
    }
}
