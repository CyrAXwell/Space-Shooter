using System;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public event Action OnTimerUpdate;
    public event Action OnWaveComplete;
    public event Action OnBossWaveComplete;
    public event Action OnStartNewWave;
    public event Action OnGameWin;

    [SerializeField] private UIWavePanel uIWavePanel;
    [SerializeField] private BossHealthBar bossHealtBar;
    [SerializeField] private float waveDuration;
    [SerializeField] private float newWaveTimeIncrease;
    [SerializeField] private float maxWaveTime;
    [SerializeField] private GameObject GemPanelBlock;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject gemToolTip;

    private GemManager _gemManager;
    private Player _player;
    private ObjectPoolManager _objectPoolManager;
    private float _timer; 
    private bool _stopTimer = true;
    private bool _isBossWave = false;
    private AudioManager _audioManager;

    [HideInInspector] public int waveCounter;
    

    public void Initialize(GemManager gemManager, Player player, ObjectPoolManager objectPoolManager)
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        _gemManager = gemManager;
        _player = player;
        _objectPoolManager = objectPoolManager;

        _timer = waveDuration;
        _stopTimer = false;

        waveCounter = 1;

        uIWavePanel.Initialize(this, gemManager);
        enemySpawner.Initialize(_player, this, _objectPoolManager);
    }
    
    public int GetWave() => waveCounter;
    public float GetTimer() => _timer;


    private void Update()
    {
        if(!_stopTimer)
        {
            _timer -= Time.deltaTime;
            OnTimerUpdate?.Invoke();
            if(_timer <= 0f)
            {
                _timer = 0f;
                _stopTimer = true;
                
                WaveComplete();
            }
        }
    }

    private void WaveComplete()
    {
        PauseGame(); // gameController

        if(!_isBossWave)
        {
            OnWaveComplete?.Invoke();
            _audioManager.PlaySFX(_audioManager.WaveComplete, 0.5f);
            
            _gemManager.CreateGems();
            GemPanelBlock.SetActive(false);
        }
        else
        {
            OnBossWaveComplete?.Invoke();
        }

        _player.transform.position = new Vector3(0f, -4.5f, 0f);

        switch(StateNameController.character)
        {
            case "Character 1":
                _player.GetComponent<ShieldSkill>().ResetSkill();
                _player.GetComponent<RapidFireSkill>().ResetSkill(); 
                break;
            case "Character 2":
                _player.GetComponent<ShieldSkill>().ResetSkill();
                _player.GetComponent<ExplosionBulletsSkill>().ResetSkill(); 
                break;
            case "Character 3":
                _player.GetComponent<RegenerationSkill>().ResetSkill();
                _player.GetComponent<LaserSkill>().ResetSkill(); 
                break;
            default:
                _player.GetComponent<ShieldSkill>().ResetSkill();
                _player.GetComponent<RapidFireSkill>().ResetSkill(); 
                break;
        }
        _player.FullHeal();

        //ClearObjects(); //spawner
    }

    public void OnBossDeath()
    {
        OnGameWin?.Invoke();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        StateNameController.isPaused = true;
    }

    private void UnPauseGame()
    {
        Time.timeScale = 1f;
        StateNameController.isPaused = false;
    }

    public void StartNextWave()
    {
        _timer = _timer > maxWaveTime ? maxWaveTime : waveDuration +  waveCounter * newWaveTimeIncrease;
        _stopTimer = false;
        waveCounter ++;
        GemPanelBlock.SetActive(true);
        
        if(waveCounter == 2)
            BossWave();
        else
            enemySpawner.UpdateProbability(waveCounter);  

        if(gemToolTip.activeInHierarchy)
            gemToolTip.SetActive(false);

        //ClearObjects();
        _player.FullHeal();

        OnStartNewWave?.Invoke();
        UnPauseGame();
    }

    private void BossWave()
    {
        _timer = 90f;
        _isBossWave = true;
        Boss boss = enemySpawner.BossSpawn(bossHealtBar);
        boss.OnDeath += OnBossDeath;
        enemySpawner.gameObject.SetActive(false);
    }

    public void ClearObjects()
    {
        GameObject[] entities =  GameObject.FindGameObjectsWithTag("Entity");
        foreach(GameObject entity in entities)
        {
            Destroy(entity);
        }
    }
}
