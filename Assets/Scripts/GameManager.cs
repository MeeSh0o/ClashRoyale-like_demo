using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<List<string>> unitData;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();

    }

    public void GameOver()
    {

    }

    public void StartBattle()
    {

    }

    public void LoadData()
    {
        unitData = DataLoader.instance.LoadData("Unit.xlsx");
    }
}

/*
 * gamemanaer传递角色数据给battlemanager，由battlemanager自行开始比赛，battlemanager在比赛完成后，将结果报告给gameManager
 *      需要定义卡组、单卡数据结构
 * 
 */
