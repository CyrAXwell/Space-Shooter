using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class CharacterSelactManager : MonoBehaviour
{
    [SerializeField] private ToggleGroup toggleGroup;

    private AudioManager _audioManager;

    void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void StartButton()
    {
        _audioManager.PlaySFX(_audioManager.ButtonClick, 0.7f);
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        StateNameController.character = toggle.GetComponent<CharacterSelector>().GetPlayerName();
        StateNameController.startTimers = false;
        SceneManager.LoadScene(2);
    }

    public void BackButton()
    {
        _audioManager.PlaySFX(_audioManager.BackButtonClick);
        SceneManager.LoadScene(0);
    }

    public void PlaySoundOnSelectCharracter() =>
        _audioManager.PlaySFX(_audioManager.CharacterSelection, 0.7f);
}
