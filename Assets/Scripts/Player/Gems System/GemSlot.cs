using UnityEngine;

public class GemSlot : MonoBehaviour
{
    public bool isEmpty = true;
    public GameObject gemManager;


    public void ActivateSlot()
    {
        isEmpty = false;
    }


}
