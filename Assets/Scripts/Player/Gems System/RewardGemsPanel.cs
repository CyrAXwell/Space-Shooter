using UnityEngine;

public class RewardGemsPanel : MonoBehaviour
{
    [SerializeField] private GameObject closeButton;

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        closeButton.SetActive(false);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }

}
