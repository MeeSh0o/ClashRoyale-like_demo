using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Soldier : Unit
{
    public Transform proxy;
    public NavMeshAgent agent;
    public Rigidbody rb;

    /*------------------------------------分割线------------------------------------------*/

    public override void Awake()
    {
        base.Awake();
        if (proxy == null) 
            proxy = transform.Find("pathfindingProxy");
        if (agent == null)
            agent = proxy.GetComponent<NavMeshAgent>();
        if (obstacle == null)
            obstacle = proxy.GetComponent<NavMeshObstacle>();
        if (rb == null)
            rb = model.GetComponent<Rigidbody>();
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
    public override void Initiate(int id)
    {
        base.Initiate(id);
        agent.speed = data.Speed;
        agent.radius = data.ProxyRadius;
        agent.avoidancePriority = data.Priority;
    }
    public override void Update()
    {
        base.Update();

        // 更新模型以跟上实际位置
        //model.position = Vector3.Lerp(new Vector3(model.position.x, model.position.y, model.position.z), new Vector3(proxy.position.x, model.position.y, proxy.position.z), Time.deltaTime * 3.5f);
        //model.position = proxy.position;
        if(Tools.Distance(model.transform, proxy.transform)>0.1)
            rb.velocity = (new Vector3(proxy.position.x - model.position.x, 0, proxy.position.z - model.position.z) * data.Speed);

        model.rotation = proxy.rotation;
    }

    /*------------------------------------分割线------------------------------------------*/

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
                    float _dis = Tools.Distance(list[i].model.transform, model.transform);
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
        SetTarget(EnemyCheck());
        if(Target == null)
        {
            return;
        }
        else if (!Target.gameObject.activeSelf || Target.state == UnitState.death)
        {
            Target = null;
            return;
        }

        float distance = Tools.Distance(Target.model.transform, proxy.transform);

        if (distance <= data.HitRange)
        {
            // 在攻击范围内就攻击
            SwitchState(UnitState.attack);
        }
        else if (distance > data.ScanRange + Target.data.ProxyRadius && Target.data.TargetType == 1)
        {
            // 如果是士兵且在仇恨范围外就放弃
            Target = null;
            return;
        }
        else
        {
            // 否则追杀
            SwitchState(UnitState.run);
        }
    }

    public override void Run()
    {
        if (Target == null || Target.state == UnitState.death) // 死了就重新找目标
        {
            SwitchState(UnitState.idle);
            return;
        }

        agent.SetDestination(Target.model.position);

        float distance = Tools.Distance(Target.model.transform, proxy.transform);
        if (distance <= data.HitRange) // 追到了就打
        {
            SwitchState(UnitState.attack);
        }
        else if (distance > data.ScanRange + Target.data.ProxyRadius && GetType() == Target.GetType())
        {
            // 如果是士兵且在仇恨范围外就放弃
            SwitchState(UnitState.idle);
            return;
        }
    }
    public override void Attack()
    {  
        if (Target == null || Target.state == UnitState.death) // 死了就重新找目标
        {
            SwitchState(UnitState.idle);
        }
        if (Tools.Distance(Target.model.transform, proxy.transform) > data.HitRange) // 打不着就追
        {
            SwitchState(UnitState.run);
            return;
        }


        proxy.transform.LookAt(Target.model);

        float distance = Tools.Distance(Target.model.transform, proxy.transform);
        if (GetType() == Target.GetType() && distance > data.ScanRange + Target.data.ProxyRadius)
        {
            // 如果对象是士兵，且超出追击范围，就换目标
            SwitchState(UnitState.idle);
            return;
        }
        if (distance > data.HitRange)
        {
            // 如果超出攻击范围，就追
            SwitchState(UnitState.run);
            return;
        }

        if (attackTime >= data.AttackTime)
        {
            // 攻击蓄力完成
            attackTime -= data.AttackTime;

            for (int i = 0; i < data.ShootNum; i++)
            {
                Bullet bullet = Instantiate(DataLoader.instance.GetPrefab("Bullet"), bulletSpawner.position, Quaternion.identity, GameManager.instance.transform).GetComponent<Bullet>();
                Vector3 offset = Random.Range(0, data.AttackOffset) * new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f).normalized;
                bullet.SetBullet(Target.model.gameObject, Fold, data.Atk, data.BulletSpeed, offset);
            }
        }

    }

    //public override void Death()
    //{
    //    base.Death();
    //}

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
                SwitchObstacle(true);
                break;
            case UnitState.run:
                SwitchObstacle(true);
                break;
        }
    }


}
