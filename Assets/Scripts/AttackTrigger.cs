using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public Unit unit;

    private void Awake()
    {
        if(unit == null)
        unit = transform.parent.parent.GetComponent<Unit>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(unit.Fold))
        {
            unit.FindEnemy(other.transform.parent.GetComponent<Unit>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(unit.Fold))
        {
            unit.EnemyLeave(other.transform.parent.GetComponent<Unit>());
        }
    }
}
