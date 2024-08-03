using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GemsInventory : MonoBehaviour
{
    [SerializeField] private Transform invenoryContent;
    [SerializeField] private GameObject invenorySlotPrefab;
    
    private List<GameObject> _inventoryGems = new List<GameObject>();


    public Transform GetTransform() => invenoryContent;
    public Transform GetSlot(int index) => invenoryContent.GetChild(index);
    public GameObject GetSlotPrefab() => invenorySlotPrefab;
    public int GetGemsAmount() => _inventoryGems.Count;

    public void AddGem(GameObject gem) 
    {
        _inventoryGems.Add(gem);
    }

    public void RemoveGem(GameObject gem) 
    {
        _inventoryGems.Remove(gem);
    }

    public void SortGems()
    {
        _inventoryGems = _inventoryGems.OrderBy(g => g.name).ToList();

        for(int i = 0; i < _inventoryGems.Count; i++)
            _inventoryGems[i].transform.SetParent(GetSlot(i), false);
    }
    

}
