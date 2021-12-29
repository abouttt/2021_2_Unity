using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Object
{
    private GameObject        _poolItem = null;
    private Queue<GameObject> _itemQueue = new Queue<GameObject>();

    public ObjectPool(GameObject poolItem, int size)
    {
        _itemQueue = new Queue<GameObject>();
        _poolItem = poolItem;
        Init(size);
    }

    public GameObject Get()
    {
        if (_itemQueue.Count <= 0)
        {
            CreatePoolItem();
        }

        GameObject poolItem = _itemQueue.Dequeue();
        poolItem.SetActive(true);

        return poolItem;
    }

    public GameObject Get(Vector3 position)
    {
        if (_itemQueue.Count <= 0)
        {
            CreatePoolItem();
        }

        GameObject poolItem = _itemQueue.Dequeue();
        poolItem.SetActive(true);
        poolItem.transform.position = position;

        return poolItem;
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        if (_itemQueue.Count <= 0)
        {
            CreatePoolItem();
        }

        GameObject poolItem = _itemQueue.Dequeue();
        poolItem.SetActive(true);
        poolItem.transform.position = position;
        poolItem.transform.rotation = rotation;

        return poolItem;
    }

    public void Return(GameObject poolItem)
    {
        poolItem.SetActive(false);
        _itemQueue.Enqueue(poolItem);
    }

    private void Init(int size)
    {
        for (int i = 0; i < size; i++)
        {
            CreatePoolItem();
        }
    }

    private void CreatePoolItem()
    {
        GameObject poolItem = Instantiate(_poolItem);
        poolItem.SetActive(false);
        _itemQueue.Enqueue(poolItem);
    }
}
