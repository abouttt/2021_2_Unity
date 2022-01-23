using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField]
    private float _range = 0.0f;
    [SerializeField]
    private int _damage = 0;

    public bool IsBuilded { get; set; } = false;

    protected GameObject _target = null;
    protected GameObject[] _targets = null;

    public float Range
    {
        get { return _range; }
        protected set { _range = value; }
    }

    public int Damage
    {
        get { return _damage; }
        protected set { _damage = value; }
    }

    protected virtual void Init(int damage, float range)
    {
        Damage = damage;
        Range = range;
    }

    protected GameObject FindTarget(string layerName)
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, Range, LayerMask.GetMask(layerName));
        foreach (Collider collider in colliders)
        {
            return collider.gameObject;
        }

        return null;
    }

    protected GameObject[] FindTargets(params string[] layerNames)
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, Range, LayerMask.GetMask(layerNames));
        GameObject[] targets = new GameObject[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            targets[i] = colliders[i].gameObject;
        }

        return targets;
    }

    protected Transform FindShootPoint()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name == "ShootPoint")
                return child;
        }

        Debug.Log("Failed find the ShootPoint");
        return null;
    }

    protected void LookAtTarget()
    {
        Vector3 dir = _target.transform.position - transform.position;
        dir.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
