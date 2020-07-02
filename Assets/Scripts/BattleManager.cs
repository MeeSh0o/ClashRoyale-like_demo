using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public GameObject Player, Enemy;

    public Group playerGroup;
    public Group EnemyGroup;
    public Controller PlayerController;
    public Controller EnemyController;

    public List<Unit> PlayerBuilding = new List<Unit>();
    public List<Unit> EnemyBuilding = new List<Unit>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if ((Player = GameObject.Find("Player")) || (Player = GameObject.Find("Player_AI")) || (Player = GameObject.Find("Player_Human"))) { }
        if ((Enemy = GameObject.Find("Enemy")) || (Enemy = GameObject.Find("Enemy_AI")) || (Enemy = GameObject.Find("Enemy_Human"))) { }

        if (playerGroup == null)
        {
            playerGroup = Player.GetComponent<Group>();
        }
        if (EnemyGroup == null)
        {
            EnemyGroup = Enemy.GetComponent<Group>();
        }
        if (PlayerController == null)
        {
            PlayerController = Player.GetComponent<Controller>();
        }
        if (EnemyController == null)
        {
            EnemyController = Enemy.GetComponent<Controller>();
        }
        playerGroup.Fold = PlayerController.Fold = "Player";
        EnemyGroup.Fold = EnemyController.Fold = "Enemy";
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
    /// Íæ¼ÒÊ¤Àû
    /// </summary>
    public void PlayerWin()
    {

    }
    /// <summary>
    /// Íæ¼ÒÊ§°Ü
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

    public void Lose(string fold)
    {
        switch (fold)
        {
            case "Player":
                PlayerWin();
                break;
            case "Enemy":
                PlayerLose();
                break;
            default:
                break;
        }
    }
}
