using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    //public static bool isPaused;
    private bool isAlredyPaused = false;
    private bool menuIsOpen = false;

    public GameObject pauseMenuUI;
    [SerializeField] GameObject tipsPanel;
    [SerializeField] AudioSource buttonSound;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!menuIsOpen)
            {
                if(!tipsPanel.activeInHierarchy)
                {
                    Pause(); 
                }
            }else{
                Resume();
            }
        }
    }

    public void Resume()
    {
        //buttonSound.Play();
        pauseMenuUI.SetActive(false);
        menuIsOpen = false;
        if(!isAlredyPaused)
        {
            Time.timeScale = 1f;
            StateNameController.isPaused = false;  
        }
        
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        menuIsOpen = true;
        if(StateNameController.isPaused)
        {
            isAlredyPaused = true;
        }else{
            isAlredyPaused = false;
            Time.timeScale = 0f;
            StateNameController.isPaused = true;
        }
        
    }
}
