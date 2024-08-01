using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Tier",menuName = "ScriptableObjects/TierUpgrade")]
public class TierUpgrade : ScriptableObject
{ 
    [SerializeField] private int probability;
    [SerializeField] private Color color;

    public int Probability => probability;
    public Color Color => color;
}

