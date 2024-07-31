using System.Collections;
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
        StartCoroutine(LoadScenWithDelay(0.19f,2));
    }

    public void BackButton()
    {
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
