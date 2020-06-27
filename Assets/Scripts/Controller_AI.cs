using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_AI : Controller
{
    public float _aggressive;
    public float _storage;

    [Space]

    public Coroutine ai;

    public float judgeTime; // AI判断周期

    public float[] K = new float[6] { 1, 1, 0, 1, 1, 0 };
    public float[] L = new float[6] { 1, 1, 0, 1, 1, 0 };

    [Range(-1, 1)]
    public float strageLimit; // 屯兵极限，低于这个极限电脑不再屯兵，有兵就放

    /// <summary>
    /// 对方Group
    /// </summary>
    private Group enemyGroup;

    private List<Unit> enemyBuilding = new List<Unit>(); // 敌方建筑物
    private List<Unit> myBuilding = new List<Unit>(); // 我方建筑物
    private List<UnitData> enemyBuildingdata = new List<UnitData>(); // 敌方建筑物原始数据
    private List<UnitData> myBuildingdata = new List<UnitData>(); // 我方建筑物原始数据
    private List<int> enemyHitted = new List<int>(); // 敌方受伤
    private List<float> enemyHpPercentage = new List<float>(); // 敌方血量比
    private List<float> myHpPercentage = new List<float>(); // 我方血量比
    private List<int> myHitted = new List<int>(); // 我方受伤
    private List<float> k1 = new List<float>(); // 计算用的矩阵
    private List<float> k2 = new List<float>(); // 计算用的矩阵
    private List<float> l1 = new List<float>(); // 计算用的矩阵
    private List<float> l2 = new List<float>(); // 计算用的矩阵


    public override void Awake()
    {
        base.Awake();
        for (int i = 0; i < HandCardUI.Count - 1; i++)
        {
            HandCardUI[i].interactable = false;
            HandCardText[i].color = Color.white;
        }
        if (judgeTime == 0)
        {
            judgeTime = Mathf.PI * 1.5f;
        }
        preLook = Resources.Load("PreLook" + Flod) as GameObject;
    }

    private void Start()
    {
        if (enemyGroup == null)
        {
            enemyGroup = Flod == "Player" ? BattleManager.instance.EnemyGroup : BattleManager.instance.playerGroup;
        }
    }

    public override void EnableAI()
    {
        base.EnableAI();

        foreach (Unit i in enemyGroup.Buildings)
        {
            enemyBuilding.Add(i);
            enemyBuildingdata.Add(i.data);
            enemyHitted.Add(0);
            enemyHpPercentage.Add(1);
            if (i.name == "基地")
            {
                k1.Add(K[0]);
                l1.Add(L[0]);
            }
            else if (i.name == "塔")
            {
                k1.Add(K[1]);
                l1.Add(L[1]);
            }

        }
        foreach (Unit i in group.Buildings)
        {
            myBuilding.Add(i);
            myBuildingdata.Add(i.data);
            myHitted.Add(0);
            myHpPercentage.Add(1);
            if (i.name == "基地")
            {
                k2.Add(K[3]);
                l2.Add(L[3]);
            }
            else if (i.name == "塔")
            {
                k2.Add(K[4]);
                l2.Add(L[4]);
            }
        }

        if (ai == null)
            ai = StartCoroutine(AI());
    }

    public override void DisableAI()
    {
        base.DisableAI();

        if (ai != null)
        {
            StopCoroutine(ai);
            ai = null;
        }
    }

    public IEnumerator AI()
    {
        yield return new WaitForSeconds(judgeTime);
        while (true)
        {
            //Debug.Log(Flod + "Aggressive " + Aggressive());
            //Debug.Log(Flod + "Storage " + Storage());
            float agg = Aggressive();
            float sto = Storage();

            int handDeckNum = HandDeckNum(); // 储存用第几张牌
            List<int> usingCard = new List<int>();
            if (handDeckNum == HandDeck.Count)
            {
                // 手牌满了，一次性放光
                for (int i = 0; i < HandDeck.Count; i++)
                {
                    usingCard.Add(i);
                }
            }
            else if (handDeckNum > 0)
            {
                for (int i = 0; i < HandDeck.Count; i++)
                {
                    if (HandDeck[i] != -1)
                    {
                        // 每张牌都随一次，看放不放
                        bool temp = Random.Range(0f, 1f) > sto;
                        if (temp)
                        {
                            usingCard.Add(i);
                        }
                    }
                }

            }

            if (usingCard.Count > 0)
            {
                int mostHittedE = 10000000;
                Unit mostHittedUnitE = null;
                for (int i = 0; i < enemyBuilding.Count; i++)
                {
                    if (enemyBuilding != null)
                    {
                        if (enemyHitted[i] < mostHittedE)
                        {
                            mostHittedE = enemyHitted[i];
                            mostHittedUnitE = enemyBuilding[i];
                        }
                        else if (enemyHitted[i] == mostHittedE)
                        {
                            if (Random.Range(0, 1) < (Mathf.Pow(0.5f, i)))
                            {
                                mostHittedE = enemyHitted[i];
                                mostHittedUnitE = enemyBuilding[i];
                            }
                        }
                    }
                }
                int mostHittedM = 10000000;
                Unit mostHittedUnitM = null;
                for (int i = 0; i < myBuilding.Count; i++)
                {
                    if (myBuilding != null)
                    {
                        if (myHitted[i] < mostHittedM)
                        {
                            mostHittedM = myHitted[i];
                            mostHittedUnitM = myBuilding[i];
                        }
                        //else if (myHitted[i] == mostHittedM)
                        //{
                        //    if (Random.Range(0, 1) < (Mathf.Pow(0.5f, i)))
                        //    {
                        //        mostHittedM = myHitted[i];
                        //        mostHittedUnitM = myBuilding[i];
                        //    }
                        //}
                    }
                }

                if (mostHittedUnitE == null)
                {
                    mostHittedUnitE = mostHittedUnitM;
                }
                if (mostHittedUnitM == null)
                {
                    continue;
                }

                foreach (int i in usingCard)
                {
                    // 位置随机偏移一点点
                    Vector3 positionOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * 1.5f;

                    float temp = Random.Range(0f, 1f);
                    //Debug.LogError(temp);
                    if (temp > agg)
                    {
                        // 防守 
                        Node nearestNode = NodeManager.instance.GetNearestNode(mostHittedUnitM.transform.position + positionOffset, Flod == "Player", 100f);
                        //Debug.LogWarning(nearestNode.transform.position);
                        StartCoroutine(AIUseCard(i, nearestNode.transform.position));
                    }
                    else
                    {
                        // 攻击
                        Node nearestNode = NodeManager.instance.GetNearestNode(mostHittedUnitE.transform.position + positionOffset, Flod == "Player", 100f);
                        //Debug.LogWarning(nearestNode.transform.position);
                        StartCoroutine(AIUseCard(i, nearestNode.transform.position));
                    }

                    yield return new WaitForSeconds(0.3f);
                }
            }


            yield return new WaitForSeconds(judgeTime);
        }
    }

    private IEnumerator AIUseCard(int i ,Vector3 vector)
    {
        GameObject preLook = Instantiate(this.preLook, vector, Quaternion.identity, transform);
        yield return new WaitForSeconds(1f);
        UseCard(i, vector);
        Destroy(preLook);
    }

    // 统计当前可用手牌数量
    private int HandDeckNum()
    {
        int temp = 0;
        foreach(int i in HandDeck)
        {
            if (i!= -1)
            {
                temp++;
            }
        }
        return temp;
    }

    /// <summary>
    /// 攻击性，越高越倾向于在敌方血量最低的建筑物最近的地方造兵，越低越倾向于在我方血量最低的建筑物最近的地方造兵
    ///    = 敌方失去血量 / 敌我失去血量
    /// </summary>
    public float Aggressive()
    {
        FlushHitData();
        float m = (Tools.MatrixMulty(myHitted, k2) + 1) * (1 + Random.Range(-K[5], K[5]));
        float e = (Tools.MatrixMulty(enemyHitted, k1) + 1) * (1 + Random.Range(-K[2], K[2]));

        float score = e / (m + e);
        _aggressive = score;
        return score;
    }


    /// <summary>
    /// 屯兵性，越低越容易在有牌的时候立即放出去 ，约为我方兵力完整度/敌我兵力完整度
    /// </summary>
    public float Storage()
    {
        FlushHitData();
        float m = (Tools.MatrixMulty(myHpPercentage, l2) + 1) * (1 + Random.Range(-L[5], L[5]));
        float e = (Tools.MatrixMulty(enemyHpPercentage, l1) + 1) * (1 + Random.Range(-L[2], L[2]));
        float score = m / (m + e);
        _storage = score;

        return score;

    }

    private void FlushHitData()
    {
        for (int i = 0; i < enemyBuilding.Count; i++)
        {
            if (enemyBuilding[i] != null)
            {
                enemyHitted[i] = enemyBuildingdata[i].Hp - enemyBuilding[i].Hp;
                enemyHpPercentage[i] = enemyBuilding[i].Hp / (float)enemyBuildingdata[i].Hp;
            }
            else
            {
                enemyHitted[i] = enemyBuildingdata[i].Hp;
                enemyHpPercentage[i] = 0;
            }
        }
        for (int i = 0; i < myBuilding.Count; i++)
        {
            if (myBuilding[i] != null)
            {
                myHitted[i] = myBuildingdata[i].Hp - myBuilding[i].Hp;
                myHpPercentage[i] = myBuilding[i].Hp / (float)myBuildingdata[i].Hp;
            }
            else
            {
                myHitted[i] = myBuildingdata[i].Hp;
                myHpPercentage[i] = 0;
            }
        }
    }
}