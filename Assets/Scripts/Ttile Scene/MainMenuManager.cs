using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Toggle soundToggle;

    private AudioManager _audioManager;

    void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        CheckSoundToggle();
    }

    public void PlayButton()
    {
        //SceneManager.LoadScene(1);
        StartCoroutine(LoadScenWithDelay(0.19f,1));
    }

    public void QuitButton()
    {
        _audioManager.PlaySFX(_audioManager.ButtonClick);
        StartCoroutine(GameQuitWithDelay(0.2f));
    }

    public void PlaySoundOnButtonClick()
    {
        _audioManager.PlaySFX(_audioManager.ButtonClick);
    }

    private IEnumerator LoadScenWithDelay(float interval, int scene)
    {
        yield return new WaitForSeconds(interval);
        SceneManager.LoadScene(scene);

    }

    private IEnumerator GameQuitWithDelay(float interval)
    {
        yield return new WaitForSeconds(interval);
        Application.Quit();
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
        Debug.Log(StateNameController.isSoundOff);
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
