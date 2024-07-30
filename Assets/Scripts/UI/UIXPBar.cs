using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIXPBar : MonoBehaviour
{
    [SerializeField] private Image xPBar;
    [SerializeField] private TextMeshProUGUI lvlTMP;
    [SerializeField] private TextMeshProUGUI xPTMP;
    [SerializeField] private TextMeshProUGUI maxXPTMP;

    private Player _player;

    public void Initialize(Player player)
    {
        _player = player;
        _player.OnXPChange += OnXPChange;
        _player.OnLevelUp += OnLevelUp;
    }

    private void OnXPChange()
    {
        UpdateXPBar(_player.GetXP(), _player.GetTargetXP());
    }

    private void OnLevelUp()
    {
        UpdateLevelText(_player.GetLevel());
    }

    private void UpdateXPBar(int xP, int maxXP)
    {
        xPBar.fillAmount = (float) xP / maxXP;
        maxXPTMP.text = maxXP.ToString();
        xPTMP.text = xP.ToString();
    }

    private void UpdateLevelText(int lvl)
    {
        lvlTMP.text = lvl.ToString();
    }

    private void OnDisable()
    {
        _player.OnXPChange -= OnXPChange;
        _player.OnLevelUp -= OnLevelUp;
    }
}
