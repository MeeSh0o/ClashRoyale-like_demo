using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public Group ���;
    public Group ����;

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

    public void BattleInitiate()
    {

    }

    /// <summary>
    /// ��������
    /// </summary>
    private void SetDeck()
    {

    }

    public void LoadPrefabs()
    {

    }
}
