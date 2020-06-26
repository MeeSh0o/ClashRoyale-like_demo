using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager instance;

    public Node[,] nodes = new Node[19,31];

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        foreach (Transform child in transform)
        {
            nodes[Convert.ToInt32(child.position.x) + 9, Convert.ToInt32(child.position.z) + 15] = child.GetComponent<Node>();
        }
    }

    /// <summary>
    /// 给出最近的合法节点
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public Node GetNearestNode(Vector3 vector)
    {
        int x = Convert.ToInt32(vector.x) + 9;
        int z = Convert.ToInt32(vector.z) + 15;

        int xmin, xmax, zmin, zmax;

        if (x < 0)
        {
            xmin = xmax = 0;
        }
        else if (x > 18)
        {
            xmin = xmax = 18;
        }
        else
        {
            xmin = x - 4 >= 0 ? x - 4 : 0;
            xmax = x + 4 <= 18 ? x + 4 : 18;
        }
        if (z < 0)
        {
            zmin = zmax = 0;
        }
        else if (z > 30)
        {
            zmin = zmax = 30;
        }
        else
        {
            zmin = z - 4 >= 0 ? z - 4 : 0;
            zmax = z + 4 <= 30 ? z + 4 : 30;
        }

        Node nearestNode = null;
        float dis = 10000;

        for(int i = xmin; i <= xmax; i++)
        {
            for(int j = zmin; j <= zmax; j++)
            {
                if(nodes[i,j]!= null)
                {
                    float tempDis = Tools.Distance(vector, nodes[i, j].transform.position);
                    if (tempDis < dis)
                    {
                        dis = tempDis;
                        nearestNode = nodes[i, j];
                    }
                }
            }
        }

        return nearestNode;
    }
    /// <summary>
    /// 给出最近的合法节点，可能返回为空
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    public Node GetNearestNode(Vector3 vector,bool isPlayer)
    {
        int x = Convert.ToInt32(vector.x) + 9;
        int z = Convert.ToInt32(vector.z) + 15;

        int xmin, xmax, zmin, zmax;

        if (x < 0)
        {
            xmin = xmax = 0;
        }
        else if (x > 18)
        {
            xmin = xmax = 18;
        }
        else
        {
            xmin = x - 4 >= 0 ? x - 4 : 0;
            xmax = x + 4 <= 18 ? x + 4 : 18;
        }
        if (z < 0)
        {
            zmin = zmax = 0;
        }
        else if (z > 30)
        {
            zmin = zmax = 30;
        }
        else
        {
            zmin = z - 4 >= 0 ? z - 4 : 0;
            zmax = z + 4 <= 30 ? z + 4 : 30;
        }

        Node nearestNode = null;
        float dis = 6;

        for (int i = xmin; i <= xmax; i++)
        {
            for (int j = zmin; j <= zmax; j++)
            {
                if (nodes[i, j] != null)
                {
                    if(isPlayer?!nodes[i, j].isBanedByEnemy: !nodes[i, j].isBanedByPlayer)
                    {
                        float tempDis = Tools.Distance(vector, nodes[i, j].transform.position);
                        if (tempDis < dis)
                        {
                            dis = tempDis;
                            nearestNode = nodes[i, j];
                        }
                    }
                }
            }
        }

        return nearestNode;
    }
}
