using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player[] characters;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private GameObject ControlsTipsPanel;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private PauseMenu pauseMenu;

    private Player _player;
    private AudioManager _audioManager;
    private WaveManager _waveManager;
    ObjectPoolManager _objectPool;

    public void InitializePlayer(WaveManager waveManager, ObjectPoolManager objectPool)
    {
        PauseGame();
        _objectPool = objectPool;
        CreateCharacter();
        DisplayControlsTips();

        _waveManager = waveManager;
        _waveManager.OnGameWin += GameWin;
        _waveManager.OnBossWaveComplete += GameOver;
        gameOverScreen.Initialize(this);
        pauseMenu.Initialize(this);

        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public Player GetPlayer()
    {
        return _player;
    }

    public void CloseControlsTips()
    {
        _audioManager.PlaySFX(_audioManager.ButtonClick, 0.7f);
        ControlsTipsPanel.SetActive(false);
        ResumeGame();
        StateNameController.startTimers = true;
    }

    private void GameOver()
    {
        _audioManager.PlaySFX(_audioManager.Lose, 0.7f);

        gameOverScreen.OpenGameOverMenu();
        PauseGame();
    }

    private void GameWin()
    {
        _audioManager.PlaySFX(_audioManager.Win, 0.7f);

        gameOverScreen.OpenGameWinMenu();
        PauseGame();
    }

    private void CreateCharacter()
    {
        Player playerPrefab = characters.Where(c => c.GetName() == StateNameController.character).FirstOrDefault();
        if (playerPrefab == null)
            playerPrefab = characters[0];

        GameObject playerObject = Instantiate(playerPrefab.gameObject, new Vector3(0f, -4.5f, 0f), Quaternion.identity);
        _player = playerObject.GetComponent<Player>();
        _player.OnDeath += GameOver;

        ExplosionBulletsSkill explosionSkill = playerObject.GetComponent<ExplosionBulletsSkill>();
        if ( explosionSkill!= null)
        {
            explosionSkill.Initialize(_objectPool);
        }

        characterName.text = _player.GetName();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        StateNameController.isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        StateNameController.isPaused = false;
    }

    private void DisplayControlsTips()
    {
        ControlsTipsPanel.SetActive(true);
    }

    private void OnDisable()
    {
        _waveManager.OnGameWin -= GameWin;
        _waveManager.OnBossWaveComplete -= GameOver; 
        _player.OnDeath -= GameOver;
    }
}
