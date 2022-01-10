using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameLobbyController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textBestScore;

    private GameObject      _playerPrefab;

    private void Start()
    {
        SetPlayerPrefab();
        SetBestScore();
    }

    private void SetPlayerPrefab()
    {
        _playerPrefab = Instantiate(GameManager.GetInstance.PlayerPrefab);
    }

    private void SetBestScore()
    {
        if (GameManager.GetInstance.BestScore < GameManager.GetInstance.CurrentScore)
        {
            GameManager.GetInstance.BestScore = GameManager.GetInstance.CurrentScore;
            GameManager.GetInstance.CurrentScore = 0;
        }
        _textBestScore.text = $"BEST\n{GameManager.GetInstance.BestScore.ToString()}";
    }

    public void OnClickButtonStart()
    {
        SceneManager.LoadScene("GamePlayScene");
    }

    public void OnClickButtonCharacter()
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    public void OnClickButtonExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
