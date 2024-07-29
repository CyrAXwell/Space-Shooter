using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpMenu : MonoBehaviour
{
    //public static bool isPaused;
    public GameObject upgrade1;
    public GameObject upgrade2;
    public GameObject upgrade3;
    public GameObject upgrade4;
    [SerializeField] private GameObject upgrade1Outline;
    [SerializeField] private GameObject upgrade2Outline;
    [SerializeField] private GameObject upgrade3Outline;
    [SerializeField] private GameObject upgrade4Outline;

    [SerializeField] private float rerollCountMax;
    private float rerollCount;

    [SerializeField] private Button rerollButton;
    [SerializeField] private GameObject rerollButtonHighlight;
    [SerializeField] AudioSource levelUpSound;
    
    void Start()
    {
        gameObject.SetActive(false);
    }

    // void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.Escape))
    //     {
    //         CloseLevelUpMenu();
    //     }
    // }

    public void OpenLevelUpMenu()
    {
        gameObject.SetActive(true);
        upgrade1.GetComponent<GetUpgrade>().Upgrade();
        upgrade2.GetComponent<GetUpgrade>().Upgrade();
        upgrade3.GetComponent<GetUpgrade>().Upgrade();
        upgrade4.GetComponent<GetUpgrade>().Upgrade();
        upgrade1Outline.SetActive(false);
        upgrade2Outline.SetActive(false);
        upgrade3Outline.SetActive(false);
        upgrade4Outline.SetActive(false);

        rerollButton.interactable = true;
        rerollCount = rerollCountMax;
        rerollButtonHighlight.SetActive(false);
        
        levelUpSound.Play();
        
        Time.timeScale = 0f;
        StateNameController.isPaused = true;
    }

    public void CloseLevelUpMenu()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        StateNameController.isPaused = false;
    }

    public void RerollUpgrades()
    {
        rerollCount --;
        if(rerollCount <= 0)
        {
            rerollButton.interactable = false;
            DeselectRerollButton();
        }
        upgrade1.GetComponent<GetUpgrade>().Upgrade();
        upgrade2.GetComponent<GetUpgrade>().Upgrade();
        upgrade3.GetComponent<GetUpgrade>().Upgrade();
        upgrade4.GetComponent<GetUpgrade>().Upgrade();

    }

    public void SelectRerollButton()
    {
        if(rerollButton.interactable)
        {
            rerollButtonHighlight.SetActive(true);
        }

    }

    public void DeselectRerollButton()
    {
        if(rerollButtonHighlight.activeInHierarchy)
        {
            rerollButtonHighlight.SetActive(false);
        }

    }
}
