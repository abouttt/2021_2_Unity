using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler
{
    private GameObject _poolObject = null;
    private Queue<GameObject> _objectPool = new Queue<GameObject>();
    private Transform _parent = null;

    public ObjectPooler(GameObject poolObject, int size, Transform parent = null)
    {
        _poolObject = poolObject;
        _parent = parent;
        Init(size);
    }

    public GameObject Get(Vector3 position)
    {
        if (_objectPool.Count <= 0)
        {
            CreatePoolObject();
        }
        GameObject poolObject = _objectPool.Dequeue();
        poolObject.SetActive(true);
        poolObject.transform.position = position;
        return poolObject;
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        if (_objectPool.Count <= 0)
        {
            CreatePoolObject();
        }
        GameObject poolObject = _objectPool.Dequeue();
        poolObject.SetActive(true);
        poolObject.transform.position = position;
        poolObject.transform.rotation = rotation;

        return poolObject;
    }

    public void Return(GameObject poolObject)
    {
        poolObject.SetActive(false);
        _objectPool.Enqueue(poolObject);
    }

    private void Init(int size)
    {
        for (int i = 0; i < size; i++)
        {
            CreatePoolObject();
        }
    }

    private void CreatePoolObject()
    {
        GameObject poolObject = GameObject.Instantiate(_poolObject);
        poolObject.transform.SetParent(_parent);
        poolObject.SetActive(false);
        _objectPool.Enqueue(poolObject);
    }
}
