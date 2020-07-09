using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITrigger : MonoBehaviour
{
    public AiTest unit;

    private void Awake()
    {
        if (unit == null)
            unit = transform.parent.GetComponent<AiTest>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(unit.Fold == "Player" ? "Enemy" : "Player"))
        {
            unit.FindEnemy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(unit.Fold == "Player" ? "Enemy" : "Player"))
        {
            unit.EnemyLeave(other.gameObject);
        }
    }
}
