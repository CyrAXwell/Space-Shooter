using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private LevelUpMenu levelUpMenu;
    [SerializeField] private Transform[] upgradeSlots;
    [SerializeField] private UpgradeSelector upgradeSelectorPrefab;
    [SerializeField] private TierUpgrade[] tierUpgrade;
    
    private List<UpgradeSelector> _upgradeSelectors = new List<UpgradeSelector>();
    private Player _player;
    private List<UpgradeSO> _upgrades = new List<UpgradeSO>();
    private int[] _upgradeProbability; //{7000, 1500, 1000, 500}; #ffffff #286ad5 #76428a #b52a2a

    public void Initialize(Player player, ISkillDisplayable[] skills)
    {
        _player = player;
        _player.OnLevelUp += OnLevelUp;

        _upgrades.AddRange(_player.GetUpgrades());

        foreach (IUpgradeable skill in skills)
            _upgrades.AddRange(skill.GetUpgrades());
        
        _upgradeProbability = new int[tierUpgrade.Length];
        for (int i = 0; i < tierUpgrade.Length; i++)
            _upgradeProbability[i] = tierUpgrade[0].Probability;

        foreach (Transform slot in upgradeSlots)
        {
            UpgradeSelector upgrade = Instantiate(upgradeSelectorPrefab, slot);
            upgrade.Initialize(_player, _upgrades, tierUpgrade, _upgradeProbability);
            upgrade.OnClick += OnSelectorClick;
            _upgradeSelectors.Add(upgrade);
        }   
        levelUpMenu.Initialize();
    }

    private void OnLevelUp()
    {
        levelUpMenu.OpenLevelUpMenu();

        foreach (UpgradeSelector upgradeSelector in _upgradeSelectors)
            upgradeSelector.OnOpenLevelUpPanel();
    }

    private void OnSelectorClick(UpgradeSelector upgradeSelector)
    {
        levelUpMenu.CloseLevelUpMenu();
    }

    private void OnDisable()
    {
        _player.OnLevelUp -= OnLevelUp;
    }
}
