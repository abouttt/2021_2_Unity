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

    public Vector3 StartPoint { get { return _startPoint; } }
    public List<Transform> WayPoints { get { return _wayPoints; } }


    private void Start()
    {
        Init();
        _startPoint = GameObject.Find("StartPoint").transform.position;
        MapEditor.Instance.SetupMap(_startPoint);

        GameObject creep = ResourceManager.Instance.Instantiate("Creeps/CreepCube_Ground", _startPoint + new Vector3(0.0f, 0.2f, 0.0f));
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
            Util.SetChildAsParent("Managers", s_instance.transform);
        }
    }
}
