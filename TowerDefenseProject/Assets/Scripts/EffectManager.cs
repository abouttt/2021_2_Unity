using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager s_instance;
    public static EffectManager Instance { get { Init(); return s_instance; } }

    private GameObject _hitSparkEftPrefab = null;
    private GameObject _hitExplosionEftPrefab = null;

    public ObjectPooler HitSparkEftPool { get; private set; }
    public ObjectPooler HitExplosionPool { get; private set; }

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
        _hitExplosionEftPrefab = Resources.Load<GameObject>("Prefabs/Effects/HitExplosion");

        HitSparkEftPool = new ObjectPooler(_hitSparkEftPrefab, 50, parent.transform);
        HitExplosionPool = new ObjectPooler(_hitExplosionEftPrefab, 1, parent.transform);
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("EffectManager");
            if (go == null)
            {
                go = new GameObject { name = "EffectManager" };
                go.AddComponent<EffectManager>();
            }

            s_instance = go.GetComponent<EffectManager>();
        }
    }
}
