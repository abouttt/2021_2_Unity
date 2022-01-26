using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGTower : TowerBase
{
    [SerializeField]
    private int _damage = 5;
    [SerializeField]
    private float _attackDelayTime = 1.0f;
    [SerializeField]
    private float _shootDelayTime = 0.1f;

    private Transform _shootPoint_1 = null;
    private Transform _shootPoint_2 = null;

    private GameObject _muzzle_1 =null;
    private GameObject _muzzle_2 =null;

    private RaycastHit hit;

    private void Start()
    {
        if (!IsBuilded)
            return;

        _shootPoint_1 = FindShootPoint("1");
        _shootPoint_2 = FindShootPoint("2");

        _muzzle_1 = ResourceManager.Instance.Instantiate("ShootObjects/Effect_MG", _shootPoint_1.position, _shootPoint_1);
        _muzzle_2 = ResourceManager.Instance.Instantiate("ShootObjects/Effect_MG", _shootPoint_2.position, _shootPoint_2);
        _muzzle_1.SetActive(false);
        _muzzle_2.SetActive(false);
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
            RayToTarget();
            if (!_isAttacking)
            {
                _isAttacking = true;
                StartCoroutine(OnAttack());
            }
        }
    }

    private void RayToTarget()
    {
        Vector3 dir = (_target.transform.position - transform.position);
        Debug.DrawRay(transform.position, dir.normalized * dir.magnitude, Color.red, 0.1f);
        Physics.Raycast(transform.position, dir.normalized, out hit, dir.magnitude, LayerMask.GetMask("EnemyGround"));
    }

    private IEnumerator OnAttack()
    {
        while (true)
        {
            if (!IsOnTarget())
            {
                _muzzle_1.SetActive(false);
                _muzzle_2.SetActive(false);
                yield break;
            }
            
            for (int i = 0; i < 4; i++)
            {
                SoundManager.Instance.Play("MGTowerShoot");

                if (i % 2 == 0)
                {
                    _muzzle_1.SetActive(true);
                    _muzzle_2.SetActive(false);

                }
                else
                {
                    _muzzle_2.SetActive(true);
                    _muzzle_1.SetActive(false);
                }

                if (hit.collider.gameObject == _target)
                {
                    ResourceManager.Instance.Instantiate("Effects/HitSpark", hit.point);
                    hit.collider.gameObject.GetComponent<CreepController>().Hp -= _damage;
                }

                yield return new WaitForSeconds(_shootDelayTime);
            }
            _muzzle_1.SetActive(false);
            _muzzle_2.SetActive(false);

            if (IsGetSupporting)
                yield return new WaitForSeconds(_attackDelayTime - (_attackDelayTime * (0.01f * AttackSpeedUpgradePer)));
            else
                yield return new WaitForSeconds(_attackDelayTime);
        }
    }
}
