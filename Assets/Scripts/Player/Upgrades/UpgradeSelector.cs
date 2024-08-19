using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;
using System;
using UnityRandom = UnityEngine.Random;

public class UpgradeSelector : MonoBehaviour, IPointerClickHandler
{
    public event Action<UpgradeSelector> OnClick;

    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text tierTMP;
    [SerializeField] private TMP_Text descriptionTMP;
    [SerializeField] private Image icon;
    [SerializeField] private Image outline;

    private TierUpgrade[] _tierUpgrade;
    private List<UpgradeSO> _upgradeList;
    private int[] _cumulativeProbability = {0, 0, 0, 0};
    private int _tier = 0;
    private int _upgradeIndex = 0;
    private Player _player;

    public void Initialize(Player player, List<UpgradeSO> upgrades, TierUpgrade[] tierUpgrade, int[] upgradeProbability)
    {
        _player = player;
        _upgradeList = new List<UpgradeSO>(upgrades);

        _tierUpgrade = tierUpgrade;

        GetProbability(upgradeProbability);
        _upgradeIndex = GetRandomUpgrade();
    }

    public void OnOpenLevelUpPanel()
    {
        SetUpgrade();
    }

    public void SetUpgrade()
    {
        _tier = GetTierUpgardeByProbability(_cumulativeProbability);
        _upgradeIndex = GetRandomUpgrade();

        nameTMP.text = _upgradeList[_upgradeIndex].Name.ToString();

        tierTMP.text = "Level: " + (_tier + 1).ToString();
        tierTMP.color = _tierUpgrade[_tier].Color;

        if(_upgradeList[_upgradeIndex].IsPercentageValue || _upgradeList[_upgradeIndex].IsFloatingValue)
            descriptionTMP.text = _upgradeList[_upgradeIndex].Description.Replace("X",((float)_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier] / 100).ToString());
        else
            descriptionTMP.text = _upgradeList[_upgradeIndex].Description.Replace("X",_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier].ToString());

        icon.sprite = _upgradeList[_upgradeIndex].Icon;

        outline.color = _tierUpgrade[_tier].Color; 
    }

    private void GetProbability(int[] upgradeProbability)
    {
        int probabilitySum = 0;
        for (int i = 0; i < upgradeProbability.Length; i++)
        {
            probabilitySum += upgradeProbability[i];
            _cumulativeProbability[i] = probabilitySum;
        }
    }

    private int GetTierUpgardeByProbability(int[] probability)
    {
        int randomNumber = UnityRandom.Range(0, 10001);
        for (int i = 0; i < probability.Length; i++)
        {
            if (randomNumber <= _cumulativeProbability[i])
                return i;
        }

        return -1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }

    public SkillType GetSkillType() => _upgradeList[_upgradeIndex].SkillType;
    public UpgradeType GeUpgradeType() => _upgradeList[_upgradeIndex].UpgradeType;
    public int GetUpgradeValue() => _upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier];
    public int GetUpgradeSlot() =>  _upgradeList[_upgradeIndex].Slot;
    public int GetUpgradeLeds() =>  _upgradeList[_upgradeIndex].Leds;
    public int GetUpgradeTier() =>  _tier;
    private int GetRandomUpgrade() => UnityRandom.Range(0, _upgradeList.Count);
}
