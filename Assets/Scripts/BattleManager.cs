using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public Objects ���;
    public Objects ����;

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
    /// ���ʤ��
    /// </summary>
    public void PlayerWin()
    {

    }
    /// <summary>
    /// ���ʧ��
    /// </summary>
    public void PlayerLose()
    {

    }

    public void BattleStart()
    {

    }


}
