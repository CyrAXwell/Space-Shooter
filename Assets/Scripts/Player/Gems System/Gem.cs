using UnityEngine;

[CreateAssetMenu(fileName = "New Gem",menuName = "ScriptableObjects/Gems")]
public class Gem : ScriptableObject
{
    [Header("Gem Main Parameters")]
    public string gemName;
    public string gemNameColor;
    public int gemStat;
    public int gemStatIncrease;
    public Sprite gemIcon;

    [Header("Gem Substats Parameters")]
    public int[] numberSubStatsProbability = {500, 5000, 3500, 1000};

    public string[] subStatsNames = {"HP", "ATK", "DEF", "CRIT DMG", "CRIT RATE"};

    public int[] subStatsHP = {1, 2, 3};
    public int[] subStatsATK = {1, 2, 3};
    public int[] subStatsDEF = {1, 1, 2};
    public int[] subStatsCRITDMG = {500, 600, 700};
    public int[] subStatsCRITRate = {200, 250, 350 };


}
