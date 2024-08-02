using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private LevelUpMenu levelUpMenu;
    [SerializeField] private UIUpgradeDisplay uiUpgradeDisplay;
    [SerializeField] private Transform[] upgradeSlots;
    [SerializeField] private UpgradeSelector upgradeSelectorPrefab;
    [SerializeField] private TierUpgrade[] tierUpgrade;
    
    private List<UpgradeSelector> _upgradeSelectors = new List<UpgradeSelector>();
    private Player _player;
    private ISkillDisplayable[] _skills;
    private List<UpgradeSO> _upgrades = new List<UpgradeSO>();
    private int[] _upgradeProbability; //{7000, 1500, 1000, 500}; #ffffff #286ad5 #76428a #b52a2a
    private AudioManager _audioManager;

    public void Initialize(Player player, ISkillDisplayable[] skills)
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        _player = player;
        _skills = skills;
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

        uiUpgradeDisplay.Initialize(_player.GetUpgradesPanelSprite());
        levelUpMenu.Initialize();
    }

    private void OnDisable()
    {
        _player.OnLevelUp -= OnLevelUp;
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
        UpgradePlayer(upgradeSelector);
        uiUpgradeDisplay.UpdateDisplay(upgradeSelector.GetUpgradeSlot(), upgradeSelector.GetUpgradeLeds(), upgradeSelector.GetUpgradeTier());

        _audioManager.PlaySFX(_audioManager.ButtonClick);
    }

    private void UpgradePlayer(UpgradeSelector upgradeSelector)
    {
        switch (upgradeSelector.GetSkillType())
        {
            case SkillType.player:
                ActivatePlayerUpgrade(upgradeSelector.GeUpgradeType(), upgradeSelector.GetUpgradeValue());
                break;
            case SkillType.shield:
                ActivateShieldUpgrade(upgradeSelector.GeUpgradeType(), upgradeSelector.GetUpgradeValue());
                break;
            case SkillType.rapidFire:
                ActivateRapidFireUpgrade(upgradeSelector.GeUpgradeType(), upgradeSelector.GetUpgradeValue());
                break;
            case SkillType.explosionBullets:
                ActivateExplosionBulletsUpgrade(upgradeSelector.GeUpgradeType(), upgradeSelector.GetUpgradeValue());
                break;
            case SkillType.laser:
                ActivateLaserUpgrade(upgradeSelector.GeUpgradeType(), upgradeSelector.GetUpgradeValue());
                break;
            case SkillType.regeneration:
                ActivateRegenerationUpgrade(upgradeSelector.GeUpgradeType(), upgradeSelector.GetUpgradeValue());
                break;
        }
    }

    private void ActivatePlayerUpgrade(UpgradeType type, int value)
    {
        switch (type)
        {
            case UpgradeType.health: _player.UpgradeHP(value); break;
            case UpgradeType.armor: _player.UpgradeArmor(value); break;
            case UpgradeType.damage: _player.UpgradeDamage(value); break;
            case UpgradeType.critDamage: _player.UpgradeCritDamage(value); break;
            case UpgradeType.critRate: _player.UpgradeCritRate(value); break;
        }
    }

    private void ActivateShieldUpgrade(UpgradeType type, int value)
    {
        var shieldSkill = _skills.OfType<ShieldSkill>().ToArray()[0];

        switch (type)
        {
            case UpgradeType.cooldown: shieldSkill.UpgradeCooldown((float)value / 100); break;
            case UpgradeType.healing: shieldSkill.UpgradeHealing(value); break;
            case UpgradeType.health: shieldSkill.UpgradeHealth(value); break;
        }
    }

    private void ActivateRapidFireUpgrade(UpgradeType type, int value)
    {
        var rapidFireSkill = _skills.OfType<RapidFireSkill>().ToArray()[0];

        switch (type)
        {
            case UpgradeType.cooldown: rapidFireSkill.UpgradeCooldown((float)value / 100); break;
            case UpgradeType.damage: rapidFireSkill.UpgradeDamage(value); break;
            case UpgradeType.duration: rapidFireSkill.UpgradeDuration((float)value / 100); break;
            case UpgradeType.firerate: rapidFireSkill.UpgradeFireRate(value); break;
        }
    }

    private void ActivateExplosionBulletsUpgrade(UpgradeType type, int value)
    {
        var explosionBulletsSkill = _skills.OfType<ExplosionBulletsSkill>().ToArray()[0];

        switch (type)
        {
            case UpgradeType.cooldown: explosionBulletsSkill.UpgradeCooldown((float)value / 100); break;
            case UpgradeType.damage: explosionBulletsSkill.UpgradeDamage(value); break;
            case UpgradeType.duration: explosionBulletsSkill.UpgradeDuration((float)value / 100); break;
            case UpgradeType.firerate: explosionBulletsSkill.UpgradeFireRate(value); break;
        }
    }

    private void ActivateLaserUpgrade(UpgradeType type, int value)
    {
        var laserSkill = _skills.OfType<LaserSkill>().ToArray()[0];

        switch (type)
        {
            case UpgradeType.cooldown: laserSkill.UpgradeCooldown((float)value / 100); break;
            case UpgradeType.damage: laserSkill.UpgradeDamage(value); break;
            case UpgradeType.duration: laserSkill.UpgradeDuration((float)value / 100); break;
            case UpgradeType.firerate: laserSkill.UpgradeFireRate(value); break;
        }
    }

    private void ActivateRegenerationUpgrade(UpgradeType type, int value)
    {
        var regenerationSkill = _skills.OfType<RegenerationSkill>().ToArray()[0];

        switch (type)
        {
            case UpgradeType.cooldown: regenerationSkill.UpgradeCooldown((float)value / 100); break;
            case UpgradeType.duration: regenerationSkill.UpgradeDurtion((float)value / 100); break;
            case UpgradeType.healing: regenerationSkill.UpgradeHealing(value); break;
        }
    }
    
}
