using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_GameData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _gameMoneyText = null;
    [SerializeField]
    private TextMeshProUGUI _gameLifeText = null;
    [SerializeField]
    private TextMeshProUGUI _gameLevelText = null;

    private void Update()
    {
        _gameMoneyText.text = GameData.Instance.GameMoney.ToString();
        _gameLifeText.text = GameData.Instance.GameLife.ToString();
        _gameLevelText.text = $"GameLevel\n{GameData.Instance.GameLevel.ToString()}";
    }
}
