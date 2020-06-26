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

    public void SpawnUnit(int id, Vector3 position)
    {
        GameObject prefab = id < 2000 ? BattleManager.instance.prefabBuilding : BattleManager.instance.prefabSoldier;
        UnitData unitData = Tools.GetUnitData(id);
        for(int i = 0; i < unitData.Number; i++)
        {
            // 获取prefab和data
            // 创建
            GameObject obj = Instantiate(prefab, position, Flod=="Player"? Quaternion.identity: new Quaternion(0,1,0,0), transform);
            obj.SetActive(true);

            Unit unit = obj.GetComponent<Unit>();
            // 给data赋值
            unit.Fold = Flod;
            unit.id = id;
            Childs.Add(unit);
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
        Vector3 position = Base.transform.position + Base.transform.forward * 2f + new Vector3 (Random.Range(-2f, 2f),0,0);
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
