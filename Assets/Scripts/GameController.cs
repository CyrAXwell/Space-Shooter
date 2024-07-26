using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
//using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject character1;
    [SerializeField] private GameObject character2;
    [SerializeField] private GameObject character3;
    [SerializeField] private TMP_Text characterName;
    //[SerializeField] private GameObject StartTimer;


    [SerializeField] Sprite[] skillIcons1;
    [SerializeField] Sprite[] skillIcons2;
    [SerializeField] Sprite[] upgradesPanel;

    public GameObject controlsTipsPanel;

    [SerializeField] AudioSource buttonClickSound;

    [SerializeField] Toggle soundToggle;

    void Awake()
    {
        Pause();
        CreateCharacter();
        DisplayControlsTips();
        CheckSoundToggle();
        //StartCoroutine(StartCountDownTimer(2f));
    }

    // void Update()
    // {
    //     Screen.SetResolution(1920, 1080, true);
    //     //Debug.Log("h " + Screen.height);
    //     //Debug.Log("w " + Screen.width);
    // }
    void CreateCharacter()
    {
        switch(StateNameController.character)
        {
            case "Character 1":
                Instantiate(character1, new Vector3(0f, -4.5f, 0f), Quaternion.identity);
                GameObject.Find("Skill 1").transform.GetChild(1).gameObject.GetComponent<Image>().sprite = skillIcons1[0];
                GameObject.Find("Skill 2").transform.GetChild(1).gameObject.GetComponent<Image>().sprite = skillIcons2[0];
                GameObject.Find("Upgrades panel").GetComponent<Image>().sprite = upgradesPanel[0];
                characterName.text = "Hunter";
                break;
            case "Character 2":
                Instantiate(character2, new Vector3(0f, -4.5f, 0f), Quaternion.identity);
                GameObject.Find("Skill 1").transform.GetChild(1).gameObject.GetComponent<Image>().sprite = skillIcons1[0];
                GameObject.Find("Skill 2").transform.GetChild(1).gameObject.GetComponent<Image>().sprite = skillIcons2[1];
                GameObject.Find("Upgrades panel").GetComponent<Image>().sprite = upgradesPanel[1];
                characterName.text = "Guardian";
                break;
            case "Character 3":
                Instantiate(character3, new Vector3(0f, -4.5f, 0f), Quaternion.identity);
                GameObject.Find("Skill 1").transform.GetChild(1).gameObject.GetComponent<Image>().sprite = skillIcons1[1];
                GameObject.Find("Skill 2").transform.GetChild(1).gameObject.GetComponent<Image>().sprite = skillIcons2[2];
                GameObject.Find("Upgrades panel").GetComponent<Image>().sprite = upgradesPanel[2];
                characterName.text = "Destroer";
                break;
            default:
                Instantiate(character1, new Vector3(0f, -4.5f, 0f), Quaternion.identity);
                GameObject.Find("Skill 1").transform.GetChild(1).gameObject.GetComponent<Image>().sprite = skillIcons1[0];
                GameObject.Find("Skill 2").transform.GetChild(1).gameObject.GetComponent<Image>().sprite = skillIcons2[0];
                GameObject.Find("Upgrades panel").GetComponent<Image>().sprite = upgradesPanel[0];
                characterName.text = "Hunter";
                break;
        }

    }

    void DisplayControlsTips()
    {
        controlsTipsPanel.SetActive(true);

    }

    public void CloseControlsTips()
    {
        controlsTipsPanel.SetActive(false);
        Time.timeScale = 1f;
        StateNameController.isPaused = false;
        StateNameController.startTimers = true;

    }

    void Pause()
    {
        Time.timeScale = 0f;
        StateNameController.isPaused = true;
    }

    // private IEnumerator StartCountDownTimer(float interval)
    // {
    //     yield return new WaitForSeconds(interval);
    //     StartTimer.SetActive(true);
    //     StartTimer.GetComponent<TMP_Text>().text = "timer";
    // }

    public void PlaySoundOnButtonClick()
    {
        buttonClickSound.Play();
    }

    public void ChangeSoundState()
    {
        StateNameController.isSoundOff = !StateNameController.isSoundOff;
        if(StateNameController.isSoundOff)
        {
            AudioListener.volume = 0f;
        }else
        {
            AudioListener.volume = 1f;
        }
        
    }
    
    private void CheckSoundToggle()
    {
        if(StateNameController.isSoundOff)
        {
            //AudioListener.volume = 0f;
            soundToggle.isOn = true;//SetValueWithoutNotify(0);
            ChangeSoundState();
            
        }else
        {
            AudioListener.volume = 1f;
            soundToggle.isOn = false;
            
        }
    }

}
