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
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private ObjectPoolManager objectPoolManager;

    private void Awake()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        gameController.InitializePlayer(waveManager, objectPoolManager);

        objectPoolManager.Initialize(waveManager);

        Player player = gameController.GetPlayer();
        player.Initialize();
        player.GetComponent<Shooting>().Initialize(objectPoolManager);

        uIShipParameters.Initialize(player);
        uIHealthBar.Initialize(player);
        uIXPBar.Initialize(player);
        upgradeManager.Initialize(player, player.GetSkills());
        
        skillDisplayPanel.Initialize(player.GetSkills());

        gemManager.Initialize(player);
        waveManager.Initialize(gemManager, player, objectPoolManager);
    }
}
