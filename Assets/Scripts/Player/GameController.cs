using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player character1;
    [SerializeField] private Player character2;
    [SerializeField] private Player character3;
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
    
    private void CreatePlayer(Player character)
    {
        GameObject playerObject = Instantiate(character.gameObject, new Vector3(0f, -4.5f, 0f), Quaternion.identity);
        _player = playerObject.GetComponent<Player>();
        _player.OnDeath += GameOver;

        ExplosionBulletsSkill explosionSkill = playerObject.GetComponent<ExplosionBulletsSkill>();
        if ( explosionSkill!= null)
        {
            explosionSkill.Initialize(_objectPool);
            Debug.Log("Explosion skill");
        }

        characterName.text = _player.GetName();
    }

    private void CreateCharacter()
    {
        switch(StateNameController.character)
        {
            default:
                CreatePlayer(character1);
                break;
            case "Character 1":
                CreatePlayer(character1);    
                break;
            case "Character 2":
                CreatePlayer(character2); 
                break;
            case "Character 3":
                CreatePlayer(character3); 
                break;
        }
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
