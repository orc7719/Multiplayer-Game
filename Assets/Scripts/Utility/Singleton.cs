using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static string debugTag = string.Format("[Singleton {0}]", typeof(T).ToString());

    protected static T _instance;
    public static T Instance
    {
        get
        {
            if (isQuitting) { return _instance; }

            _instance = FindObjectOfType<T>();
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                _instance = obj.AddComponent<T>();
            }

            return _instance;
        }
    }


    protected static bool isQuitting;
    protected virtual void OnApplicationQuit()
    {
        isQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        if (isQuitting) { return; }
        if (_instance == this)
        {
            _instance = null;
        }
    }

    protected virtual void Awake() { }
    protected virtual void Start()
    {
        if (IsDuplicate(this))
        {
            Debug.LogWarningFormat("[Singleton {0}] Another instance already found! Destroying self as a duplicate.", typeof(T).ToString());
            Destroy(this);
        }
    }

    protected static bool IsDuplicate(Singleton<T> instance)
    {
        if (instance == null) { return false; }
        if (_instance == null) { return false; }
        return _instance != instance;
    }
}