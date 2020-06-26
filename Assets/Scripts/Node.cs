﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    /// <summary>
    /// Enemy是否被Player禁止在此生成单位
    /// </summary>
    public bool isBanedByPlayer;
    /// <summary>
    /// Player是否被Enemy禁止在此生成单位
    /// </summary>
    public bool isBanedByEnemy;

    public List<Collider> inPlayerBanner = new List<Collider>();
    public List<Collider> inEnemyBanner = new List<Collider>();

    MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponentInChildren<MeshRenderer>();
    }

    private void Update()
    {
        FlushBanned();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!inPlayerBanner.Contains(other))
            {
                inPlayerBanner.Add(other);
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            if (!inEnemyBanner.Contains(other))
            {
                inEnemyBanner.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (inPlayerBanner.Contains(other))
            {
                inPlayerBanner.Remove(other);
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            if (inEnemyBanner.Contains(other))
            {
                inEnemyBanner.Remove(other);
            }
        }
    }

    private void FlushBanned()
    {
        if (inPlayerBanner.Count > 0)
        {
            for (int i = inPlayerBanner.Count - 1; i >= 0; i--)
            {
                if (inPlayerBanner[i] == null) inPlayerBanner.RemoveAt(i);
            }
            isBanedByPlayer = inPlayerBanner.Count > 0 ? true : false;
        }
        else isBanedByPlayer = false;

        if (inEnemyBanner.Count > 0)
        {
            for (int i = inEnemyBanner.Count - 1; i >= 0; i--)
            {
                if (inEnemyBanner[i] == null) inEnemyBanner.RemoveAt(i);
            }
            isBanedByEnemy = inEnemyBanner.Count > 0 ? true : false;
        }
        else isBanedByEnemy = false;

        if (!isBanedByEnemy && !isBanedByPlayer)
        {
            mr.material = BattleManager.instance.materialNeither;
        }
        else if (isBanedByEnemy && isBanedByPlayer)
        {
            mr.material = BattleManager.instance.materialBoth;
        }
        else if (isBanedByPlayer)
        {
            mr.material = BattleManager.instance.mareialPlayer;
        }
        else if (isBanedByEnemy)
        {
            mr.material = BattleManager.instance.mareialEnemy;
        }
    }
}
