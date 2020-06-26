using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<List<string>> unitData;

    public List<int> PlayerDeck;
    public List<int> EnemyDeck;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();

    }

    private void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    if (EventSystem.current.IsPointerOverGameObject())
        //    {
        //        Debug.Log("touch area is UI");
        //    }
        //    else
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
        //        RaycastHit hit;
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            Debug.Log(hit.point);
        //            GameObject point = GameObject.Instantiate(pointPrefab, hit.point, transform.rotation) as GameObject;
        //        }
        //    }
        //}
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
