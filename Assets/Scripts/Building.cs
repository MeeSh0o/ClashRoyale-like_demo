using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Unit
{
    //public override void Awake()
    //{
    //    base.Awake();
    //}

    public override void Start()
    {
        base.Start();
        if(Fold == "Player")
        {
            BattleManager.instance.PlayerBuilding.Add(this);
        }
        else
        {
            BattleManager.instance.EnemyBuilding.Add(this);
        }
        if (!group.Buildings.Contains(this))
        {
            group.Buildings.Add(this);
        }
        if (!group.Childs.Contains(this))
        {
            group.Childs.Add(this);
        }
    }

    //public override void Idle()
    //{
    //    base.Idle();
    //}

    //public override void Attack()
    //{
    //    base.Attack();
    //}

    public override void Death()
    {
        base.Death();

        obstacle.enabled = false;
    }

    private void OnDestroy()
    {
        List<Unit> list = Fold == "Player" ? BattleManager.instance.PlayerBuilding : BattleManager.instance.EnemyBuilding;
        if (list != null)  list.Remove(this);
    }
}
