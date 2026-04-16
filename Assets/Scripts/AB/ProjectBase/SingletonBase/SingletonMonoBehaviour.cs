using UnityEngine;

/// <summary>
/// 单例模式的MonoBehaviour，继承该类后该MonoBehaviour派生类拥有单例模式的特性，继承该类的类只能依附一个对象且只能依附一次！
/// </summary>
/// <typeparam name="T">继承该类的派生类自己</typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    //所有单例模式的MonoBehaviour派生类都有的私有静态变量，用于装载该类的唯一的实例化对象
    private static T instance;

    /// <summary>
    /// 该MonoBehaviour派生类对外开放调用其唯一实例化对象的属性
    /// </summary>
    public static T Instance
    {
        get
        {
            return instance;
        }
    }
    
    //在该类被创建时就让派生类自己装载到其唯一的静态私有变量内
    protected virtual void Awake()
    {
        instance = this as T;
    }
}
