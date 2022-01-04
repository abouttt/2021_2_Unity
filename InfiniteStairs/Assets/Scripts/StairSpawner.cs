using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum StairType
{
    Right,
    Left,
}

public class StairSpawner : MonoBehaviour
{
    private static StairSpawner  s_instance;
    public static StairSpawner   GetInstance => s_instance;

    [SerializeField]
    private GameObject   _stairPrefab;
    [SerializeField]
    private Vector3      _startSpawnPosition;
    [SerializeField]
    private Transform    _stairsParent;

    [SerializeField]
    private float        _incrementX;
    [SerializeField]
    private float        _incrementY;

    private ObjectPooler _stairPool;
    private Vector3      _currentSpawnPosition;

    public ObjectPooler  StairPool => _stairPool;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }

        _stairPool = new ObjectPooler(_stairPrefab, 35, _stairsParent);
    }

    public void SetStair()
    {
        _currentSpawnPosition = _startSpawnPosition;
        SetAllActiveFalse();
        StairType stairType = StairType.Left;
        SpawnStair(5, stairType);
    }

    private void SetAllActiveFalse()
    {
        Transform[] stairs = _stairsParent.GetComponentsInChildren<Transform>();

        foreach (Transform stair in stairs)
        {
            if (stair.name == _stairsParent.transform.name)
            {
                continue;
            }

            stair.gameObject.GetComponent<Stair>().IsFinal = false;
            _stairPool.Return(stair.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Stair")
        {
            if (collision.GetComponent<Stair>().IsFinal)
            {
                StairType stairType = (StairType)Random.Range(0, 2);
                SpawnStair(2, stairType);
            }
        }
    }

    private void SpawnStair(int createCycle, StairType stairType)
    {
        for (int cycle = 0; cycle < createCycle; cycle++)
        {
            int length = Random.Range(1, 10);

            for (int spawnCount = 0; spawnCount < length; spawnCount++)
            {
                GameObject stair = _stairPool.Get(_currentSpawnPosition);

                switch (stairType)
                {
                    case StairType.Right:
                        _currentSpawnPosition += new Vector3(_incrementX, _incrementY, 0);
                        break;
                    case StairType.Left:
                        _currentSpawnPosition += new Vector3(-_incrementX, _incrementY, 0);
                        break;
                }

                if (cycle == createCycle - 1 && spawnCount == length - 1)
                {
                    stair.GetComponent<Stair>().IsFinal = true;
                }
            }

            stairType = stairType == StairType.Left ? StairType.Right : StairType.Left;
        }
    }
}
