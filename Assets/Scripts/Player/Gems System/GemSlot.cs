using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSlot : MonoBehaviour
{
    public bool isEmpty = true;
    public GameObject gemManager;


    public void ActivateSlot()
    {
        isEmpty = false;
        //gemManager.GetComponent<GemManager>().CalculeteStats();
    }


}
