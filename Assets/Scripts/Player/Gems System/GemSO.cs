using UnityEngine;

[CreateAssetMenu(fileName = "New Gem",menuName = "ScriptableObjects/Gems")]
public class GemSO : ScriptableObject
{
    [SerializeField] private GemType type;
    [SerializeField] private Color color;

    public string gemName;
    public string gemNameColor;
    public int gemStat;
    public int gemStatIncrease;
    public Sprite gemIcon;

    public int[] numberSubStatsProbability = {500, 5000, 3500, 1000};

    public string[] subStatsNames = {"HP", "ATK", "DEF", "CRIT DMG", "CRIT RATE"};

    public int[] subStatsHP = {1, 2, 3};
    public int[] subStatsATK = {1, 2, 3};
    public int[] subStatsDEF = {1, 1, 2};
    public int[] subStatsCRITDMG = {500, 600, 700};
    public int[] subStatsCRITRate = {200, 250, 350 };

    public GemType Type => type;
    public Color Color => color;


}
