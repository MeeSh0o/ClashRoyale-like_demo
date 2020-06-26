using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNode : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Node node = other.GetComponent<Node>();
        if(node!= null)
        {
            node.SetVisible(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Node node = other.GetComponent<Node>();
        if (node != null)
        {
            node.SetVisible(false);
        }
    }
}
