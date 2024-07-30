using UnityEngine;
using UnityEngine.UI;

public class LevelUpMenu : MonoBehaviour
{
    [SerializeField] private UpgradeSelector[] upgrades; // UpgradeSelector , SetUpgrade
    [SerializeField] private GameObject[] upgradesHighlights;
    [SerializeField] private float rerollUpgradeAmount;
    [SerializeField] private Button rerollButton;
    [SerializeField] private GameObject rerollButtonHighlight;
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] AudioSource levelUpSound;

    private float _rerollCounter;
    private Player _player;

    public void Initialize(Player player)
    {
        levelUpPanel.SetActive(false);

        _player = player;
        _player.OnLevelUp += OnLevelUp;
    }

    private void OnLevelUp()
    {
        OpenLevelUpMenu();
    }

    private void InitializeUpgrades()
    {
        foreach (UpgradeSelector upgrade in upgrades)
            upgrade.Upgrade();
    }

    private void DisableUpgradesOutline()
    {
        foreach (GameObject highlight in upgradesHighlights)
            highlight.SetActive(false);
    }

    public void OpenLevelUpMenu()
    {
        levelUpSound.Play();

        levelUpPanel.SetActive(true);
        DisableUpgradesOutline();
        InitializeUpgrades();

        rerollButton.interactable = true;
        _rerollCounter = rerollUpgradeAmount;
        rerollButtonHighlight.SetActive(false);
        
        Time.timeScale = 0f;
        StateNameController.isPaused = true;
    }

    public void CloseLevelUpMenu()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
        StateNameController.isPaused = false;
    }

    public void RerollUpgrades()
    {
        _rerollCounter --;
        if(_rerollCounter <= 0)
        {
            rerollButton.interactable = false;
            DeselectRerollButton();
        }
        InitializeUpgrades();
    }

    private void OnDisable()
    {
        _player.OnLevelUp -= OnLevelUp;
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
}
