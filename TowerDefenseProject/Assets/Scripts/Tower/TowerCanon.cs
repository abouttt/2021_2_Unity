using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCanon : TowerBase
{
    [SerializeField]
    private float _attackDelayTime = 1.0f;

    private Transform _shootPoint = null;
    private bool _isAttacking = false;

    private void Start()
    {
        _shootPoint = FindShootPoint();
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

            yield return new WaitForSeconds(_attackDelayTime);
        }
    }

    private bool IsOnTarget()
    {
        float dist = (_target.transform.position - transform.position).magnitude;
        if (dist > Range + 0.8f || _target.GetComponent<CreepController>().Hp <= 0 || !_target.activeSelf)
        {
            _target = null;
            _isAttacking = false;
            return false;
        }

        return true;
    }
}
