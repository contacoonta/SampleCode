
using UnityEngine;

public abstract class BehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    
    private static T _instance;

    public static T I
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if(IsDontdestroy())
            transform.SetParent(null);

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;

        if(IsDontdestroy())
            DontDestroyOnLoad(gameObject);

        OnAwaked();
    }

    protected abstract bool IsDontdestroy();
    public abstract void OnAwaked();
}
