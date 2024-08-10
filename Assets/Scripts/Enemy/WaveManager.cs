using System;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public event Action OnTimerUpdate;
    public event Action OnWaveComplete;
    public event Action OnBossWaveComplete;
    public event Action OnStartNewWave;

    [SerializeField] private UIWavePanel uIWavePanel;
    [SerializeField] private float waveDuration;
    [SerializeField] private float newWaveTimeIncrease;
    [SerializeField] private float maxWaveTime;
    [SerializeField] private GameObject GemPanelBlock;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject gemToolTip;
    [SerializeField] private GameObject bossHealtBar;
    [SerializeField] AudioSource waveCompleteSound;

    private GemManager _gemManager;
    private Player _player;
    private float _timer; 
    private bool _stopTimer = true;
    private bool _isBossWave = false;

    [HideInInspector] public int waveCounter;
    

    public void Initialize(GemManager gemManager, Player player)
    {
        _gemManager = gemManager;
        _player = player;

        _timer = waveDuration;
        _stopTimer = false;

        waveCounter = 1;

        uIWavePanel.Initialize(this, gemManager);
        enemySpawner.Initialize(_player, this);
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
        ClearObjects(); // spawner
        PauseGame(); // gameController
        
        if(!_isBossWave)
        {
            OnWaveComplete?.Invoke();
            waveCompleteSound.Play();
            
            _gemManager.CreateGems();
            GemPanelBlock.SetActive(false);
        }else
        {
            OnBossWaveComplete?.Invoke();
            ClearObjects();
            BossWaveComplete();
            ClearObjects();
        }
        
        ClearObjects();
        _player.transform.position = new Vector3(0f, -4.5f, 0f);

        switch(StateNameController.character)
        {
            case "Character 1":
                _player.GetComponent<ShieldSkill>().ResetShieldSkill();
                _player.GetComponent<RapidFireSkill>().ResetRapidFireSkill(); 
                break;
            case "Character 2":
                _player.GetComponent<ShieldSkill>().ResetShieldSkill();
                _player.GetComponent<ExplosionBulletsSkill>().ResetSkill(); 
                break;
            case "Character 3":
                _player.GetComponent<RegenerationSkill>().ResetSkill();
                _player.GetComponent<LaserSkill>().ResetSkill(); 
                break;
            default:
                _player.GetComponent<ShieldSkill>().ResetShieldSkill();
                _player.GetComponent<RapidFireSkill>().ResetRapidFireSkill(); 
                break;
        }
        _player.FullHeal();

    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        StateNameController.isPaused = true;
    }

    void UnPauseGame()
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

        ClearObjects();
        _player.FullHeal();

        OnStartNewWave?.Invoke();
        UnPauseGame();
    }

    void BossWave()
    {
        _timer = 90f;
        _isBossWave = true;
        bossHealtBar.SetActive(true);
        enemySpawner.BossSpawn();
        enemySpawner.gameObject.SetActive(false);
    }

    public void ClearObjects()
    {
        GameObject[] surviveEnemies =  GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject surviveEnemy in surviveEnemies)
        {
            Destroy(surviveEnemy);
        }

        GameObject[] entities =  GameObject.FindGameObjectsWithTag("Entity");
        foreach(GameObject entity in entities)
        {
            Destroy(entity);
        }
    }

    private void BossWaveComplete()
    {
        GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().Death();
    }
}
