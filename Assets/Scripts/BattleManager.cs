using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public Group 玩家;
    public Group 敌人;

    public List<Unit> PlayerBuilding = new List<Unit>();
    public List<Unit> EnemyBuilding = new List<Unit>();


    public GameObject prefabBullet;
    public GameObject prefabSoldier;
    public GameObject prefabBuilding;


    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 玩家胜利
    /// </summary>
    public void PlayerWin()
    {

    }
    /// <summary>
    /// 玩家失败
    /// </summary>
    public void PlayerLose()
    {

    }

    public void BattleInitiate()
    {

    }

    /// <summary>
    /// 设置牌组
    /// </summary>
    private void SetDeck()
    {

    }

    public void LoadPrefabs()
    {

    }
}
