using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public int Hp;
    public Transform HpBar;

    public Transform model;
    public Group group;
    public NavMeshObstacle obstacle;
    public AttackTrigger attackTrigger;
    public Transform bulletSpawner;

    /// <summary>
    /// 本单位参数
    /// </summary>
    public UnitData data;
    public int id;
    private bool initiated = false;
    /// <summary>
    /// 状态
    /// </summary>
    public UnitState state;
   
    /// <summary>
    /// 当前仇恨对象
    /// </summary>
    public Unit Target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
        }
    }
    [SerializeField]
    private Unit _target;
    /// <summary>
    /// 登记过的仇恨对象列表
    /// </summary>
    public List<Unit> EnemiesInField = new List<Unit>();
    ///// <summary>
    ///// 被登记过的仇恨对象列表
    ///// </summary>
    //public List<Unit> InEnemiesField = new List<Unit>();

    /// <summary>
    /// 阵营
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

    public enum UnitState
    {
        idle,
        run,
        attack,
        death
    }

    public float attackTime = 0;

    /*------------------------------------分割线------------------------------------------*/

    public virtual void Awake()
    {
        EnemiesInField = new List<Unit>();

        if (model == null)
            model = transform.Find("model");

        if (attackTrigger == null)
            attackTrigger = transform.Find("AttackTrigger").GetComponent<AttackTrigger>();

        if(HpBar == null)
        {
            HpBar = model.transform.Find("HP");
        }

    }

    public virtual void Start()
    {
        if (group == null)
        {
            if (GameObject.Find(Fold)) { group = GameObject.Find(Fold).GetComponent<Group>(); }
            //else if (GameObject.Find(Fold + "_AI")) { group = GameObject.Find(Fold + "_AI").GetComponent<Group>(); }
            //else if (GameObject.Find(Fold + "_Human")) { group = GameObject.Find(Fold + "_Human").GetComponent<Group>(); }
        }
        if (!initiated) Initiate(id);
        HpBarManager.instance.CallABar(this);
    }

    public virtual void Initiate(int id)
    {
        data = Tools.GetUnitData(id);
        this.id = id;
        Hp = data.Hp;
        gameObject.name = data.Name;
        GameObject model = Instantiate(DataLoader.instance.GetModelPrefab(data.Model), this.model.transform);
        model.name = "模型";
        attackTrigger.GetComponent<SphereCollider>().radius = data.ScanRange;

        if (bulletSpawner == null)
            bulletSpawner = model.transform.Find("BulletSpawner");

        for (int i = 0; i < model.transform.childCount; i++)
        {
            MeshRenderer meshR = model.transform.GetChild(i).GetComponent<MeshRenderer>();
            if(meshR) meshR.material = DataLoader.instance.GetMaterial(Fold);
        }

        initiated = true;
    }

    public virtual void Update()
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
        if (attackTime < data.AttackTime)
            attackTime += Time.deltaTime;
    }

    /*------------------------------------分割线------------------------------------------*/

    /// <summary>
    /// 静止状态，作用是找人
    /// </summary>
    public virtual void Idle()
    {
        SetTarget(EnemyCheck());
        if (Target == null) return;
        if (Tools.Distance(Target.model.transform, model.transform) <= data.HitRange) SwitchState(UnitState.attack);
  
    }

    /// <summary>
    /// 移动状态，作用是移动或者追人，追人需判断是否可到达，不可到达则放弃
    /// </summary>
    public virtual void Run()
    {

    }

    /// <summary>
    /// 攻击状态，作用是站着不动打人
    /// </summary>
    public virtual void Attack()
    {
        if (Target == null)
        {
            // 如果死了，就换目标
            SwitchState(UnitState.idle);
            return;
        }
        else if (!Target.gameObject.activeSelf || Target.state == UnitState.death)
        {
            // 如果死了，就换目标
            SwitchState(UnitState.idle);
            return;
        }

        float distance = Tools.Distance(Target.model.transform, model.transform);
        //Debug.LogError(distance + "  " + data.HitRange, gameObject);


        if(distance<= data.HitRange)
        {
            if (attackTime >= data.AttackTime)
            {
                attackTime -= data.AttackTime;

                for (int i = 0; i < data.ShootNum; i++)
                {
                    Bullet bullet = Instantiate(DataLoader.instance.GetPrefab("Bullet"), bulletSpawner.position, Quaternion.identity, GameManager.instance.transform).GetComponent<Bullet>();
                    Vector3 offset = Random.Range(0, data.AttackOffset) * new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f).normalized;
                    bullet.SetBullet(Target.model.gameObject, Fold, data.Atk, data.BulletSpeed, offset);
                }
            }
        }     
    }

    /// <summary>
    /// 非存活状态，出现在角色死亡后
    /// </summary>
    public virtual void Death()
    {
        if (group != null)
        {
            group.Die(this);
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="state"></param>
    public virtual void SwitchState(UnitState state)
    {
        switch (state)
        {
            case UnitState.attack:
                this.state = UnitState.attack;
                break;
            case UnitState.death:
                this.state = UnitState.death;
                break;
            case UnitState.idle:
                this.state = UnitState.idle;
                break;
            case UnitState.run:
                this.state = UnitState.run;
                break;
        }
    }



    /// <summary>
    /// 添加敌人到探测列表中
    /// </summary>
    /// <param name="enemy"></param>
    public void FindEnemy(Unit enemy)
    {
        //Debug.Log("添加敌人" + gameObject + enemy);

        EnemiesInField.Add(enemy);

        if(state == UnitState.run)
            SwitchState(UnitState.idle);
    }

    /// <summary>
    /// 将敌人从探测列表移除
    /// </summary>
    /// <param name="enemy"></param>
    public void EnemyLeave(Unit enemy)
    {
        //Debug.Log("移除敌人" + gameObject + enemy);

        EnemiesInField.Remove(enemy);
    }
    /// <summary>
    /// 更新记录的敌人信息
    /// </summary>
    public Unit EnemyCheck()
    {
        Unit nearestOne = null;
        //float dist = data.ScanRange;
        float dist = 100;

        for (int i = EnemiesInField.Count - 1; i >= 0; i--)
        {
            Unit temp = EnemiesInField[i];

            if (temp == null)
            {
                // 找到空的就删了
                EnemiesInField.RemoveAt(i);
                continue;
            }

            // 跳过死亡的
            if (temp.state == UnitState.death)
            {
                EnemiesInField.RemoveAt(i);
                continue;
            }

            float tdis = Tools.Distance(temp.transform, model.transform);

            // 找到最近的那个
            if (tdis <= dist)
            {
                nearestOne = temp;
                dist = tdis;
            }

        }

        return nearestOne;
    }

    public virtual void SetTarget(Unit target = null)
    {
        if (target != null)
        {
            Target = target;
        }
    }

    public virtual void Hit(int damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            SwitchState(UnitState.death);
        }
    }


}

