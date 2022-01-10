using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager  GetInstance { get { Init(); return s_instance; } }

    public GameObject          PlayerPrefab;
    [HideInInspector]
    public GameObject          Player;

    [HideInInspector]
    public bool                IsGameStart = false;

    [HideInInspector]
    public int                 Gauge = 100;

    [HideInInspector]
    public int                 CurrentScore = 0;
    [HideInInspector]
    public int                 BestScore = 0;

    private IEnumerator        _DecreaseGaugeEnumerator;
    private readonly float     r_decreaseGaugeValue = 0.03f;

    private void Start()
    {
        Init();
        _DecreaseGaugeEnumerator = GaugeDecrease();
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
        StartCoroutine(_DecreaseGaugeEnumerator);
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

    public void ResetValue()
    {
        StopCoroutine(_DecreaseGaugeEnumerator);
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
