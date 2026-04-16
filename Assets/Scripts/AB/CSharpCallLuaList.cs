using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XLua;

public static class CSharpCallLuaList
{
    [CSharpCallLua]
    public static List<Type> csharpCallLuaList = new List<Type>()
    {
        //UI控件常用委托
        typeof(UnityAction<int>),
        typeof(UnityAction<bool>),
        typeof(UnityAction<string>),
        typeof(UnityAction<float>),
        typeof(UnityAction<Vector2>),
    };
}
