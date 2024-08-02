using UnityEngine;
using UnityEngine.UI;

public class SkillDisplay : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Color backgroundSkillOffColor; // "#16181b"
    [SerializeField] private Color backgroundSkillOnColor; // "#3557d2"
    [SerializeField] private Color iconSkillOffColor; // "#2d3234"
    [SerializeField] AudioSource skillSound;

    private ISkillDisplayable _skill;

    public void Initialize(ISkillDisplayable skill)
    {
        _skill = skill;
        _skill.OnSkillCooldown += OnSkillCooldown;
        _skill.OnUseSkill += OnUseSkill;
        _skill.OnStartWave += OnStartWave;
        icon.sprite = _skill.GetSkillIcon();
    }

    private void OnDisable()
    {
        _skill.OnSkillCooldown -= OnSkillCooldown;
        _skill.OnUseSkill -= OnUseSkill;
        _skill.OnStartWave -= OnStartWave;
    }

    private void OnSkillCooldown()
    {
        DisplaySkillReady();
    }

    private void OnUseSkill()
    {
        DisplaySkillActive();
    }

    private void OnStartWave()
    {
        DisplaySkillCharge();
    }

    public void DisplaySkillReady()
    {
        transform.GetChild(0).gameObject.GetComponent<Image>().color = backgroundSkillOnColor;
        transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.white;
    }

    public void DisplaySkillActive()
    {
        skillSound.Play();

        transform.GetChild(0).gameObject.GetComponent<Image>().color = backgroundSkillOffColor;
        transform.GetChild(1).gameObject.GetComponent<Image>().color = iconSkillOffColor;    
    }

    public void DisplaySkillCharge()
    {
        transform.GetChild(0).gameObject.GetComponent<Image>().color = backgroundSkillOffColor;
        transform.GetChild(1).gameObject.GetComponent<Image>().color = iconSkillOffColor;
    }

    

}
