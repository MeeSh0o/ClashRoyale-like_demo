using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public string Flod;
    /// <summary>
    /// 卡组
    /// </summary>
    public List<int> Deck;
    /// <summary>
    /// 抽牌堆
    /// </summary>
    public List<int> DrawDeck;
    /// <summary>
    /// 弃牌堆
    /// </summary>
    public List<int> AbandonDeck;

    /// <summary>
    /// 准备单位
    /// </summary>
    public int PrepareUnit;

    /// <summary>
    /// 准备时间计时
    /// </summary>
    public float prepareTime;
    /// <summary>
    /// 准备时间
    /// </summary>
    public float PrepareTime;

    /// <summary>
    /// 手牌,-1表示没有牌
    /// </summary>
    public List<int> HandDeck;
    /// <summary>
    /// 手牌UI
    /// </summary>
    public List<GameObject> HandCardUI;

    public List<Text> HandCardText;

    public Group group;

    /// <summary>
    /// 设置牌组
    /// </summary>
    /// <param name="list"></param>
    public void SetDeck(List<int> list)
    {
        // 去重
        for (int i = 0; i < list.Count; i++)
        {
            if (!Deck.Contains(list[i]))
            {
                Deck.Add(list[i]);
            }
        }
        // 拷到抽牌堆
        for (int i = 0; i < Deck.Count; i++)
        {
            DrawDeck.Add(Deck[i]);
        }
    }
    /// <summary>
    /// 从抽牌堆抽一张牌
    /// </summary>
    /// <returns></returns>
    public virtual int DrawACard()
    {
        // 如果抽牌堆空，把弃牌堆洗入抽牌堆
        if(DrawDeck.Count == 0)
        {
            for(int i = 0; i < AbandonDeck.Count; i++)
            {
                int temp = AbandonDeck[i];
                DrawDeck.Add(temp);
                AbandonDeck.Remove(temp);
            }
        }

        if(DrawDeck.Count > 0)
        {
            int index = Random.Range(0, DrawDeck.Count);
            int id = DrawDeck[index];
            DrawDeck.Remove(id);
            Debug.Log("玩家" + Flod + "抽到了牌: " + id.ToString());
            return id;
        }
        else
        {
            Debug.Log("玩家" + Flod + "没抽到牌:");
            return -1;
        }

    }

    /// <summary>
    /// 更新手牌UI
    /// </summary>
    /// <param name="i"></param>
    public void UpdateCardUI(int i)
    {
        if (HandDeck[i] == -1)
        {
            HandCardUI[i].SetActive(false);
        }
        else
        {
            HandCardUI[i].SetActive(true);
            HandCardText[i].text = Tools.GetUnitData(HandDeck[i]).Name + "\n" + (i + 1).ToString();
        }
    }

    public virtual void Awake()
    {
        for(int i = 0; i < 5; i++)
        {
            HandCardUI.Add(GameObject.Find(Flod + "Deck" + (i + 1).ToString()));
        }
        for (int i = 0; i < HandCardUI.Count; i++)
        {
            HandCardText.Add(HandCardUI[i].GetComponentInChildren<Text>());
        }
        HandDeck = new List<int>(new int[]{-1,-1,-1,-1});
        for (int i = 0; i < HandCardUI.Count - 1; i++)
        {
            UpdateCardUI(i);
        }
        PrepareUnit = -1;

        group = GetComponent<Group>();
    }

    /// <summary>
    /// 发牌器
    /// </summary>
    /// <returns></returns>
    public IEnumerator PrepareCard()
    {
        // 无限循环的抽牌
        string name = "";
        while (true)
        {
            if (PrepareUnit == -1)
            {
                PrepareUnit = DrawACard();
                PrepareTime = Tools.GetUnitData(PrepareUnit).PrepareTime;
                prepareTime = 0;
                name = Tools.GetUnitData(PrepareUnit).Name;
            }
            // 计时
            while (prepareTime < PrepareTime)
            {
                HandCardText[HandCardText.Count - 1].text = name + "\n" + (prepareTime / PrepareTime * 100).ToString("#0.00") + "%";

                prepareTime += Time.deltaTime;
                yield return 0;
            }
            // 计时充足，把卡填充到手牌，准备区重置
            if(prepareTime >= PrepareTime)
            {
                HandCardText[HandCardText.Count - 1].text = name + "\n" + "100%";
                for(int i = 0; i < HandDeck.Count; i++)
                {
                    // 找到第一张空牌,就把这张准备好的牌发过去
                    if(HandDeck[i] == -1)
                    {
                        HandDeck[i] = PrepareUnit;
                        PrepareUnit = -1;

                        UpdateCardUI(i);

                        break;
                    }
                }
            }
            yield return 0;
        }
    }
    /// <summary>
    /// 使用牌，在场地上召唤单位，并把牌放入弃牌堆
    /// </summary>
    /// <param name="i"></param>
    /// <param name="position"></param>
    public void UseCard(int i, Vector3 position)
    {
        int id = HandDeck[i];

        group.SpawnUnit(id, position);

        HandDeck[i] = -1;
        UpdateCardUI(i);
    }



    /// <summary>
    /// 判断坐标是否合法,并返回一个合法值
    /// </summary>
    /// <param name="vector3"></param>
    public virtual Vector3 ISPositionAllowed(Vector3 vector3)
    {
        Ray ray = Camera.main.ScreenPointToRay(vector3);//从摄像机发出到点击坐标的射线

        return Vector3.zero;
    }
}
