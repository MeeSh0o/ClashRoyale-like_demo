using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public Unit 玩家;
    public Unit 敌人;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
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

    private void SetDeck()
    {

    }


}
