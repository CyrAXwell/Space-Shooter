using UnityEngine;

public class GemSlot : MonoBehaviour
{
    private GemStats _equipedGem;
    private bool _isEmpty = true;

    public bool IsEmpty => _isEmpty;
    
    public GemStats GetEquipedGem() => _equipedGem;

    public void EquipGem(GemStats gem)
    {
        _equipedGem = gem;
        _isEmpty = false;
    }

    public void UnequipGem()
    {
        _equipedGem = null;
        _isEmpty = true;
    }

}
