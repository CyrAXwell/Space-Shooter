using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GemManager : MonoBehaviour
{
    [SerializeField] private GemsInventory gemsInventory;

    [SerializeField] private GemTooltip gemInfoPanel;
    [SerializeField] private GemStats gemPrefab;
    [SerializeField] private List<GemSlot> gemSlots;
    [SerializeField] private GameObject display;
    [SerializeField] private int[] probability;
    [SerializeField] private Button nextWaveButton;

    private int _gemsHealth;
    private int _gemsDamage;
    private int _gemsDefense;
    private int _gemsCritDamage;
    private int _gemsCritRate;
    private GemStats _gem;
    private Player _player;
    
    public float gemFragments;
    public GameObject gemRewardPanel;

    [Header("Inventory")]
    private int[] cumulativeProbability = {0, 0, 0, 0, 0};

    [SerializeField] private List<GameObject> gemSlotReward;
    private List<GemStats> _rewardGems;

    public void Initialize(Player player)
    {
        _player = player;
        gemRewardPanel.SetActive(false);
        gemInfoPanel.Initialize(this);
        gemInfoPanel.gameObject.SetActive(false);
        UpdateFragmentsDisplay();
    }

    public float GetMoney() => gemFragments;

    private void GetProbability(int[] probability)
    {
        int probabilitySum = 0;
        for (int i = 0; i < probability.Length; i++)
        {
            probabilitySum += probability[i];
            cumulativeProbability[i] = probabilitySum;
        }
    }

    private int GetNumberOfGemsByProbability(int[] probability)
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
        _rewardGems = new List<GemStats>();
        gemRewardPanel.SetActive(true);
        for(int i = 0; i < 5; i++)
        {
            _gem = Instantiate(gemPrefab.gameObject, gemSlotReward[i].transform).GetComponent<GemStats>();
            _gem.Initialize(this, gemInfoPanel, gemRewardPanel);
            _rewardGems.Add(_gem);
        }
        gemRewardPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
    }   

    public void TakeGem()
    {
        for(int i = 0; i < _rewardGems.Count; i++)
        {   
            gemsInventory.CreateSlot(_rewardGems[i]);
            _rewardGems[i].isReward = false;
            _rewardGems[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        gemRewardPanel.SetActive(false);

        if(_gem.gameObject.activeInHierarchy)
        {
            gemInfoPanel.gameObject.SetActive(false);
            _gem.transform.GetChild(0).gameObject.SetActive(false);
        }

        gemsInventory.SortInventory();
        _rewardGems.Clear();
    }

    public void EquipGem(GemStats equipGem)
    {
        
        for (int i = 0; i <= 4; i++)
        {
            if(gemSlots[i].isEmpty)
            {
                equipGem.isEquip = true;
                equipGem.slot = i;

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
                gemInfoPanel.gameObject.SetActive(false);
                
                if(!equipGem.GetComponent<GemStats>().isReward)
                {
                    gemsInventory.DestroySlot(equipGem);
                }
                else
                {
                    equipGem.isReward = false;
                }
                break;
            }
        }
    }

    public void UnequipGem(GemStats UnequipGem)
    {
        gemSlots[UnequipGem.slot].GetComponent<GemSlot>().isEmpty = true;
        UnequipGem.isEquip = false;
        UnequipGem.transform.GetChild(1).gameObject.SetActive(false);

        gemsInventory.CreateSlot(UnequipGem);
        gemsInventory.SortInventory();

        CalculeteStats();
        gemInfoPanel.gameObject.SetActive(false);
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


    public void BreakGem(GemStats gem)
    {
        gemFragments += 500 + gem.GetTotalExp() * 0.8f;
        
        UpdateFragmentsDisplay();
        
        if(gem.isReward)
        {
            Destroy(gem.gameObject);
        }
        else if(gem.isEquip)
        {
            gem.isEquip = false;
            gemSlots[gem.slot].isEmpty = true;
            Destroy(gem.gameObject);
            CalculeteStats();
        }
        else
        {
            gemsInventory.DestroySlot(gem);
            Destroy(gem.gameObject);
        }
        
        if(gemRewardPanel.activeInHierarchy && _rewardGems.Count == 1)
        {
            gemRewardPanel.SetActive(false);
        }
        _rewardGems.Remove(gem);
        gemInfoPanel.gameObject.SetActive(false);
    }

    public void UpdateFragmentsDisplay()
    {
        display.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = gemFragments.ToString();
    }

}
