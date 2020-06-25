using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    List<Unit> Childs;
    public string Flod = "Player";

    public List<Unit> Buildings = new List<Unit>();
    public List<Unit> Soldiers = new List<Unit>();

    private void Awake()
    {
        Childs = new List<Unit>();
    }
    public void Spawn(GameObject prefab, Vector3 position,UnitData data =null)
    {
        // 获取prefab和data
        // 创建
        GameObject obj = Instantiate(prefab, position, Quaternion.identity, transform);
        obj.SetActive(true);
        
        Unit unit = obj.GetComponent<Unit>();
        // 给data赋值
        unit.data = data ?? new UnitData();
        unit.Fold = Flod;

        Childs.Add(unit);
    }

    public void Die(Unit unit)
    {
        if (Childs.Contains(unit))
        {
            unit.gameObject.SetActive(false);
            CheckUnitRemain();
        }
        else
        {
            Debug.LogWarning("死的东西不在我管辖之中", gameObject);
        }
    }

    /// <summary>
    /// 检查还剩几个Unit
    /// </summary>
    private void CheckUnitRemain()
    {
        int unitAlive = 0;
        for (int i = 0; i < Childs.Count; i++)
        {
            if (Childs[i].gameObject.activeSelf)
            {
                unitAlive++;
            }
        }
    }

    public void SpawnUnit(int id,Vector3 position)
    {
        for(int i = 0; i < GameManager.instance.unitData.Count; i++)
        {
            if( Convert.ToDouble(GameManager.instance.unitData[i][0]).Equals(id))
            {

            }
        }
        UnitData data = new UnitData();

    }
}
