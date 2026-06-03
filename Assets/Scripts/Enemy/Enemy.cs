using UnityEngine;

/// <summary>
/// 적 기본 클래스
/// 이동, 피해, 죽음을 관리합니다
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("이동")]
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 knockbackVelocity = Vector2.zero;
    private float knockbackDuration = 0.1f;
    private float knockbackTimer = 0f;

    [Header("스탯")]
    public float maxHealth = 30f;
    private float currentHealth;
    public float damage = 10f;
    public float attackCooldown = 1f;
    private float attackTimer = 0f;

    [Header("경험치")]
    public int experienceValue = 10;

    private Transform playerTransform;
    private PlayerController playerController;
    private bool isAlive = true;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        // 플레이어 찾기
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerController = playerObj.GetComponent<PlayerController>();
        }

        gameObject.tag = "Enemy";
    }

    private void Update()
    {
        if (!isAlive) return;

        if (playerTransform != null)
        {
            // 플레이어 방향으로 이동
            moveDirection = (playerTransform.position - transform.position).normalized;
            
            // 스프라이트 뒤집기 (왼쪽 보기)
            if (moveDirection.x < 0)
                spriteRenderer.flipX = true;
            else if (moveDirection.x > 0)
                spriteRenderer.flipX = false;
        }

        attackTimer += Time.deltaTime;
        
        // 넉백 타이머
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        // 넉백 중이면 넉백 방향으로 이동
        if (knockbackTimer > 0)
        {
            rb.velocity = knockbackVelocity;
        }
        else
        {
            // 플레이어를 향해 이동
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    /// <summary>
    /// 적이 피해를 입습니다
    /// </summary>
    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 적에게 넉백을 적용합니다
    /// </summary>
    public void ApplyKnockback(Vector2 knockback)
    {
        knockbackVelocity = knockback;
        knockbackTimer = knockbackDuration;
    }

    /// <summary>
    /// 플레이어와 충돌했을 때 피해를 줍니다
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isAlive) return;

        if (collision.CompareTag("Player") && playerController != null)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackCooldown)
            {
                playerController.TakeDamage(damage);
                attackTimer = 0f;
            }
        }
    }

    /// <summary>
    /// 적이 죽습니다
    /// </summary>
    private void Die()
    {
        isAlive = false;

        // 경험치 드롭
        if (playerTransform != null)
        {
            ExperienceSystem expSystem = playerTransform.GetComponent<ExperienceSystem>();
            if (expSystem != null)
            {
                expSystem.GainExperience(experienceValue);
            }
        }

        // 점수 추가
        GameManager.Instance.AddScore(experienceValue);

        // 적 제거
        Destroy(gameObject);
    }

    // Getter 메서드들
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public float GetHealthPercentage() => currentHealth / maxHealth;
    public bool IsAlive() => isAlive;
}
