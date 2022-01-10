using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private readonly float _moveX = 1.0f;
    private readonly float _moveY = 0.6f;

    private bool           _isRight = false;
    private bool           _isDie = false;
    private readonly int   r_decreaseGaugeValue = 8;

    private void Update()
    {
        CheckPlayerDie();

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
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
    }

    private void CheckOnStair()
    {
        Vector3 pos = transform.position + new Vector3(0.0f, -1.3f, 0.0f);
        RaycastHit2D hit = Physics2D.Raycast(pos, transform.TransformDirection(Vector2.down), 0.3f);
        Debug.DrawRay(pos, transform.TransformDirection(Vector2.down) * 0.3f, Color.red, 1.0f);
        if (hit.collider != null)
        {
            GameManager.GetInstance.CurrentScore++;
            GameManager.GetInstance.Gauge += r_decreaseGaugeValue;
        }
        else
        {
            _isDie = true;
        }
    }

    private void CheckPlayerDie()
    {
        if (_isDie || GameManager.GetInstance.Gauge <= 0)
        {
            GameManager.GetInstance.ResetValue();
            SceneManager.LoadScene("GameLobbyScene");
        }
    }
}
