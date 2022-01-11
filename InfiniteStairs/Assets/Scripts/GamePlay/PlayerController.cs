using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public bool             IsAutoClimb = false;

    private readonly float  _moveX = 1.0f;
    private readonly float  _moveY = 0.6f;

    private bool            _isRight = false;
    private bool            _isDie = false;
    private bool            _isRunGameOver = false;
    private readonly int    r_decreaseGaugeValue = 8;

    private void Update()
    {
        if (IsAutoClimb) return;

        CheckPlayerDie();

        if (_isDie) return;

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

    private IEnumerator AutoClimb()
    {
        int cnt = 0;

        while (true)
        {
            Vector3 rightPos = transform.position + new Vector3(1.0f, -1.0f, 0.0f);
            RaycastHit2D rightHit = Physics2D.Raycast(rightPos, transform.TransformDirection(Vector2.down), 0.3f);

            Vector3 leftPos = transform.position + new Vector3(-1.0f, -1.0f, 0.0f);
            RaycastHit2D leftHit = Physics2D.Raycast(leftPos, transform.TransformDirection(Vector2.down), 0.3f);

            Debug.DrawRay(rightPos, transform.TransformDirection(Vector2.down) * 0.3f, Color.red, 1.0f);
            Debug.DrawRay(leftPos, transform.TransformDirection(Vector2.down) * 0.3f, Color.red, 1.0f);

            if (rightHit.collider != null)
            {
                if (!_isRight)
                {
                    Turn();
                }
                Climb();
            }
            else if (leftHit.collider != null)
            {
                if (_isRight)
                {
                    Turn();
                }
                Climb();
            }

            cnt++;
            if (cnt >= 30)
            {
                IsAutoClimb = false;
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CheckOnStair()
    {
        Vector3 pos = transform.position + new Vector3(0.0f, -1.3f, 0.0f);
        RaycastHit2D hit = Physics2D.Raycast(pos, transform.TransformDirection(Vector2.down), 0.3f);
        Debug.DrawRay(pos, transform.TransformDirection(Vector2.down) * 0.3f, Color.red, 1.0f);
        if (hit.collider != null)
        {
            if ( hit.collider.tag == "ItemAutoClimb")
            {
                Destroy(hit.collider.gameObject);
                if (!IsAutoClimb)
                {
                    IsAutoClimb = true;
                    StartCoroutine(AutoClimb());
                }
            }

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
            if (!_isRunGameOver)
            {
                _isRunGameOver = true;
                StartCoroutine(GameManager.GetInstance.GameOver());
            }
        }
    }
}
