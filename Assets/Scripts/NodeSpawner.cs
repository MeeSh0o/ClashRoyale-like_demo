using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NodeSpawner : MonoBehaviour
{
    public GameObject nodePrefab;

    public void Update()
    {
        Spawn();
        Debug.LogError("");
        this.enabled = false;
    }


    public void Spawn()
    {
        for (int i = -9; i < 10; i++)
        {
            for (int j = -15; j < 16; j++)
            {
                GameObject node = Instantiate(nodePrefab, new Vector3(i, 0, j), Quaternion.identity, transform);
                node.name = i.ToString() + "," + j.ToString();
            }
        }
    }
}
