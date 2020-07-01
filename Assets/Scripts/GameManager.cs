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

    public List<int> PlayerDeck;
    public List<int> EnemyDeck;

    public GameObject battleManager;

    public bool inBattleScene = false;

    public List<GameObject> BattleSceneObjects;

    public bool isPlayerHuman;
    public bool isEnemyHuman;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (battleManager == null)
        battleManager = Resources.Load("BattleManager") as GameObject;

        if (SceneManager.GetActiveScene().buildIndex != 0) inBattleScene = true;

        if (inBattleScene)
        {
            OnEnterBattleScene();
            GameObject.Find("BackToMenu").GetComponent<Button>().onClick.AddListener(delegate () { instance.BackToMenu(); });
            GameObject.Find("ReloadGame").GetComponent<Button>().onClick.AddListener(delegate () { instance.Reload(); });
        }
        else if (!inBattleScene)
        {
            OnEnterMenuScene();
            //GameObject.Find("PVE").GetComponent<Button>().onClick.AddListener(delegate () { instance.EnterBattleScene("Battle"); });
            //GameObject.Find("PVP").GetComponent<Button>().onClick.AddListener(delegate () { instance.EnterBattleScene("Battle_PVP"); });
            //GameObject.Find("EVE").GetComponent<Button>().onClick.AddListener(delegate () { instance.EnterBattleScene("Battle_EVE"); });
        }
    }


    public void OnEnterBattleScene()
    {
        Debug.Log("进入战斗场景");

        foreach(GameObject obj in BattleSceneObjects)
        {
            //Instantiate(obj).name = obj.name;
        }

        if (BattleManager.instance == null) Instantiate(battleManager);

    }
    public void OnEnterMenuScene()
    {
        Debug.Log("进入主选单");

    }

    // 用于按钮
    public void EnterBattleScene(string scene)
    {
        SceneManager.LoadScene(scene);
        //SceneManager.sceneLoaded += LoadedBattle;
        inBattleScene = true;
    }

    // 用于按钮
    public void BackToMenu()
    {
        SceneManager.LoadScene("GameStartScene");
        //SceneManager.sceneLoaded += LoadedMenu;
        inBattleScene = false;
    }

    // 用于按钮
    public void Reload()
    {
        //if(SceneManager.GetActiveScene().buildIndex == 0)
        //{
        //    SceneManager.sceneLoaded += LoadedMenu;
        //}
        //else
        //{
        //    SceneManager.sceneLoaded += LoadedBattle;
        //}
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    //static void LoadedBattle(Scene s, LoadSceneMode l)
    //{
    //    SceneManager.sceneLoaded -= LoadedBattle;
    //    //instance.OnEnterBattleScene();
    //}
    //static void LoadedMenu(Scene s, LoadSceneMode l)
    //{
    //    SceneManager.sceneLoaded -= LoadedMenu;
    //    //instance.OnEnterMenuScene();
    //}


}


/*
 * gamemanaer传递角色数据给battlemanager，由battlemanager自行开始比赛，battlemanager在比赛完成后，将结果报告给gameManager
 *      需要定义卡组、单卡数据结构
 * 
 */
