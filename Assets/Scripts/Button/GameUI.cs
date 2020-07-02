using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Dropdown dropdown;

    public Button buttom;

    public string ButtonName;


    private void Start()
    {
        switch (ButtonName)
        {
            case ("BlueDropdown"):
                dropdown.onValueChanged.AddListener((int v) => OnValueChange(v));
                break;
            case ("RedDropdown"):
                dropdown.onValueChanged.AddListener((int v) => OnValueChange(v));
                break;
            case ("EnterBattle"):
                buttom.onClick.AddListener(delegate () { GameManager.instance.EnterBattleScene("Battle"); });
                break;
            case ("Relaod"):
                buttom.onClick.AddListener(delegate () { Reload(); });
                break;
            case ("StartBattle"):
                buttom.onClick.AddListener(delegate () { BattleManager.instance.BattleInitiate(); });
                break;
            default:
                break;
        }
    }


    public void SetPlayer(bool isHuman)
    {
        GameManager.instance.isPlayerHuman = isHuman;
    }

    public void SetEnemy(bool isHuman)
    {
        GameManager.instance.isEnemyHuman = isHuman;
    }

    /// <summary>
    /// 当点击后值改变是触发 (切换下拉选项)
    /// </summary>
    /// <param name="v">是点击的选项在OptionData下的索引值</param>
    void OnValueChange(int v)
    {
        //切换选项 时处理其他的逻辑

        switch (ButtonName)
        {
            case ("BlueDropdown"):
                if (v == 0) SetPlayer(true);
                else SetPlayer(false);
                break;
            case ("RedDropdown"):
                if (v == 1) SetEnemy(true);
                else SetEnemy(false);
                break;
            default:
                break;
        }
    }

    void Reload()
    {
        GameManager.instance.Reload();
    }
}
