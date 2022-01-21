using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolManager : MonoBehaviour
{
    #region Pool

    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }
        private Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{Original.name}_Root";
            Root.transform.parent = _root;

            for (int i = 0; i < count; i++)
                Push(Create());
        }

        private Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return Util.GetOrAddComponent<Poolable>(go);
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);
            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    private static PoolManager s_instance = null;
    public static PoolManager Instance { get { Init(); return s_instance; } }

    private Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    private static Transform _root;

    private void Start()
    {
        Init();
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        if (parent == null)
            parent = _pool[original.name].Root;
        else
        {
            //_pool[original.name].Root.parent = original.transform;
           // parent = _pool[original.name].Root;
        }

        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;

        return _pool[name].Original;
    }

    public void Clear()
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@PoolManager");
            if (go == null)
            {
                go = new GameObject { name = "@PoolManager" };
                go.AddComponent<PoolManager>();
            }

            s_instance = go.GetComponent<PoolManager>();

            if (_root == null)
            {
                _root = new GameObject { name = "@Pool_Root" }.transform;
            }
        }
    }
}
