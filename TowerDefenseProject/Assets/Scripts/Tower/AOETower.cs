using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETower : TowerBase
{
    [SerializeField]
    private float SlowPercent = 0.0f;

    private void Update()
    {
        if (!IsBuilded)
            return;

        FindTargets("EnemyGround", "EnemyFlying");
        SlowTargets();
        CheckTargetsState();
    }

    private void SlowTargets()
    {
        foreach (GameObject target in _targets)
        {
            target.GetComponent<CreepController>().SlowPer = SlowPercent;
            target.GetComponent<CreepController>().IsSlow = true;
        }
    }

    private void CheckTargetsState()
    {
        for (int i = _targets.Count - 1; i >= 0; i--)
        {
            float dist = Vector3.Distance(_targets[i].transform.position, transform.position);
            if (dist > Range + 0.8f || _targets[i].GetComponent<CreepController>().Hp <= 0 || !_targets[i].activeSelf)
            {
                _targets[i].GetComponent<CreepController>().SlowPer = 0;
                _targets[i].GetComponent<CreepController>().IsSlow = false;
                _targets.Remove(_targets[i]);
            }
        }
    }

    public override void Destroy()
    {
        foreach (GameObject target in _targets)
        {
            target.GetComponent<CreepController>().SlowPer = 0;
            target.GetComponent<CreepController>().IsSlow = false;
        }

        base.Destroy();
    }
}
