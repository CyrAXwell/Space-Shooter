using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private UIShipParameters uIShipParameters;
    // [SerializeField] private GameController gameController;
    // [SerializeField] private GameController gameController;

    private void Awake()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        gameController.InitializePlayer();
        uIShipParameters.Initialize(gameController.GetPlayer());
    }
}
