using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager  GetInstance => s_instance;

    [SerializeField]
    private float          _gaugeDecrease;

    public bool            IsStart { get; set; } = false;
    public int             GameScore { get; set; } = 0;
    public int             GameScoreMax { get; set; } = 0;
    public float           GameGauge { get; set; } = 100.0f;

    private IEnumerator    _coroutineGaugeReduction;

    private readonly float r_gameGaugeDecreaseTime = 0.01f;

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
        _coroutineGaugeReduction = GaugeReduction();
        StairSpawner.GetInstance.SetStair();
    }

    private void Update()
    {
        if (IsStart)
        {
            CheckPlayerState();
        }
    }

    public void StartGame()
    {
        IsStart = true;
        GameUIController.GetInstance.SetActiveUI();
        StartCoroutine(_coroutineGaugeReduction);
    }

    private void CheckPlayerState()
    {
        if (Player.GetInstance.IsDie)
        {
            IsStart = false;

            StopCoroutine(_coroutineGaugeReduction);
            ResetValue();

            GameUIController.GetInstance.SetActiveUI();

            Player.GetInstance.gameObject.transform.position = Vector3.zero;
            Player.GetInstance.IsDie = false;

            StairSpawner.GetInstance.SetStair();

            Debug.Log("Die");
        }
    }

    private IEnumerator GaugeReduction()
    {
        while (true)
        {
            yield return new WaitForSeconds(r_gameGaugeDecreaseTime);

            GameGauge -= _gaugeDecrease;

            if (GameGauge <= 0)
            {
                Player.GetInstance.IsDie = true;
            }
        }
    }

    private void ResetValue()
    {
        if (GameScoreMax < GameScore)
        {
            GameScoreMax = GameScore;
        }

        GameGauge = 100.0f;
        GameScore = 0;
    }
}
