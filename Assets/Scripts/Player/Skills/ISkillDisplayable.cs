using System;
using UnityEngine;

public interface ISkillDisplayable
{   
    public event Action OnStartWave;
    public event Action OnUseSkill;
    public event Action OnResetSkill;
    public event Action OnSkillCooldown;
    public event Action<float> OnTimerUpdate;

    public Sprite GetSkillIcon();

}
