using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Objects : MonoBehaviour
{
    public Camera cam;

    public NavMeshAgent agent;

    public enum ObjectsState
    {
        idle,
        run,
        attack,
        death
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        agent.SetDestination(hit.point);
        //    }
        //}
    }

    public virtual void Idle()
    {

    }

    public virtual void Attack()
    {

    }
    public virtual void Run()
    {

    }

    public virtual void Death()
    {

    }

    public virtual void Spawn()
    {

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