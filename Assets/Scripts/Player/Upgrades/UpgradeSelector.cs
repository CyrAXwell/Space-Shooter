using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UpgradeSelector : MonoBehaviour
{
    [SerializeField] private List<UpgradeSO> upgradeListChar1;
    [SerializeField] private List<UpgradeSO> upgradeListChar2;
    [SerializeField] private List<UpgradeSO> upgradeListChar3;
    [SerializeField] private List<TierUpgrade> tierUpgrade;

    private List<UpgradeSO> _upgradeList;
    private int[] upgradeProbability;
    private int[] cumulativeProbability = {0, 0, 0, 0};
    private int tier = 0;
    private int upgrade = 0;
    private GameObject player;
    private string newDescription;

    public GameObject type;

    void Awake()
    {
        switch(StateNameController.character)
        {
            default:
                _upgradeList = upgradeListChar1;
                break;
            case "Character 1":
                _upgradeList = upgradeListChar1;
                break;
            case "Character 2":
                _upgradeList = upgradeListChar2;
                break;
            case "Character 3":
                _upgradeList = upgradeListChar3;
                break;
        }
        upgradeProbability = transform.parent.gameObject.GetComponent<UpgradeProbability>().upgradeProbability;
        GetProbability(upgradeProbability);
    }

    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Upgrade()
    {
        tier = GetTierUpgardeByProbability(cumulativeProbability);
        upgrade = GetRandomUpgrade();

        
        transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = _upgradeList[upgrade].Name.ToString();

        transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text =
        "<color=" + tierUpgrade[tier].color + ">Level: " + (tier + 1).ToString() +"</color>";
        
        if(_upgradeList[upgrade].IsPercentageValue)
        {
            newDescription = _upgradeList[upgrade].Description.Replace("X",((float)_upgradeList[upgrade].UpgradeValues.ToArray()[tier] / 100).ToString());
        } else if(_upgradeList[upgrade].IsFloatingValue)
        {
            newDescription = _upgradeList[upgrade].Description.Replace("X",((float)_upgradeList[upgrade].UpgradeValues.ToArray()[tier] / 100).ToString());  
        } else
        {
            newDescription = _upgradeList[upgrade].Description.Replace("X",_upgradeList[upgrade].UpgradeValues.ToArray()[tier].ToString());
        }

        transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = newDescription;

        transform.GetChild(4).gameObject.GetComponent<Image>().sprite = _upgradeList[upgrade].Icon;
        Color color;
        ColorUtility.TryParseHtmlString(tierUpgrade[tier].color.ToString(), out color);
        transform.GetChild(5).gameObject.GetComponent<Image>().color = color; 

    }

    private int GetRandomTier()
    {
        return Random.Range(0,tierUpgrade.Count);
    }

    private int GetRandomUpgrade()
    {
        return Random.Range(0,_upgradeList.Count);
    }

    int GetTierUpgardeByProbability(int[] probability)
    {
        int randomNumber = Random.Range(0, 10001);
        for (int i = 0; i < probability.Length; i++)
        {
            if (randomNumber <= cumulativeProbability[i])
            {
                return i;
            }
        }

        return -1;
    }

    void GetProbability(int[] upgradeProbability)
    {
        int probabilitySum = 0;
        for (int i = 0; i < upgradeProbability.Length; i++)
        {
            probabilitySum += upgradeProbability[i];
            cumulativeProbability[i] = probabilitySum;
        }
    }

    public void ChooseUpgarde()
    {
        if(StateNameController.character == "Character 1" || StateNameController.character == null)
        {
            switch(_upgradeList[upgrade].Name)
            {
                case "Armor upgrade":
                    player.GetComponent<Player>().UpgradeArmor(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 3").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Crit damage upgrade":
                    player.GetComponent<Player>().UpgradeCritDamage(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 4").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Crit rate upgrade":
                    player.GetComponent<Player>().UpgradeCritRate(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 5").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Damage upgrade":
                    player.GetComponent<Player>().UpgradeDamage(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 2").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "HP upgrade":
                    player.GetComponent<Player>().UpgradeHP(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 1").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Shield cooldown upgrade":
                    player.GetComponent<Ship1Shield>().ChangeShieldCD((float)_upgradeList[upgrade].UpgradeValues.ToArray()[tier] / 100);
                    GameObject.Find("LEDS 9").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Shield heal upgrade":
                    player.GetComponent<Ship1Shield>().ChangeShieldHealing(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 10").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Shield HP upgrade":
                    player.GetComponent<Ship1Shield>().ChangeShieldHP(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 8").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Rapid Fire action time upgrade":
                    player.GetComponent<Ship1RapidFire>().ActionTimeUpgarde((float)_upgradeList[upgrade].UpgradeValues.ToArray()[tier] / 100);
                    GameObject.Find("LEDS 17").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Rapid Fire cooldown upgrade":
                    player.GetComponent<Ship1RapidFire>().CooldownUpgarde((float)_upgradeList[upgrade].UpgradeValues.ToArray()[tier] / 100);
                    GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Rapid Fire damage upgrade":
                    player.GetComponent<Ship1RapidFire>().DamageUpgarde(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 15").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                // case "Rapid Fire rate of fire  upgrade":
                //     player.GetComponent<Ship1RapidFire>().RateUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                //     GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(tier);
                //     break;
                
            }
        }

        if(StateNameController.character == "Character 2")
        {
            switch(_upgradeList[upgrade].Name)
            {
                case "Armor upgrade":
                    player.GetComponent<Player>().UpgradeArmor(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 3").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Crit damage upgrade":
                    player.GetComponent<Player>().UpgradeCritDamage(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 4").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Crit rate upgrade":
                    player.GetComponent<Player>().UpgradeCritRate(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 5").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Damage upgrade":
                    player.GetComponent<Player>().UpgradeDamage(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 2").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "HP upgrade":
                    player.GetComponent<Player>().UpgradeHP(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 1").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Shield cooldown upgrade":
                    player.GetComponent<Ship1Shield>().ChangeShieldCD((float)_upgradeList[upgrade].UpgradeValues.ToArray()[tier] / 100);
                    GameObject.Find("LEDS 9").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Shield heal upgrade":
                    player.GetComponent<Ship1Shield>().ChangeShieldHealing(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 10").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Shield HP upgrade":
                    player.GetComponent<Ship1Shield>().ChangeShieldHP(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 8").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Blast Fire action time upgrade":
                    player.GetComponent<Ship2ExplosionBullets>().ActionTimeUpgarde((float)_upgradeList[upgrade].UpgradeValues.ToArray()[tier] / 100);
                    GameObject.Find("LEDS 17").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Blast Fire cooldown upgrade":
                    player.GetComponent<Ship2ExplosionBullets>().CooldownUpgarde((float)_upgradeList[upgrade].UpgradeValues.ToArray()[tier] / 100);
                    GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Blast Fire damage upgrade":
                    player.GetComponent<Ship2ExplosionBullets>().DamageUpgarde(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 15").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                // case "Blast Fire rate of fire upgrade":
                //     player.GetComponent<Ship2ExplosionBullets>().RateUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                //     GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(tier);
                //     break;
                
            }
        }

        if(StateNameController.character == "Character 3")
        {
            switch(_upgradeList[upgrade].Name)
            {
                case "Armor upgrade":
                    player.GetComponent<Player>().UpgradeArmor(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 3").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Crit damage upgrade":
                    player.GetComponent<Player>().UpgradeCritDamage(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 4").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Crit rate upgrade":
                    player.GetComponent<Player>().UpgradeCritRate(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 5").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Damage upgrade":
                    player.GetComponent<Player>().UpgradeDamage(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 2").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "HP upgrade":
                    player.GetComponent<Player>().UpgradeHP(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 1").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Health Regen action time upgrade":
                    player.GetComponent<Ship3Regen>().ActionTimeUpgarde((float)_upgradeList[upgrade].UpgradeValues.ToArray()[tier] / 100);
                    GameObject.Find("LEDS 8").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Health Regen cooldown upgrade":
                    player.GetComponent<Ship3Regen>().CooldownUpgarde((float)_upgradeList[upgrade].UpgradeValues.ToArray()[tier] / 100);
                    GameObject.Find("LEDS 9").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Health Regen heal upgrade":
                    player.GetComponent<Ship3Regen>().HealingUpgarde(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 10").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Laser action time upgrade":
                    player.GetComponent<Ship3Laser>().ActionTimeUpgarde((float)_upgradeList[upgrade].UpgradeValues.ToArray()[tier] / 100);
                    GameObject.Find("LEDS 17").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Laser cooldown upgrade":
                    player.GetComponent<Ship3Laser>().CooldownUpgarde((float)_upgradeList[upgrade].UpgradeValues.ToArray()[tier] / 100);
                    GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Laser damage upgrade":
                    player.GetComponent<Ship3Laser>().DamageUpgarde(_upgradeList[upgrade].UpgradeValues.ToArray()[tier]);
                    GameObject.Find("LEDS 15").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                // case "Laser damage speed upgrade":
                //     player.GetComponent<Ship3Laser>().RateUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                //     GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(tier);
                //     break;
                
            }
        }
        

        transform.parent.gameObject.transform.parent.transform.parent.gameObject.GetComponent<LevelUpMenu>().CloseLevelUpMenu();
    }

}
