using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leds : MonoBehaviour
{
    public List<string> lightColor;
    private int colorID = 0;

    private int ledID = 1;
    public int ledMaxNums = 7;
    private Image[] led;
    private Color color;

    void Start()
    {
        led = transform.GetComponentsInChildren<Image>();
        
        
    }

    public void LEDUpdate(int lvl)
    {   
        for(int i = 0; i <= lvl; i++)
        {
            ColorUtility.TryParseHtmlString(lightColor[colorID], out color);
            led[ledID].color = color;
            ledID ++;
            if(ledID >= ledMaxNums)
            {
                ledID = 1;
                colorID ++;
            }
        }
        
    }

}
