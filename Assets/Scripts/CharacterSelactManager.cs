using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class CharacterSelactManager : MonoBehaviour
{

    [SerializeField] private GameObject characterSelectPanel;
    ToggleGroup toggleGroup;

    [SerializeField] AudioSource characterSelectSound;
    [SerializeField] AudioSource buttonPlaySound;
    [SerializeField] AudioSource buttonBackSound;

    void Start()
    {
        toggleGroup = characterSelectPanel.GetComponent<ToggleGroup>();
    }

    public void StartButton()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        StateNameController.character = toggle.name;
        StateNameController.startTimers = false;
        //SceneManager.LoadScene(2);
        StartCoroutine(LoadScenWithDelay(0.19f,2));
    }

    public void BackButton()
    {
        //Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        //Debug.Log(toggle.name);
        //StateNameController.cahracter = toggle.name;
        //Debug.Log(StateNameController.cahracter);

        //SceneManager.LoadScene(0);
        StartCoroutine(LoadScenWithDelay(0.19f,0));
    }

    public void PlaySoundOnSelectCharracter()
    {
        characterSelectSound.Play();
    }

    public void PlaySoundOnPlayButton()
    {
        buttonPlaySound.Play();
    }

    public void PlaySoundOnBackButton()
    {
        buttonBackSound.Play();
    }

    private IEnumerator LoadScenWithDelay(float interval, int scene)
    {
        yield return new WaitForSeconds(interval);
        SceneManager.LoadScene(scene);

    }
}
