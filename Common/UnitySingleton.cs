using UnityEngine;

/// <summary>
/// GameObject에 기생하는 컴포넌트형 Singleton의 가상 클래스.
/// </summary>
/// <para>상속받은 클래스에서 Awake, OnDestroy, OnApplicationQuit 메소드는 반드시 override 하여 부모 메소드를 호출할것</para>
/// <typeparam name="T"></typeparam>
public abstract class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T>
{
    private static T _instance = null;
    private static object _lock = new object();
    private static bool _isQuitApp = false;

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(T)) as T;
                    if (_instance == null)
                    {
                        _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                        // DontDestroyOnLoad(_instance.gameObject);
                    }
#if UNITY_EDITOR
                    else if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogWarning(typeof(T).ToString() + ": Already instance.");
                        return _instance;
                    }
#endif
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = this as T;
                // DontDestroyOnLoad(this.gameObject);
            }
            else if (_instance != this.GetComponent<T>())
            {
                Destroy(this.gameObject);
            }
        }
    }

    protected virtual void OnDestroy()
    {
        if (!_isQuitApp)
        {
            _instance = null;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _isQuitApp = true;
    }
}
