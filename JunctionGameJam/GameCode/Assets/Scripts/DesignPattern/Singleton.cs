using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{

    #region Fields

    /// <summary>
    /// The instance.
    /// </summary>
    private static T instance = null;
    private static object syncobj = new object();
    private static bool appIsClosing = false;
    #endregion

    #region Properties

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static T Instance
    {
        get
        {
            if (appIsClosing)
                return null;

            lock (syncobj)
            {
                if (instance == null)
                {
                    T[] objs = FindObjectsOfType<T>();

                    if (objs.Length > 0)
                        instance = objs[0];

                    if (objs.Length > 1)
                        Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");

                    if (instance == null)
                    {
                        string singletonObjName = typeof(T).ToString();
                        GameObject singletonObj = GameObject.Find(singletonObjName);
                        if (singletonObj == null)
                            singletonObj = new GameObject(singletonObjName);
                        instance = singletonObj.AddComponent<T>();
                    }
                }
                return instance;
            }
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        // release reference on exit
        appIsClosing = true;
    }
    #endregion

}