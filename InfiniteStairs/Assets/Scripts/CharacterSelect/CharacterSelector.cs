using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject   _SelectLightPrefab;

    private Vector2      _mousePosition;
    private LayerMask    _layerMask;
    private RaycastHit2D _hit;

    private void Start()
    {
        _layerMask = LayerMask.GetMask("CharacterSelect");
        _SelectLightPrefab = Instantiate(_SelectLightPrefab);
        _SelectLightPrefab.SetActive(false);
    }

    private void Update()
    {
        RaycasetHit();
        OnMouseCheck();
        OnMouseClick();
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
            _SelectLightPrefab.SetActive(true);
            Vector3 pos = _hit.collider.transform.position;
            _SelectLightPrefab.transform.position = new Vector3(pos.x + 0.1f, pos.y - 0.6f, 0.0f);
        }
        else
        {
            _SelectLightPrefab.SetActive(false);
        }
    }

    private void OnMouseClick()
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
                    SceneManager.LoadScene("GameLobbyScene");
                }
            }
        }
    }
}
