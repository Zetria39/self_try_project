using UnityEngine;

/// <summary>
/// 单例模式且自动创建的MonoBehaviour，继承该类的派生类无需进行依附任何对象，在第一次被调用时就会自动创建一个依附该类的空对象
/// </summary>
/// <typeparam name="T">继承该类的派生类自己</typeparam>
public class SingletonAutoMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
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
            //如果是第一次调用该单例，就先自动在场景上创建一个对象，对象名改为自己类名，并将自己依附在上面
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).ToString();
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }
}
