using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager s_instance;
    public static ObjectPoolManager Instance { get { Init(); return s_instance; } }

    private GameObject _hitSparkEftPrefab = null;

    public ObjectPooler HitSparkEftPool { get; private set; }

    private void Start()
    {
        Init();
        InitEffect();
    }

    private void InitEffect()
    {
        GameObject parent = GameObject.Find("Effects");
        if (parent == null)
        {
            parent = new GameObject { name = "Effects" };
        }

        _hitSparkEftPrefab = Resources.Load<GameObject>("Prefabs/Effects/HitSpark");
        HitSparkEftPool = new ObjectPooler(_hitSparkEftPrefab, 50, parent.transform);
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("ObjectPoolManager");
            if (go == null)
            {
                go = new GameObject { name = "ObjectPoolManager" };
                go.AddComponent<ObjectPoolManager>();
            }

            s_instance = go.GetComponent<ObjectPoolManager>();
        }
    }
}
