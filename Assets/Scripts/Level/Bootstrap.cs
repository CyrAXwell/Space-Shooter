using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private UIShipParameters uIShipParameters;
    [SerializeField] private UIHealthBar uIHealthBar;
    [SerializeField] private UIXPBar uIXPBar;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private SkillDisplayPanel skillDisplayPanel;
    [SerializeField] private GemManager gemManager;
    

    private void Awake()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        gameController.InitializePlayer();

        Player player = gameController.GetPlayer();
        player.Initialize();
        uIShipParameters.Initialize(player);
        uIHealthBar.Initialize(player);
        uIXPBar.Initialize(player);
        upgradeManager.Initialize(player, player.GetSkills());
        
        skillDisplayPanel.Initialize(player.GetSkills());

        gemManager.Initialize(player);
    }
}
