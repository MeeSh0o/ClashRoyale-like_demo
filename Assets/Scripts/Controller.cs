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
    public List<Button> HandCardUI;

    public List<Text> HandCardText;

    public Group group;

    /// <summary>
    /// 当前是否选中牌
    /// </summary>
    public bool isChosing = false;
    /// <summary>
    /// 当前选中的牌
    /// </summary>
    public int chosenCard = -1;


    public Button notSelect;
    public GameObject preLook;

    public virtual void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            HandCardUI.Add(GameObject.Find(Flod + "Deck" + (i + 1).ToString()).GetComponent<Button>());
        }
        for (int i = 0; i < HandCardUI.Count; i++)
        {
            HandCardText.Add(HandCardUI[i].GetComponentInChildren<Text>());
            int j = i;
            HandCardUI[i].GetComponent<Button>().onClick.AddListener(delegate () { this.CallACard(j); });// 给按钮挂监听脚本
        }
        HandDeck = new List<int>(new int[] { -1, -1, -1, -1 });
        for (int i = 0; i < HandCardUI.Count - 1; i++)
        {
            UpdateCardUI(i);
        }
        if (notSelect == null)
        {
            notSelect = GameObject.Find("NotSelect").GetComponent<Button>();
        }

        PrepareUnit = -1;

        if(group == null)
        {
            group = GetComponent<Group>();
        }
        
    }

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
        if (DrawDeck.Count == 0)
        {
            for (int i = 0; i < AbandonDeck.Count; i++)
            {
                int temp = AbandonDeck[i];
                DrawDeck.Add(temp);
                AbandonDeck.Remove(temp);
            }
        }

        if (DrawDeck.Count > 0)
        {
            int index = Random.Range(0, DrawDeck.Count);
            int id = DrawDeck[index];
            DrawDeck.Remove(id);
            Debug.Log("玩家" + Flod + "抽到了牌: " + id.ToString() + Tools.GetUnitData(id).Name);
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
            HandCardUI[i].gameObject.SetActive(false);
        }
        else
        {
            HandCardUI[i].gameObject.SetActive(true);
            HandCardText[i].text = Tools.GetUnitData(HandDeck[i]).Name + "\n" + (i + 1).ToString();
        }
    }



    /// <summary>
    /// 发牌器
    /// </summary>
    /// <returns></returns>
    public IEnumerator PrepareCard()
    {
        EnableAI();
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
            if (prepareTime >= PrepareTime)
            {
                HandCardText[HandCardText.Count - 1].text = name + "\n" + "100%";
                for (int i = 0; i < HandDeck.Count; i++)
                {
                    // 找到第一张空牌,就把这张准备好的牌发过去
                    if (HandDeck[i] == -1)
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
        Debug.Log("玩家" + Flod + "使用牌 " + (i + 1).ToString() + " " + Tools.GetUnitData(HandDeck[i]).Name);

        int id = HandDeck[i];

        group.SpawnUnit(id, position);

        isChosing = false;
        chosenCard = -1;

        AbandonDeck.Add(id);

        HandDeck[i] = -1;
        UpdateCardUI(i);
    }

    /// <summary>
    /// 选中卡片
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public bool CallACard(int i)
    {
        if (!isChosing)
        {
            // 当前没有在选牌
            Debug.Log("玩家" + Flod + "尝试选中牌" + (i+1).ToString());

            if (HandDeck[i] != -1)
            {
                Debug.Log("玩家" + Flod + "尝试选中牌" + (i + 1).ToString() + "成功");
                ChoseACard(i);
                return true;
            }
        }
        else
        {
            // 已经选了牌
            if (i == chosenCard)
            {
                // 选中相同牌,则为取消选牌
                CancelCallACard();
                return false;
            }
            else
            {
                // 选中不同牌
                Debug.Log("玩家" + Flod + "将牌从 " + (chosenCard + 1).ToString() + " 更换到 " + (i + 1).ToString());

                if (HandDeck[i] != -1)
                {
                    Debug.Log("玩家" + Flod + "将牌从 " + (chosenCard + 1).ToString() + " 更换到 " + (i + 1).ToString() + "成功");
                    ChoseACard(i);
                    return true;
                }
            }
        }

        return false;
    }
    /// <summary>
    /// 选中卡牌
    /// </summary>
    /// <param name="i"></param>
    public virtual void ChoseACard(int i)
    {
        isChosing = true;
        chosenCard = i;
        if (Flod == "Player")
        {
            HandCardUI[chosenCard].Select(); 
        }       
    }
    /// <summary>
    /// 取消选中卡片
    /// </summary>
    public virtual void CancelCallACard()
    {
        Debug.Log("玩家" + Flod + "取消选中牌 " + (chosenCard + 1).ToString());
        isChosing = false;
        chosenCard = -1;
        if(Flod == "Player")
        {
            notSelect.Select();
        }
    }
    /// <summary>
    /// 开启AI功能
    /// </summary>
    public virtual void EnableAI()
    {

    }
    /// <summary>
    /// 关闭AI功能
    /// </summary>
    public virtual void DisableAI()
    {

    }
}
