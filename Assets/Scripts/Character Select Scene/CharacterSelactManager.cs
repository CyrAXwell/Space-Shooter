using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class CharacterSelactManager : MonoBehaviour
{
    [SerializeField] private GameObject characterSelectPanel;

    private ToggleGroup _toggleGroup;
    private AudioManager _audioManager;

    void Start()
    {
        _toggleGroup = characterSelectPanel.GetComponent<ToggleGroup>();
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void StartButton()
    {
        Toggle toggle = _toggleGroup.ActiveToggles().FirstOrDefault();
        StateNameController.character = toggle.name;
        StateNameController.startTimers = false;
        StartCoroutine(LoadScenWithDelay(0.19f,2));
    }

    public void BackButton()
    {
        StartCoroutine(LoadScenWithDelay(0.19f,0));
    }

    private IEnumerator LoadScenWithDelay(float interval, int scene)
    {
        yield return new WaitForSeconds(interval);
        SceneManager.LoadScene(scene);
    }

    public void PlaySoundOnSelectCharracter() =>
        _audioManager.PlaySFX(_audioManager.CharacterSelection);

    public void PlaySoundOnPlayButton() =>
        _audioManager.PlaySFX(_audioManager.ButtonClick);

    public void PlaySoundOnBackButton() =>
        _audioManager.PlaySFX(_audioManager.BackButtonClick);
}
