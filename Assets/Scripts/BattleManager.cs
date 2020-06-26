using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.AI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public Group playerGroup;
    public Group EnemyGroup;
    public Controller PlayerController;
    public Controller EnemyController;

    public List<Unit> PlayerBuilding = new List<Unit>();
    public List<Unit> EnemyBuilding = new List<Unit>();


    public GameObject prefabBullet;
    public GameObject prefabSoldier;
    public GameObject prefabBuilding;
    public Material mareialPlayer, mareialEnemy,materialBoth,materialNeither;

    private void Awake()
    {
        instance = this;
    }

    public Vector3 GetRandomLocation()
    {
        NavMeshTriangulation  navMeshData = NavMesh.CalculateTriangulation();

        int t = Random.Range(0, navMeshData.indices.Length - 3);

        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
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
        PlayerController.SetDeck(GameManager.instance.PlayerDeck);
        EnemyController.SetDeck(GameManager.instance.EnemyDeck);
        StartCoroutine(PlayerController.PrepareCard());
        StartCoroutine(EnemyController.PrepareCard());
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
