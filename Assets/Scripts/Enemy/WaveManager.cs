using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{

    [SerializeField] private TMP_Text waveTimer;
    [NonSerialized] public bool startCount = false;
    [SerializeField] private float waveTime;
    [SerializeField] private float waveTimeAdd;
    [SerializeField] private float MaxWaveTime;
    private float time; 

    [SerializeField] private TMP_Text waveDisplay;
    [HideInInspector] public int waveCounter;
    
    [SerializeField] private Image[] stageLeds;
    private Color clearStageColor;
    private Color currentStageColor;

    //public static bool isPaused;
    [SerializeField] private GameObject WaveComoletePanel;
    private GemManager gemManager;
    [SerializeField] private GameObject GemPanelBlock;
    private GameObject player;
    [SerializeField] private GameObject gemRewardPanel;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject gemToolTip;

    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject bossHealtBar;
    private bool isBossWave = false;
    [SerializeField] AudioSource waveCompleteSound;

    void Awake()
    {
        gemManager = GameObject.Find("Gems panel").GetComponent<GemManager>();
        WaveComoletePanel.SetActive(false);
    }

    void Start()
    {
        time = waveTime;
        startCount = true;

        waveCounter = 1;
        waveDisplay.text = "Wave " + waveCounter.ToString() + "/20";

        ColorUtility.TryParseHtmlString("#286ad5", out clearStageColor);
        ColorUtility.TryParseHtmlString("#6abe30", out currentStageColor);
        stageLeds[waveCounter - 1].color = currentStageColor;

        
        player = GameObject.FindWithTag("Player");

    }

    void Update()
    {
        waveTimer.text = Mathf.Round(time).ToString();

    }

    void FixedUpdate()
    {
        if(startCount)
        {
            time -= Time.fixedDeltaTime;
            if(time <= 0f)
            {
                time = 0f;
                startCount = false;
                //waveCounter ++;
                stageLeds[waveCounter - 1].color = clearStageColor;
                WaveComplete();
                
            }

        }
    }

    void WaveComplete()
    {
        ClearObjects();
        PauseGame();
        
        if(!isBossWave)
        {
            waveCompleteSound.Play();
            
            WaveComoletePanel.SetActive(true);
            WaveComoletePanel.transform.GetChild(3).GetComponent<Button>().interactable = false;
            WaveComoletePanel.transform.GetChild(2).gameObject.SetActive(false);
            gemManager.CreateGem();
            GemPanelBlock.SetActive(false);
        }else
        {
            ClearObjects();
            BossWaveComplete();
            ClearObjects();
        }
        
        ClearObjects();
        player.transform.position = new Vector3(0f, -4.5f, 0f);

        switch(StateNameController.character)
        {
            case "Character 1":
                player.GetComponent<Ship1Shield>().ResetShieldSkill();
                player.GetComponent<Ship1RapidFire>().ResetRapidFireSkill(); 
                break;
            case "Character 2":
                player.GetComponent<Ship1Shield>().ResetShieldSkill();
                player.GetComponent<Ship2ExplosionBullets>().ResetSkill(); 
                break;
            case "Character 3":
                player.GetComponent<Ship3Regen>().ResetSkill();
                player.GetComponent<Ship3Laser>().ResetSkill(); 
                break;
            default:
                player.GetComponent<Ship1Shield>().ResetShieldSkill();
                player.GetComponent<Ship1RapidFire>().ResetRapidFireSkill(); 
                break;
        }
        player.GetComponent<Player>().MaxHeal();

    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        StateNameController.isPaused = true;
    }

    void UnPauseGame()
    {
        Time.timeScale = 1f;
        StateNameController.isPaused = false;
    }

    public void StartNextWave()
    {
        
        time = waveTime +  waveCounter * waveTimeAdd;
        if(time > MaxWaveTime)
        {
           time = MaxWaveTime;
        }
        waveCounter ++;
        startCount = true;
        GemPanelBlock.SetActive(true);
        WaveComoletePanel.transform.GetChild(3).GetComponent<Button>().interactable = false;
        
        if(waveCounter == 20)
        {
            BossWave();
        }else
        {
            enemySpawner.UpdateProbability(waveCounter);
        }
        

        if(gemToolTip.activeInHierarchy)
        {
            gemToolTip.SetActive(false);
        }

        ClearObjects();
        player.GetComponent<Player>().MaxHeal();
        UpdateWaveDisplay();
        UnPauseGame();
    }

    void BossWave()
    {
        time = 90f;
        isBossWave = true;
        bossHealtBar.SetActive(true);
        GameObject.Find("EnemySpawner").SetActive(false);
        Instantiate(bossPrefab,new Vector3(0f, 3.54f, 0f), Quaternion.identity);
        
    }

    void UpdateWaveDisplay()
    {
        waveDisplay.text = "Wave " + waveCounter.ToString() + "/20";
        stageLeds[waveCounter - 1].color = currentStageColor;
    }

    public void ClearObjects()
    {
        GameObject[] surviveEnemies =  GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject surviveEnemy in surviveEnemies)
        {
            Destroy(surviveEnemy);
        }

        GameObject[] entities =  GameObject.FindGameObjectsWithTag("Entity");
        foreach(GameObject entity in entities)
        {
            Destroy(entity);
        }
    }

    public void ButtonHighlightOn()
    {
        if(!gemRewardPanel.activeInHierarchy)
        {
            WaveComoletePanel.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    public void ButtonHighlightOff()
    {
        WaveComoletePanel.transform.GetChild(2).gameObject.SetActive(false);
    }

    private void BossWaveComplete()
    {
        GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().Death();
    }
    // void RewardPanelIsClose()
    // {
    //     transform.GetChild(3).GetComponent<Button>().interactable = true;
    // }
}
