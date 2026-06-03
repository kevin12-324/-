using UnityEngine;

/// <summary>
/// 플레이어의 공격 시스템을 담당합니다
/// 화면 내의 적들을 자동으로 조준하여 탄환을 발사합니다
/// </summary>
public class PlayerCombat : MonoBehaviour
{
    [Header("공격")]
    public float shootCooldown = 0.3f; // 공격 간격
    private float shootTimer = 0f;
    public GameObject bulletPrefab; // 탄환 프리팹
    public Transform shootPoint; // 발사 지점 (없으면 플레이어 위치 사용)

    [Header("탄환 성능")]
    public float bulletSpeed = 15f;
    public float bulletDamage = 10f;
    public float bulletLifetime = 5f; // 탄환이 사라지는 시간
    public int bulletsPerShot = 1; // 한 번에 발사되는 탄환 개수
    private float bulletKnockback = 0f; // 탄환 넉백 (기본값 0)

    [Header("조준")]
    public float detectionRange = 15f; // 적 감지 범위
    private Camera mainCamera;
    private PlayerController playerController;
    private ExperienceSystem experienceSystem;

    private void Start()
    {
        mainCamera = Camera.main;
        playerController = GetComponent<PlayerController>();
        experienceSystem = GetComponent<ExperienceSystem>();

        if (bulletPrefab == null)
        {
            Debug.LogError("PlayerCombat: bulletPrefab이 설정되지 않았습니다!");
        }

        // shootPoint가 없으면 플레이어 위치를 사용
        if (shootPoint == null)
        {
            shootPoint = transform;
        }
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootCooldown)
        {
            TryShoot();
            shootTimer = 0f;
        }
    }

    /// <summary>
    /// 화면 내의 적들을 찾아 공격합니다
    /// </summary>
    private void TryShoot()
    {
        // 화면 내의 모든 적 찾기
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        
        if (allEnemies.Length == 0) return;

        // 화면 내에 보이는 적들만 필터링
        System.Collections.Generic.List<Enemy> visibleEnemies = new System.Collections.Generic.List<Enemy>();
        
        foreach (Enemy enemy in allEnemies)
        {
            if (IsEnemyVisible(enemy) && enemy.IsAlive())
            {
                visibleEnemies.Add(enemy);
            }
        }

        // 화면 내의 적이 없으면 반환
        if (visibleEnemies.Count == 0) return;

        // 모든 화면 내의 적에게 탄환 발사
        foreach (Enemy enemy in visibleEnemies)
        {
            ShootAtEnemy(enemy);
        }
    }

    /// <summary>
    /// 적이 화�� 내에 보이는지 확인합니다
    /// </summary>
    private bool IsEnemyVisible(Enemy enemy)
    {
        if (enemy == null || !enemy.gameObject.activeInHierarchy) return false;

        Vector3 screenPos = mainCamera.WorldToViewportPoint(enemy.transform.position);
        
        // 뷰포트 좌표가 0~1 범위 내인지 확인 (z는 카메라 앞에 있는지 확인)
        bool isVisible = screenPos.x > 0 && screenPos.x < 1 && 
                         screenPos.y > 0 && screenPos.y < 1 && 
                         screenPos.z > 0;

        return isVisible;
    }

    /// <summary>
    /// 특정 적을 향해 탄환을 발사합니다
    /// </summary>
    private void ShootAtEnemy(Enemy enemy)
    {
        if (bulletPrefab == null) return;

        // bulletsPerShot만큼 탄환 발사
        for (int i = 0; i < bulletsPerShot; i++)
        {
            // 탄환 생성
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                // 적 방향 계산 (여러 탄환일 경우 약간의 각도 차이 적용)
                Vector2 direction = (enemy.transform.position - shootPoint.position).normalized;
                
                if (bulletsPerShot > 1)
                {
                    // 여러 탄환이면 약간씩 각도를 벌림
                    float angleOffset = (i - (bulletsPerShot - 1) / 2f) * 10f;
                    direction = Quaternion.Euler(0, 0, angleOffset) * direction;
                }
                
                // 탄환 초기화
                bulletScript.Initialize(direction, bulletSpeed, bulletDamage, bulletLifetime);
                
                // 넉백 설정
                if (bulletKnockback > 0)
                {
                    bulletScript.SetKnockbackForce(bulletKnockback);
                }
            }
        }
    }

    /// <summary>
    /// 공격 파라미터를 설정합니다
    /// </summary>
    public void SetAttackStats(float cooldown, float damage)
    {
        shootCooldown = cooldown;
        bulletDamage = damage;
    }

    /// <summary>
    /// 탄환 개수를 증가시킵니다
    /// </summary>
    public void IncreaseBulletCount()
    {
        bulletsPerShot++;
    }

    /// <summary>
    /// 공격 속도를 증가시킵니다 (쿨다운 감소)
    /// </summary>
    public void IncreaseAttackSpeed(float percentage)
    {
        shootCooldown *= (1f - percentage / 100f);
    }

    /// <summary>
    /// 탄환 데미지를 증가시킵니다
    /// </summary>
    public void IncreaseBulletDamage(float percentage)
    {
        bulletDamage *= (1f + percentage / 100f);
    }

    /// <summary>
    /// 넉백 강도를 설정합니다
    /// </summary>
    public void SetKnockback(float force)
    {
        bulletKnockback = force;
    }

    // Getter 메서드들
    public float GetShootCooldown() => shootCooldown;
    public float GetBulletDamage() => bulletDamage;
    public float GetBulletSpeed() => bulletSpeed;
    public int GetBulletsPerShot() => bulletsPerShot;
    public float GetKnockback() => bulletKnockback;
}
