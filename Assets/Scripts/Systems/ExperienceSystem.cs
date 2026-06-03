using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 플레이어의 경험치 및 레벨 시스템을 관리합니다
/// </summary>
public class ExperienceSystem : MonoBehaviour
{
    [Header("경험치")]
    private float currentExperience = 0f;
    private int currentLevel = 1;
    public float experiencePerLevel = 100f; // 레벨업에 필요한 경험치
    private float nextLevelExp = 100f;

    [Header("패시브 스킬")]
    private List<PassiveSkill> acquiredSkills = new List<PassiveSkill>();
    private PlayerCombat playerCombat;
    private PlayerController playerController;

    private SkillSelectUI skillSelectUI;
    private bool isLevelingUp = false;

    private void Start()
    {
        playerCombat = GetComponent<PlayerCombat>();
        playerController = GetComponent<PlayerController>();
        skillSelectUI = FindObjectOfType<SkillSelectUI>();
    }

    /// <summary>
    /// 경험치를 획득합니다
    /// </summary>
    public void GainExperience(float amount)
    {
        currentExperience += amount;
        Debug.Log($"경험치 획득: {amount}, 현재: {currentExperience}/{nextLevelExp}");

        // 레벨업 확인
        while (currentExperience >= nextLevelExp)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// 플레이어가 레벨업합니다
    /// </summary>
    private void LevelUp()
    {
        currentExperience -= nextLevelExp;
        currentLevel++;
        nextLevelExp = experiencePerLevel * currentLevel; // 다음 레벨 경험치 증가

        Debug.Log($"레벨 업! 현재 레벨: {currentLevel}");

        // 스킬 선택 UI 표시
        if (skillSelectUI != null)
        {
            isLevelingUp = true;
            Time.timeScale = 0f; // 게임 일시 정지
            skillSelectUI.ShowSkillSelection(this);
        }
    }

    /// <summary>
    /// 패시브 스킬을 적용합니다
    /// </summary>
    public void ApplySkill(PassiveSkill skill)
    {
        acquiredSkills.Add(skill);
        Debug.Log($"스킬 획득: {skill.skillName}");

        // 스킬 효과 적용
        switch (skill.skillType)
        {
            case SkillType.BulletCountIncrease:
                playerCombat.IncreaseBulletCount();
                break;

            case SkillType.AttackSpeedIncrease:
                playerCombat.IncreaseAttackSpeed(20f);
                break;

            case SkillType.BulletDamageIncrease:
                playerCombat.IncreaseBulletDamage(25f);
                break;

            case SkillType.HealthIncrease:
                playerController.Heal(20f);
                playerController.maxHealth += 20f;
                break;

            case SkillType.MovementSpeedIncrease:
                playerController.moveSpeed += 1f;
                break;

            case SkillType.BulletKnockback:
                // 탄환에 넉백 효과 추가
                break;
        }

        // 게임 재개
        isLevelingUp = false;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// 사용 가능한 랜덤 스킬 3개를 반환합니다
    /// </summary>
    public List<PassiveSkill> GetRandomSkills()
    {
        List<PassiveSkill> availableSkills = new List<PassiveSkill>();
        System.Array skillTypes = System.Enum.GetValues(typeof(SkillType));

        // 모든 스킬 타입에 대한 데이터 생성
        foreach (SkillType skillType in skillTypes)
        {
            PassiveSkill skill = CreateSkillFromType(skillType);
            availableSkills.Add(skill);
        }

        // 랜덤하게 3개 선택
        List<PassiveSkill> selectedSkills = new List<PassiveSkill>();
        for (int i = 0; i < 3 && availableSkills.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableSkills.Count);
            selectedSkills.Add(availableSkills[randomIndex]);
            availableSkills.RemoveAt(randomIndex);
        }

        return selectedSkills;
    }

    /// <summary>
    /// 스킬 타입에서 PassiveSkill 객체를 생성합니다
    /// </summary>
    private PassiveSkill CreateSkillFromType(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.BulletCountIncrease:
                return new PassiveSkill { skillName = "탄환 개수 +1", skillDescription = "한 번에 발사되는 탄환이 1개 증가합니다", skillType = skillType };

            case SkillType.AttackSpeedIncrease:
                return new PassiveSkill { skillName = "공격 속도 ⚡", skillDescription = "공격 간격이 20% 단축됩니다", skillType = skillType };

            case SkillType.BulletDamageIncrease:
                return new PassiveSkill { skillName = "탄환 데미지 💥", skillDescription = "탄환 데미지가 25% 증가합니다", skillType = skillType };

            case SkillType.HealthIncrease:
                return new PassiveSkill { skillName = "체력 +20 🛡️", skillDescription = "최대 체력이 20 증가합니다", skillType = skillType };

            case SkillType.MovementSpeedIncrease:
                return new PassiveSkill { skillName = "이동 속도 🚀", skillDescription = "이동 속도가 1 증가합니다", skillType = skillType };

            case SkillType.BulletKnockback:
                return new PassiveSkill { skillName = "적 밀치기 👊", skillDescription = "탄환이 적을 더 강하게 밀어냅니다", skillType = skillType };

            default:
                return new PassiveSkill { skillName = "알 수 없음", skillDescription = "", skillType = skillType };
        }
    }

    // Getter 메서드들
    public int GetCurrentLevel() => currentLevel;
    public float GetCurrentExperience() => currentExperience;
    public float GetNextLevelExperience() => nextLevelExp;
    public float GetExperiencePercentage() => currentExperience / nextLevelExp;
    public List<PassiveSkill> GetAcquiredSkills() => acquiredSkills;
    public bool IsLevelingUp() => isLevelingUp;
}
