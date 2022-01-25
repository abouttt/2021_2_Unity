using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTower : TowerBase
{
    [SerializeField]
    private float _attackDelayTime = 1.0f;

    private Transform[] _shootPoints = null;

    private void Start()
    {
        if (!IsBuilded)
            return;

        _shootPoints = new Transform[2];
        _shootPoints[0] = FindShootPoint("1");
        _shootPoints[1] = FindShootPoint("2");
    }

    private void Update()
    {
        if (!IsBuilded)
            return;

        if (_target == null)
        {
            FindTarget("EnemyFlying");
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

            for (int i = 0; i < _shootPoints.Length; i++)
            {
                GameObject projectile = ResourceManager.Instance.Instantiate("ShootObjects/Missile_Bolt");
                projectile.GetComponent<Projectile>().Setup(_shootPoints[i].position, _target);
                SoundManager.Instance.Play("MissileTowerShoot");
            }

            yield return new WaitForSeconds(_attackDelayTime);
        }
    }
}
