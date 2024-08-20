using System;
using TMPro;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text damageTMP;
    [SerializeField] TMP_Text armorTMP;
    [SerializeField] TMP_Text critDamageTMP;
    [SerializeField] TMP_Text critRateTMP;

    private PlayerSO _playerSO;

    private void Start()
    {
        _playerSO = player.GetPlayerSO();
        nameTMP.text = "<uppercase>" + _playerSO.PlayerName + "</uppercase>";
        healthTMP.text = _playerSO.Health.ToString();
        damageTMP.text = _playerSO.Damage.ToString();
        armorTMP.text = _playerSO.Defense.ToString();
        critDamageTMP.text = ((float)_playerSO.CritDamage / 100).ToString() + "%";
        critRateTMP.text = ((float)_playerSO.CritChance / 100).ToString() + "%";
    }

    public String GetPlayerName() => _playerSO.PlayerName;
}
