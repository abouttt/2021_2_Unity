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

    [ReadOnly, SerializeField]
    private int _currentHp = 0;
    private int _wayIndex = 0;
    public float SlowPer { get; set; } = 0;
    public bool IsSlow { get; set; }
    private bool _isDeactive = false;

    private Animator _animator = null;

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
                    _animator.SetBool("isDie", true);
                    _isDeactive = true;
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
                _animator.SetBool("isArrival", true);
                if (!_isDeactive)
                {
                    _isDeactive = true;
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
        _animator.SetBool("isDie", false);
        _animator.SetBool("isArrival", false);
    }

    private IEnumerator OnDeactive()
    {
        yield return new WaitForSeconds(_deactiveTime);

        Clear();
        ResourceManager.Instance.Destroy(gameObject);
    }
}
