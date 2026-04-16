using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingMode_Change : MonoBehaviour
{
    //public enum RenderingMode
    //{
    //    Opaque,
    //    Cutout,
    //    Fade,
    //    Transparent,
    //}

    //public static void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    //{
    //    switch (renderingMode)
    //    {
    //        case RenderingMode.Opaque:
    //            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
    //            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
    //            material.SetInt("_ZWrite", 1);
    //            material.DisableKeyword("_ALPHATEST_ON");
    //            material.DisableKeyword("_ALPHABLEND_ON");
    //            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //            material.renderQueue = -1;
    //            break;
    //        case RenderingMode.Cutout:
    //            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
    //            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
    //            material.SetInt("_ZWrite", 1);
    //            material.EnableKeyword("_ALPHATEST_ON");
    //            material.DisableKeyword("_ALPHABLEND_ON");
    //            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //            material.renderQueue = 2450;
    //            break;
    //        case RenderingMode.Fade:
    //            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
    //            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    //            material.SetInt("_ZWrite", 0);
    //            material.DisableKeyword("_ALPHATEST_ON");
    //            material.EnableKeyword("_ALPHABLEND_ON");
    //            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //            material.renderQueue = 3000;
    //            break;
    //        case RenderingMode.Transparent:
    //            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
    //            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    //            material.SetInt("_ZWrite", 0);
    //            material.DisableKeyword("_ALPHATEST_ON");
    //            material.DisableKeyword("_ALPHABLEND_ON");
    //            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
    //            material.renderQueue = 3000;
    //            break;
    //    }
    //}

    public static void SetMaterialRenderingMode(Material material, int renderingMode)
    {
        switch (renderingMode)
        {
            case 0:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case 1:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case 2:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case 3:
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

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer mr = this.gameObject.GetComponent<MeshRenderer>();
        // 根据渲染器路径找到渲染器。跟资源目录无关
        int shader = mr.material.shader.FindPropertyIndex("_renderingMode");
        int now = (int)mr.material.GetFloat("_renderingMode");
        //int shader = mr.material.shader.subshaderCount;
        //// 新建Material
        //material = new Material(shader);
        //// 将新材质赋给物体
        //mr.material = material;
        //GetComponent<MeshRenderer>().material._renderingMode = new Color(1, 0, 0, 0.5f);
        SetMaterialRenderingMode(mr.material, 3);
    }

    // Update is called once per frame
    void Update()
    {

        MeshRenderer mr = this.gameObject.GetComponent<MeshRenderer>();
        // 根据渲染器路径找到渲染器。跟资源目录无关
        int shader = mr.material.shader.FindPropertyIndex("_renderingMode");
        int now = (int)mr.material.GetFloat("_renderingMode");
        //int shader = mr.material.shader.subshaderCount;
        //// 新建Material
        //material = new Material(shader);
        //// 将新材质赋给物体
        //mr.material = material;
        //GetComponent<MeshRenderer>().material._renderingMode = new Color(1, 0, 0, 0.5f);
        SetMaterialRenderingMode(mr.material, 3);
    }

}
