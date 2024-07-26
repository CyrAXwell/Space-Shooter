using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBar : MonoBehaviour
{
    public Image xPBar;
    public TextMeshProUGUI lvlTMP;
    public TextMeshProUGUI xPTMP;
    public TextMeshProUGUI maxXPTMP;


    public void SetXP(int xP, int maxXP)
    {
        xPBar.fillAmount = (float) xP / maxXP;
        maxXPTMP.text = maxXP.ToString();
        xPTMP.text = xP.ToString();

    }

    public void SetLvl(int lvl)
    {
        lvlTMP.text = lvl.ToString();
    }
}
