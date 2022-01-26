using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    private static GameData s_instance;
    public static GameData Instance { get { Init(); return s_instance; } }

    public int WaveLevel;
    public int GameMoney;
    public int GameLife;
    public int GameLevel;

    private void Start()
    {
        Init();
        InitData();
    }

    private void Update()
    {
        CheckGameData();
    }

    private void InitData()
    {
        WaveLevel = 1;
        GameMoney = 400;
        GameLife = 100;
        GameLevel = 1;
    }

    private void CheckGameData()
    {
        if (GameLife <= 0)
        {
            Debug.Log("GameOver");
        }
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("GameData");
            if (go == null)
            {
                go = new GameObject { name = "GameData" };
                go.AddComponent<GameData>();
            }

            s_instance = go.GetComponent<GameData>();
            Util.FindOrAddParentSetChild("Managers", s_instance.transform);
        }
    }
}
