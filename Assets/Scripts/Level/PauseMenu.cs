using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Toggle soundToggle;

    private GameController _gameController;
    private AudioManager _audioManager;

    public void Initialize(GameController gameController)
    {
        _gameController = gameController;
        pauseMenuUI.SetActive(false);

        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        soundToggle.SetIsOnWithoutNotify(StateNameController.isSoundOff);
    }

    public void OnResumeButton()
    {
        _audioManager.PlaySFX(_audioManager.ButtonClick, 0.7f);
        ResumeGame();
    }

    public void OnMainMenuButton()
    {
        _audioManager.PlaySFX(_audioManager.ButtonClick, 0.7f);
        _gameController.ResumeGame();
        SceneManager.LoadScene(0);
    }

    public void OnGameQuitButton()
    {
        _audioManager.PlaySFX(_audioManager.ButtonClick, 0.7f);
        Application.Quit();
    }

    public void OnMuteSoundButton()
    {
        Debug.Log("mute");
        StateNameController.isSoundOff = !StateNameController.isSoundOff;
        _audioManager.MuteSound(StateNameController.isSoundOff);
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (StateNameController.startTimers && !StateNameController.isPaused && !pauseMenuUI.activeInHierarchy)
                PauseGame();
            else if (StateNameController.startTimers && StateNameController.isPaused && pauseMenuUI.activeInHierarchy)
                ResumeGame();
        }
    }

    private void PauseGame()
    {
        _gameController.PauseGame();
        pauseMenuUI.SetActive(true);
    }

    private void ResumeGame()
    {
        _gameController.ResumeGame();
        pauseMenuUI.SetActive(false);
    }
}
