using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GetUpgrade : MonoBehaviour
{
    [SerializeField] private List<Upgrade> upgradeListChar1;
    [SerializeField] private List<Upgrade> upgradeListChar2;
    [SerializeField] private List<Upgrade> upgradeListChar3;
    private List<Upgrade> upgradeList;
    [SerializeField] private List<TierUpgrade> tierUpgrade;

    public GameObject type;

    private int[] upgradeProbability;
    private int[] cumulativeProbability = {0, 0, 0, 0};
    private int tier = 0;
    private int upgrade = 0;

    private GameObject player;
    private string newDescription;

    void Awake()
    {
        switch(StateNameController.character)
        {
            case "Character 1":
                upgradeList = upgradeListChar1;
                break;
            case "Character 2":
                upgradeList = upgradeListChar2;
                break;
            case "Character 3":
                upgradeList = upgradeListChar3;
                break;
            default:
                upgradeList = upgradeListChar1;
                break;
        }
        upgradeProbability = transform.parent.gameObject.GetComponent<UpgradeProbability>().upgradeProbability;
        GetProbability(upgradeProbability);
        //TMP_Text typeText = type.GetComponent<TMP_Text>();
        //Upgrade();
    }

    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // void Update()
    // {
    //     if(Input.GetKeyDown("u"))
    //     {
    //         Upgrade();
    //     }
    // }

    public void Upgrade()
    {
        tier = GetTierUpgardeByProbability(cumulativeProbability);
        upgrade = GetRandomUpgrade();

        
        transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = upgradeList[upgrade].nameUpgrade.ToString();

        transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text =
        "<color=" + tierUpgrade[tier].color + ">Level: " + (tier + 1).ToString() +"</color>";
        
        if(upgradeList[upgrade].percentage)
        {
            newDescription = upgradeList[upgrade].description.Replace("X",((float)upgradeList[upgrade].value[tier] / 100).ToString());
        } else if(upgradeList[upgrade].floating)
        {
            newDescription = upgradeList[upgrade].description.Replace("X",((float)upgradeList[upgrade].value[tier] / 100).ToString());  
        } else
        {
            newDescription = upgradeList[upgrade].description.Replace("X",upgradeList[upgrade].value[tier].ToString());
        }

        

        transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = newDescription;

        transform.GetChild(4).gameObject.GetComponent<Image>().sprite = upgradeList[upgrade].upgradeIcon;
        Color color;
        ColorUtility.TryParseHtmlString(tierUpgrade[tier].color.ToString(), out color);
        transform.GetChild(5).gameObject.GetComponent<Image>().color = color; 

        // transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text.Replace("X","5");
        // Debug.Log(childObject);
        //Debug.Log(tier);
        //Debug.Log(upgrade);

    }

    private int GetRandomTier()
    {
        return Random.Range(0,tierUpgrade.Count);
    }

    private int GetRandomUpgrade()
    {
        return Random.Range(0,upgradeList.Count);
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
            switch(upgradeList[upgrade].nameUpgrade)
            {
                case "Armor upgrade":
                    player.GetComponent<Player>().UpgradeArmor(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 3").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Crit damage upgrade":
                    player.GetComponent<Player>().UpgradeCritDamage(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 4").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Crit rate upgrade":
                    player.GetComponent<Player>().UpgradeCritRate(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 5").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Damage upgrade":
                    player.GetComponent<Player>().UpgradeDamage(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 2").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "HP upgrade":
                    player.GetComponent<Player>().UpgradeHP(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 1").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Shield cooldown upgrade":
                    player.GetComponent<Ship1Shield>().ChangeShieldCD((float)upgradeList[upgrade].value[tier] / 100);
                    GameObject.Find("LEDS 9").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Shield heal upgrade":
                    player.GetComponent<Ship1Shield>().ChangeShieldHealing(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 10").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Shield HP upgrade":
                    player.GetComponent<Ship1Shield>().ChangeShieldHP(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 8").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Rapid Fire action time upgrade":
                    player.GetComponent<Ship1RapidFire>().ActionTimeUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                    GameObject.Find("LEDS 17").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Rapid Fire cooldown upgrade":
                    player.GetComponent<Ship1RapidFire>().CooldownUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                    GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Rapid Fire damage upgrade":
                    player.GetComponent<Ship1RapidFire>().DamageUpgarde(upgradeList[upgrade].value[tier]);
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
            switch(upgradeList[upgrade].nameUpgrade)
            {
                case "Armor upgrade":
                    player.GetComponent<Player>().UpgradeArmor(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 3").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Crit damage upgrade":
                    player.GetComponent<Player>().UpgradeCritDamage(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 4").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Crit rate upgrade":
                    player.GetComponent<Player>().UpgradeCritRate(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 5").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Damage upgrade":
                    player.GetComponent<Player>().UpgradeDamage(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 2").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "HP upgrade":
                    player.GetComponent<Player>().UpgradeHP(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 1").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Shield cooldown upgrade":
                    player.GetComponent<Ship1Shield>().ChangeShieldCD((float)upgradeList[upgrade].value[tier] / 100);
                    GameObject.Find("LEDS 9").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Shield heal upgrade":
                    player.GetComponent<Ship1Shield>().ChangeShieldHealing(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 10").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Shield HP upgrade":
                    player.GetComponent<Ship1Shield>().ChangeShieldHP(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 8").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Blast Fire action time upgrade":
                    player.GetComponent<Ship2ExplosionBullets>().ActionTimeUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                    GameObject.Find("LEDS 17").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Blast Fire cooldown upgrade":
                    player.GetComponent<Ship2ExplosionBullets>().CooldownUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                    GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Blast Fire damage upgrade":
                    player.GetComponent<Ship2ExplosionBullets>().DamageUpgarde(upgradeList[upgrade].value[tier]);
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
            switch(upgradeList[upgrade].nameUpgrade)
            {
                case "Armor upgrade":
                    player.GetComponent<Player>().UpgradeArmor(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 3").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Crit damage upgrade":
                    player.GetComponent<Player>().UpgradeCritDamage(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 4").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Crit rate upgrade":
                    player.GetComponent<Player>().UpgradeCritRate(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 5").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Damage upgrade":
                    player.GetComponent<Player>().UpgradeDamage(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 2").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "HP upgrade":
                    player.GetComponent<Player>().UpgradeHP(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 1").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Health Regen action time upgrade":
                    player.GetComponent<Ship3Regen>().ActionTimeUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                    GameObject.Find("LEDS 8").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Health Regen cooldown upgrade":
                    player.GetComponent<Ship3Regen>().CooldownUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                    GameObject.Find("LEDS 9").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Health Regen heal upgrade":
                    player.GetComponent<Ship3Regen>().HealingUpgarde(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 10").GetComponent<Leds>().LEDUpdate(tier);
                    break;

                case "Laser action time upgrade":
                    player.GetComponent<Ship3Laser>().ActionTimeUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                    GameObject.Find("LEDS 17").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Laser cooldown upgrade":
                    player.GetComponent<Ship3Laser>().CooldownUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                    GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                case "Laser damage upgrade":
                    player.GetComponent<Ship3Laser>().DamageUpgarde(upgradeList[upgrade].value[tier]);
                    GameObject.Find("LEDS 15").GetComponent<Leds>().LEDUpdate(tier);
                    break;
                
                // case "Laser damage speed upgrade":
                //     player.GetComponent<Ship3Laser>().RateUpgarde((float)upgradeList[upgrade].value[tier] / 100);
                //     GameObject.Find("LEDS 16").GetComponent<Leds>().LEDUpdate(tier);
                //     break;
                
            }
        }
        

        transform.parent.gameObject.transform.parent.gameObject.GetComponent<LevelUpMenu>().CloseLevelUpMenu();
    }

}
