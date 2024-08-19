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
    private AudioManager _audioManager;

    public void Initialize(ISkillDisplayable skill)
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        _skill = skill;
        _skill.OnSkillCooldown += OnSkillCooldown;
        _skill.OnUseSkill += OnUseSkill;
        _skill.OnResetSkill += OnResetSkill;
        _skill.OnStartWave += OnStartWave;
        icon.sprite = _skill.GetSkillIcon();
    }

    private void OnDisable()
    {
        _skill.OnSkillCooldown -= OnSkillCooldown;
        _skill.OnUseSkill -= OnUseSkill;
        _skill.OnResetSkill -= OnResetSkill;
        _skill.OnStartWave -= OnStartWave;
    }

    private void OnSkillCooldown()
    {
        DisplaySkillReady();
    }

    private void OnUseSkill()
    {
        _audioManager.PlaySFX(_audioManager.UseSkill, 0.5f);

        DisplaySkillActive();
    }

    private void OnResetSkill()
    {
        DisplaySkillActive();
    }

    private void OnStartWave()
    {
        DisplaySkillActive();
    }

    private void DisplaySkillReady()
    {
        transform.GetChild(0).gameObject.GetComponent<Image>().color = backgroundSkillOnColor;
        transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.white;
    }

    private void DisplaySkillActive()
    {
        transform.GetChild(0).gameObject.GetComponent<Image>().color = backgroundSkillOffColor;
        transform.GetChild(1).gameObject.GetComponent<Image>().color = iconSkillOffColor;    
    }
}