/// <summary>
/// 单位数据
/// </summary>
public class UnitData
{
    public int ID;
    public float PrepareTime;
    public string Name;
    public int Hp;
    public int Atk;
    public int Number;
    public int Model;
    public float Speed;
    public float ProxyRadius;
    public int Priority;
    public float HitRange;
    public float ScanRange;
    public float AttackTime;
    public float AttackOffset;
    public float ShootNum;
    public float BulletSpeed;
    public int AttackType;
    public int TargetType;
    public int IsGround;

    public UnitData()
    {
        ID = 0;
        PrepareTime = 0;
        Name = "default";
        Hp = 10;
        Atk = 1;
        Number = 1;
        Model = 0;
        Speed = 0;
        ProxyRadius = 0;
        Priority = 50;
        HitRange = 6f;
        ScanRange = 1000;
        AttackTime = 1f;
        AttackOffset = 0;
        ShootNum = 1;
        BulletSpeed = 5;
        AttackType = 0;
        TargetType = 0;
        IsGround = 1;
    }

    public UnitData(UnitData data)
    {
        ID = data.ID;
        PrepareTime = data.PrepareTime;
        Name = data.Name;
        Hp = data.Hp;
        Atk = data.Atk;
        Number = data.Number;
        Model = data.Model;
        Speed = data.Speed;
        ProxyRadius = data.ProxyRadius;
        Priority = data.Priority;
        HitRange = data.HitRange;
        ScanRange = data.ScanRange;
        AttackTime = data.AttackTime;
        AttackOffset = data.AttackOffset;
        ShootNum = data.ShootNum;
        BulletSpeed = data.BulletSpeed;
        AttackType = data.AttackType;
        TargetType = data.TargetType;
        IsGround = data.IsGround;
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
