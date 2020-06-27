using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controller_Player : Controller
{

    public Camera mainCamera;
    public GameObject nodeshower;
    public LayerMask rayHitMask;

    public override void Awake()
    {
        base.Awake();
        if (nodeshower == null)
        {
            nodeshower = Instantiate(Resources.Load("NodeShower") as GameObject, transform);
        }
        if(preLook == null)
        {
            preLook = Instantiate(Resources.Load("PreLook" + Flod) as GameObject, transform);
        }

        for(int i = 0;i<HandCardUI.Count - 1; i++)
        {
            HandCardUI[i].interactable = true;
            HandCardText[i].color = Color.black;
        }

        rayHitMask = 4096;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        int callCard = -1;
        // 获取到按键输入
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            callCard = 0;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            callCard = 1;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            callCard = 2;
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            callCard = 3;
        }

        if(callCard != -1)
        {
            //// 使用按键callCard了
            //CallACard(callCard);
            // 按对应按钮
            HandCardUI[callCard].onClick.Invoke();
        }


        if (isChosing)
        {
            // 射线打到场景上
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit,100f, rayHitMask))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                //Debug.LogWarning(hit.collider.gameObject.name + " " +  hit.point);
                nodeshower.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
            }

            // 在合法坐标创建一个预览模型
            Node nearestNode = NodeManager.instance.GetNearestNode(hit.point, Flod == "Player",6);
            if (nearestNode != null)
            {
                // 鼠标位置合法
                Vector3 nearestPos = nearestNode.transform.position;
                preLook.transform.position = nearestPos;

                if (Input.GetMouseButtonUp(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        Debug.LogWarning("正在点击UI");
                        // 点击UI
                    }
                    else
                    {
                        // 点击场景位置合法
                        UseCard(chosenCard, nearestNode.transform.position);
                        nodeshower.transform.position = Vector3.up * 100;
                        preLook.transform.position = Vector3.up * 100;

                    }

                }

            }
            else preLook.transform.position = Vector3.up * 100;

            if (Input.GetMouseButtonUp(1))
            {
                CancelCallACard();
            }
        }
        else
        {
            preLook.transform.position = Vector3.up * 100;
            nodeshower.transform.position = Vector3.up * 100;
        }
    }

    //public override void ChoseACard(int i)
    //{
    //    base.ChoseACard(i);
    //    HandCardUI[chosenCard].Select();
    //}

    //public override void CancelCallACard()
    //{
    //    base.CancelCallACard();
    //    PrepareUnit = -1; if (notSelect == null)
    //        notSelect.Select();
    //}
}