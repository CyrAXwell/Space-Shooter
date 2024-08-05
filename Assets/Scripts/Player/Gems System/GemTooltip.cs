using UnityEngine;
using TMPro;
using System.Linq;

public class GemTooltip : MonoBehaviour
{
    private Vector3 IVENTORY_POS = new Vector3(-234f, -94.5f, 0f);
    private Vector3 REWARD_PANEL_POS = new Vector3(0, -312f, 0f);

    [SerializeField] private TMP_Text gemNameTMP;
    [SerializeField] private TMP_Text gemLevelTMP;
    [SerializeField] private TMP_Text gemStatNameTMP;
    [SerializeField] private TMP_Text gemStatValueTMP;
    [SerializeField] private TMP_Text[] subStatsNamesTMP;
    [SerializeField] private TMP_Text[] subStatsValuesTMP;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject unequipButton;
    [SerializeField] private GameObject breakMenu;
    [SerializeField] private GameObject upgradeMenu;

    private GemStats _selectGem;
    private GemManager _gemManager;

    public void Initialize(GemManager gemManager)
    {
        _gemManager = gemManager;
    }

    public void SelectGem(GemStats gem)
    {
        if(_selectGem != null && _selectGem != gem)
            UnselectGem();

        _selectGem = gem;
    }

    private void UnselectGem()
    {
        if(_selectGem.isEquip)
            _selectGem.transform.GetChild(1).gameObject.SetActive(false);
        else
            _selectGem.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OpenGemTooltip(GemStats gem, bool isReward)
    {
        gameObject.SetActive(true); 

        if (isReward)
            transform.localPosition = REWARD_PANEL_POS;
        else
            transform.localPosition = IVENTORY_POS;

        SelectGem(gem);

        equipButton.SetActive(!_selectGem.isEquip);
        unequipButton.SetActive(_selectGem.isEquip);
        breakMenu.SetActive(false);
        upgradeMenu.SetActive(false);
        
        gemNameTMP.text = gem.GetName();
        gemNameTMP.color = gem.GetColor();
        gemLevelTMP.text = "Lvl. " + gem.GetLevel();
        gemStatNameTMP.text = gem.GetName().Replace(" GEM","");

        if (gem.GetName() == "CRIT DMG GEM" || gem.GetName() == "CRIT RATE GEM")
            gemStatValueTMP.text = ((float)gem.GetMainStat() / 100).ToString() + "%";
        else
            gemStatValueTMP.text = gem.GetMainStat().ToString();

        int[] subStatsValues = gem.GetSubStatsValues().ToArray();
        GemType[] subStatsTypes = gem.GetSubStatsTypes().ToArray();

        for (int i = 0; i < subStatsValues.Length; i++)
        {
            SetSubStatName(subStatsTypes[i], i);

            if(subStatsTypes[i] == GemType.critDamage || subStatsTypes[i] == GemType.critRate)
                subStatsValuesTMP[i].text = ((float)subStatsValues[i] / 100).ToString() + "%";
            else
                subStatsValuesTMP[i].text = subStatsValues[i].ToString();
        }

        for (int i = subStatsValues.Length; i < 4; i++)
        {
            subStatsValuesTMP[i].text = "";
            subStatsNamesTMP[i].text = "";
        }
    }

    private void SetSubStatName(GemType type, int index)
    {
        switch (type)
        {
            case GemType.health: subStatsNamesTMP[index].text = "-" + "HP"; break;
            case GemType.damage: subStatsNamesTMP[index].text = "-" + "ATK"; break;
            case GemType.defense: subStatsNamesTMP[index].text = "-" + "DEF"; break;
            case GemType.critDamage: subStatsNamesTMP[index].text = "-" + "CRIT DMG"; break;
            case GemType.critRate: subStatsNamesTMP[index].text = "-" + "CRIT RATE"; break;
        }
    }


    public void OnEquipGemButton()
    {
        _gemManager.EquipGem(_selectGem);
    }

    public void OnUnequipGemButton()
    {
        _gemManager.UnequipGem(_selectGem);
    }

    public void BreakGemTip()
    {
        upgradeMenu.SetActive(false);
        breakMenu.SetActive(true);
        breakMenu.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "+" + (500 + _selectGem.GetTotalExp() * 0.8f).ToString();
    }

    public void BreakGemConfirm()
    {
        breakMenu.SetActive(false);
        _gemManager.BreakGem(_selectGem);
    }

    public void BreakGemCancel()
    {
        breakMenu.SetActive(false);
    }

    public void UpgradeGemTip()
    {
        breakMenu.SetActive(false);
        upgradeMenu.SetActive(true);
        upgradeMenu.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "-" + (_selectGem.GetNeedExp()).ToString();
    }

    public void UpgradeGemConfirm()
    {
        if(_selectGem.GetNeedExp() <= _gemManager.GetMoney())
            upgradeMenu.SetActive(false);

        _selectGem.Upgrade();
    }

    public void UpgradeGemCancel()
    {
       upgradeMenu.SetActive(false);
    }

    public void CloseWindow()
    {
        if(_selectGem != null)
            UnselectGem();

        gameObject.SetActive(false);
    }

    
}
