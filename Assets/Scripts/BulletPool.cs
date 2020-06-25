using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;
    private void Awake()
    {
        instance = this;
    }
}
