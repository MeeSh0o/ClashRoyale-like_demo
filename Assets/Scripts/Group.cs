using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Group : MonoBehaviour
{
    public List<Unit> Childs;
    public string Fold = "Player";

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
            GameObject obj = Instantiate(prefab, position, Fold=="Player"? Quaternion.identity: new Quaternion(0,1,0,0), transform);
            Unit unit = obj.GetComponent<Unit>();
            unit.Initiate(id);
            unit.Fold = Fold;
            Childs.Add(unit);

            obj.SetActive(true);    
        }
        
    }


    public void Die(Unit unit)
    {
        if (Childs.Contains(unit))
        {
            //unit.gameObject.SetActive(false);
            Destroy(unit.gameObject);
            Childs.Remove(unit);
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
        if(Base == null)
        {
            BattleManager.instance.Lose(Fold);
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
        for(int i = 0; i < 1000; i++)
        {
            SpawnTest();
            yield return new WaitForSeconds(4);
        }
    }

    public void SpawnTest2()
    {
        StartCoroutine(SpawnTest1());
    }
}
