using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDisplay : MonoBehaviour
{
    public string backgroundOff;
    public string backgroundOn; 
    private string iconOff = "#2d3234";
    private Color colorOff;
    private Color colorOn;
    private Color colorIconOff;
    [SerializeField] AudioSource skillSound;

    void Awake()
    {
        ColorUtility.TryParseHtmlString(backgroundOff, out colorOff);
        ColorUtility.TryParseHtmlString(backgroundOn, out colorOn);
        ColorUtility.TryParseHtmlString(iconOff, out colorIconOff);
    }

    public void SkillReady()
    {
        transform.GetChild(0).gameObject.GetComponent<Image>().color = colorOn;
        transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.white;
        transform.GetChild(2).gameObject.GetComponent<SkillTimer>().DisplayTimeOff();
    }

    public void SkillActive()
    {
        skillSound.Play();
        //transform.GetChild(0).gameObject.GetComponent<Image>().color = colorOn;
        //transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.white;
        transform.GetChild(0).gameObject.GetComponent<Image>().color = colorOff;
        transform.GetChild(1).gameObject.GetComponent<Image>().color = colorIconOff;

        transform.GetChild(2).gameObject.GetComponent<SkillTimer>().DisplayTimeOff();
        
    }

    public void SkillCharge()
    {
        transform.GetChild(0).gameObject.GetComponent<Image>().color = colorOff;
        transform.GetChild(1).gameObject.GetComponent<Image>().color = colorIconOff;
        
    }

}
