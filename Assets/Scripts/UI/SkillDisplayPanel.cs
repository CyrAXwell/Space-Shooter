using UnityEngine;

public class SkillDisplayPanel : MonoBehaviour
{
    [SerializeField] private SkillDisplay[] skillDisplays;
    [SerializeField] private SkillTimer[] skillTimers;

    public void Initialize(ISkillDisplayable[] skills)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skillDisplays[i].Initialize(skills[i]);
            skillTimers[i].Initialize(skills[i]);
        }
    }
}
