using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    /// <summary>
    /// 计算两个Transform的水平距离
    /// </summary>
    /// <param name="t1"></param>
    /// <param name="t2"></param>
    /// <returns></returns>
    public static float Distance(Transform t1, Transform t2)
    {
        Vector3 sub = new Vector3(t1.position.x, 0, t1.position.z) - new Vector3(t2.position.x, 0, t2.position.z);
        return sub.magnitude;
    }
    /// <summary>
    /// 转换表格字符数据为int
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int String2Int(string str)
    {
        return (int)Convert.ToDouble(str);
    }
    /// <summary>
    /// 转换表格字符数据为float
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static float String2Float(string str)
    {
        return (float)Convert.ToDouble(str);
    }

    public static UnitData GetUnitData(int id)
    {
        int dataIndex = 0;
        for (int i = 0; i < GameManager.instance.unitData.Count; i++)
        {
            if (String2Int(GameManager.instance.unitData[i][0]).Equals(id))
            {
                dataIndex = i;
                break;
            }
        }
        UnitData data = new UnitData
        {
            ID = String2Int(GameManager.instance.unitData[dataIndex][0]),
            PrepareTime = String2Float(GameManager.instance.unitData[dataIndex][1]),
            Name = GameManager.instance.unitData[dataIndex][2],
            Hp = String2Int(GameManager.instance.unitData[dataIndex][3]),
            Atk = String2Int(GameManager.instance.unitData[dataIndex][4]),
            Number = String2Int(GameManager.instance.unitData[dataIndex][5]),
            Size = String2Float(GameManager.instance.unitData[dataIndex][6]),
            Speed = String2Float(GameManager.instance.unitData[dataIndex][7]),
            Height = String2Float(GameManager.instance.unitData[dataIndex][8]),
            Priority = String2Int(GameManager.instance.unitData[dataIndex][9]),
            HitRange = String2Float(GameManager.instance.unitData[dataIndex][10]),
            ScanRange = String2Float(GameManager.instance.unitData[dataIndex][11]),
            AttackTime = String2Float(GameManager.instance.unitData[dataIndex][12]),
            AttackOffset = String2Float(GameManager.instance.unitData[dataIndex][13]),
            ShootNum = String2Float(GameManager.instance.unitData[dataIndex][14]),
            BulletSpeed = String2Float(GameManager.instance.unitData[dataIndex][15]),

        };
        if(dataIndex == 0)
        {
            Debug.LogWarning("数据ID为0或查找失败");
        }
        return data;
    }
}
