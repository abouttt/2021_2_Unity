using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static private Player s_instance;
    public static Player  GetInstance => s_instance;

    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private float      _moveX;
    [SerializeField]
    private float      _moveY;

    [SerializeField]
    private float      _gameGaugeIncrement;

    private GameObject _player;
    private bool       _isRight = false;
    [HideInInspector]
    public bool        IsDie = false;

    private void Awake()
    {
        if (s_instance == null)
        {
            DontDestroyOnLoad(this);
            s_instance = this;
        }
    }

    private void Start()
    {
        SpawnPlayerPrefab();
    }

    private void Update()
    {
        if (GameManager.GetInstance.IsStart)
        {
            if (!IsDie)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Climb();
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    Turn();
                    Climb();
                }
            }
        }
    }

    public void SetPlayerPrefab(GameObject playerPrefab)
    {
        _playerPrefab = playerPrefab;

        if (_player != null)
        {
            Destroy(_player);
        }

        SpawnPlayerPrefab();
    }

    public void SpawnPlayerPrefab()
    {
        ResetValue();
        _player = Instantiate(_playerPrefab);
        _player.transform.SetParent(transform);
    }

    private void Climb()
    {
        if (_isRight)
        {
            transform.position += new Vector3(_moveX, _moveY, 0);
        }
        else
        {
            transform.position += new Vector3(-_moveX, _moveY, 0);
        }

        CheckOnStair();
    }

    private void Turn()
    {
        _isRight = _isRight == true ? false : true;

        if (_isRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void CheckOnStair()
    {
        if (IsDie)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), 1f);

        if (hit.collider != null)
        {
            FillingGameGauge();
            GameManager.GetInstance.GameScore++;
        }
        else
        {
            IsDie = true;
        }
    }

    private void FillingGameGauge()
    {
        if (GameManager.GetInstance.GameGauge < 100.0f)
        {
            GameManager.GetInstance.GameGauge += _gameGaugeIncrement;

            if (GameManager.GetInstance.GameGauge > 100.0f)
            {
                GameManager.GetInstance.GameGauge = 100.0f;
            }
        }
    }

    private void ResetValue()
    {
        _isRight = false;
        transform.localScale = new Vector3(1, 1, 1);
    }
}
