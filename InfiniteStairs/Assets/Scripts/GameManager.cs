using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager  GetInstance { get { Init(); return s_instance; } }

    [HideInInspector]
    public GameObject          PlayerPrefab;
    [HideInInspector]
    public GameObject          Player;

    [HideInInspector]
    public bool                IsGameStart = false;

    [HideInInspector]
    public int                 Gauge = 100;

    [HideInInspector]
    public int                 CurrentScore = 0;

    private IEnumerator        _decreaseGaugeEnumerator;
    private readonly float     r_decreaseGaugeValue = 0.03f;

    private void Start()
    {
        Init();
        if (PlayerPrefs.HasKey("BestScore") == false)
            PlayerPrefs.SetInt("BestScore", 0);
        _decreaseGaugeEnumerator = GaugeDecrease();
    }

    private void Update()
    {
        if (!IsGameStart && SceneManager.GetActiveScene().name == "GamePlayScene")
        {
            IsGameStart = true;
            GameStart();
        }
    }

    private void GameStart()
    {
        SetPlayer();
        StartCoroutine(_decreaseGaugeEnumerator);
    }

    private void SetPlayer()
    {
        Player = Instantiate(PlayerPrefab);
        Player.AddComponent<PlayerController>();
    }

    private IEnumerator GaugeDecrease()
    {
        while (true)
        {
            Gauge--;
            if (Gauge > 100)
            {
                Gauge = 100;
            }

            yield return new WaitForSeconds(r_decreaseGaugeValue);
        }
    }

    public IEnumerator GameOver()
    {
        StopCoroutine(_decreaseGaugeEnumerator);

        yield return new WaitForSeconds(0.5f);

        GameObject eft = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerDieEffect"), Player.transform.position, Quaternion.identity);
        Destroy(Player);
        Invoke("ReturnMainMenu", 1.5f);
    }

    public void ReturnMainMenu()
    {
        ResetValue();
        SceneManager.LoadScene("GameMainMenuScene");
    }

    private void ResetValue()
    {
        GameManager.GetInstance.IsGameStart = false;
        GameManager.GetInstance.Gauge = 100;
    }

    static private void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("GameManager");
            if (go == null)
            {
                go = new GameObject { name = "GameManager" };
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<GameManager>();
        }
    }
}
