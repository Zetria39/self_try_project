using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 通过AB包进行资源加载的管理器
/// </summary>
public class ABManager : SingletonAutoMonoBehaviour<ABManager>
{
    //主包
    private AssetBundle mainAB = null;
    //依赖包获取用的配置文件
    private AssetBundleManifest manifest = null;

    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// AB包存放路径，随项目修改
    /// </summary>
    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    private string MainABName
    {
        get
        {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#else
            return "PC";
#endif
        }
    }

    #region 同步加载
    public void LoadAB(string abName)
    {
        //加载主包
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //获取依赖包相关信息
        AssetBundle ab;
        //加载主包中的关键配置文件 获取依赖包
        string[] strs = manifest.GetAllDependencies(abName);
        //加载依赖包
        for (int i = 0; i < strs.Length; i++)
        {
            //判断包是否加载过
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
        //加载目标包
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
    }

    //同步加载，不指定类型
    public Object LoadRes(string abName, string resName)
    {
        //加载AB包
        LoadAB(abName);
        Object obj = abDic[abName].LoadAsset(resName);
        //为了外部使用方便，在加载资源时，先判断是否为Object，如果是直接实例化返回出去
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }

    //同步加载，使用type指定类型
    public Object LoadRes(string abName, string resName, System.Type type)
    {
        //加载AB包
        LoadAB(abName);
        Object obj = abDic[abName].LoadAsset(resName, type);
        //为了外部使用方便，在加载资源时，先判断是否为Object，如果是直接实例化返回出去
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }

    //同步加载，使用泛型指定类型
    public T LoadRes<T>(string abName, string resName) where T : Object
    {
        //加载AB包
        LoadAB(abName);
        T obj = abDic[abName].LoadAsset<T>(resName);
        //为了外部使用方便，在加载资源时，先判断是否为Object，如果是直接实例化返回出去
        if (obj is GameObject)
            return Instantiate(obj);
        else
            return obj;
    }

    #endregion

    #region 异步加载
    //这里的异步加载 AB包并没有使用异步加载，只是从AB包中加载资源时异步加载

    //通过名字异步加载资源
    public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        StartCoroutine(LoadResCoroutine(abName, resName, callBack));
    }

    private IEnumerator LoadResCoroutine(string abName, string resName, UnityAction<Object> callBack)
    {
        //加载AB包
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;
        //异步加载结束后，通过委托，传递给外部，来使用
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset));
        else
            callBack(abr.asset);
    }

    //根据Type异步加载资源
    public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        StartCoroutine(LoadResCoroutine(abName, resName, type, callBack));
    }

    private IEnumerator LoadResCoroutine(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        //加载AB包
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
        yield return abr;
        //异步加载结束后，通过委托，传递给外部，来使用
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset));
        else
            callBack(abr.asset);
    }

    //根据泛型异步加载
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        StartCoroutine(LoadResCoroutine(abName, resName, callBack));
    }

    private IEnumerator LoadResCoroutine<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        //加载AB包
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;
        //异步加载结束后，通过委托，传递给外部，来使用
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset as T));
        else
            callBack(abr.asset as T);
    }
    #endregion

    //单个包卸载
    public void UnLoad(string abName)
    {
        if (abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }

    //所有包的卸载
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }
}
