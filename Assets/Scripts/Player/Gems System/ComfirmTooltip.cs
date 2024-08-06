using TMPro;
using UnityEngine;

public class ComfirmTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text price;

    public void PrintText(string text)
    {
        price.text = text;
    }
}
