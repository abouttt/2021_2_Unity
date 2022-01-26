using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : TowerBase
{
    [SerializeField]
    private int _damage = 20;
    [SerializeField]
    private float _attackDelayTime = 1.0f;

    private Transform _shootPoint = null;
    private GameObject _beamLaser = null;

    private void Start()
    {
        if (!IsBuilded)
            return;

        _shootPoint = FindShootPoint();
        _beamLaser = ResourceManager.Instance.Instantiate("ShootObjects/Beam_Laser", _shootPoint);
        _beamLaser.SetActive(false);
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

            _beamLaser.SetActive(true);
            _beamLaser.GetComponentInChildren<LaserFade>().SetTargetPos(_target.transform.position);
            _target.GetComponent<CreepController>().Hp -= _damage;
            SoundManager.Instance.Play("Sounds/LaserTowerShoot");

            if (IsGetSupporting)
                yield return new WaitForSeconds(_attackDelayTime - (_attackDelayTime * (0.01f * AttackSpeedUpgradePer)));
            else
                yield return new WaitForSeconds(_attackDelayTime);

            _beamLaser.SetActive(false);
        }
    }
}
