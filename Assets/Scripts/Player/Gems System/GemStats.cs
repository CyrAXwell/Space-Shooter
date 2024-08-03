using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GemStats : MonoBehaviour
{

    [SerializeField] private List<Gem> gemList;

    public int id;
    public string gemName; 
    public int level;
    public int mainStat;
    public int mainStatIncrease;
    private Color color;

    private int baseExp = 100;
    private int addExp = 50;
    public int needExp;
    public int totalExp;


    public List<string> subStatName;
    public List<int> SubStat;

    private int[] cumulativeProbability = {0, 0, 0, 0};
    private int numOfSubStats;

    public int gemHP;
    public int gemATK;
    public int gemDEF;
    public int gemCRITDMG;
    public int gemCRITRate;

    private GameObject gemInfoPanel;

    private Vector3 position1 = new Vector3(-234f, -94.5f, 0f);
    private Vector3 position2 = new Vector3(0, -312f, 0f);
    public bool isReward = true;
    public bool isEquip = false;
    public int slot;
    
    public bool isSelect = false;

    private GemManager gemManager;
    private GameObject gemRewardPanel;

    void Awake()
    {
        gemManager = GameObject.Find("Gems panel").GetComponent<GemManager>();
        gemInfoPanel = gemManager.gemInfoPanel;
        gemRewardPanel = gemManager.gemRewardPanel;
        //Debug.Log(gemInfoPanel);
    }
    public void GetGemStats()
    {
        id = GetRundomGem();
        level = 0;
        needExp = baseExp; 
        totalExp = 0;
        gemName = gemList[id].gemName;
        mainStat = gemList[id].gemStat;
        ColorUtility.TryParseHtmlString(gemList[id].gemNameColor.ToString(), out color);
        CalculeteStats(gemName, mainStat);

        mainStatIncrease = gemList[id].gemStatIncrease;
        transform.GetComponent<Image>().sprite = gemList[id].gemIcon;

        GetProbability(gemList[id].numberSubStatsProbability);
        numOfSubStats = GetNumberOfStatsByProbability(cumulativeProbability);
        GetSubStats();

    }

    int GetRundomGem()
    {
        return Random.Range(0,gemList.Count);
    }

    void GetSubStats()
    {
        for (int i = 0; i <= numOfSubStats; i++)
        {
            subStatName.Add(gemList[id].subStatsNames[Random.Range(0, 5)]);
            switch(subStatName[i])
            {
                case "HP":
                    SubStat.Add(gemList[id].subStatsHP[Random.Range(0, 2)]);
                    break;
                
                case "ATK":
                    SubStat.Add(gemList[id].subStatsATK[Random.Range(0, 2)]);
                    break;
                
                case "DEF":
                    SubStat.Add(gemList[id].subStatsDEF[Random.Range(0, 2)]);
                    break;
                
                case "CRIT DMG":
                    SubStat.Add(gemList[id].subStatsCRITDMG[Random.Range(0, 2)]);
                    break;
                
                case "CRIT RATE":
                    SubStat.Add(gemList[id].subStatsCRITRate[Random.Range(0, 2)]);
                    break;
                
            }
            CalculeteStats(subStatName[i], SubStat[i]);
        }
    }

    
    int GetNumberOfStatsByProbability(int[] probability)
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

    void GetProbability(int[] probability)
    {
        int probabilitySum = 0;
        for (int i = 0; i < probability.Length; i++)
        {
            probabilitySum += probability[i];
            cumulativeProbability[i] = probabilitySum;
        }
    }

    public void CalculeteStats(string stat, int value)
    {
        switch(stat)
            {
                case "HP":
                    gemHP += value;
                    break;
                
                case "ATK":
                    gemATK += value;
                    break;
                
                case "DEF":
                    gemDEF += value;
                    break;
                
                case "CRIT DMG":
                    gemCRITDMG += value;
                    break;
                
                case "CRIT RATE":
                    gemCRITRate += value;
                    break;
                
                case "HP GEM":
                    gemHP += value;
                    break;
                
                case "ATK GEM":
                    gemATK += value;
                    break;
                
                case "DEF GEM":
                    gemDEF += value;
                    break;
                
                case "CRIT DMG GEM":
                    gemCRITDMG += value;
                    break;
                
                case "CRIT RATE GEM":
                    gemCRITRate += value;
                    break;
            }  
    }


    public void DisplayStats()
    {
        SelectGem();
        gemInfoPanel.GetComponent<GemTooltip>().SelectGem(gameObject);
        if(!isEquip)
            {
                gemInfoPanel.transform.GetChild(13).gameObject.SetActive(false);
                gemInfoPanel.transform.GetChild(14).gameObject.SetActive(true);
            }else
            {
                gemInfoPanel.transform.GetChild(13).gameObject.SetActive(true);
                gemInfoPanel.transform.GetChild(14).gameObject.SetActive(false);
            }
        if(!gemInfoPanel.activeInHierarchy)
        {
            gemInfoPanel.SetActive(true);
            // if(isEquip)
            // {
            //     gemInfoPanel.GetComponent<GemTooltip>().UnequipHighlight.SetActive(false);
            // }else
            // {
            //     gemInfoPanel.GetComponent<GemTooltip>().EquipHighlight.SetActive(false);
            // }
            

            gemInfoPanel.GetComponent<GemTooltip>().BreakHighlight.SetActive(false);
            gemInfoPanel.GetComponent<GemTooltip>().UpgardeHighlight.SetActive(false);
            gemInfoPanel.GetComponent<GemTooltip>().CloseHighlight.SetActive(false);
            
        }
        if(gemRewardPanel.activeInHierarchy)
        {
            gemInfoPanel.transform.localPosition = position2;
        }else{
            gemInfoPanel.transform.localPosition = position1;
        }

        gemInfoPanel.GetComponent<GemTooltip>().UnequipHighlight.SetActive(false);
        gemInfoPanel.GetComponent<GemTooltip>().EquipHighlight.SetActive(false);
        gemInfoPanel.transform.GetChild(17).gameObject.SetActive(false);
        gemInfoPanel.transform.GetChild(19).gameObject.SetActive(false);
        
        // if(isReward)
        // {
        //     gemInfoPanel.transform.localPosition = position2;
        // }else
        // {
        //     gemInfoPanel.transform.localPosition = position1;
        // }
        //transform.GetChild(0).gameObject.SetActive(true);
        
        gemInfoPanel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = gemName;
        gemInfoPanel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().color = color;
        gemInfoPanel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "Lvl. " + level;
        gemInfoPanel.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = gemName.Replace(" GEM","");
        if(gemName == "CRIT DMG GEM" || gemName == "CRIT RATE GEM")
        {
            gemInfoPanel.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = ((float)mainStat / 100).ToString() + "%";
        }else
        {
            gemInfoPanel.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = mainStat.ToString();
        }
        

        for(int i = 0; i < SubStat.Count; i++)
        {
            gemInfoPanel.transform.GetChild(8+i).gameObject.GetComponent<TMP_Text>().text = "-" + subStatName[i];
            if(subStatName[i] == "CRIT DMG" || subStatName[i] == "CRIT RATE")
            {
                gemInfoPanel.transform.GetChild(4+i).gameObject.GetComponent<TMP_Text>().text = ((float)SubStat[i] / 100).ToString() + "%";
            }else
            {
                gemInfoPanel.transform.GetChild(4+i).gameObject.GetComponent<TMP_Text>().text = SubStat[i].ToString();
            }
            
        }

        for(int i = SubStat.Count; i < 4; i++)
        {
            gemInfoPanel.transform.GetChild(4+i).gameObject.GetComponent<TMP_Text>().text = "";
            gemInfoPanel.transform.GetChild(8+i).gameObject.GetComponent<TMP_Text>().text = "";
        }

    }


    private void SelectGem()
    {
        if(isEquip)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        
        
        
    }

    public void Upgrade()
    {
        if(level < 15 && gemManager.gemFragments >= needExp)
        {
            gemManager.gemFragments -= needExp; 
            totalExp = needExp;
            needExp += addExp;
            level ++;
            mainStat += mainStatIncrease;
            CalculeteStats(gemName, mainStatIncrease);
            if(isEquip)
            {
                gemManager.GetComponent<GemManager>().CalculeteStats();
            }
            if(level % 3 == 0)
            {
                if(numOfSubStats < 3)
                {
                    numOfSubStats++;
                    
                    subStatName.Add(gemList[id].subStatsNames[Random.Range(0, 5)]);
                    switch(subStatName[numOfSubStats])
                    {
                        case "HP":
                            SubStat.Add(gemList[id].subStatsHP[Random.Range(0, 2)]);
                            break;
                        
                        case "ATK":
                            SubStat.Add(gemList[id].subStatsATK[Random.Range(0, 2)]);
                            break;
                        
                        case "DEF":
                            SubStat.Add(gemList[id].subStatsDEF[Random.Range(0, 2)]);
                            break;
                        
                        case "CRIT DMG":
                            SubStat.Add(gemList[id].subStatsCRITDMG[Random.Range(0, 2)]);
                            break;
                        
                        case "CRIT RATE":
                            SubStat.Add(gemList[id].subStatsCRITRate[Random.Range(0, 2)]);
                            break;
                        
                    }
                    CalculeteStats(subStatName[numOfSubStats], SubStat[numOfSubStats]);
                    if(isEquip)
                    {
                        gemManager.GetComponent<GemManager>().CalculeteStats();
                    }
                }else
                {
                    int num = Random.Range(0, numOfSubStats + 1);
                    switch(subStatName[num])
                    {
                        case "HP":
                            SubStat[num] += gemList[id].subStatsHP[Random.Range(0,3)];
                            break;
                        
                        case "ATK":
                            SubStat[num] += gemList[id].subStatsATK[Random.Range(0,3)];
                            break;
                        
                        case "DEF":
                            SubStat[num] += gemList[id].subStatsDEF[Random.Range(0,3)];
                            break;
                        
                        case "CRIT DMG":
                            SubStat[num] += gemList[id].subStatsCRITDMG[Random.Range(0,3)];
                            break;
                        
                        case "CRIT RATE":
                            SubStat[num] += gemList[id].subStatsCRITRate[Random.Range(0,3)];
                            break;
                        
                    }
                    CalculeteStats(subStatName[num], SubStat[num]);
                    if(isEquip)
                    {
                        gemManager.GetComponent<GemManager>().CalculeteStats();
                    }
                    
                }
            }
            
            DisplayStats();
            gemManager.UpdateFragmentsDisplay();

            
        }

    }





}
