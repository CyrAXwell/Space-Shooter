using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeDisplay : MonoBehaviour
{
    [SerializeField] private Image upgradesImage;

    [SerializeField] private Leds[] playerLeds;
    [SerializeField] private Leds[] firstSkillLeds;
    [SerializeField] private Leds[] secondSkillLeds;

    private List<Leds[]> _ledsList = new List<Leds[]>();

    public void Initialize(Sprite sprite)
    {
        upgradesImage.sprite = sprite;

        _ledsList.Add(playerLeds);
        _ledsList.Add(firstSkillLeds);
        _ledsList.Add(secondSkillLeds);

        foreach (Leds[] leds in _ledsList)
            foreach (Leds led in leds)
                led.Initialize();
    }

    public void UpdateDisplay(int slot, int leds, int level)
    {
        Debug.Log(leds);
        _ledsList[slot][leds].LEDUpdate(level);
    }
}
