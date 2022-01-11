using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject   _YellowLightPrefab;

    private Vector2      _mousePosition;
    private LayerMask    _layerMask;
    private RaycastHit2D _hit;

    private void Start()
    {
        _layerMask = LayerMask.GetMask("CharacterSelect");
        _YellowLightPrefab = Instantiate(_YellowLightPrefab);
        _YellowLightPrefab.SetActive(false);
    }

    private void Update()
    {
        RaycasetHit();
        OnMouseCheck();
        CharacterSelect();
    }

    private void RaycasetHit()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _hit = Physics2D.Raycast(_mousePosition, Vector2.zero, 0.0f, _layerMask);
    }

    private void OnMouseCheck()
    {
        if (_hit.collider != null)
        {
            _YellowLightPrefab.SetActive(true);
            Vector3 pos = _hit.collider.transform.position;
            _YellowLightPrefab.transform.position = new Vector3(pos.x + 0.1f, pos.y - 0.6f, 0.0f);
        }
        else
        {
            _YellowLightPrefab.SetActive(false);
        }
    }

    private void CharacterSelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_hit.collider != null)
            {
                string path = $"Prefabs/Players/{_hit.collider.name}";
                GameObject go = Resources.Load<GameObject>(path);

                if (go == null)
                {
                    Debug.Log($"null from Resources.Load() : {path}");
                }
                else
                {
                    GameManager.GetInstance.PlayerPrefab = go;
                    SceneManager.LoadScene("GameMainMenuScene");
                }
            }
        }
    }
}
