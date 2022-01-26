using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager s_instance;
    public static SpawnManager Instance { get { Init(); return s_instance; } }

    [SerializeField]
    private Vector3 _startPoint;

    [SerializeField]
    private List<Transform> _wayPoints = null;

    [SerializeField]
    private float _spawnDelayTime = 1.0f;

    public Vector3 StartPoint { get { return _startPoint; } }
    public List<Transform> WayPoints { get { return _wayPoints; } }


    private void Start()
    {
        Init();
        _startPoint = GameObject.Find("StartPoint").transform.position;
        MapEditor.Instance.SetupMap(_startPoint);
    }

    private IEnumerator SpawnEnemy()
    {
        int level = GameData.Instance.GameLevel;
        for (int i = 1; i < level + 1; i++)
        {
            if (i % 3 == 0)
            {
                CreateCreep("CreepCube_Flying");
            }
            else if (i % 5 == 0)
            {
                CreateCreep("CreepCubeShield_Ground");
            }
            else
            {
                CreateCreep("CreepCube_Ground");
            }

            yield return new WaitForSeconds(_spawnDelayTime);
        }
    }

    private void CreateCreep(string name)
    {
        string path = $"Creeps/{name}";
        Vector3 spawnPos = _startPoint + new Vector3(0.0f, 0.2f, 0.0f);
        GameObject creep = ResourceManager.Instance.Instantiate(path, spawnPos);
        GameObject uiHpBar = ResourceManager.Instance.Instantiate("UI/UI_HpBar", creep.transform);
        uiHpBar.GetComponent<UI_HpBar>().Creep = creep.GetComponent<CreepController>();
    }

    public void OnClickButtonStartWave()
    {
        StartCoroutine(SpawnEnemy());
        GameData.Instance.GameLevel++;
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("SpawnManager");
            if (go == null)
            {
                go = new GameObject { name = "SpawnManager" };
                go.AddComponent<SpawnManager>();
            }

            s_instance = go.GetComponent<SpawnManager>();
            Util.FindOrAddParentSetChild("Managers", s_instance.transform);
        }
    }
}
