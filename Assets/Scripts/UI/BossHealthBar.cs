using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;

    private Boss _boss;

    public void Initialize(Boss boss)
    {
        gameObject.SetActive(true);
        _boss = boss;
        _boss.OnTakeDamage += OnTakeDamage;

        SetHealth(_boss.GetHealth(), _boss.GetMaxHealth());
    }

    private void OnDisable()
    {
        _boss.OnTakeDamage -= OnTakeDamage;
    }

    private void OnTakeDamage()
    {
        SetHealth(_boss.GetHealth(), _boss.GetMaxHealth());
    }

    private void SetHealth(int health, int maxHealth)
    {
        healthBar.fillAmount = (float) health / maxHealth;
    }

}
