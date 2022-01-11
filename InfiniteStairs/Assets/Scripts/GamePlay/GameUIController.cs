using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textGameScore;
    [SerializeField]
    private TextMeshProUGUI _textAutoClimb;
    [SerializeField]
    private Image           _imageGauge;

    private void Update()
    {
        _textGameScore.text = GameManager.GetInstance.CurrentScore.ToString();
        _imageGauge.fillAmount = GameManager.GetInstance.Gauge * 0.01f;
        if (GameManager.GetInstance.Player != null && GameManager.GetInstance.Player.GetComponent<PlayerController>().IsAutoClimb)
        {
            _textAutoClimb.gameObject.SetActive(true);
        }
        else
        {
            _textAutoClimb.gameObject.SetActive(false);
        }
    }
}
