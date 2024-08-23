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
    private GameController _gameController;
    private int _waveCounter;
    private float _timer; 
    private bool _stopTimer = true;
    private bool _isBossWave = false;
    private Boss _boss;
    private AudioManager _audioManager;
    
    public void Initialize(GemManager gemManager, Player player, ObjectPoolManager objectPoolManager, GameController gameController)
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        _gemManager = gemManager;
        _player = player;
        _objectPoolManager = objectPoolManager;
        _gameController = gameController;

        _timer = waveDuration;
        _stopTimer = false;

        _waveCounter = 1;

        uIWavePanel.Initialize(this, gemManager);
        enemySpawner.Initialize(_player, this, _objectPoolManager);
    }

    public void StartNextWave()
    {
        float newWaveDuration = waveDuration +  _waveCounter * newWaveTimeIncrease;
        _timer = newWaveDuration > maxWaveTime ? maxWaveTime : newWaveDuration;
        _stopTimer = false;
        _waveCounter ++;
        GemPanelBlock.SetActive(true);
        
        if(_waveCounter == 20)
            BossWave();
        else
            enemySpawner.UpdateProbability(_waveCounter);  

        if(gemToolTip.activeInHierarchy)
            gemToolTip.SetActive(false);

        _player.FullHeal();

        OnStartNewWave?.Invoke();
        _gameController.ResumeGame();
    }

    public int GetWave() => _waveCounter;
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
        _gameController.PauseGame();

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

        ISkillDisplayable[] skills = _player.GetComponents<ISkillDisplayable>();
        
        foreach (ISkillDisplayable skill in skills)
        {
            switch(skill)
            {
                case ShieldSkill shieldSkill: shieldSkill.ResetSkill(); break;
                case RapidFireSkill rapidFireSkill: rapidFireSkill.ResetSkill(); break;
                case ExplosionBulletsSkill explosionBulletsSkill: explosionBulletsSkill.ResetSkill(); break;
                case RegenerationSkill regenerationSkill: regenerationSkill.ResetSkill(); break;
                case LaserSkill laserSkill: laserSkill.ResetSkill(); break;
            }
        }
        _player.FullHeal();
    }

    private void BossWave()
    {
        _timer = 90f;
        _isBossWave = true;
        _boss = enemySpawner.BossSpawn(bossHealtBar);
        _boss.OnDeath += OnBossDeath;
        enemySpawner.gameObject.SetActive(false);
    }

    private void OnBossDeath()
    {
        OnGameWin?.Invoke();
    }

    private void OnDisable()
    {
        if (_boss != null)
            _boss.OnDeath -= OnBossDeath;
    }
}
