using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonTower : TowerBase
{
    [SerializeField]
    private float _attackDelayTime = 1.0f;
    [SerializeField]
    private bool _isUpgrade = false;

    private Transform _shootPoint = null;

    private void Start()
    {
        if (!IsBuilded)
            return;

        _shootPoint = FindShootPoint();
    }

    private void Update()
    {
        if (!IsBuilded)
            return;

        if (_target == null)
        {
            FindTarget("EnemyGround");
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

            GameObject projectile = null;
            if (_isUpgrade)
                projectile = ResourceManager.Instance.Instantiate("ShootObjects/Canon_Bolt2");
            else
                projectile = ResourceManager.Instance.Instantiate("ShootObjects/Canon_Bolt");

            projectile.GetComponent<Projectile>().Setup(_shootPoint.position, _target);
            SoundManager.Instance.Play("CanonTowerShoot");

            if (IsGetSupporting)
                yield return new WaitForSeconds(_attackDelayTime - (_attackDelayTime * (0.01f * AttackSpeedUpgradePer)));
            else
                yield return new WaitForSeconds(_attackDelayTime);
        }
    }
}
