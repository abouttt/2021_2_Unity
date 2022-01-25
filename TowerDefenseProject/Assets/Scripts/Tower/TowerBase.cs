using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField]
    private float _range = 0.0f;

    public bool IsBuilded { get; set; } = false;

    protected bool _isAttacking = false;

    protected GameObject _target = null;
    protected List<GameObject> _targets = new List<GameObject>();

    public float Range
    {
        get { return _range; }
        protected set { _range = value; }
    }

    protected void FindTarget(string layerName)
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, Range, LayerMask.GetMask(layerName));
        foreach (Collider collider in colliders)
        {
            _target = collider.gameObject;
        }
    }

    protected void FindTargets(params string[] layerNames)
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, Range, LayerMask.GetMask(layerNames));
        for (int i = 0; i < colliders.Length; i++)
        {
            _targets.Add(colliders[i].gameObject);
        }
    }

    protected Transform FindShootPoint(string num = null)
    {
        string shootPointStr = "ShootPoint";
        if(num != null)
        {
            shootPointStr += num;
        }

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name == shootPointStr)
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

    protected bool IsOnTarget()
    {
        float dist = Vector3.Distance(_target.transform.position, transform.position);
        if (dist > Range + 0.8f || _target.GetComponent<CreepController>().Hp <= 0 || !_target.activeSelf)
        {
            _target = null;
            _isAttacking = false;
            return false;
        }

        return true;
    }
}
