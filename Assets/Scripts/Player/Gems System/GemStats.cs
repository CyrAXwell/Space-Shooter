using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemStats : MonoBehaviour
{
    [SerializeField] private List<GemSO> gemList;

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
    private GameObject _gemRewardPanel;

    public int gemHP;
    public int gemATK;
    public int gemDEF;
    public int gemCRITDMG;
    public int gemCRITRate;

    public bool isReward = true;
    public bool isEquip = false;
    public int slot;
    
    public bool isSelect = false;

    public void Initialize(GemManager gemManager, GemTooltip gemInfoPanel, GameObject gemRewardPanel)
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
        gameObject.name = gemList[_id].gemName;
        _mainStatValue = gemList[_id].gemStat;
        CalculeteStats(gemList[_id].Type, _mainStatValue);

        transform.GetComponent<Image>().sprite = gemList[_id].gemIcon;

        GetProbability(gemList[_id].numberSubStatsProbability);
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
            case GemType.health: _subStatsVulues.Add(gemList[_id].subStatsHP[Random.Range(0, 2)]); break;
            case GemType.damage: _subStatsVulues.Add(gemList[_id].subStatsATK[Random.Range(0, 2)]); break;
            case GemType.defense: _subStatsVulues.Add(gemList[_id].subStatsDEF[Random.Range(0, 2)]); break;
            case GemType.critDamage: _subStatsVulues.Add(gemList[_id].subStatsCRITDMG[Random.Range(0, 2)]); break;
            case GemType.critRate: _subStatsVulues.Add(gemList[_id].subStatsCRITRate[Random.Range(0, 2)]); break;  
        }
    }

    private void UpgradeRandomSubStat(int statIndex)
    {
        switch(_subStats[statIndex])
        {
            case GemType.health: _subStatsVulues[statIndex] += gemList[_id].subStatsHP[Random.Range(0,3)]; break;                      
            case GemType.damage: _subStatsVulues[statIndex] += gemList[_id].subStatsATK[Random.Range(0,3)]; break;                      
            case GemType.defense: _subStatsVulues[statIndex] += gemList[_id].subStatsDEF[Random.Range(0,3)]; break;                       
            case GemType.critDamage: _subStatsVulues[statIndex] += gemList[_id].subStatsCRITDMG[Random.Range(0,3)]; break;    
            case GemType.critRate: _subStatsVulues[statIndex] += gemList[_id].subStatsCRITRate[Random.Range(0,3)]; break;
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
            case GemType.health: gemHP += value; break;
            case GemType.damage: gemATK += value; break;
            case GemType.defense: gemDEF += value; break;          
            case GemType.critDamage: gemCRITDMG += value; break;           
            case GemType.critRate: gemCRITRate += value; break;
        }  
    }

    private void SelectGem()
    {
        if(isEquip)
            transform.GetChild(1).gameObject.SetActive(true);
        else
            transform.GetChild(0).gameObject.SetActive(true);    
    }

    public void DisplayStats()
    {
        SelectGem();
        _gemInfoPanel.OpenGemTooltip(this, _gemRewardPanel.activeInHierarchy);
    }

    public int GetLevel() => _level;
    public string GetName() => gemList[_id].gemName;
    public Color GetColor() => gemList[_id].Color;
    public int GetMainStat() => _mainStatValue;
    public IEnumerable<int> GetSubStatsValues() => _subStatsVulues;
    public IEnumerable<GemType> GetSubStatsTypes() => _subStats;
    public int GetNeedExp() => _needExp;
    public int GetTotalExp() => _totalExp;


    public void Upgrade()
    {
        if(_level < 15 && _gemManager.gemFragments >= _needExp)
        {
            _gemManager.gemFragments -= _needExp; 
            _totalExp = _needExp;
            _needExp += _addExp;
            _level ++;
            _mainStatValue += gemList[_id].gemStatIncrease;
            CalculeteStats(gemList[_id].Type, gemList[_id].gemStatIncrease);

            if(isEquip)
                _gemManager.CalculeteStats();

            if(_level % 3 == 0)
            {
                if(_subStatsAmount < 3)
                {
                    _subStatsAmount++;
                    CreateRandomSubStat(_subStatsAmount);
                    CalculeteStats(_subStats[_subStatsAmount], _subStatsVulues[_subStatsAmount]);

                    if(isEquip)
                        _gemManager.CalculeteStats();

                }else
                {
                    int subStatIndex = Random.Range(0, _subStatsAmount + 1);
                    UpgradeRandomSubStat(subStatIndex);
                    CalculeteStats(_subStats[subStatIndex], _subStatsVulues[subStatIndex]);
                    if(isEquip)
                        _gemManager.GetComponent<GemManager>().CalculeteStats();
                    
                }
            }
            
            DisplayStats();
            _gemManager.UpdateFragmentsDisplay();
        }
    }
}
