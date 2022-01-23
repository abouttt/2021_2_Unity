using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCanon : TowerBase
{
    private Transform _shootPoint = null;
    private bool _isAttacking = false;
    private readonly float _attackDelaySec = 1.0f;

    private void Start()
    {
        Init(5, 3.5f);
    }

    private void Update()
    {
        if (!IsBuilded)
            return;

        if (_target == null)
        {
            _target = FindTarget("Enemy");
        }
        else
        {
            LookAtTarget();

            if (!_isAttacking)
            {
                _isAttacking = true;
                StartCoroutine(OnAttack());
            }
        }
    }

    protected override void Init(int damage, float range)
    {
        base.Init(damage, range);

        _shootPoint = FindShootPoint();
    }

    private IEnumerator OnAttack()
    {
        while (true)
        {
            if (!IsOnTarget())
                yield break;

            GameObject projectile = ResourceManager.Instance.Instantiate("ShootObjects/Projectile_Bolt");
            projectile.transform.position = _shootPoint.position;
            projectile.GetComponent<Projectile>().Target = _target.transform;
            SoundManager.Instance.Play("CanonTowerShoot");

            yield return new WaitForSeconds(_attackDelaySec);
        }
    }

    private bool IsOnTarget()
    {
        float dist = Vector3.Distance(transform.position, _target.transform.position);
        if (dist > Range + 0.8f)
        {
            _target = null;
            _isAttacking = false;
            return false;
        }

        return true;
    }
}
