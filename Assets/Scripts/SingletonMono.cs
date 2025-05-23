
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance()
    {
        if(instance == null)
        {
            var obj = new GameObject();
            obj.name = typeof(T).Name;
            instance = obj.AddComponent<T>();
        }
        return instance;
    }
    protected virtual void Awake()
    {
        if (instance == null)
            instance = this as T;
        DontDestroyOnLoad(instance);
    }

}
