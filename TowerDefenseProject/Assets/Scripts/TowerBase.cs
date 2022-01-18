using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public float AttackRange { get; private set; }
    public float AttackDamage { get; private set; }

    public bool IsBuilded { get; set; } = false;

    protected GameObject _target = null;

    protected virtual void Init(float attackDamage, float fucntionRange)
    {
        AttackDamage = attackDamage;
        AttackRange = fucntionRange;
    }

    protected GameObject FindTarget(params string[] layerNames)
    {
        LayerMask layerMask = LayerMask.GetMask(layerNames);
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, AttackRange, layerMask);
        foreach (Collider collider in colliders)
        {
            return collider.gameObject;
        }

        return null;
    }

    protected GameObject[] FindTargets(params string[] layerNames)
    {
        LayerMask layerMask = LayerMask.GetMask(layerNames);
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, AttackRange, layerMask);
        GameObject[] gameObjects = new GameObject[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            gameObjects[i] = colliders[i].gameObject;
        }

        return gameObjects;
    }
}
