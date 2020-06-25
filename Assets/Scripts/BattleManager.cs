using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.AI;

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

    public Vector3 GetRandomLocation()
    {
        NavMeshTriangulation  navMeshData = NavMesh.CalculateTriangulation();

        int t = Random.Range(0, navMeshData.indices.Length - 3);

        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
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
    /// <summary>
    /// �����������꣬���������ϵ�����
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    //public Vector3 SpawnPosition(Vector3 vector)
    //{

    //}
}
