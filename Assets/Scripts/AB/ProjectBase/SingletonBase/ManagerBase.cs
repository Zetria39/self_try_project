/// <summary>
/// 单例模式管理器基类，不继承MonoBehaviour，继承该类可以实现单例，继承该类的类不应该在外部new()
/// </summary>
/// <typeparam name="T">继承该类的管理器自己</typeparam>
public abstract class ManagerBase<T> where T : new()
{
    //所有管理器类的都有的私有静态变量，用于装载该类的唯一的实例化对象
    private static T instance;

    /// <summary>
    /// 管理器对外开放调用其唯一实例化对象的属性
    /// </summary>
    public static T Instance
    {
        get
        {
            //如果私有变量没有自己类的类对象，就先使用构造方法实例化一个，因此这里只能用公开无参构造方法约束
            if (instance == null)
                instance = new T();
            return instance;
        }
    }

    //由于使用了公开无参构造方法约束，这里不再使用私有构造方法，但因此外部可能会构造该类的对象，因此需要注意
}
