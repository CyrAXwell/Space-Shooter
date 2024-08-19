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
        soundToggle.SetIsOnWithoutNotify(StateNameController.isSoundOff);
    }

    public void PlayButton()
    {
        _audioManager.PlaySFX(_audioManager.ButtonClick, 0.7f);
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        _audioManager.PlaySFX(_audioManager.ButtonClick, 0.7f);
        Application.Quit();
    }

    public void OnMuteSoundButton()
    {
        StateNameController.isSoundOff = !StateNameController.isSoundOff;
        _audioManager.MuteSound(StateNameController.isSoundOff);
    }
}
