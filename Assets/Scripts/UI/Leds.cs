using UnityEngine;
using UnityEngine.UI;

public class Leds : MonoBehaviour
{
    [SerializeField] private Color[] ledColors;

    private int colorID = 0;
    private int ledID = 1;
    private Image[] _leds;

    public void Initialize()
    {
        _leds = transform.GetComponentsInChildren<Image>();
    }

    public void LEDUpdate(int lvl)
    {   
        for(int i = 0; i <= lvl; i++)
        {
            _leds[ledID].color = ledColors[colorID];
            ledID ++;
            if(ledID >= _leds.Length)
            {
                ledID = 1;
                colorID ++;
            }
        }
    }
}
