using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UpgradeSO",menuName = "ScriptableObjects/UpgradeSO")]
public class UpgradeSO : ScriptableObject
{
    [SerializeField] private string nameUpgrade;
    [SerializeField] private List<int> value = new List<int>(4);
    [SerializeField] private bool percentage;
    [SerializeField] private bool floating;
    [SerializeField, TextArea] private string description;
    [SerializeField] private string slotType;
    [SerializeField] private string typeColor;
    [SerializeField] private Sprite upgradeIcon;
    [SerializeField] private SkillType skillType;
    [SerializeField] private UpgradeType upgradeType;
    [SerializeField, Range(0, 2)] private int slot;
    [SerializeField, Range(0, 6)] private int leds;
    
    public string Name => nameUpgrade;
    public IEnumerable<int> UpgradeValues => value;
    public bool IsPercentageValue => percentage;
    public bool IsFloatingValue => floating;
    public string Description => description;
    public string SlotType => slotType;
    public string TypeColor => typeColor;
    public Sprite Icon => upgradeIcon;
    public SkillType SkillType => skillType;
    public UpgradeType UpgradeType => upgradeType;
    public int Slot => slot;
    public int Leds => leds;
}
