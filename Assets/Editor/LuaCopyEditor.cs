using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class LuaCopyEditor : Editor
{
    [MenuItem("XLua/自动生成txt后缀的Lua")]
    public static void CopyLuaToTxt()
    {
        string path = Application.dataPath + "/AB/Lua/";
        if (!Directory.Exists(path))
            return;
        //获取所有lua文件的路径
        string[] strs = Directory.GetFiles(path, "*.lua");
        //检查新路径是否存在，若不存在就创建
        string newPath = Application.dataPath + "/AB/LuaTxt/";
        //为了避免一些被删除的lua文件，不再使用，我们应该先清空目标路径
        if (!Directory.Exists(newPath))
            Directory.CreateDirectory(newPath);
        else
        {
            string[] oldFileStrs = Directory.GetFiles(newPath, "*.txt");
            for (int i = 0; i < oldFileStrs.Length; i++)
            {
                File.Delete(oldFileStrs[i]);
            }
        }
        List<string> newFileNames = new List<string>();
        string fileName;
        for (int i = 0; i < strs.Length; i++)
        {
            //将原Lua文件拷贝到新路径下，并加上.txt后缀
            fileName = newPath + strs[i].Substring(strs[i].LastIndexOf("/") + 1) + ".txt";
            newFileNames.Add(fileName);
            File.Copy(strs[i], fileName);
        }
        AssetDatabase.Refresh();
        //刷新过后再来改变文件的AB包，如果不刷新，第一次改变无效
        for (int i = 0; i < newFileNames.Count; i++)
        {
            AssetImporter importer = AssetImporter.GetAtPath(newFileNames[i].Substring(newFileNames[i].IndexOf("Assets")));
            if (importer != null)
                importer.assetBundleName = "lua";
        }
    }
}