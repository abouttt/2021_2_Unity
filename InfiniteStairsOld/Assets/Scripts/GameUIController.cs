using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoBehaviour
{
    static private GameUIController s_instance;
    public static GameUIController GetInstance => s_instance;

    [SerializeField]
    private TextMeshProUGUI _textGameScore;
    [SerializeField]
    private TextMeshProUGUI _textGameScoreMax;
    [SerializeField]
    private Image           _imageGauge;
    [SerializeField]
    private Canvas          _canvasButtonUI;
    [SerializeField]
    private Canvas          _canvasButtonCharacter;

    [SerializeField]
    private GameObject[]    _gameCharacters;

    private void Awake()
    {
        if (s_instance == null)
        {
            DontDestroyOnLoad(this);
            s_instance = this;
        }
    }

    private void Update()
    {
        UpdateUIText();
    }

    public void SetActiveUI()
    {
        if (!GameManager.GetInstance.IsStart)
        {
            _textGameScoreMax.gameObject.SetActive(true);
            _canvasButtonUI.gameObject.SetActive(true);
        }
        else
        {
            _textGameScoreMax.gameObject.SetActive(false);
            _canvasButtonUI.gameObject.SetActive(false);
            _canvasButtonCharacter.gameObject.SetActive(false);
        }
    }

    private void UpdateUIText()
    {
        _textGameScore.text = GameManager.GetInstance.GameScore.ToString();
        _textGameScoreMax.text = "Best : " + GameManager.GetInstance.GameScoreMax.ToString();

        _imageGauge.fillAmount = GameManager.GetInstance.GameGauge / 100.0f;
    }

    public void OnClickButtonStart()
    {
        GameManager.GetInstance.StartGame();
    }

    public void OnClickButtonCharacter()
    {
        _canvasButtonUI.gameObject.SetActive(false);
        _canvasButtonCharacter.gameObject.SetActive(true);
    }

    public void OnClickButtonExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickButtonMaskDude()
    {
        Player.GetInstance.SetPlayerPrefab(_gameCharacters[0]);

        _canvasButtonUI.gameObject.SetActive(true);
        _canvasButtonCharacter.gameObject.SetActive(false);
    }

    public void OnClickButtonNinjaFrog()
    {
        Player.GetInstance.SetPlayerPrefab(_gameCharacters[1]);

        _canvasButtonUI.gameObject.SetActive(true);
        _canvasButtonCharacter.gameObject.SetActive(false);
    }

    public void OnClickButtonPinkMan()
    {
        Player.GetInstance.SetPlayerPrefab(_gameCharacters[2]);

        _canvasButtonUI.gameObject.SetActive(true);
        _canvasButtonCharacter.gameObject.SetActive(false);
    }

    public void OnClickButtonVirtualGuy()
    {
        Player.GetInstance.SetPlayerPrefab(_gameCharacters[3]);

        _canvasButtonUI.gameObject.SetActive(true);
        _canvasButtonCharacter.gameObject.SetActive(false);
    }
}
