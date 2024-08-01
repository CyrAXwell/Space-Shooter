using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private LevelUpMenu levelUpMenu;
    [SerializeField] private UpgradeSelector[] upgradeSelectors;
    

    private Player _player;
    private ISkillDisplayable[] _skills;
    private List<UpgradeSO> _upgrades = new List<UpgradeSO>();

    public void Initialize(Player player, ISkillDisplayable[] skills)
    {
        _player = player;
        _player.OnLevelUp += OnLevelUp;

        _skills = skills;

        _upgrades.AddRange(_player.GetUpgrades());

        foreach (IUpgradeable skill in skills)
            _upgrades.AddRange(skill.GetUpgrades());
        
        foreach (UpgradeSelector upgradeSelector in upgradeSelectors)
            upgradeSelector.Initialize(_player, _upgrades);

        levelUpMenu.Initialize();
    }

    private void OnLevelUp()
    {
        levelUpMenu.OpenLevelUpMenu();

        foreach (UpgradeSelector upgradeSelector in upgradeSelectors)
            upgradeSelector.OnOpenLevelUpPanel();

    }

    private void OnDisable()
    {
        _player.OnLevelUp -= OnLevelUp;
    }
}
