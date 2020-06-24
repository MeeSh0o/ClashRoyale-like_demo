using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public Camera cam;

    public Transform model;
    public Transform proxy;

    public NavMeshAgent agent;
    public NavMeshObstacle obstacle;

    /// <summary>
    /// 本单位参数
    /// </summary>
    public UnitData data = null;

    /// <summary>
    /// 状态
    /// </summary>
    public UnitState state;

    /// <summary>
    /// 当前仇恨对象
    /// </summary>
    public Unit target 
    {
        get
        {
            if(_target == null)
            {
                SetTarget();
            }
            return _target;
        }
        set
        {
            _target = value;
            Debug.LogWarning("外部设置单位目标警告", gameObject);
        }
    }
    private Unit _target;
    /// <summary>
    /// 登记过的仇恨对象列表
    /// </summary>
    List<Unit> EnemiesInField;
    /// <summary>
    /// 查看阵营
    /// </summary>
    public string Fold
    {
        get
        {
            return model.tag;
        }
        set
        {
            model.tag = value;
        }
    }
    private void Awake()
    {
        agent = proxy.GetComponent<NavMeshAgent>();
        obstacle = proxy.GetComponent<NavMeshObstacle>();
        EnemiesInField = new List<Unit>();
        this.data = new UnitData();
    }

    /// <summary>
    /// 激活,需要限定激活范围
    /// </summary>
    public void Spawn(Vector3 position, UnitData data)
    {
        proxy.position = position;
        model.position = position;
        this.data = new UnitData(data);
    }

    public enum UnitState
    {
        idle,
        run,
        attack,
        death
    }

    void Update()
    {
        switch (state)
        {
            case UnitState.attack:
                Attack();
                break;
            case UnitState.death:
                Death();
                break;
            case UnitState.idle:
                Idle();
                break;
            case UnitState.run:
                Run();
                break;
        }

        // 更新模型以跟上实际位置
        model.position = Vector3.Lerp(new Vector3(model.position.x, model.position.y, model.position.z), new Vector3(proxy.position.x, model.position.y, proxy.position.z), Time.deltaTime * 3.5f);
        model.rotation = proxy.rotation;
    }

    /// <summary>
    /// 静止状态，作用是找人
    /// </summary>
    public virtual void Idle()
    {
        SetTarget(EnemyCheck());
        if (target)
        {
            SwitchState(UnitState.run);
        }
    }
    /// <summary>
    /// 攻击状态，作用是站着不动打人
    /// </summary>
    public virtual void Attack()
    {
        if(target == null || !target.gameObject.activeSelf || target.state == UnitState.death) // 死了就重新找目标
        {
            SwitchState(UnitState.idle);
        }
        if (Tools.Distance(target.model.transform, model.transform) > data.HitRange) // 打不着就追
        {
            SwitchState(UnitState.run);
        }
    }
    /// <summary>
    /// 移动状态，作用是移动或者追人，追人需判断是否可到达，不可到达则放弃
    /// </summary>
    public virtual void Run()
    {
        agent.SetDestination(target.model.position);
        if(Tools.Distance(target.model.transform,model.transform) <= data.HitRange) // 追到了就打
        {
            SwitchState(UnitState.attack);
        }
        else if(Tools.Distance(target.model.transform, model.transform) <= data.ScanRange) // 追不到就放弃
        {
            //SwitchState(UnitState.idle);
        }
    }
    /// <summary>
    /// 非存活状态，出现在放置角色和角色死亡后
    /// </summary>
    public virtual void Death()
    {

    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="state"></param>
    public void SwitchState(UnitState state)
    {
        switch (state)
        {
            case UnitState.attack:
                SwitchObstacle(false);
                this.state = UnitState.attack;
                break;
            case UnitState.death:
                SwitchObstacle(false);
                this.state = UnitState.death;
                break;
            case UnitState.idle:
                SwitchObstacle(false);
                this.state = UnitState.idle;
                break;
            case UnitState.run:
                SwitchObstacle(true);
                this.state = UnitState.run;
                break;
        }
    }

    /// <summary>
    /// 切换Obstacle
    /// </summary>
    /// <param name="isAgent"></param>
    public void SwitchObstacle(bool isAgent)
    {
        agent.enabled = isAgent;
        obstacle.enabled = !isAgent;
    }
    /// <summary>
    /// 添加敌人到探测列表中
    /// </summary>
    /// <param name="enemy"></param>
    public void FindEnemy(Unit enemy)
    {
        if (!EnemiesInField.Contains(enemy))
        {
            EnemiesInField.Add(enemy);
        }
    }
    /// <summary>
    /// 更新记录的敌人信息
    /// </summary>
    private Unit EnemyCheck()
    {
        Unit nearestOne = null;
        float dist = data.ScanRange;
        if (EnemiesInField.Count > 0)
        {
            for (int i = 0; i <EnemiesInField.Count; i++)
            {
                Unit temp = EnemiesInField[i];

                // 剔除已经死掉的
                if (target == null || !temp.gameObject.activeSelf) 
                {
                    EnemiesInField.Remove(temp);
                    continue;
                }
                // 跳过死亡的
                if(temp.state == UnitState.death)
                {
                    continue;
                }
                
                float tdis = Tools.Distance(temp.transform, model.transform);
                
                // 找到最近的那个
                if(tdis <= dist)
                {
                    nearestOne = temp;
                    dist = tdis;
                }
            }
        }
        return nearestOne;
    }

    public void SetTarget(Unit target = null) 
    {
        if (target != null) _target = target;
        else
        {
            // 请求从建筑物中选取
            //临时：
            switch (Fold)
            {
                case "Player":
                    _target = GameObject.Find("帅阵").GetComponent<Unit>();
                    break;
                default:
                    _target = GameObject.Find("将台").GetComponent<Unit>();
                    break;
            }
        }
    }
}




