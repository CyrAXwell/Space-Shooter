using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gem",menuName = "ScriptableObjects/Gems")]
public class GemSO : ScriptableObject
{
    [SerializeField] private GemType type;
    [SerializeField] private Color color;
    [SerializeField] private string gemName;
    [SerializeField] private int gemStat;
    [SerializeField] private int gemStatIncrease;
    [SerializeField] private Sprite gemIcon;

    private int[] numberSubStatsProbability = {500, 5000, 3500, 1000};
    private int[] subStatsHP = {1, 2, 3};
    private int[] subStatsATK = {1, 2, 3};
    private int[] subStatsDEF = {1, 1, 2};
    private int[] subStatsCRITDMG = {500, 600, 700};
    private int[] subStatsCRITRate = {200, 250, 350 };

    public GemType Type => type;
    public Color Color => color;
    public string Name => gemName;
    public int MainStatValue => gemStat;
    public int MainStatIncreaseValue => gemStatIncrease;
    public Sprite Sprite => gemIcon;
    public IEnumerable<int> SubStatsAmountProbability => numberSubStatsProbability;
    public IEnumerable<int> HealthUpgrades => subStatsHP;
    public IEnumerable<int> DamageUpgrades => subStatsATK;
    public IEnumerable<int> DefenseUpgrades => subStatsDEF;
    public IEnumerable<int> CritDamageUpgrades => subStatsCRITDMG;
    public IEnumerable<int> CritRateUpgrades => subStatsCRITRate;
}
