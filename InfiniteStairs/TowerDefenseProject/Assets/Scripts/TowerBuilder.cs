using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TowerBuilder : MonoBehaviour
{
    private static TowerBuilder s_instance;
    public static TowerBuilder Instance { get { Init(); return s_instance; } }

    private GameObject _towerPrefab = null;
    private GameObject _towerSelect = null;

    private RaycastHit _hit;
    private readonly float _towerBuildOverlapRadius = 0.7f;
    private bool _isBuildable = false;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        CheckBuildable();

        if (Input.GetMouseButtonDown(0))
            BuildTower();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("TowerBuilder");
            if (go == null)
            {
                go = new GameObject { name = "TowerBuilder" };
                go.AddComponent<TowerBuilder>();
            }

            s_instance = go.GetComponent<TowerBuilder>();
        }
    }

    private void CheckBuildable()
    {
        if (_towerSelect == null || _towerPrefab == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask layerMask = LayerMask.GetMask("Ground", "BuildGround");
        if (Physics.Raycast(ray, out _hit, 100.0f, layerMask))
        {
            _towerSelect.transform.position = _hit.point;

            Util.DrawCircle(_towerSelect, _towerSelect.GetComponent<TowerBase>().AttackRange, 0.1f);

            Collider[] colliders = Physics.OverlapSphere(_towerSelect.transform.position, _towerBuildOverlapRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.tag == "Tower")
                {
                    _isBuildable = false;
                    Util.SetColorInChildren(_towerSelect, Color.red);
                    return;
                }
            }

            if (_hit.collider.gameObject.layer == LayerMask.NameToLayer("BuildGround"))
            {
                _isBuildable = true;
                Util.SetColorInChildren(_towerSelect, Color.green);
            }
            else
            {
                _isBuildable = false;
                Util.SetColorInChildren(_towerSelect, Color.red);
            }
        }
    }

    private void BuildTower()
    {
        if (_towerSelect == null || _towerPrefab == null)
            return;

        if (_isBuildable)
        {
            Destroy(_towerSelect);
            GameObject tower = Instantiate(_towerPrefab, _hit.point, Quaternion.identity);
            tower.tag = "Tower";
            tower.GetComponent<TowerBase>().IsBuilded = true;
        }
        else
        {
            Destroy(_towerSelect);
        }

        _towerSelect = null;
        _towerPrefab = null;
        _isBuildable = false;
    }

    private void SetupTower(string path)
    {
        GameObject go = Resources.Load<GameObject>(path);
        if (go == null)
        {
            Debug.Log("Failed to load prefab");
            return;
        }

        _towerPrefab = go;
        _towerSelect = Instantiate(go, Input.mousePosition, Quaternion.identity);
    }

    public void OnClickButtonTowerCanon()
    {
        SetupTower("Prefabs/Towers/TowerCanon");
    }
}
