using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCanon : TowerBase
{
    [SerializeField]
    private GameObject _shootEffect = null;
    [SerializeField]
    private Transform _shootPoint = null;

    private void Start()
    {
        Init(5.0f, 3.5f);
    }

    private void FixedUpdate()
    {
        CheckTargetDistance();
        AttackToTarget();
    }

    private void Update()
    {
        if (!IsBuilded)
            return;

        if (_target == null)
        {
            _target = FindTarget("Enemy");
            return;
        }
    }

    protected override void Init(float attackDamage, float fucntionRange)
    {
        base.Init(attackDamage, fucntionRange);

        _shootEffect = Instantiate(_shootEffect, _shootPoint);
        _shootEffect.SetActive(false);
    }

    private void AttackToTarget()
    {
        if (_target == null)
            return;

        _shootEffect.SetActive(true);
        transform.LookAt(_target.transform.position);

        Vector3 dir = _target.transform.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, dir.magnitude, LayerMask.GetMask("Enemy")))
        {
            ObjectPoolManager.Instance.HitSparkEftPool.Get(hit.point);
            _target.GetComponent<Enemy>().TakeDamage(AttackDamage);
        }

        
    }

    private void CheckTargetDistance()
    {
        if (_target == null)
            return;

        float dist = Vector3.Distance(transform.position, _target.transform.position);
        if (dist > AttackRange)
        {
            _target = null;
            _shootEffect.SetActive(false);
        }
    }
}
