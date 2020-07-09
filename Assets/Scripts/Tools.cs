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
    public static float Distance(Vector3 v1, Vector3 v2)
    {
        Vector3 sub = v1 - v2;
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
        for (int i = 0; i < DataLoader.instance.unitData.Count; i++)
        {
            if (String2Int(DataLoader.instance.unitData[i][0]).Equals(id))
            {
                dataIndex = i;
                break;
            }
        }
        if (dataIndex == 0)
        {
            Debug.LogWarning("数据ID为0或查找失败");
        }
        List<string> data = DataLoader.instance.unitData[dataIndex];
        UnitData temp = new UnitData
        {
            ID = String2Int(data[0]),
            PrepareTime = String2Float(data[1]),
            Name = data[2],
            Hp = String2Int(data[3]),
            Atk = String2Int(data[4]),
            Number = String2Int(data[5]),
            Model = String2Int(data[6]),
            Speed = String2Float(data[7]),
            ProxyRadius = String2Float(data[8]),
            Priority = String2Int(data[9]),
            HitRange = String2Float(data[10]),
            ScanRange = String2Float(data[11]),
            AttackTime = String2Float(data[12]),
            AttackOffset = String2Float(data[13]),
            ShootNum = String2Float(data[14]),
            BulletSpeed = String2Float(data[15]),
            AttackType = String2Int(data[16]),
            TargetType = String2Int(data[17]),
            IsGround = String2Int(data[18])
        };
        return temp;
    }
    /// <summary>
    /// 矩阵乘法，AI专用
    /// </summary>
    /// <param name="a1"></param>
    /// <param name="a2"></param>
    /// <returns></returns>
    public static float MatrixMulty(List<int> a1, List<float> a2)
    {
        if (a1.Count == a2.Count)
        {
            float score = 0;
            for(int i = 0; i < a1.Count; i++)
            {
                score += a1[i] * a2[i];
            }

            return score;
        }
        else return 0;
    }
    /// <summary>
    /// 矩阵乘法，AI专用
    /// </summary>
    /// <param name="a1"></param>
    /// <param name="a2"></param>
    /// <returns></returns>
    public static float MatrixMulty(List<float> a1, List<float> a2)
    {
        if (a1.Count == a2.Count)
        {
            float score = 0;
            for (int i = 0; i < a1.Count; i++)
            {
                score += a1[i] * a2[i];
            }

            return score;
        }
        else return 0;
    }
}
