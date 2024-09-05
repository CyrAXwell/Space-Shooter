using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpMenu : MonoBehaviour
{
    public event Action OnRerollButton;

    [SerializeField] private UpgradeSelector[] upgrades;
    [SerializeField] private GameObject[] upgradesHighlights;
    [SerializeField] private float rerollUpgradeAmount;
    [SerializeField] private Button rerollButton;
    [SerializeField] private GameObject rerollButtonHighlight;
    [SerializeField] private GameObject levelUpPanel;

    private float _rerollCounter;
    private GameController _gameController;
    private AudioManager _audioManager;

    public void Initialize(GameController gameController)
    {   
        _gameController = gameController;
    }

    public void OpenLevelUpMenu()
    {
        _gameController.PauseGame();
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        _audioManager.PlaySFX(_audioManager.LevleUp);

        levelUpPanel.SetActive(true);
        DisableUpgradesOutline();

        rerollButton.interactable = true;
        _rerollCounter = rerollUpgradeAmount;
        rerollButtonHighlight.SetActive(false);
        
    }

    public void CloseLevelUpMenu()
    {
        levelUpPanel.SetActive(false);
        _gameController.ResumeGame();
    }

    public void RerollUpgrades()
    {
        OnRerollButton?.Invoke();
        _rerollCounter --;
        if(_rerollCounter <= 0)
        {
            rerollButton.interactable = false;
            DeselectRerollButton();
        }
    }

    public void SelectRerollButton()
    {
        if(rerollButton.interactable)
            rerollButtonHighlight.SetActive(true);
    }

    public void DeselectRerollButton()
    {
        if(rerollButtonHighlight.activeInHierarchy)
            rerollButtonHighlight.SetActive(false);
    }

    private void DisableUpgradesOutline()
    {
        foreach (GameObject highlight in upgradesHighlights)
            highlight.SetActive(false);
    }
}
