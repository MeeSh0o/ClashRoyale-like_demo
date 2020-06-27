using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarManager : MonoBehaviour
{
    public static HpBarManager instance;
    public GameObject prefabHp;

    public List<Unit> bars;
    public List<Text> hps;

    private void Awake()
    {
        instance = this;
        prefabHp = Resources.Load("Hp") as GameObject;
    }
    /// <summary>
    /// 要一个血条
    /// </summary>
    public void CallABar(Unit unit)
    {
        Vector3 SceenPosition = Camera.main.WorldToScreenPoint(unit.HpBar.transform.position);
        bars.Add(unit);
        hps.Add(Instantiate(prefabHp, SceenPosition, Quaternion.identity, transform).GetComponent<Text>());
    }

    private void Update()
    {
        for(int i = hps.Count - 1; i >= 0; i--)
        {
            if(bars[i] != null)
            {
                PHFollowEnemy(bars[i].HpBar.transform, hps[i], Vector3.forward * 2);
                hps[i].text = bars[i].Hp.ToString() + "/" + bars[i].data.Hp.ToString();
            }
            else
            {
                Destroy(hps[i]);
                bars.RemoveAt(i);
                hps.RemoveAt(i);
            }
        }
    }

    void PHFollowEnemy(Transform transform, Text text, Vector3 offset)
    {
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position) + offset;
        text.transform.position = position;
    }  
}
