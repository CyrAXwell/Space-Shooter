using UnityEngine;
using TMPro;

public class UIShipParameters : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI maxHealthTMP;
    [SerializeField] private TextMeshProUGUI damageTMP;
    [SerializeField] private TextMeshProUGUI armorTMP;
    [SerializeField] private TextMeshProUGUI critDamageTMP;
    [SerializeField] private TextMeshProUGUI critRateTMP;

    private Player _player;
    
    public void Initialize(Player player)
    {
        _player = player;
        _player.OnStatsChange += OnStatsChange;
    }

    private void OnStatsChange()
    {
        PrintStats(_player.GetActiveMaxHp(), _player.GetActiveATK(), _player.GetActiveDEF(), _player.GetActiveCRITDMG(), _player.GetActiveCRITRate());
    }

    public void PrintStats(int health, int damage, int armor, int critDamage, int critChance)
    {
        maxHealthTMP.text = health.ToString();
        damageTMP.text = damage.ToString();
        armorTMP.text = armor.ToString();
        critDamageTMP.text = ((float)critDamage / 100).ToString() + "%";
        critRateTMP.text = ((float)critChance / 100).ToString() + "%";
    }

    private void OnDisable()
    {
        _player.OnStatsChange -= OnStatsChange;
    }
}
