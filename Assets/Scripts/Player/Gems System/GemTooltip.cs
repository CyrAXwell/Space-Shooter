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
    [SerializeField] private ComfirmTooltip breakMenu;
    [SerializeField] private ComfirmTooltip upgradeMenu;

    private GemStats _selectGem;
    private GemManager _gemManager;
    private AudioManager _audioManager;

    public void Initialize(GemManager gemManager)
    {
        _gemManager = gemManager;
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void SelectGem(GemStats gem)
    {
        if(_selectGem != null && _selectGem != gem)
            UnselectGem();

        _selectGem = gem;
    }

    private void UnselectGem()
    {
        if(_selectGem.GetState() == GemStats.GemState.equiped)
            _selectGem.HideEquipedOutline();
        else
            _selectGem.HideStandartOutline();
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

    public void OpenGemTooltip(GemStats gem, bool isReward)
    {
        _audioManager.PlaySFX(_audioManager.TooltipButtonClick, 0.7f);
        gameObject.SetActive(true); 

        if (isReward)
            transform.localPosition = REWARD_PANEL_POS;
        else
            transform.localPosition = IVENTORY_POS;

        SelectGem(gem);

        equipButton.SetActive(_selectGem.GetState() != GemStats.GemState.equiped);
        unequipButton.SetActive(_selectGem.GetState() == GemStats.GemState.equiped);
        breakMenu.gameObject.SetActive(false);
        upgradeMenu.gameObject.SetActive(false);
        
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

    public void OnEquipGemButton()
    {
        _audioManager.PlaySFX(_audioManager.TooltipButtonClick, 0.7f);
        _gemManager.EquipGem(_selectGem);
    }

    public void OnUnequipGemButton()
    {
        _audioManager.PlaySFX(_audioManager.TooltipButtonClick, 0.7f);
        _gemManager.UnequipGem(_selectGem);
    }

    public void BreakGemTip()
    {
        _audioManager.PlaySFX(_audioManager.TooltipButtonClick, 0.7f);
        upgradeMenu.gameObject.SetActive(false);
        breakMenu.gameObject.SetActive(true);
        breakMenu.PrintText("+" + (500 + _selectGem.GetTotalExp() * 0.8f).ToString());
    }

    public void BreakGemConfirm()
    {
        _audioManager.PlaySFX(_audioManager.TooltipButtonClick, 0.7f);
        breakMenu.gameObject.SetActive(false);
        _gemManager.BreakGem(_selectGem);
    }

    public void BreakGemCancel()
    {
        _audioManager.PlaySFX(_audioManager.TooltipButtonClick, 0.7f);
        breakMenu.gameObject.SetActive(false);
    }

    public void UpgradeGemTip()
    {
        _audioManager.PlaySFX(_audioManager.TooltipButtonClick, 0.7f);
        breakMenu.gameObject.SetActive(false);
        upgradeMenu.gameObject.SetActive(true);
        upgradeMenu.PrintText("-" + (_selectGem.GetNeedExp()).ToString());
    }

    public void UpgradeGemConfirm()
    {
        //_audioManager.PlaySFX(_audioManager.TooltipButtonClick, 0.7f);
        if(_selectGem.GetNeedExp() <= _gemManager.GetCurrentCoins())
            upgradeMenu.gameObject.SetActive(false);

        _selectGem.Upgrade();
    }

    public void UpgradeGemCancel()
    {
        _audioManager.PlaySFX(_audioManager.TooltipButtonClick, 0.7f);
       upgradeMenu.gameObject.SetActive(false);
    }

    public void CloseWindow()
    {
        _audioManager.PlaySFX(_audioManager.TooltipButtonClick, 0.7f);
        if(_selectGem != null)
            UnselectGem();

        gameObject.SetActive(false);
    }

    
}
