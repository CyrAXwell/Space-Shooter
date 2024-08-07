using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWavePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text waveTimer;
    [SerializeField] private TMP_Text waveCounter;
    [SerializeField] private Image[] wavesLeds;
    [SerializeField] private Color waveCompleteColor;
    [SerializeField] private Color currentWaveColor;
    [SerializeField] private UIWaveCompletePanel uIWaveCompletePanel;

    private WaveManager _waveManager;
    private GemManager _gemManager;

    public void Initialize(WaveManager waveManager, GemManager gemManager)
    {
        _waveManager = waveManager;
        _gemManager = gemManager;
        _waveManager.OnTimerUpdate += OnTimerUpdate;
        _waveManager.OnWaveComplete += OnWaveComplete;
        _waveManager.OnStartNewWave += OnStartNewWave;
        _waveManager.OnBossWaveComplete += OnWaveComplete;
        _gemManager.OnHideRewardPanel += OnHideRewardPanel;

        DisplayNewWave();
    }

    private void OnDisable()
    {
        _waveManager.OnTimerUpdate -= OnTimerUpdate;
        _waveManager.OnWaveComplete -= OnWaveComplete;
        _waveManager.OnStartNewWave -= OnStartNewWave;
        _waveManager.OnBossWaveComplete -= OnWaveComplete;
        _gemManager.OnHideRewardPanel -= OnHideRewardPanel;
    }

    private void OnTimerUpdate()
    {
        UpdateTimer();
    }

    private void OnWaveComplete()
    {
        UpdateTimer();
        wavesLeds[_waveManager.GetWave() - 1].color = waveCompleteColor;
        uIWaveCompletePanel.ShowPanel();
    }

    private void OnStartNewWave()
    {
        DisplayNewWave();
        uIWaveCompletePanel.HidePanel();
    } 

    private void OnHideRewardPanel()
    {
        uIWaveCompletePanel.OnHideRewardPanel();
    }   

    private void UpdateTimer()
    {
        waveTimer.text = Mathf.Round(_waveManager.GetTimer()).ToString();
    }

    private void DisplayNewWave()
    {
        waveCounter.text = "Wave " + _waveManager.GetWave().ToString() + "/20";
        wavesLeds[_waveManager.GetWave() - 1].color = currentWaveColor;
    }

    
}
