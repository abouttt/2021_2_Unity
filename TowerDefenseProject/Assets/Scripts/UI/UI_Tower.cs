using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tower : MonoBehaviour
{
    [SerializeField]
    private int _upgradePrice = 50;

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;
    }

    public void OnClickButtonUpgradeTower()
    {
        if (GameData.Instance.GameMoney < _upgradePrice)
            return;

        GameObject originalTower = transform.parent.gameObject;

        string name = originalTower.name;
        name = name.Replace("(Clone)", "");
        name += "2";

        GameObject upgradeTower = ResourceManager.Instance.
            Instantiate($"Towers/{name}", originalTower.transform.position, originalTower.transform.parent);
        upgradeTower.layer = LayerMask.NameToLayer("Tower");
        upgradeTower.GetComponent<TowerBase>().IsBuilded = true;

        Destroy(originalTower);
    }

    public void OnClickButtonSellTower()
    {
        GameObject tower = transform.parent.gameObject;
        GameData.Instance.GameMoney += (int)(tower.GetComponent<TowerBase>().Price * 0.5f);
        tower.GetComponent<TowerBase>().Destroy();
    }
}
