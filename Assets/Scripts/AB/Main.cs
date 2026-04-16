using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        // LuaManager.Instance.DoString("bag_now=BagPanel:Init");
        // LuaManager.Instance.DoString("BagPanel:ShowMe()");
        LuaManager.Instance.Init();
        LuaManager.Instance.DoLuaFile("Main");
        LuaManager.Instance.DoLuaFile("BagPanel");
        LuaManager.Instance.DoLuaFile("RolePanel");
        LuaManager.Instance.DoString("MainPanel:HideMe()");
        DontDestroyOnLoad(GameObject.Find("MainPanel"));
        DontDestroyOnLoad(GameObject.Find("BagPanel"));
        DontDestroyOnLoad(GameObject.Find("RolePanel"));
    }
    bool isshow_bag = false;
    bool isshow_role = false;
    // Update is called once per frame
    void Update()
    {
        if  (Input.GetKeyDown(KeyCode.J)) LuaManager.Instance.DoString("MainPanel:ShowORHideMe()");
        if (Input.GetKeyDown(KeyCode.H))
        {
            LuaManager.Instance.DoString("BagPanel:ShowORHideMe()");
            // if (isshow_bag)
            // {
            //     LuaManager.Instance.DoString("BagPanel:ShowORHideMe()");
            //     isshow_bag = false;
            // }
            // else
            // {
            //     LuaManager.Instance.DoString("BagPanel:ShowMe()");
            //     isshow_bag = true;
            // }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            LuaManager.Instance.DoString("RolePanel:ShowORHideMe()");
            // if (isshow_role)
            // {
            //     LuaManager.Instance.DoString("RolePanel:HideMe()");
            //     isshow_role = false;
            // }
            // else
            // {
            //     LuaManager.Instance.DoString("RolePanel:ShowMe()");
            //     isshow_role = true;
            // }
        }
    }
    
}
