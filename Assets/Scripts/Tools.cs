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
    
}
