using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<List<string>> unitData;

    public List<int> PlayerDeck;
    public List<int> EnemyDeck;

    public GameObject battleManager;

    public bool inBattleScene = false;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        if(instance == null)
            instance = this;

        DontDestroyOnLoad(gameObject);

        LoadData();

        battleManager = Resources.Load("BattleManager") as GameObject;

        if (SceneManager.GetActiveScene().buildIndex != 0) inBattleScene = true;

        if (inBattleScene)
        {
            OnEnterBattleScene();
        }
        else if (!inBattleScene)
        {
            OnEnterMenuScene();
        }
    }

    public void LoadData()
    {
        unitData = DataLoader.instance.LoadData("Unit.xlsx");
    }

    public void OnEnterBattleScene()
    {
        if (BattleManager.instance == null) Instantiate(battleManager);
        GameObject.Find("BackToMenu").GetComponent<Button>().onClick.AddListener(delegate () { BackToMenu(); });
        GameObject.Find("ReloadGame").GetComponent<Button>().onClick.AddListener(delegate () { Reload(); });
    }
    public void OnEnterMenuScene()
    {
        GameObject.Find("PVE").GetComponent<Button>().onClick.AddListener(delegate () { EnterBattleScene("Battle_PVE"); });
        GameObject.Find("PVP").GetComponent<Button>().onClick.AddListener(delegate () { EnterBattleScene("Battle_PVP"); });
        GameObject.Find("EVE").GetComponent<Button>().onClick.AddListener(delegate () { EnterBattleScene("Battle_EVE"); });
    }

    public void EnterBattleScene(string scene)
    {
        SceneManager.LoadScene(scene);
        SceneManager.sceneLoaded += LoadedBattle;
        inBattleScene = true;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("GameStartScene");
        SceneManager.sceneLoaded += LoadedMenu;
        inBattleScene = false;
    }

    public void Reload()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.sceneLoaded += LoadedMenu;
        }
        else
        {
            SceneManager.sceneLoaded += LoadedBattle;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    static void LoadedBattle(Scene s, LoadSceneMode l)
    {
        SceneManager.sceneLoaded -= LoadedBattle;
        instance.OnEnterBattleScene();
    }
    static void LoadedMenu(Scene s, LoadSceneMode l)
    {
        SceneManager.sceneLoaded -= LoadedMenu;
        instance.OnEnterMenuScene();
    }
}


/*
 * gamemanaer传递角色数据给battlemanager，由battlemanager自行开始比赛，battlemanager在比赛完成后，将结果报告给gameManager
 *      需要定义卡组、单卡数据结构
 * 
 */
