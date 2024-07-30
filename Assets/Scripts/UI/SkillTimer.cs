using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTimer : MonoBehaviour
{

    private TMP_Text skillTimer;

    void Awake()
    {
        skillTimer = transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
    }

    public void DisplayTime(float time)
    {
        
        if(time >= 9.95f)
        {
            skillTimer.text = time.ToString("0.0");
        } else 
        {
            skillTimer.text = " " + time.ToString("0.0");
        }
        
    }

    public void DisplayTimeOff()
    {
        skillTimer.text = "";
    }
}
