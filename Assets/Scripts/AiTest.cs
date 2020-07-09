using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiTest : MonoBehaviour
{
    public UnitState state;
    public float AttackTime = 2;
    public float attackTime = 0;

    public GameObject Target;

    public List<GameObject> EnemiesInField = new List<GameObject>();


    public NavMeshAgent agent;
    public NavMeshObstacle obstacle;


    public string Fold
    {
        get
        {
            return tag;
        }
        set
        {
            tag = value;
        }
    }

    public enum UnitState
    {
        idle,
        run,
        attack,
        death
    }

    /*------------------------------------分割线------------------------------------------*/

    private void Awake()
    {

    }

    private void Start()
    {

    }


    private void Update()
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
        //if(attackTime<AttackTime)
        //    attackTime += Time.deltaTime;

    }


    /*------------------------------------分割线------------------------------------------*/

    public void SwitchObstacle(bool isAgent)
    {
        if (isAgent)
        {
            obstacle.enabled = !isAgent;
            agent.enabled = isAgent;
            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true;
            agent.enabled = isAgent;
            obstacle.enabled = !isAgent;
        }

    }

    public virtual void SwitchState(UnitState state)
    {
        switch (state)
        {
            case UnitState.attack:
                this.state = UnitState.attack;              
                SwitchObstacle(false);
                break;
            case UnitState.death:
                this.state = UnitState.death;
                SwitchObstacle(false);
                break;
            case UnitState.idle:
                this.state = UnitState.idle;
                SwitchObstacle(false);
                break;
            case UnitState.run:
                this.state = UnitState.run;
                SwitchObstacle(true);
                break;
        }
    }
    public virtual void SetTarget(GameObject target = null)
    {
        if (target != null)
        {
            Target = target;
        }
        else
        {
            // 如果范围内搜索不到敌人，打对方建筑物
        }
    }

    public GameObject EnemyCheck()
    {
        GameObject nearestOne = null;
        //float dist = data.ScanRange;
        float dist = 100;

        for (int i = EnemiesInField.Count - 1; i >= 0; i--)
        {
            GameObject temp = EnemiesInField[i];

            if(temp == null)
            {
                // 找到空的就删了
                EnemiesInField.RemoveAt(i);
                continue;
            }

            // 跳过死亡的
            if (temp.GetComponent<AiTest>().state == UnitState.death)
            {
                continue;
            }

            float tdis = Tools.Distance(temp.transform, transform);

            // 找到最近的那个
            if (tdis <= dist)
            {
                nearestOne = temp;
                dist = tdis;
            }

        }

        return nearestOne;
    }

    public virtual void Idle()
    {
        //SetTarget(EnemyCheck());

        if (Target != null)
        {
            if ((Target.CompareTag("Player") || Target.CompareTag("Enemy")) && Tools.Distance(Target.transform, transform) > 5 + 0/*对象半径*/) // 这里是判断目标超出仇恨范围
            {
                //如果它超出范围,原则上应该调用不到这里的代码，只有测试才会触发,这里存在一个探测对象在范围+半径时就会进入碰撞的问题
                Target = null;
                Debug.LogWarning("敌人超出范围，只有测试会触发才合理", gameObject);
                return;
            }

            float distance = Tools.Distance(Target.transform, transform);
            if (distance<= 3)
            {
                // 在攻击范围内就攻击
                SwitchState(UnitState.attack);
            }
            else if(distance > 5 && (Target.CompareTag("Player") || Target.CompareTag("Enemy")))
            {
                // 如果是士兵且在仇恨范围外就放弃
                return;
            }
            else
            {
                // 否则追杀
                SwitchState(UnitState.run);
            }
            
        }
    }

    public virtual void Run()
    {
        //if (Target == null || !Target.activeSelf /*|| Target.GetComponent<AiTest>().state == UnitState.death*/) 
        //{
        //    // 死了就重新找目标
        //    SwitchState(UnitState.idle);
        //    return;
        //}
        
        agent.SetDestination(Target.transform.position);

        float distance = Tools.Distance(Target.transform, transform);
        if (distance <= 3) 
        {
            // 追到了就打
            SwitchState(UnitState.attack);       
            return;
        }
        //else if (distance >= 5 && (Target.CompareTag("Player")|| Target.CompareTag("Enemy"))) 
        //{
        //    // 追不到就放弃
        //    Debug.Log("放弃");
        //    SwitchState(UnitState.idle);
        //    return;
        //}

    }

    public virtual void Attack()
    {
        //if(Target == null || !Target.activeSelf /*|| Target.GetComponent<AiTest>().state == UnitState.death*/)
        //{
        //    // 如果死了，就换目标
        //    SwitchState(UnitState.idle);
        //}

        float distance = Tools.Distance(Target.transform, transform);
        if (distance > 3 && distance <= 5)
        {
            // 如果超出攻击范围，就追
            SwitchState(UnitState.run);
        }
        if (distance > 5)
        {
            // 如果超出追击范围，就换目标
            SwitchState(UnitState.idle);
        }
        // 到这应该是在范围内，就普通攻击它
        // 暂不做抬手前摇
        //Debug.Log(name + "攻击", gameObject);
    }


    public virtual void Death()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 添加敌人到探测列表中
    /// </summary>
    /// <param name="enemy"></param>
    public void FindEnemy(GameObject enemy)
    {
        //Debug.Log("添加敌人" + gameObject + enemy);

        EnemiesInField.Add(enemy);

        if (state == UnitState.run)
            SwitchState(UnitState.idle);
    }

    /// <summary>
    /// 将敌人从探测列表移除
    /// </summary>
    /// <param name="enemy"></param>
    public void EnemyLeave(GameObject enemy)
    {
        //Debug.Log("移除敌人" + gameObject + enemy);

        EnemiesInField.Remove(enemy);
    }
}
