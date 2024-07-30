using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthTMP;
    [SerializeField] private TextMeshProUGUI maxHealthTMP;

    private Player _player;

    public void Initialize(Player player)
    {
        _player = player;
        _player.OnHealthChange += OnHealthChange;
    }

    private void OnHealthChange()
    {
        UpdateHealthBar(_player.GetActiveHp(), _player.GetActiveMaxHp());
    }

    private void UpdateHealthBar(int health, int maxHealth)
    {
        healthBar.fillAmount = (float) health / maxHealth;
        maxHealthTMP.text = maxHealth.ToString();
        healthTMP.text = health.ToString();
    }

    private void OnDisable()
    {
        _player.OnHealthChange -= OnHealthChange;
    }
}
