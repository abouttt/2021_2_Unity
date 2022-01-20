using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public float AttackRange { get; protected set; }
    public float AttackDamage { get; protected set; }

    public bool IsBuilded { get; set; } = false;

    protected GameObject _target = null;
    protected GameObject _targets = null;

    protected virtual void Init(float attackDamage, float fucntionRange)
    {
        AttackDamage = attackDamage;
        AttackRange = fucntionRange;
    }

    protected GameObject FindTarget(string layerName)
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, AttackRange, LayerMask.GetMask(layerName));
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

    protected Transform FindShootPoint()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform t in allChildren)
        {
            if (t.name == "ShootPoint")
                return t;
        }

        Debug.Log("Failed find the ShootPoint");
        return null;
    }
}
