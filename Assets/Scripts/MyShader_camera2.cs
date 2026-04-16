using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MyShader_camera2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //public Material material;
    //private void OnRenderImage(RenderTexture source, RenderTexture destination)
    //{
    //    Graphics.Blit(source, destination, material);
    //}

    //替换渲染
    //需要初始化的内容,替换用的shader,不给就是null
    public Shader ReplacementShader;

    private void OnEnable()
    {
        // 有替换的shader ,就挑其中的某个subshader方法,"RenderType"就是所有的都用这个替换
        //
        if (ReplacementShader != null)
            GetComponent<Camera>().SetReplacementShader(ReplacementShader, "RenderType");
    }

    //上一个函数一旦设置过后,会一直运行,直观的来说,就是不运行,camera小窗就会有效果
    //你如果不写下面这个函数,即使在inspector里reset了,camera小窗也不会reset,也就是没有成功reset
    //下面这个就是在inspector里reset时,对应的 GetComponent<Camera>()会清空重置
    //如果没看出来有这个特点,把c#脚本开关一下就行
    private void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }

}
