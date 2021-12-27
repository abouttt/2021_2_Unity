using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Game Object")]
    [SerializeField]
    private GameObject _muzzleFlashEffect;
    [SerializeField]
    private GameObject _bulletShell;
    [SerializeField]
    private GameObject _bulletMark;

    [Header("Spawn Position")]
    [SerializeField]
    private Transform _bulletSpawnPosition;
    [SerializeField]
    private Transform  _bulletShellSpawnPosition;

    [Header("Audio Clip")]
    [SerializeField]
    private AudioClip  _audioClipFire;
    [SerializeField]
    private AudioClip  _audioClipReload;

    private AudioSource _audioSource;
    private Camera      _mainCamera;

    private ObjectPool _bulletShellPool;
    private ObjectPool _bulletMarkPool;

    private readonly float _fireRate = 0.15f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _mainCamera  = Camera.main;

        _bulletShellPool = new ObjectPool(_bulletShell, 30);
        _bulletMarkPool = new ObjectPool(_bulletMark, 30);
    }

    private void Update()
    {
        OnAttack();
    }

    private void OnEnable()
    {
        _muzzleFlashEffect.SetActive(false);
    }

    private void OnAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("start fire");
            StartCoroutine("Fire");
            StartCoroutine("MuzzleFlashEffect");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("end fire");
            StopCoroutine("Fire");
            StopCoroutine("MuzzleFlashEffect");
            _muzzleFlashEffect.SetActive(false);
        }
    }

    private IEnumerator Fire()
    {
        while(true)
        {
            FireBullet();
            SpawnBulletShell();
            PlaySound(_audioClipFire);
            yield return new WaitForSeconds(_fireRate);
        }
    }

    private void FireBullet()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        // 화면의 중앙 좌표 (Aim 기준으로 Raycast 연산)
        ray = _mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        // 부딪히는 오브젝트가 있으면 targetPoint는 광선에 부딪힌 위치
        if (Physics.Raycast(ray, out hit, 999f))
        {
            targetPoint = hit.point;
        }
        Debug.DrawRay(ray.origin, ray.direction * 999f, Color.red);

        // 첫번째 Raycast 연산으로 얻어진 targetPoint를 목표지점으로 설정하고,
        // 총구를 시작지점으로 하여 Raycast 연산
        Vector3 attackDirection = (targetPoint - _bulletSpawnPosition.position).normalized;
        if (Physics.Raycast(_bulletSpawnPosition.position, attackDirection, out hit, 999f))
        {
            //m_impaceMemoryPool.SpawnImpact(hit);
            SpawnBulletMark(hit);
        }
        Debug.DrawRay(_bulletSpawnPosition.position, attackDirection * 999f, Color.red);
    }

    private void SpawnBulletShell()
    {
        GameObject bulletShell = _bulletShellPool.Get(_bulletShellSpawnPosition.position, Random.rotation);
        bulletShell.GetComponent<BulletShell>().Setup(_bulletShellPool, transform.right);
    }

    private void SpawnBulletMark(RaycastHit hit)
    {
        GameObject bulletMark = _bulletMarkPool.Get(hit.point + hit.normal * 0.001f, Quaternion.identity);
        bulletMark.transform.LookAt(hit.point + hit.normal);
        bulletMark.GetComponent<BulletMark>().Setup(_bulletMarkPool);
    }

    private IEnumerator MuzzleFlashEffect()
    {
        while (true)
        {
            _muzzleFlashEffect.SetActive(true);
            yield return new WaitForSeconds(_fireRate * 0.3f);
            _muzzleFlashEffect.SetActive(false);
            yield return new WaitForSeconds(_fireRate * 0.3f);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
