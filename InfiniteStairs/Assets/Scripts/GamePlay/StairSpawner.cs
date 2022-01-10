using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StairType
{
    Right,
    Left,
}

public class StairSpawner : MonoBehaviour
{
    private static StairSpawner s_instance;
    public static StairSpawner  GetInstance { get { Init(); return s_instance; } }

    [SerializeField]
    private GameObject   _stairPrefab;
    [SerializeField]
    private Vector3      _startSpawnPosition;
    [SerializeField]
    private Transform    _stairsParent;

    [SerializeField]
    private float        _spawnIncrementX;
    [SerializeField]
    private float        _spawnIncrementY;

    private ObjectPooler _stairPool;
    public ObjectPooler  StairPool => _stairPool;

    private Vector3      _currentSpawnPosition;
    private StairType    _stairType;

    private void Awake()
    {
        _stairPool = new ObjectPooler(_stairPrefab, 35, _stairsParent);
    }

    private void Start()
    {
        _currentSpawnPosition = _startSpawnPosition;
        _stairType = StairType.Left;
        SpawnStair(5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Stair")
        {
            if (collision.GetComponent<Stair>().IsFinal)
            {
                StairType stairType = (StairType)Random.Range(0, 2);
                SpawnStair(2);
            }
        }
    }

    public void SpawnStair(int createCycle)
    {
        for (int cycle = 0; cycle < createCycle; cycle++)
        {
            int length = Random.Range(1, 9);

            for (int spawnCount = 0; spawnCount < length; spawnCount++)
            {
                GameObject stair = _stairPool.Get(_currentSpawnPosition);

                switch (_stairType)
                {
                    case StairType.Right:
                        _currentSpawnPosition += new Vector3(_spawnIncrementX, _spawnIncrementY, 0);
                        break;
                    case StairType.Left:
                        _currentSpawnPosition += new Vector3(-_spawnIncrementX, _spawnIncrementY, 0);
                        break;
                }

                if (cycle == createCycle - 1 && spawnCount == length - 1)
                {
                    stair.GetComponent<Stair>().IsFinal = true;
                }
            }

            _stairType = _stairType == StairType.Left ? StairType.Right : StairType.Left;
        }
    }

    static private void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("StairSpawner");
            if (go == null)
            {
                go = new GameObject { name = "StairSpawner" };
                go.AddComponent<StairSpawner>();
            }

            s_instance = go.GetComponent<StairSpawner>();
        }
    }
}
