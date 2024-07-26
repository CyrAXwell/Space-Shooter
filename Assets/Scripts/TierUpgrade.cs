using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Tier",menuName = "ScriptableObjects/TierUpgrade")]
public class TierUpgrade : ScriptableObject
{ 
    public Sprite levelOutline;
    public string color;
}

