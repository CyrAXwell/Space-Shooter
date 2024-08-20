using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject GameOverScreenUI;
    [SerializeField] private GameObject GameWinScreenUI;

    private GameController _gameController;
    private AudioManager _audioManager;

    public void Initialize(GameController gameController)
    {
        _gameController = gameController;
        GameOverScreenUI.SetActive(false);
        GameWinScreenUI.SetActive(false);

        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void OpenGameOverMenu()
    {
        GameOverScreenUI.SetActive(true);
    }

    public void OpenGameWinMenu()
    {
        GameWinScreenUI.SetActive(true);
    }

    public void MainMenu()
    {
        _audioManager.PlaySFX(_audioManager.ButtonClick, 0.7f);
        _gameController.ResumeGame();
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        _audioManager.PlaySFX(_audioManager.ButtonClick, 0.7f);
        Application.Quit();
    }
    
}
