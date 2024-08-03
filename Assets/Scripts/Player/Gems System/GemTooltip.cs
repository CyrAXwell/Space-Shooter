using UnityEngine;
using TMPro;

public class GemTooltip : MonoBehaviour
{
    private GemStats _selectGem;
    public GameObject gemManager;

    public GameObject ConfirmHighlight;
    public GameObject CancelHighlight;

    public GameObject ConfirmHighlight2;
    public GameObject CancelHighlight2;

    public GameObject BreakHighlight;
    public GameObject UnequipHighlight;
    public GameObject EquipHighlight;
    public GameObject UpgardeHighlight;
    public GameObject CloseHighlight;
    
    public void SelectGem(GemStats gem)
    {
        if(_selectGem != null && _selectGem != gem)
        {
            if(_selectGem.GetComponent<GemStats>().isEquip)
                _selectGem.transform.GetChild(1).gameObject.SetActive(false);
            else
                _selectGem.transform.GetChild(0).gameObject.SetActive(false);
        }
        _selectGem = gem;
    }

    public void UpgradeGem()
    {
        _selectGem.GetComponent<GemStats>().Upgrade();
    }

    public void Equip()
    {
        gemManager.GetComponent<GemManager>().EquipGem(_selectGem);
    }

    public void BreakGemTip()
    {
        transform.GetChild(19).gameObject.SetActive(false);
        transform.GetChild(17).gameObject.SetActive(true);
        ConfirmHighlight.SetActive(false);
        CancelHighlight.SetActive(false);
        transform.GetChild(17).GetChild(2).gameObject.GetComponent<TMP_Text>().text = "+" + (500 + _selectGem.totalExp*0.8f).ToString();
    }

    public void BreakGemConfirm()
    {
        transform.GetChild(17).gameObject.SetActive(false);
        gemManager.GetComponent<GemManager>().BreakGem(_selectGem);
    }

    public void BreakGemCancel()
    {
        transform.GetChild(17).gameObject.SetActive(false);
    }

    public void UpgradeGemTip()
    {
        transform.GetChild(17).gameObject.SetActive(false);
        transform.GetChild(19).gameObject.SetActive(true);
        ConfirmHighlight2.SetActive(false);
        CancelHighlight2.SetActive(false);
        transform.GetChild(19).GetChild(2).gameObject.GetComponent<TMP_Text>().text = "-" + (_selectGem.needExp).ToString();
    }

    public void UpgradeGemConfirm()
    {
        if(_selectGem.needExp <= gemManager.GetComponent<GemManager>().gemFragments)
        {
            transform.GetChild(19).gameObject.SetActive(false);
        }
        UpgradeGem();
    }

    public void UpgradeGemCancel()
    {
        transform.GetChild(19).gameObject.SetActive(false);
    }

    public void CloseWindow()
    {
        if(_selectGem != null)
        {
            if(_selectGem.isEquip)
                _selectGem.transform.GetChild(1).gameObject.SetActive(false);
            else
                _selectGem.transform.GetChild(0).gameObject.SetActive(false);
        }
        
        gameObject.SetActive(false);
    }

    public void UnequipGemButton()
    {
        gemManager.GetComponent<GemManager>().UnequipGem(_selectGem);
    }
}
