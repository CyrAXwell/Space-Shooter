using UnityEngine;
using UnityEngine.UI;

public class UIWaveCompletePanel : MonoBehaviour
{
    [SerializeField] private Button nextWaveButton;

    public void OnHideRewardPanel()
    {
        nextWaveButton.interactable = true;
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        nextWaveButton.interactable = false;
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
