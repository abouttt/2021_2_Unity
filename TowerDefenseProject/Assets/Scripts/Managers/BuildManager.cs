using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private static BuildManager s_instance;
    public static BuildManager Instance { get { Init(); return s_instance; } }

    private GameObject _towerPrefab = null;
    private GameObject _towerSelect = null;
    private GameObject _buildedTower = null;

    private RaycastHit _hit;
    private readonly float _towerBuildOverlapRadius = 0.75f;
    private bool _isBuildable = false;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        RayToTower();

        CheckBuildable();

        if (Input.GetMouseButtonDown(0))
        {
            BuildTower();
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

            Util.DrawCircle(_towerSelect, _towerSelect.GetComponent<TowerBase>().Range, 0.1f);

            Collider[] colliders = Physics.OverlapSphere(_towerSelect.transform.position, _towerBuildOverlapRadius, LayerMask.GetMask("Tower"));
            if ((colliders.Length > 0) || (_hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")))
            {
                if (_isBuildable)
                {
                    _isBuildable = false;
                    Util.SetColorInChildren(_towerSelect, Color.red);
                }
            }
            else
            {
                if (!_isBuildable)
                {
                    _isBuildable = true;
                    Util.SetColorInChildren(_towerSelect, Color.green);
                }
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
            Util.FindOrAddParentSetChild("Towers", tower.transform);
            tower.layer = LayerMask.NameToLayer("Tower");
            tower.GetComponent<TowerBase>().IsBuilded = true;
            GameData.Instance.GameMoney -= tower.GetComponent<TowerBase>().Price;
        }
        else
        {
            Destroy(_towerSelect);
        }

        _towerSelect = null;
        _towerPrefab = null;
        _isBuildable = false;
    }

    private void RayToTower()
    {
        if (_towerSelect != null || _towerPrefab != null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 999f, LayerMask.GetMask("Tower")))
            {
                if (_buildedTower != hit.collider.gameObject)
                    DeleteUITower();
                else
                    return;

                _buildedTower = hit.collider.gameObject;
                CreateUITower();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            DeleteUITower();
        }
    }

    private void CreateUITower()
    {
        ResourceManager.Instance.Instantiate("UI/UI_Tower", _buildedTower.transform);
    }

    private void DeleteUITower()
    {
        if (_buildedTower == null)
            return;

        Destroy(Util.FindChild<UI_Tower>(_buildedTower).gameObject);
        _buildedTower = null;
    }

    private void SetupTower(string towerName)
    {
        string path = $"Prefabs/Towers/{towerName}";
        GameObject tower = ResourceManager.Instance.Load<GameObject>(path);

        if (GameData.Instance.GameMoney < tower.GetComponent<TowerBase>().Price)
            return;

        _towerPrefab = tower;
        _towerSelect = Instantiate(tower, Input.mousePosition, Quaternion.identity);
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("BuildManager");
            if (go == null)
            {
                go = new GameObject { name = "BuildManager" };
                go.AddComponent<BuildManager>();
            }

            s_instance = go.GetComponent<BuildManager>();
            Util.FindOrAddParentSetChild("Managers", s_instance.transform);
        }
    }

    public void OnClickButtonCanonTower()
    {
        SetupTower("TowerCanon");
    }

    public void OnClickButtonMGTower()
    {
        SetupTower("TowerMG");
    }

    public void OnClickButtonLaserTower()
    {
        SetupTower("TowerBeamLaser");
    }

    public void OnClickButtonMissileTower()
    {
        SetupTower("TowerMissile");
    }

    public void OnClickButtonAOETower()
    {
        SetupTower("TowerAOE");
    }

    public void OnClickButtonSupportTower()
    {
        SetupTower("TowerSupport");
    }
}