/*
 * 激活后，默认处于idle
 *      idle内容：
 *          查找目标
 *          找到最近的目标，指定目标为前进终点，切换到run
 *      run内容：
 *          向目标移动，如果仇恨范围内，敌人仇恨值大于目标权重，指定敌人为前进终点
 *          如果攻击范围内出现敌人，按仇恨权重选择最高的一个作为攻击对象，停止移动，切换到attack
 *      attack内容：
 *          持续发射攻击
 *          如果攻击对象死亡，切换到idle
 *      death：
 *          如果血量归零，切换到death，被pool回收
 * 
 */

    /// <summary>
    /// 单位数据
    /// </summary>
public class UnitData
{
    public int ID;
    public int Lev;
    public float Size;
    public float Speed;
    public float Height;
    public int Priority;
    public float HitRange;
    public float ScanRange;

    public UnitData()
    {
        ID = 0;
        Lev = 0;
        Size = 1;
        Speed = 0;
        Height = 0;
        Priority = 50;
        HitRange = 1.5f;
        ScanRange = 1000;
        Debug.LogWarning("有新的空角色数据被创建");
    }

    public UnitData(UnitData data)
    {
        ID = data.ID;
        Lev = data.Lev;
        Size = data.Size;
        Speed = data.Speed;
        Height = data.Height;
        Priority = data.Priority;
        HitRange = data.HitRange;
        ScanRange = data.ScanRange;
    }
}

//抄的有关RVO代码
public class enemyMovement : MonoBehaviour
{
    public Transform player;
    public Transform model;
    public Transform proxy;
    NavMeshAgent agent;
    NavMeshObstacle obstacle;
    void Start()
    {
        agent = proxy.GetComponent<NavMeshAgent>();
        obstacle = proxy.GetComponent<NavMeshObstacle>();
    }
    void Update()
    {
        // Test if the distance between the agent (which is now the proxy) and the player 
        // is less than the attack range (or the stoppingDistance parameter)
        if ((player.position - proxy.position).sqrMagnitude < Mathf.Pow(agent.stoppingDistance, 2))
        {
            // If the agent is in attack range, become an obstacle and 
            // disable the NavMeshAgent component 
            obstacle.enabled = true;
            agent.enabled = false;
        }
        else
        {
            // If we are not in range, become an agent again 
            obstacle.enabled = false;
            agent.enabled = true;
            // And move to the player's position 
            agent.destination = player.position;
        }
        model.position = Vector3.Lerp(model.position, proxy.position, Time.deltaTime * 2);
        model.rotation = proxy.rotation;
    }
}