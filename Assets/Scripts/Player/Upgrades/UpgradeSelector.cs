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
        //ChooseUpgarde();
    }

    public SkillType GetSkillType() => _upgradeList[_upgradeIndex].SkillType;

    public UpgradeType GeUpgradeType() => _upgradeList[_upgradeIndex].UpgradeType;

    public int GetUpgradeValue() => _upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier];

    private int GetRandomUpgrade() => UnityRandom.Range(0, _upgradeList.Count);

    public void ChooseUpgarde()
    {
        if(StateNameController.character == "Character 1" || StateNameController.character == null)
        {
            switch(_upgradeList[_upgradeIndex].Name)
            {
                case "Armor upgrade":
                    _player.GetComponent<Player>().UpgradeArmor(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 3").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Crit damage upgrade":
                    _player.GetComponent<Player>().UpgradeCritDamage(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 4").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Crit rate upgrade":
                    _player.GetComponent<Player>().UpgradeCritRate(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 5").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Damage upgrade":
                    _player.GetComponent<Player>().UpgradeDamage(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 2").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "HP upgrade":
                    _player.GetComponent<Player>().UpgradeHP(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 1").GetComponent<Leds>().LEDUpdate(_tier);
                    break;

                case "Shield cooldown upgrade":
                    _player.GetComponent<ShieldSkill>().UpgradeCooldown((float)_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier] / 100);
                    GameObject.Find("LEDS 9").GetComponent<Leds>().LEDUpdate(_tier);
                    break;

                case "Shield heal upgrade":
                    _player.GetComponent<ShieldSkill>().UpgradeHealing(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 10").GetComponent<Leds>().LEDUpdate(_tier);
                    break;

                case "Shield HP upgrade":
                    _player.GetComponent<ShieldSkill>().UpgradeHealth(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 8").GetComponent<Leds>().LEDUpdate(_tier);
                    break;

                case "Rapid Fire action time upgrade":
                    _player.GetComponent<RapidFireSkill>().UpgradeDuration((float)_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier] / 100);
                    GameObject.Find("LEDS 17").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Rapid Fire cooldown upgrade":
                    _player.GetComponent<RapidFireSkill>().UpgradeCooldown((float)_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier] / 100);
                    GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Rapid Fire damage upgrade":
                    _player.GetComponent<RapidFireSkill>().UpgradeDamage(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 15").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                // case "Rapid Fire rate of fire  upgrade":
                //     player.GetComponent<Ship1RapidFire>().RateUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                //     GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(tier);
                //     break;
                
            }
        }

        if(StateNameController.character == "Character 2")
        {
            switch(_upgradeList[_upgradeIndex].Name)
            {
                case "Armor upgrade":
                    _player.GetComponent<Player>().UpgradeArmor(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 3").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Crit damage upgrade":
                    _player.GetComponent<Player>().UpgradeCritDamage(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 4").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Crit rate upgrade":
                    _player.GetComponent<Player>().UpgradeCritRate(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 5").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Damage upgrade":
                    _player.GetComponent<Player>().UpgradeDamage(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 2").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "HP upgrade":
                    _player.GetComponent<Player>().UpgradeHP(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 1").GetComponent<Leds>().LEDUpdate(_tier);
                    break;

                case "Shield cooldown upgrade":
                    _player.GetComponent<ShieldSkill>().UpgradeCooldown((float)_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier] / 100);
                    GameObject.Find("LEDS 9").GetComponent<Leds>().LEDUpdate(_tier);
                    break;

                case "Shield heal upgrade":
                    _player.GetComponent<ShieldSkill>().UpgradeHealing(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 10").GetComponent<Leds>().LEDUpdate(_tier);
                    break;

                case "Shield HP upgrade":
                    _player.GetComponent<ShieldSkill>().UpgradeHealth(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 8").GetComponent<Leds>().LEDUpdate(_tier);
                    break;

                case "Blast Fire action time upgrade":
                    _player.GetComponent<ExplosionBulletsSkill>().UpgradeDuration((float)_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier] / 100);
                    GameObject.Find("LEDS 17").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Blast Fire cooldown upgrade":
                    _player.GetComponent<ExplosionBulletsSkill>().UpgradeCooldown((float)_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier] / 100);
                    GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Blast Fire damage upgrade":
                    _player.GetComponent<ExplosionBulletsSkill>().UpgradeDamage(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 15").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                // case "Blast Fire rate of fire upgrade":
                //     player.GetComponent<Ship2ExplosionBullets>().RateUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                //     GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(tier);
                //     break;
                
            }
        }

        if(StateNameController.character == "Character 3")
        {
            switch(_upgradeList[_upgradeIndex].Name)
            {
                case "Armor upgrade":
                    _player.GetComponent<Player>().UpgradeArmor(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 3").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Crit damage upgrade":
                    _player.GetComponent<Player>().UpgradeCritDamage(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 4").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Crit rate upgrade":
                    _player.GetComponent<Player>().UpgradeCritRate(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 5").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Damage upgrade":
                    _player.GetComponent<Player>().UpgradeDamage(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 2").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "HP upgrade":
                    _player.GetComponent<Player>().UpgradeHP(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 1").GetComponent<Leds>().LEDUpdate(_tier);
                    break;

                case "Health Regen action time upgrade":
                    _player.GetComponent<RegenerationSkill>().UpgradeDurtion((float)_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier] / 100);
                    GameObject.Find("LEDS 8").GetComponent<Leds>().LEDUpdate(_tier);
                    break;

                case "Health Regen cooldown upgrade":
                    _player.GetComponent<RegenerationSkill>().UpgradeCooldown((float)_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier] / 100);
                    GameObject.Find("LEDS 9").GetComponent<Leds>().LEDUpdate(_tier);
                    break;

                case "Health Regen heal upgrade":
                    _player.GetComponent<RegenerationSkill>().UpgradeHealing(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 10").GetComponent<Leds>().LEDUpdate(_tier);
                    break;

                case "Laser action time upgrade":
                    _player.GetComponent<LaserSkill>().UpgradeDuration((float)_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier] / 100);
                    GameObject.Find("LEDS 17").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Laser cooldown upgrade":
                    _player.GetComponent<LaserSkill>().UpgradeCooldown((float)_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier] / 100);
                    GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                case "Laser damage upgrade":
                    _player.GetComponent<LaserSkill>().UpgradeDamage(_upgradeList[_upgradeIndex].UpgradeValues.ToArray()[_tier]);
                    GameObject.Find("LEDS 15").GetComponent<Leds>().LEDUpdate(_tier);
                    break;
                
                // case "Laser damage speed upgrade":
                //     player.GetComponent<Ship3Laser>().RateUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                //     GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(tier);
                //     break;
                
            }
        }
        

        //transform.parent.gameObject.transform.parent.transform.parent.gameObject.GetComponent<LevelUpMenu>().CloseLevelUpMenu();
    }
}
