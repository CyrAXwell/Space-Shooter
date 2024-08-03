using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GemManager : MonoBehaviour
{
    [SerializeField] private GemsInventory gemsInventory;

    [SerializeField] private GameObject gemPrefab;
    [SerializeField] private List<GemSlot> gemSlots;
    [SerializeField] private GameObject display;
    [SerializeField] private int[] probability;
    [SerializeField] private Button nextWaveButton;

    private int _gemsHealth;
    private int _gemsDamage;
    private int _gemsDefense;
    private int _gemsCritDamage;
    private int _gemsCritRate;
    private GameObject _gem;
    private Player _player;
    
    public float gemFragments;
    public GameObject gemRewardPanel;
    public GameObject gemInfoPanel;

    [Header("Inventory")]
    private int[] cumulativeProbability = {0, 0, 0, 0, 0};

    [SerializeField] private List<GameObject> gemSlotReward;
    private List<GameObject> _rewardGems;

    public void Initialize(Player player)
    {
        _player = player;
        gemRewardPanel.SetActive(false);
        gemInfoPanel.SetActive(false);
        UpdateFragmentsDisplay();
    }

    void GetProbability(int[] probability)
    {
        int probabilitySum = 0;
        for (int i = 0; i < probability.Length; i++)
        {
            probabilitySum += probability[i];
            cumulativeProbability[i] = probabilitySum;
        }
    }

    int GetNumberOfGemsByProbability(int[] probability)
    {
        int randomNumber = Random.Range(0, 10001);
        for (int i = 0; i < probability.Length; i++)
        {
            if (randomNumber <= cumulativeProbability[i])
                return i + 1;
        }
        return -1;
    }

    public void CreateGem()
    {
        GetProbability(probability);
         int numberGemReward = GetNumberOfGemsByProbability(cumulativeProbability);
        _rewardGems = new List<GameObject>();
        gemRewardPanel.SetActive(true);
        for(int i = 0; i < numberGemReward; i++)
        {
            _gem = Instantiate(gemPrefab,gemSlotReward[i].transform);
            _rewardGems.Add(_gem);
            _gem.GetComponent<GemStats>().GetGemStats();
            _gem.name = _gem.GetComponent<GemStats>().gemName;
        }
        gemRewardPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        _gem.name = _gem.GetComponent<GemStats>().gemName;
    }   

    public void TakeGem()
    {
        for(int i = 0; i < _rewardGems.Count; i++)
        {   
            Instantiate(gemsInventory.GetSlotPrefab(), gemsInventory.GetTransform());
            gemsInventory.AddGem(_rewardGems[i]);
            _rewardGems[i].transform.SetParent(gemsInventory.GetSlot(gemsInventory.GetGemsAmount() -1 ), false);
            //Create slot

            _rewardGems[i].GetComponent<GemStats>().isReward = false;

            _rewardGems[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        gemRewardPanel.SetActive(false);
        if(_gem.activeInHierarchy)
        {
            gemInfoPanel.SetActive(false);
            _gem.transform.GetChild(0).gameObject.SetActive(false);
        }
        gemsInventory.SortGems();
        _rewardGems.Clear();
    }

    public void EquipGem(GameObject equipGem)
    {
        
        for (int i = 0; i <= 4; i++)
        {
            if(gemSlots[i].isEmpty)
            {
                equipGem.GetComponent<GemStats>().isEquip = true;
                equipGem.GetComponent<GemStats>().slot = i;

                equipGem.transform.SetParent(gemSlots[i].transform, false);
                gemSlots[i].isEmpty = false;
   

                if(gemRewardPanel.activeInHierarchy && _rewardGems.Count == 1)
                {
                    gemRewardPanel.SetActive(false);
                    nextWaveButton.interactable = true;
                }
                _rewardGems.Remove(equipGem);
                CalculeteStats();
                equipGem.transform.GetChild(0).gameObject.SetActive(false);
                gemInfoPanel.GetComponent<GemTooltip>().EquipHighlight.SetActive(false);
                gemInfoPanel.SetActive(false);
                
                if(!equipGem.GetComponent<GemStats>().isReward)
                {
                    // Destroy slot
                    gemsInventory.RemoveGem(equipGem);
                    gemsInventory.SortGems();
                    Destroy(gemsInventory.GetSlot(gemsInventory.GetGemsAmount()).gameObject);
                }else
                {
                    equipGem.GetComponent<GemStats>().isReward = false;
                }
                break;
            }
        }
    }

    public void UnequipGem(GameObject UnequipGem)
    {
        gemSlots[UnequipGem.GetComponent<GemStats>().slot].GetComponent<GemSlot>().isEmpty = true;
        UnequipGem.GetComponent<GemStats>().isEquip = false;
        UnequipGem.transform.GetChild(1).gameObject.SetActive(false);

        Instantiate(gemsInventory.GetSlotPrefab(), gemsInventory.GetTransform());
        gemsInventory.AddGem(UnequipGem);
        UnequipGem.transform.SetParent(gemsInventory.GetSlot(gemsInventory.GetGemsAmount() - 1), false);

        gemsInventory.SortGems();
        CalculeteStats();
        gemInfoPanel.SetActive(false);
    }

    public void CalculeteStats()
    {
        _gemsHealth = 0;
        _gemsDamage = 0;
        _gemsDefense = 0;
        _gemsCritDamage = 0;
        _gemsCritRate = 0;

        for (int i = 0; i <= 4; i++)
        {
            if(!gemSlots[i].GetComponent<GemSlot>().isEmpty)
            {
                _gemsHealth += gemSlots[i].transform.GetChild(0).gameObject.GetComponent<GemStats>().gemHP;
                _gemsDamage += gemSlots[i].transform.GetChild(0).gameObject.GetComponent<GemStats>().gemATK;
                _gemsDefense += gemSlots[i].transform.GetChild(0).gameObject.GetComponent<GemStats>().gemDEF;
                _gemsCritDamage += gemSlots[i].transform.GetChild(0).gameObject.GetComponent<GemStats>().gemCRITDMG;
                _gemsCritRate += gemSlots[i].transform.GetChild(0).gameObject.GetComponent<GemStats>().gemCRITRate;    
            } 
        }

        _player.UpdateGemStats(_gemsHealth, _gemsDamage, _gemsDefense, _gemsCritDamage, _gemsCritRate);
    }


    public void BreakGem(GameObject gem)
    {
        gemFragments += 500 + gem.GetComponent<GemStats>().totalExp*0.8f;
        
        UpdateFragmentsDisplay();
        
        if(gem.GetComponent<GemStats>().isReward)
        {
            Destroy(gem);
        }else if(gem.GetComponent<GemStats>().isEquip)
        {
            gem.GetComponent<GemStats>().isEquip = false;
            gemSlots[gem.GetComponent<GemStats>().slot].GetComponent<GemSlot>().isEmpty = true;
            Destroy(gem);
            CalculeteStats();
        }else
        {
            // Destroy slot
            gemsInventory.RemoveGem(gem);
            Destroy(gem);
            gemsInventory.SortGems();
            Destroy(gemsInventory.GetSlot(gemsInventory.GetGemsAmount()).gameObject);
        }
        
        if(gemRewardPanel.activeInHierarchy && _rewardGems.Count == 1)
        {
            gemRewardPanel.SetActive(false);
        }
        _rewardGems.Remove(gem);
        gemInfoPanel.SetActive(false);
    }

    public void UpdateFragmentsDisplay()
    {
        display.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = gemFragments.ToString();
    }

    // public void SortGems()
    // {
    //     InventoryGems = InventoryGems.OrderBy(g => g.name).ToList();
    //     for(int i = 0; i < InventoryGems.Count; i++)
    //     {
    //         InventoryGems[i].transform.SetParent(gemsInventory.GetSlot(i), false);
    //     }
    // }

    // public void DeleteInventorySlot(GameObject delGem)
    // {
    //     Destroy(invenoryContent.transform.GetChild(InventoryGems.Count-1).gameObject);
    //     Debug.Log(invenoryContent.transform.GetChild(InventoryGems.Count-1));
    // }
}
