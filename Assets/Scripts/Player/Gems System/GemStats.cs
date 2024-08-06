using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GemStats : MonoBehaviour
{
    [SerializeField] private List<GemSO> gemList;
    [SerializeField] private GameObject standardOutline;
    [SerializeField] private GameObject equipedOutline;

    private int _id;
    private int _level;
    private int _addExp = 50;
    private int _needExp;
    private int _totalExp;
    private int _mainStatValue;
    private List<GemType> _subStats  = new List<GemType>();
    private List<int> _subStatsVulues = new List<int>();
    private int[] _cumulativeProbability = {0, 0, 0, 0};
    private int _subStatsAmount;
    private GemManager _gemManager;
    private GemTooltip _gemInfoPanel;
    private RewardGemsPanel _gemRewardPanel;
    private int _health;
    private int _damage;
    private int _defense;
    private int _critDamage;
    private int _critRate;
    private int _slotIndex;
    private GemState _state;
    
    public enum GemState
    {
        reward,
        equiped,
        inventory,
    }

    public void Initialize(GemManager gemManager, GemTooltip gemInfoPanel, RewardGemsPanel gemRewardPanel)
    {
        _gemManager = gemManager;
        _gemInfoPanel = gemInfoPanel;
        _gemRewardPanel = gemRewardPanel;

        GetGemStats();
    }

    private void GetGemStats()
    {
        _id = Random.Range(0,gemList.Count);
        _needExp = 100; 
        gameObject.name = gemList[_id].Name;
        _mainStatValue = gemList[_id].MainStatValue;
        CalculeteStats(gemList[_id].Type, _mainStatValue);

        transform.GetComponent<Image>().sprite = gemList[_id].Sprite;

        GetProbability(gemList[_id].SubStatsAmountProbability.ToArray());
        _subStatsAmount = GetNumberOfStatsByProbability(_cumulativeProbability);
        CreateSubStats();
    }

    private void CreateSubStats()
    {
        for (int i = 0; i <= _subStatsAmount; i++)
        {
            CreateRandomSubStat(i);
            CalculeteStats(_subStats[i], _subStatsVulues[i]);
        }
    }

    private void CreateRandomSubStat(int statIndex)
    {
        _subStats.Add((GemType) Random.Range(0, 5));
        switch(_subStats[statIndex])
        {
            case GemType.health: _subStatsVulues.Add(gemList[_id].HealthUpgrades.ToArray()[Random.Range(0, 2)]); break;
            case GemType.damage: _subStatsVulues.Add(gemList[_id].DamageUpgrades.ToArray()[Random.Range(0, 2)]); break;
            case GemType.defense: _subStatsVulues.Add(gemList[_id].DefenseUpgrades.ToArray()[Random.Range(0, 2)]); break;
            case GemType.critDamage: _subStatsVulues.Add(gemList[_id].CritDamageUpgrades.ToArray()[Random.Range(0, 2)]); break;
            case GemType.critRate: _subStatsVulues.Add(gemList[_id].CritRateUpgrades.ToArray()[Random.Range(0, 2)]); break;  
        }
    }

    private void UpgradeRandomSubStat(int statIndex)
    {
        switch(_subStats[statIndex])
        {
            case GemType.health: _subStatsVulues[statIndex] += gemList[_id].HealthUpgrades.ToArray()[Random.Range(0,3)]; break;                      
            case GemType.damage: _subStatsVulues[statIndex] += gemList[_id].DamageUpgrades.ToArray()[Random.Range(0,3)]; break;                      
            case GemType.defense: _subStatsVulues[statIndex] += gemList[_id].DefenseUpgrades.ToArray()[Random.Range(0,3)]; break;                       
            case GemType.critDamage: _subStatsVulues[statIndex] += gemList[_id].CritDamageUpgrades.ToArray()[Random.Range(0,3)]; break;    
            case GemType.critRate: _subStatsVulues[statIndex] += gemList[_id].CritRateUpgrades.ToArray()[Random.Range(0,3)]; break;
        }
    }

    
    private int GetNumberOfStatsByProbability(int[] probability)
    {
        int randomNumber = Random.Range(0, 10001);
        for (int i = 0; i < probability.Length; i++)
        {
            if (randomNumber <= _cumulativeProbability[i])
                return i;
        }
        return -1;
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

    private void CalculeteStats(GemType stat, int value)
    {
        switch(stat)
        {
            case GemType.health: _health += value; break;
            case GemType.damage: _damage += value; break;
            case GemType.defense: _defense += value; break;          
            case GemType.critDamage: _critDamage += value; break;           
            case GemType.critRate: _critRate += value; break;
        }  
    }

    private void SelectGem()
    {
        if(_state == GemState.equiped)
            equipedOutline.SetActive(true);
        else
            standardOutline.SetActive(true);    
    }

    public void DisplayStats()
    {
        SelectGem();
        _gemInfoPanel.OpenGemTooltip(this, _gemRewardPanel.gameObject.activeInHierarchy);
    }

    public int GetLevel() => _level;
    public string GetName() => gemList[_id].Name;
    public Color GetColor() => gemList[_id].Color;
    public int GetMainStat() => _mainStatValue;
    public IEnumerable<int> GetSubStatsValues() => _subStatsVulues;
    public IEnumerable<GemType> GetSubStatsTypes() => _subStats;
    public int GetNeedExp() => _needExp;
    public int GetTotalExp() => _totalExp;
    public int GetHealth() => _health;
    public int GetDamage() => _damage;
    public int GetDefense() => _defense;
    public int GetCritDamage() => _critDamage;
    public int GetCritRate() => _critRate;
    public int GetSlotIndex() => _slotIndex;
    public GemState GetState() => _state;
    public void SetSlotIndex(int index) => _slotIndex = index;
    public void SetState(GemState state) => _state = state;
    public void HideStandartOutline() => standardOutline.SetActive(false);
    public void HideEquipedOutline() => equipedOutline.SetActive(false);

    public void Upgrade()
    {
        if(_level < 15 && _gemManager.GetCurrentCoins() >= _needExp)
        {
            _gemManager.Spend(_needExp); 
            _totalExp = _needExp;
            _needExp += _addExp;
            _level ++;
            _mainStatValue += gemList[_id].MainStatIncreaseValue;
            CalculeteStats(gemList[_id].Type, gemList[_id].MainStatIncreaseValue);

            if(_state == GemState.equiped)
                _gemManager.CalculeteStats();

            if(_level % 3 == 0)
            {
                if(_subStatsAmount < 3)
                {
                    _subStatsAmount++;
                    CreateRandomSubStat(_subStatsAmount);
                    CalculeteStats(_subStats[_subStatsAmount], _subStatsVulues[_subStatsAmount]);

                    if(_state == GemState.equiped)
                        _gemManager.CalculeteStats();

                }else
                {
                    int subStatIndex = Random.Range(0, _subStatsAmount + 1);
                    UpgradeRandomSubStat(subStatIndex);
                    CalculeteStats(_subStats[subStatIndex], _subStatsVulues[subStatIndex]);
                    if(_state == GemState.equiped)
                        _gemManager.CalculeteStats();
                    
                }
            }
            
            DisplayStats();
            _gemManager.UpdateFragmentsDisplay();
        }
    }
}
