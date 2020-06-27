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


    public GameObject prefabBullet;
    public GameObject prefabSoldier;
    public GameObject prefabBuilding;
    //public Material materialPlayer, materialEnemy,materialBoth,materialNeither;

    

    private void Awake()
    {
        instance = this;

        if (Player = GameObject.Find("Player")) { }
        else if (Player = GameObject.Find("Player_AI")) { }
        else if (Player = GameObject.Find("Player_Human")) { }

        if (Enemy = GameObject.Find("Enemy")) { }
        else if (Enemy = GameObject.Find("Enemy_AI")) { }
        else if (Enemy = GameObject.Find("Enemy_Human")) { }

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
        //materialBoth = Resources.Load("Materials/Both") as Material;
        //materialEnemy = Resources.Load("Materials/Enemy") as Material;
        //materialPlayer = Resources.Load("Materials/Player") as Material;
        //materialNeither = Resources.Load("Materials/Neither") as Material;

        prefabBullet = Resources.Load("Bullet") as GameObject;
        prefabSoldier = Resources.Load("Soldier") as GameObject;
        prefabBuilding = Resources.Load("Building") as GameObject;

        GameObject.Find("StartGame").GetComponent<Button>().onClick.AddListener(delegate () { this.BattleInitiate(); });
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
