using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Image healthBar;

    public void SetHealth(int health, int maxHealth)
    {
        healthBar.fillAmount = (float) health / maxHealth;

    }
}
