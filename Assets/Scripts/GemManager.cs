using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GemManager : MonoBehaviour
{
    private int gemsHP;
    private int gemsATK;
    private int gemsDEF;
    private int gemsCRITDMG;
    private int gemsCRITRate;


    public GameObject gemPrefab;
    public GameObject gemRewardPanel;
    private GameObject gem;
    public List<GameObject> gemSlot;

    private GameObject player;
    public GameObject gemInfoPanel;

    public float gemFragments;
    public GameObject display;

    

    
    [Header("Inventory")]
    public GameObject invenoryContent;
    public GameObject invenorySlotPrefab;
    public List<bool> isEmptySlot;

    public List<GameObject> InventoryGems;

    public static bool isPaused;

    public List<GameObject> gemSlotReward;
    public List<GameObject> gemReward;
    public float numberGemReward;
    private int[] cumulativeProbability = {0, 0, 0, 0, 0};
    [SerializeField] private int[] probability;
    [SerializeField] private Button nextWaveButton;
    


    void Awake()
    {
        
        gemRewardPanel.SetActive(false);
        
        gemInfoPanel.SetActive(false);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateFragmentsDisplay();
    }

    // void Update()
    // {
    //     if(Input.GetKeyDown("g"))
    //     {
    //         CreateGem();
    //     }
        
    // }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    void UnPauseGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
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

    int GetNumberOfGemsByProbability(int[] probability)
    {
        int randomNumber = Random.Range(0, 10001);
        for (int i = 0; i < probability.Length; i++)
        {
            if (randomNumber <= cumulativeProbability[i])
            {
                return i+1;
            }
        }

        return -1;
    }

    public void CreateGem()
    {
        GetProbability(probability);
        numberGemReward = GetNumberOfGemsByProbability(cumulativeProbability);
        
        gemRewardPanel.SetActive(true);
        for(int i = 0; i < numberGemReward; i++)
        {
            gem = Instantiate(gemPrefab,gemSlotReward[i].transform);
            gemReward.Add(gem);
            gem.GetComponent<GemStats>().GetGemStats();
            gem.name = gem.GetComponent<GemStats>().gemName;
        }
        // gem = Instantiate(gemPrefab,gemRewardPanel.transform.GetChild(0));
        gemRewardPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        // gem.GetComponent<GemStats>().GetGemStats();
        gem.name = gem.GetComponent<GemStats>().gemName;
        //PauseGame();
    }   

    public void TakeGem()
    {
        for(int i = 0; i < gemReward.Count; i++)
        {
            Instantiate(invenorySlotPrefab, invenoryContent.transform);
            InventoryGems.Add(gemReward[i]);
            gemReward[i].transform.SetParent(invenoryContent.transform.GetChild(InventoryGems.Count-1), false);
            gemReward[i].GetComponent<GemStats>().isReward = false;

            gemReward[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        gemRewardPanel.SetActive(false);
        if(gem.activeInHierarchy)
        {
            gemInfoPanel.SetActive(false);
            gem.transform.GetChild(0).gameObject.SetActive(false);
        }
        SortGems();
        gemReward.Clear();
        //isEmptySlot.Add(false);

        // Instantiate(invenorySlotPrefab, invenoryContent.transform);
        // InventoryGems.Add(gem);
        // gem.transform.SetParent(invenoryContent.transform.GetChild(InventoryGems.Count-1), false);
        // gem.GetComponent<GemStats>().isReward = false;
        // gemRewardPanel.SetActive(false);

        // if(gem.activeInHierarchy)
        // {
        //     gemInfoPanel.SetActive(false);
        //     gem.transform.GetChild(0).gameObject.SetActive(false);
        // }


        // SortGems();

        //UnPauseGame();
    }

    public void EquipGem(GameObject equipGem)
    {
        
        for (int i = 0; i <= 4; i++)
        {
            if(gemSlot[i].GetComponent<GemSlot>().isEmpty)
            {
                


                equipGem.GetComponent<GemStats>().isEquip = true;
                equipGem.GetComponent<GemStats>().slot = i;

                equipGem.transform.SetParent(gemSlot[i].transform, false);
                gemSlot[i].GetComponent<GemSlot>().isEmpty = false;
                

                if(gemRewardPanel.activeInHierarchy && gemReward.Count == 1)
                {
                    gemRewardPanel.SetActive(false);
                    nextWaveButton.interactable = true;
                }
                gemReward.Remove(equipGem);
                CalculeteStats();
                equipGem.transform.GetChild(0).gameObject.SetActive(false);
                gemInfoPanel.GetComponent<GemTooltip>().EquipHighlight.SetActive(false);
                gemInfoPanel.SetActive(false);
                
                
                // if(isPaused)
                // {
                //     UnPauseGame();
                // }
                if(!equipGem.GetComponent<GemStats>().isReward)
                {
                    InventoryGems.Remove(equipGem);
                    SortGems();
                    Destroy(invenoryContent.transform.GetChild(InventoryGems.Count).gameObject);
                }else
                {
                    equipGem.GetComponent<GemStats>().isReward = false;
                }
                break;
            }
        }
        
        
    }

    public void UnequipGem(GameObject UnequipGem)
    {
        gemSlot[UnequipGem.GetComponent<GemStats>().slot].GetComponent<GemSlot>().isEmpty = true;
        UnequipGem.GetComponent<GemStats>().isEquip = false;
        UnequipGem.transform.GetChild(1).gameObject.SetActive(false);

        Instantiate(invenorySlotPrefab, invenoryContent.transform);
        InventoryGems.Add(UnequipGem);
        UnequipGem.transform.SetParent(invenoryContent.transform.GetChild(InventoryGems.Count-1), false);
        //UnequipGem.transform.GetChild(0).gameObject.SetActive(true);

        SortGems();
        CalculeteStats();
        gemInfoPanel.SetActive(false);
        //UnequipGem.GetComponent<GemStats>().DisplayStats();

    }

    public void CalculeteStats()
    {
        gemsHP = 0;
        gemsATK = 0;
        gemsDEF = 0;
        gemsCRITDMG = 0;
        gemsCRITRate = 0;

        for (int i = 0; i <= 4; i++)
        {
            if(!gemSlot[i].GetComponent<GemSlot>().isEmpty)
            {
                gemsHP += gemSlot[i].transform.GetChild(0).gameObject.GetComponent<GemStats>().gemHP;
                gemsATK += gemSlot[i].transform.GetChild(0).gameObject.GetComponent<GemStats>().gemATK;
                gemsDEF += gemSlot[i].transform.GetChild(0).gameObject.GetComponent<GemStats>().gemDEF;
                gemsCRITDMG += gemSlot[i].transform.GetChild(0).gameObject.GetComponent<GemStats>().gemCRITDMG;
                gemsCRITRate += gemSlot[i].transform.GetChild(0).gameObject.GetComponent<GemStats>().gemCRITRate;
                
                
            } 
        }
        Debug.Log("gem " +gemsATK);
        player.GetComponent<Player>().UpdateGemStats(gemsHP, gemsATK, gemsDEF, gemsCRITDMG, gemsCRITRate);
    }


    public void BreakGem(GameObject gem)
    {
        gemFragments += 500 + gem.GetComponent<GemStats>().totalExp*0.8f;
        
        UpdateFragmentsDisplay();
        //DeleteInventorySlot(gem);
        
        if(gem.GetComponent<GemStats>().isReward)
        {
            Destroy(gem);
        }else if(gem.GetComponent<GemStats>().isEquip)
        {
            gem.GetComponent<GemStats>().isEquip = false;
            gemSlot[gem.GetComponent<GemStats>().slot].GetComponent<GemSlot>().isEmpty = true;
            Destroy(gem);
            CalculeteStats();
        }else
        {
            InventoryGems.Remove(gem);
            Destroy(gem);
            SortGems();
            Destroy(invenoryContent.transform.GetChild(InventoryGems.Count).gameObject);
            //Debug.Log("sort");
        }
        
        if(gemRewardPanel.activeInHierarchy && gemReward.Count == 1)
        {
            gemRewardPanel.SetActive(false);
        }
        gemReward.Remove(gem);
        gemInfoPanel.SetActive(false);

           
        
        // if(isPaused)
        // {
        //     UnPauseGame();
        // }
        //break;
    }

    public void UpdateFragmentsDisplay()
    {
        display.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = gemFragments.ToString();
    }

    public void SortGems()
    {
        InventoryGems = InventoryGems.OrderBy(g => g.name).ToList();
        for(int i = 0; i < InventoryGems.Count; i++)
        {
            
            InventoryGems[i].transform.SetParent(invenoryContent.transform.GetChild(i), false);
        }
    }

    public void DeleteInventorySlot(GameObject delGem)
    {
        Destroy(invenoryContent.transform.GetChild(InventoryGems.Count-1).gameObject);
        Debug.Log(invenoryContent.transform.GetChild(InventoryGems.Count-1));
        //InventoryGems.Remove(delGem);
    }
}
