using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public Unit unit;

    private void Awake()
    {
        unit = transform.parent.parent.GetComponent<Unit>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(unit.Fold))
        {
            unit.FindEnemy(other.transform.parent.GetComponent<Unit>());
        }
    }
}
