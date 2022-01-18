using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCanon : TowerBase
{
    private GameObject _shootEffect = null;
    private Transform _shootPoint = null;
    private bool _isAttacking = false;
    private readonly float _attackDelaySec = 0.1f;

    private void Start()
    {
        Init(5.0f, 3.5f);
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
        else
        {
            transform.LookAt(_target.transform.position);

            if (!_isAttacking)
            {
                _isAttacking = true;
                StartCoroutine(OnAttack());
            }
        }
    }

    protected override void Init(float attackDamage, float fucntionRange)
    {
        base.Init(attackDamage, fucntionRange);

        _shootPoint = FindShootPoint();
        _shootEffect = Instantiate(Resources.Load<GameObject>("Prefabs/Effects/ShootEffect"), _shootPoint);
        _shootEffect.SetActive(false);
    }

    private IEnumerator OnAttack()
    {
        while (true)
        {
            if (!IsOnTarget())
                yield break;

            AttackToTarget();

            yield return new WaitForSeconds(_attackDelaySec);
        }
    }

    private void AttackToTarget()
    {
        _shootEffect.SetActive(true);
        Vector3 dir = _target.transform.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, dir.magnitude, LayerMask.GetMask("Enemy")))
        {
            EffectManager.Instance.HitSparkEftPool.Get(hit.point);
            _target.GetComponent<Enemy>().TakeDamage(AttackDamage);
        }
    }

    private bool IsOnTarget()
    {
        float dist = Vector3.Distance(transform.position, _target.transform.position);
        if (dist > AttackRange + 0.7f)
        {
            _target = null;
            _isAttacking = false;
            _shootEffect.SetActive(false);
            return false;
        }

        return true;
    }
}
