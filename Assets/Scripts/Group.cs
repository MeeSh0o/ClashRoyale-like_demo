using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Group : MonoBehaviour
{
    public List<Unit> Childs;
    public string Flod = "Player";

    public List<Unit> Buildings = new List<Unit>();
    public List<Unit> Soldiers = new List<Unit>();
    public Unit Base;

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
        unit.Hp = unit.data.Hp;

        Childs.Add(unit);
    }
    public void SpawnUnit(int id, Vector3 position)
    {
        GameObject prefab = id < 2000 ? BattleManager.instance.prefabBuilding : BattleManager.instance.prefabSoldier;
        UnitData unitData = Tools.GetUnitData(id);
        for(int i = 0; i < unitData.Number; i++)
        {
            // TODO 需要计算一次position 的最近有效位置
            Spawn(prefab, position, unitData);
        }
        
    }


    public void Die(Unit unit)
    {
        if (Childs.Contains(unit))
        {
            //unit.gameObject.SetActive(false);
            Destroy(unit.gameObject);
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
    private int CheckUnitRemain()
    {
        int unitAlive = 0;
        for (int i = 0; i < Childs.Count; i++)
        {
            if (Childs[i] != null && Childs[i].gameObject.activeSelf)
            {
                unitAlive++;
            }
        }
        return unitAlive;
    }

    public void SpawnTest()
    {
        int[] array = { 0, 1001, 1002, 2001, 2002, 2003, 2004, 2005 };
        int id = Random.Range(3, 7);
        Vector3 position = Base.transform.position + Base.transform.forward * 2f + new Vector3 (Random.Range(-2, 2),0,0);
        SpawnUnit(array[id], position);
    }

    public  IEnumerator SpawnTest1()
    {
        yield return new WaitForSeconds(1);
        for(int i = 0; i < 5; i++)
        {
            SpawnTest();
            yield return new WaitForSeconds(3);
        }
    }

    public void SpawnTest2()
    {
        StartCoroutine(SpawnTest1());
    }
}
