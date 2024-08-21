using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GemManager : MonoBehaviour
{
    public event Action OnHideRewardPanel;

    [SerializeField] private GemStats gemPrefab;
    [SerializeField] private GemsInventory gemsInventory;
    [SerializeField] private List<GemSlot> gemSlots;
    [SerializeField] private GemTooltip gemInfoPanel;
    [SerializeField] private RewardGemsPanel gemRewardPanel;
    [SerializeField] private List<GameObject> gemRewardSlots;
    [SerializeField] private GameObject moneyDisplay;
    [SerializeField] private Button nextWaveButton;
    [SerializeField] private int[] probability;

    private int _gemsHealth;
    private int _gemsDamage;
    private int _gemsDefense;
    private int _gemsCritDamage;
    private int _gemsCritRate;
    private Player _player;
    private float _money;
    private List<GemStats> _rewardGems;
    private int[] _cumulativeProbability = {0, 0, 0, 0, 0};
    private AudioManager _audioManager;

    public void Initialize(Player player)
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        _money = 500;
        _player = player;
        gemInfoPanel.Initialize(this);
        UpdateFragmentsDisplay();
    }

    public float GetCurrentCoins() => _money;
    public void Spend(int coins) => _money -= coins;

    public void CreateGems()
    {
        GetProbability(probability);
        int numberGemReward = GetNumberOfGemsByProbability(_cumulativeProbability);
        _rewardGems = new List<GemStats>();
        gemRewardPanel.ShowPanel();
        for(int i = 0; i < numberGemReward; i++)
        {
            GemStats gem = Instantiate(gemPrefab.gameObject, gemRewardSlots[i].transform).GetComponent<GemStats>();
            gem.Initialize(this, gemInfoPanel, gemRewardPanel);
            gem.SetState(GemStats.GemState.reward);
            _rewardGems.Add(gem);
        }
    }   

    public void TakeGem()
    {
        _audioManager.PlaySFX(_audioManager.TooltipButtonClick, 0.7f);

        for(int i = 0; i < _rewardGems.Count; i++)
        {   
            gemsInventory.CreateSlot(_rewardGems[i]);
            _rewardGems[i].SetState(GemStats.GemState.inventory);
            _rewardGems[i].HideStandartOutline();
        }

        gemRewardPanel.HidePanel();
        OnHideRewardPanel?.Invoke();
        gemInfoPanel.gameObject.SetActive(false);

        gemsInventory.SortInventory();
        _rewardGems.Clear();
    }

    public void EquipGem(GemStats equipGem)
    {
        for (int i = 0; i <= 4; i++)
        {
            if(gemSlots[i].IsEmpty)
            {
                equipGem.SetSlotIndex(i);

                equipGem.transform.SetParent(gemSlots[i].transform, false);
                gemSlots[i].EquipGem(equipGem);
   
                if(gemRewardPanel.gameObject.activeInHierarchy && _rewardGems.Count == 1)
                {
                    gemRewardPanel.HidePanel();
                    OnHideRewardPanel?.Invoke();
                }

                _rewardGems.Remove(equipGem);

                CalculeteStats();

                equipGem.HideStandartOutline();
                gemInfoPanel.gameObject.SetActive(false);

                if(equipGem.GetState() == GemStats.GemState.inventory)
                    gemsInventory.DestroySlot(equipGem);
                
                equipGem.SetState(GemStats.GemState.equiped);

                break;
            }
        }
    }

    public void UnequipGem(GemStats UnequipGem)
    {
        gemSlots[UnequipGem.GetSlotIndex()].UnequipGem();
        UnequipGem.SetState(GemStats.GemState.inventory);
        UnequipGem.HideEquipedOutline();

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
            if(!gemSlots[i].IsEmpty)
            {
                _gemsHealth += gemSlots[i].GetEquipedGem().GetHealth();
                _gemsDamage += gemSlots[i].GetEquipedGem().GetDamage();
                _gemsDefense += gemSlots[i].GetEquipedGem().GetDefense();
                _gemsCritDamage += gemSlots[i].GetEquipedGem().GetCritDamage();
                _gemsCritRate += gemSlots[i].GetEquipedGem().GetCritRate();    
            } 
        }
        _player.UpdateGemStats(_gemsHealth, _gemsDamage, _gemsDefense, _gemsCritDamage, _gemsCritRate);
    }

    public void BreakGem(GemStats gem)
    {
        _money += 500 + gem.GetTotalExp() * 0.8f;
        
        UpdateFragmentsDisplay();
        
        if(gem.GetState() == GemStats.GemState.reward)
        {
            Destroy(gem.gameObject);
        }
        else if(gem.GetState() == GemStats.GemState.equiped)
        {
            gemSlots[gem.GetSlotIndex()].UnequipGem();
            Destroy(gem.gameObject);
            CalculeteStats();
        }
        else
        {
            gemsInventory.DestroySlot(gem);
            Destroy(gem.gameObject);
        }
        
        if(gemRewardPanel.gameObject.activeInHierarchy && _rewardGems.Count == 1)
            gemRewardPanel.HidePanel();
        
        _rewardGems.Remove(gem);
        gemInfoPanel.gameObject.SetActive(false);
    }

    public void UpdateFragmentsDisplay()
    {
        moneyDisplay.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = _money.ToString();
    }

    private void GetProbability(int[] probability)
    {
        int probabilitySum = 0;
        for (int i = 0; i < probability.Length; i++)
        {
            probabilitySum += probability[i];
            _cumulativeProbability[i] = probabilitySum;
        }
    }

    private int GetNumberOfGemsByProbability(int[] probability)
    {
        int randomNumber = UnityEngine.Random.Range(0, 10001);
        for (int i = 0; i < probability.Length; i++)
        {
            if (randomNumber <= _cumulativeProbability[i])
                return i + 1;
        }
        return -1;
    }
}
