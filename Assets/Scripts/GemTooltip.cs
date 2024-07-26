using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GemTooltip : MonoBehaviour
{
    public GameObject selectGem;
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
    
    public void SelectGem(GameObject gem)
    {
        if(selectGem != null)
        {
            if(selectGem != gem)
            {
                if(selectGem.GetComponent<GemStats>().isEquip)
                {
                    selectGem.transform.GetChild(1).gameObject.SetActive(false);
                }else
                {
                    selectGem.transform.GetChild(0).gameObject.SetActive(false);
                }
                
            }
        }
        selectGem = gem;
    }

    public void UpgradeGem()
    {
        selectGem.GetComponent<GemStats>().Upgrade();
    }

    public void Equip()
    {
        gemManager.GetComponent<GemManager>().EquipGem(selectGem);
    }

    public void BreakGemTip()
    {
        transform.GetChild(19).gameObject.SetActive(false);
        transform.GetChild(17).gameObject.SetActive(true);
        ConfirmHighlight.SetActive(false);
        CancelHighlight.SetActive(false);
        transform.GetChild(17).GetChild(2).gameObject.GetComponent<TMP_Text>().text = "+" + (500 + selectGem.GetComponent<GemStats>().totalExp*0.8f).ToString();
        //gemManager.GetComponent<GemManager>().BreakGem(selectGem);
    }

    public void BreakGemConfirm()
    {
        transform.GetChild(17).gameObject.SetActive(false);
        gemManager.GetComponent<GemManager>().BreakGem(selectGem);
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
        transform.GetChild(19).GetChild(2).gameObject.GetComponent<TMP_Text>().text = "-" + (selectGem.GetComponent<GemStats>().needExp).ToString();
        //gemManager.GetComponent<GemManager>().BreakGem(selectGem);
    }

    public void UpgradeGemConfirm()
    {
        if(selectGem.GetComponent<GemStats>().needExp <= gemManager.GetComponent<GemManager>().gemFragments)
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
        if(selectGem != null)
        {
            if(selectGem.GetComponent<GemStats>().isEquip)
            {
                selectGem.transform.GetChild(1).gameObject.SetActive(false);
            }else
            {
                selectGem.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        
        gameObject.SetActive(false);
    }

    public void UnequipGemButton()
    {
        gemManager.GetComponent<GemManager>().UnequipGem(selectGem);
    }




}
