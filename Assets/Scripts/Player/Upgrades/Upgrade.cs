using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[CreateAssetMenu(fileName = "New Upgrade",menuName = "ScriptableObjects/Upgrade")]
public class Upgrade : ScriptableObject
{
    public string nameUpgrade;
    public List<int> value = new List<int>(4);
    public bool percentage;
    public bool floating;
    [TextAreaAttribute]
    public string description;
    public string slotType;
    public string typeColor;
    //public GUI.TextArea slotType;
    public Sprite upgradeIcon;
    //public Sprite levelOutline;

    

}
