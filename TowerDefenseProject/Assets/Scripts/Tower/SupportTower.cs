using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportTower : TowerBase
{
    [SerializeField]
    private float _supportAttackSpeedPer = 0.0f;

    private void Update()
    {
        if (!IsBuilded)
            return;

        FindTargets("Tower");
        CheckTargetsState();
        SupportTowers();
    }

    private void SupportTowers()
    {
        foreach (GameObject target in _targets)
        {
            target.GetComponent<TowerBase>().IsGetSupporting = true;
            target.GetComponent<TowerBase>().AttackSpeedUpgradePer = _supportAttackSpeedPer;
        }
    }

    private void CheckTargetsState()
    {
        for (int i = _targets.Count - 1; i >= 0; i--)
        {
            if (_targets[i] == null)
            {
                _targets.Remove(_targets[i]);
                continue;
            }

            float dist = Vector3.Distance(_targets[i].transform.position, transform.position);
            if (dist > Range + 0.8f)
                _targets.Remove(_targets[i]);
        }
    }

    public override void Destroy()
    {
        foreach (GameObject target in _targets)
        {
            target.GetComponent<TowerBase>().AttackSpeedUpgradePer = 0;
            target.GetComponent<TowerBase>().IsGetSupporting = false;
        }

        base.Destroy();
    }
}
