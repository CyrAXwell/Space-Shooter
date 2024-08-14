using UnityEngine;
using TMPro;

public class SkillTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private ISkillDisplayable _skill;

    public void Initialize(ISkillDisplayable skill)
    {
        _skill = skill;
        _skill.OnTimerUpdate += OnTimerUpdate;
        _skill.OnResetSkill += OnResetSkill;
        _skill.OnSkillCooldown += OnSkillCooldown;
        _skill.OnUseSkill += OnUseSkill;
    }

    private void OnTimerUpdate(float value)
    {
        DisplayTime(value);
    }

    private void OnResetSkill()
    {
        DisplayTimeOff();
    }

    private void OnSkillCooldown()
    {
        DisplayTimeOff();
    }

    private void OnUseSkill()
    {
        DisplayTimeOff();
    }

    public void DisplayTime(float time)
    { 
        timerText.text = time >= 9.95f ? time.ToString("0.0") : " " + time.ToString("0.0");
    }

    public void DisplayTimeOff()
    {
        timerText.text = "";
    }

    private void OnDisable()
    {
        _skill.OnTimerUpdate -= OnTimerUpdate;
    }
}
