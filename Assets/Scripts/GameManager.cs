using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void GameOver()
    {

    }

    public void StartBattle()
    {

    }
}

/*
 * gamemanaer传递角色数据给battlemanager，由battlemanager自行开始比赛，battlemanager在比赛完成后，将结果报告给gameManager
 *      需要定义卡组、单卡数据结构
 * 
 */