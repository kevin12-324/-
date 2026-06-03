/// <summary>
/// 패시브 스킬 타입
/// </summary>
public enum SkillType
{
    BulletCountIncrease,      // 탄환 개수 증가
    AttackSpeedIncrease,      // 공격 속도 증가
    BulletDamageIncrease,     // 탄환 데미지 증가
    HealthIncrease,           // 체력 증가
    MovementSpeedIncrease,    // 이동 속도 증가
    BulletKnockback           // 적 밀치기
}

/// <summary>
/// 패시브 스킬 데이터
/// </summary>
public class PassiveSkill
{
    public string skillName;           // 스킬 이름
    public string skillDescription;    // 스킬 설명
    public SkillType skillType;        // 스킬 타입
}
