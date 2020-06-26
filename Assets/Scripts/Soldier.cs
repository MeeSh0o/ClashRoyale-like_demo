using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : Unit
{
    public Transform proxy;
    public NavMeshAgent agent;
    public override void Awake()
    {
        base.Awake();
        if (proxy == null) 
            proxy = transform.Find("pathfindingProxy");
        if (agent == null)
            agent = proxy.GetComponent<NavMeshAgent>();
        if (obstacle == null)
            obstacle = proxy.GetComponent<NavMeshObstacle>();
    }

    public override void Start()
    {
        base.Start();
        if (!group.Soldiers.Contains(this))
        {
            group.Soldiers.Add(this);
        }
        if (!group.Childs.Contains(this))
        {
            group.Childs.Add(this);
        }
    }

    public override void Update()
    {
        base.Update();

        // 更新模型以跟上实际位置
        model.position = Vector3.Lerp(new Vector3(model.position.x, model.position.y, model.position.z), new Vector3(proxy.position.x, model.position.y, proxy.position.z), Time.deltaTime * 3.5f);
        model.rotation = proxy.rotation;
    }

    public override void SetTarget(Unit target = null)
    {
        base.SetTarget(target);

        // 找最近的建筑物
        if (Target == null)
        {
            List<Unit> list = Fold == "Enemy" ? BattleManager.instance.PlayerBuilding : BattleManager.instance.EnemyBuilding;

            float dis = 1000;
            Unit clost = null;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].enabled)
                {
                    float _dis = Tools.Distance(list[i].transform, transform);
                    if (_dis < dis)
                    {
                        dis = _dis;
                        clost = list[i];
                    }
                }
            }
            Target = clost;
        }
    }

    public override void Idle()
    {
        base.Idle();
        if (Target != null)
        {
            SwitchState(UnitState.run);
        }
    }

    public override void Run()
    {
        base.Run();

        if (Target == null || Target.state == UnitState.death) // 死了就重新找目标
        {
            SwitchState(UnitState.idle);
            return;
        }
        agent.SetDestination(Target.model.position);
        if (Tools.Distance(Target.model.transform, model.transform) <= data.HitRange) // 追到了就打
        {
            SwitchState(UnitState.attack);
        }
        else if (GetType() == Target.GetType() && Tools.Distance(Target.model.transform, model.transform) >= data.ScanRange) // 追不到就放弃
        {
            SwitchState(UnitState.idle);
        }
    }
    public override void Attack()
    {  
        if (Target == null || Target.state == UnitState.death) // 死了就重新找目标
        {
            SwitchState(UnitState.idle);
        }
        if (Tools.Distance(Target.model.transform, model.transform) > data.HitRange) // 打不着就追
        {
            SwitchState(UnitState.run);
            return;
        }
        base.Attack();
    }

    public override void Death()
    {
        base.Death();
    }

    /// <summary>
    /// 切换Obstacle
    /// </summary>
    /// <param name="isAgent"></param>
    public void SwitchObstacle(bool isAgent)
    {
        if (isAgent)
        {
            obstacle.enabled = !isAgent;
            agent.enabled = isAgent;
        }
        else
        {
            agent.enabled = isAgent;
            obstacle.enabled = !isAgent;
        }

    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="state"></param>
    public override void SwitchState(UnitState state)
    {
        base.SwitchState(state);
        switch (state)
        {
            case UnitState.attack:
                SwitchObstacle(false);
                break;
            case UnitState.death:
                SwitchObstacle(false);
                break;
            case UnitState.idle:
                SwitchObstacle(false);
                break;
            case UnitState.run:
                SwitchObstacle(true);
                break;
        }
    }


}
