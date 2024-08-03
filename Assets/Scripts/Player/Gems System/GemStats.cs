using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GemStats : MonoBehaviour
{

    [SerializeField] private List<GemSO> gemList;

    private int _id;
    private int _baseExp = 100;
    private int _addExp = 50;
    private int _level;
    private int _mainStatValue;

    private Color color; // del

    public string gemName; // change
    public int mainStatIncrease; //del
    public int needExp;
    public int totalExp;


    public List<string> subStatName;
    private List<int> _subStatsVulues = new List<int>();

    private int[] cumulativeProbability = {0, 0, 0, 0};
    private int _subStatsAmount;

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
    }

    public void GetGemStats()
    {
        _id = GetRundomGem();
        _level = 0;
        needExp = _baseExp; 
        totalExp = 0;
        gemName = gemList[_id].gemName;
        _mainStatValue = gemList[_id].gemStat;
        ColorUtility.TryParseHtmlString(gemList[_id].gemNameColor.ToString(), out color);
        CalculeteStats(gemName, _mainStatValue);

        mainStatIncrease = gemList[_id].gemStatIncrease;
        transform.GetComponent<Image>().sprite = gemList[_id].gemIcon;

        GetProbability(gemList[_id].numberSubStatsProbability);
        _subStatsAmount = GetNumberOfStatsByProbability(cumulativeProbability);
        GetSubStats();

    }

    int GetRundomGem()
    {
        return Random.Range(0,gemList.Count);
    }

    void GetSubStats()
    {
        for (int i = 0; i <= _subStatsAmount; i++)
        {
            subStatName.Add(gemList[_id].subStatsNames[Random.Range(0, 5)]);
            switch(subStatName[i])
            {
                case "HP":
                    _subStatsVulues.Add(gemList[_id].subStatsHP[Random.Range(0, 2)]);
                    break;
                
                case "ATK":
                    _subStatsVulues.Add(gemList[_id].subStatsATK[Random.Range(0, 2)]);
                    break;
                
                case "DEF":
                    _subStatsVulues.Add(gemList[_id].subStatsDEF[Random.Range(0, 2)]);
                    break;
                
                case "CRIT DMG":
                    _subStatsVulues.Add(gemList[_id].subStatsCRITDMG[Random.Range(0, 2)]);
                    break;
                
                case "CRIT RATE":
                    _subStatsVulues.Add(gemList[_id].subStatsCRITRate[Random.Range(0, 2)]);
                    break;
                
            }
            CalculeteStats(subStatName[i], _subStatsVulues[i]);
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
        gemInfoPanel.GetComponent<GemTooltip>().SelectGem(this);
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
        gemInfoPanel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "Lvl. " + _level;
        gemInfoPanel.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = gemName.Replace(" GEM","");
        if(gemName == "CRIT DMG GEM" || gemName == "CRIT RATE GEM")
        {
            gemInfoPanel.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = ((float)_mainStatValue / 100).ToString() + "%";
        }else
        {
            gemInfoPanel.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = _mainStatValue.ToString();
        }
        

        for(int i = 0; i < _subStatsVulues.Count; i++)
        {
            gemInfoPanel.transform.GetChild(8+i).gameObject.GetComponent<TMP_Text>().text = "-" + subStatName[i];
            if(subStatName[i] == "CRIT DMG" || subStatName[i] == "CRIT RATE")
            {
                gemInfoPanel.transform.GetChild(4+i).gameObject.GetComponent<TMP_Text>().text = ((float)_subStatsVulues[i] / 100).ToString() + "%";
            }else
            {
                gemInfoPanel.transform.GetChild(4+i).gameObject.GetComponent<TMP_Text>().text = _subStatsVulues[i].ToString();
            }
            
        }

        for(int i = _subStatsVulues.Count; i < 4; i++)
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
        if(_level < 15 && gemManager.gemFragments >= needExp)
        {
            gemManager.gemFragments -= needExp; 
            totalExp = needExp;
            needExp += _addExp;
            _level ++;
            _mainStatValue += mainStatIncrease;
            CalculeteStats(gemName, mainStatIncrease);
            if(isEquip)
            {
                gemManager.GetComponent<GemManager>().CalculeteStats();
            }
            if(_level % 3 == 0)
            {
                if(_subStatsAmount < 3)
                {
                    _subStatsAmount++;
                    
                    subStatName.Add(gemList[_id].subStatsNames[Random.Range(0, 5)]);
                    switch(subStatName[_subStatsAmount])
                    {
                        case "HP":
                            _subStatsVulues.Add(gemList[_id].subStatsHP[Random.Range(0, 2)]);
                            break;
                        
                        case "ATK":
                            _subStatsVulues.Add(gemList[_id].subStatsATK[Random.Range(0, 2)]);
                            break;
                        
                        case "DEF":
                            _subStatsVulues.Add(gemList[_id].subStatsDEF[Random.Range(0, 2)]);
                            break;
                        
                        case "CRIT DMG":
                            _subStatsVulues.Add(gemList[_id].subStatsCRITDMG[Random.Range(0, 2)]);
                            break;
                        
                        case "CRIT RATE":
                            _subStatsVulues.Add(gemList[_id].subStatsCRITRate[Random.Range(0, 2)]);
                            break;
                        
                    }
                    CalculeteStats(subStatName[_subStatsAmount], _subStatsVulues[_subStatsAmount]);
                    if(isEquip)
                    {
                        gemManager.GetComponent<GemManager>().CalculeteStats();
                    }
                }else
                {
                    int num = Random.Range(0, _subStatsAmount + 1);
                    switch(subStatName[num])
                    {
                        case "HP":
                            _subStatsVulues[num] += gemList[_id].subStatsHP[Random.Range(0,3)];
                            break;
                        
                        case "ATK":
                            _subStatsVulues[num] += gemList[_id].subStatsATK[Random.Range(0,3)];
                            break;
                        
                        case "DEF":
                            _subStatsVulues[num] += gemList[_id].subStatsDEF[Random.Range(0,3)];
                            break;
                        
                        case "CRIT DMG":
                            _subStatsVulues[num] += gemList[_id].subStatsCRITDMG[Random.Range(0,3)];
                            break;
                        
                        case "CRIT RATE":
                            _subStatsVulues[num] += gemList[_id].subStatsCRITRate[Random.Range(0,3)];
                            break;
                        
                    }
                    CalculeteStats(subStatName[num], _subStatsVulues[num]);
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
