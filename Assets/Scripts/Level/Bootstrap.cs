using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private UIShipParameters uIShipParameters;
    [SerializeField] private UIHealthBar uIHealthBar;
    [SerializeField] private UIXPBar uIXPBar;
    [SerializeField] private LevelUpMenu levelUpMenu;
    [SerializeField] private SkillDisplayPanel skillDisplayPanel;
    

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
        levelUpMenu.Initialize(player);
        skillDisplayPanel.Initialize(player.Getskills());
    }
}
