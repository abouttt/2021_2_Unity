using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    private static Managers Instance { get { Init(); return s_instance; } }

    MouseManager _mouse = new MouseManager();
    ResourceManager _resource = new ResourceManager();

    public static MouseManager Mouse { get { return Instance._mouse; } }
    public static ResourceManager Resource { get { return Instance._resource; } }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        _mouse.OnUpdate();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("Managers");
            if (go == null)
            {
                go = new GameObject { name = "Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._mouse.Init();
        }
    }
}
