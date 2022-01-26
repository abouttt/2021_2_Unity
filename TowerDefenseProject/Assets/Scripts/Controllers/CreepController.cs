using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepController : MonoBehaviour
{
    [SerializeField]
    private int _maxHp = 50;
    [SerializeField]
    private float _moveSpeed = 5.0f;
    [SerializeField]
    private float _deactiveTime = 1.0f;
    [SerializeField]
    private int _plusGameMoney = 50;

    [ReadOnly, SerializeField]
    private int _currentHp = 0;
    private int _wayIndex = 0;
    public float SlowPer { get; set; } = 0;
    public bool IsSlow { get; set; }
    private bool _isDeactive = false;

    private Animator _animator = null;

    public int MaxHp { get { return _maxHp; } private set { } }

    public int Hp
    {
        get { return _currentHp; }
        set
        {
            if (!_isDeactive)
            {
                _currentHp = value;

                if (_currentHp <= 0)
                {
                    _isDeactive = true;
                    GameData.Instance.GameMoney += _plusGameMoney;
                    if (MaxHp >= 200)
                        ResourceManager.Instance.Instantiate("Effects/Explosion_ShockWave", transform.position);
                    else
                        ResourceManager.Instance.Instantiate("Effects/Explosion", transform.position);
                    //SoundManager.Instance.Play("UnitDestroyed");
                    StartCoroutine(OnDeactive());
                }
            }
        }
    }

    private void Start()
    {
        _animator = transform.Find("Body").GetComponent<Animator>();
        Clear();
    }

    private void Update()
    {
        if (!_isDeactive)
            MoveToWayPoint();
    }

    private void MoveToWayPoint()
    {
        Vector3 dir = SpawnManager.Instance.WayPoints[_wayIndex].position - transform.position;
        dir.y = 0.0f;

        if (IsSlow)
            transform.position += dir.normalized * (_moveSpeed - (_moveSpeed * (0.01f * SlowPer))) * Time.deltaTime;
        else
            transform.position += dir.normalized * _moveSpeed * Time.deltaTime;

        transform.rotation = Quaternion.LookRotation(dir);

        if (dir.magnitude <= 0.1f)
        {
            _wayIndex++;
            if (SpawnManager.Instance.WayPoints.Count <= _wayIndex)
            {
                if (!_isDeactive)
                {
                    _isDeactive = true;
                    GameData.Instance.GameLife -= 10;
                    StartCoroutine(OnDeactive());
                }
            }
        }
    }

    private void Clear()
    {
        _currentHp = _maxHp;
        _wayIndex = 0;
        _isDeactive = false;
    }

    private IEnumerator OnDeactive()
    {
        yield return new WaitForSeconds(_deactiveTime);

        Clear();
        ResourceManager.Instance.Destroy(gameObject);
    }
}
