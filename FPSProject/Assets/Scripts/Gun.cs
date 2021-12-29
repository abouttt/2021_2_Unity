using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }

public class Gun : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent          _onAmmoEvent = new AmmoEvent();
    [HideInInspector]
    public MagazineEvent      _onMagazinEvent = new MagazineEvent();

    [Header("Infomation")]
    [SerializeField]
    private WeaponInformation _weaponInformation;
    [SerializeField]
    private float             _fireRate = 0.10f;

    [Header("Game Object")]
    [SerializeField]
    private List<GameObject>  _muzzleFlashEffectList;
    [SerializeField]
    private GameObject        _bulletShell;
    [SerializeField]
    private GameObject        _bulletMark;

    [Header("Spawn Position")]
    [SerializeField]
    private Transform         _bulletSpawnPosition;
    [SerializeField]
    private Transform         _bulletShellSpawnPosition;

    [Header("Audio Clip")]
    [SerializeField]
    private AudioClip         _audioClipFire;
    [SerializeField]
    private AudioClip         _audioClipReload;
    [SerializeField]
    private AudioClip         _audioClipEmptyGun;

    [Header("Crosshair UI")]
    [SerializeField]
    private Image _crosshairImage;

    private AudioSource       _audioSource;
    private Camera            _mainCamera;

    private ObjectPool        _bulletShellPool;
    private ObjectPool        _bulletMarkPool;

    private bool              _isReload = false;
    private readonly float    _defaultFOV = 60;
    private readonly float    _zoomFOV = 30;

    public string             WeaponName => _weaponInformation.Name;
    public int                MaxMagazine => _weaponInformation.MaxMagazine;
    public int                CurrentMagazine => _weaponInformation.CurrentMagazine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _mainCamera  = Camera.main;

        _bulletShellPool = new ObjectPool(_bulletShell, 30);
        _bulletMarkPool  = new ObjectPool(_bulletMark, 30);

        _weaponInformation.CurrentAmmo     = _weaponInformation.MaxAmmo;
        _weaponInformation.CurrentMagazine = _weaponInformation.MaxMagazine;
    }

    private void Update()
    {
        OnAttack();
        Reload();
        _onAmmoEvent.Invoke(_weaponInformation.CurrentAmmo, _weaponInformation.MaxAmmo);
        _onMagazinEvent.Invoke(_weaponInformation.CurrentMagazine);
    }

    private void OnEnable()
    {
        SetActiveFalseMuzzleFlashEffect();
        _onAmmoEvent.Invoke(_weaponInformation.CurrentAmmo, _weaponInformation.MaxAmmo);
        _onMagazinEvent.Invoke(_weaponInformation.CurrentMagazine);
    }

    private void OnAttack()
    {
        if (Input.GetMouseButtonDown(0) && !_isReload)
        {
            if (_weaponInformation.CurrentAmmo > 0)
            {
                StartCoroutine("Fire");
                StartCoroutine("MuzzleFlashEffect");
            }
            else
            {
                PlaySound(_audioClipEmptyGun);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopCoroutine("Fire");
            StopCoroutine("MuzzleFlashEffect");
            SetActiveFalseMuzzleFlashEffect();
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetZoomMode();
        }

        if (Input.GetMouseButtonUp(1))
        {
            ReleaseZoomMode();
        }
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            if (_isReload)
            {
                StopCoroutine("MuzzleFlashEffect");
                SetActiveFalseMuzzleFlashEffect();
                yield break;
            }

            if (_weaponInformation.CurrentAmmo > 0)
            {
                FireBullet();
                SpawnBulletShell();
                PlaySound(_audioClipFire);
                _weaponInformation.CurrentAmmo--;
            }
            else
            {
                StopCoroutine("MuzzleFlashEffect");
                SetActiveFalseMuzzleFlashEffect();
                PlaySound(_audioClipEmptyGun);
                yield break;
            }

            yield return new WaitForSeconds(_fireRate);
        }
    }

    private void FireBullet()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        // 화면의 중앙 좌표 (crosshair 기준으로 Raycast 연산)
        ray = _mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        // 부딪히는 오브젝트가 있으면 targetPoint는 광선에 부딪힌 위치
        if (Physics.Raycast(ray, out hit, 999f))
        {
            targetPoint = hit.point;
        }
        Debug.DrawRay(ray.origin, ray.direction * 999f, Color.red);

        if (hit.collider.gameObject.tag == "Border")
        {
            return;
        }

        // 첫번째 Raycast 연산으로 얻어진 targetPoint를 목표지점으로 설정하고,
        // 총구를 시작지점으로 하여 Raycast 연산
        Vector3 attackDirection = (targetPoint - _bulletSpawnPosition.position).normalized;
        if (Physics.Raycast(_bulletSpawnPosition.position, attackDirection, out hit, 999f))
        {
            if (hit.collider.gameObject.tag != "BulletShell")
            {
                SpawnBulletMark(hit);
            }

            if (hit.collider.gameObject.tag == "ExplosionDrum")
            {
                hit.collider.gameObject.GetComponent<ExplosionDrum>().TakeDamage(_weaponInformation.WeaponDamage);
            }
            else if (hit.collider.gameObject.tag == "NormalDrum")
            {
                hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(attackDirection, ForceMode.Impulse);
            }
        }
        Debug.DrawRay(_bulletSpawnPosition.position, attackDirection * 999f, Color.red);
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R)                                  &&
            _weaponInformation.CurrentAmmo != _weaponInformation.MaxAmmo &&
            _weaponInformation.CurrentMagazine > 0                       &&
            !_isReload)
        {
            _isReload = true;
            PlaySound(_audioClipReload);
            StartCoroutine("Reloading");
        }
    }

    private IEnumerator Reloading()
    {
        while (true)
        {
            if (!_audioSource.isPlaying)
            {
                _isReload = false;
                FillBullet();
                yield break;
            }

            yield return null;
        }
    }

    private void SetZoomMode()
    {
        _crosshairImage.enabled = false;
        _mainCamera.fieldOfView = _zoomFOV;
        GameObject equipWeapon = PlayerInformation.Instnace.GetEquippedWeapon;
        equipWeapon.transform.position = PlayerInformation.Instnace.WeaponZoomPosition.position;
        equipWeapon.transform.rotation = PlayerInformation.Instnace.WeaponZoomPosition.rotation;
    }

    private void ReleaseZoomMode()
    {
        _crosshairImage.enabled = true;
        _mainCamera.fieldOfView = _defaultFOV;
        GameObject equipWeapon = PlayerInformation.Instnace.GetEquippedWeapon;
        equipWeapon.transform.position = PlayerInformation.Instnace.WeaponDefaultPosition.position;
        equipWeapon.transform.rotation = PlayerInformation.Instnace.WeaponDefaultPosition.rotation;
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
        bulletMark.transform.SetParent(hit.transform);
    }

    private IEnumerator MuzzleFlashEffect()
    {
        while (true)
        {
            int randIndex = Random.Range(0, _muzzleFlashEffectList.Count);
            _muzzleFlashEffectList[randIndex].SetActive(true);

            yield return new WaitForSeconds(_fireRate * 0.3f);

            _muzzleFlashEffectList[randIndex].SetActive(false);

            yield return new WaitForSeconds(_fireRate * 0.3f);
        }
    }

    private void SetActiveFalseMuzzleFlashEffect()
    {
        foreach (GameObject muzzleFlashEffect in _muzzleFlashEffectList)
        {
            muzzleFlashEffect.SetActive(false);
        }
    }

    private void FillBullet()
    {
        _weaponInformation.CurrentAmmo = _weaponInformation.MaxAmmo;
        _weaponInformation.CurrentMagazine--;
    }

    private void PlaySound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
