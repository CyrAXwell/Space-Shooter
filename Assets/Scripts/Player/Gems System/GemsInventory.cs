using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GemsInventory : MonoBehaviour
{
    [SerializeField] private Transform invenoryContent;
    [SerializeField] private GameObject invenorySlotPrefab;
    
    private List<GemStats> _inventoryGems = new List<GemStats>();

    private Transform GetSlot(int index) => invenoryContent.GetChild(index);

    public void SortInventory()
    {
        _inventoryGems = _inventoryGems.OrderBy(g => g.name).ToList();

        for(int i = 0; i < _inventoryGems.Count; i++)
            _inventoryGems[i].transform.SetParent(GetSlot(i), false);
    }
    
    public void CreateSlot(GemStats gem)
    {
        Instantiate(invenorySlotPrefab, invenoryContent);
        _inventoryGems.Add(gem);
        gem.transform.SetParent(invenoryContent.GetChild(_inventoryGems.Count - 1), false);
    }

    public void DestroySlot(GemStats gem)
    {
        _inventoryGems.Remove(gem);
        SortInventory();
        Destroy(invenoryContent.GetChild(_inventoryGems.Count).gameObject);
    }

}
