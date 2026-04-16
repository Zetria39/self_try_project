using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using XLua;

/// <summary>
/// Lua管理器，提供Lua解析器
/// </summary>
public class LuaManager : ManagerBase<LuaManager>
{
    private LuaEnv luaEnv;

    public LuaTable Global
    {
        get
        {
            if (luaEnv == null)
            {
                Debug.LogError("Lua解析器未初始化！");
                return null;
            }
            return luaEnv.Global;
        }
    }

    public void Init()
    {
        if (luaEnv != null)
            return;
        //初始化
        luaEnv = new LuaEnv();
        //luaEnv.AddLoader(CustomDebugLoader);
        luaEnv.AddLoader(CustomABLoader);
    }

    /// <summary>
    /// 执行lua脚本
    /// </summary>
    /// <param name="fileName">要执行的脚本名</param>
    public void DoLuaFile(string fileName)
    {
        string str = string.Format("require('{0}')", fileName);
        DoString(str);
    }

    /// <summary>
    /// 执行Lua语句
    /// </summary>
    /// <param name="str">要执行的Lua语句</param>
    public void DoString(string str)
    {
        if (luaEnv == null)
        {
            Debug.LogError("Lua解析器未初始化！");
            return;
        }
        luaEnv.DoString(str);
    }

    /// <summary>
    /// 释放Lua垃圾
    /// </summary>
    public void Tick()
    {
        if (luaEnv == null)
        {
            Debug.LogError("Lua解析器未初始化！");
            return;
        }
        luaEnv.Tick();
    }

    /// <summary>
    /// 销毁解析器
    /// </summary>
    public void Dispose()
    {
        if (luaEnv == null)
        {
            Debug.LogError("Lua解析器未初始化！");
            return;
        }
        luaEnv.Dispose();
        luaEnv = null;
    }

    private byte[] CustomDebugLoader(ref string filePath)
    {
        string path = Application.dataPath + "/Lua/" + filePath + ".lua";
        if (File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }
        else
        {
            Debug.Log("CustomDebugLoader重定向失败，文件名为: " + filePath);
        }
        return null;
    }

    private byte[] CustomABLoader(ref string filePath)
    {
        TextAsset luaScript = ABManager.Instance.LoadRes<TextAsset>("lua", filePath + ".lua");
        if (luaScript != null)
            return luaScript.bytes;
        else
            Debug.Log("CustomABLoader重定向失败，文件名: " + filePath);
        return null;
    }
}
