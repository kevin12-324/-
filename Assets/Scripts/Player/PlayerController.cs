using UnityEngine;

/// <summary>
/// 플레이어 이동 및 기본 컨트롤을 담당합니다
/// 2D 탑뷰 게임에 최적화됨
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("이동")]
    public float moveSpeed = 6f;
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 lastDirection = Vector2.down; // 마지막 이동 방향 (공격 방향)
    private Rigidbody2D rb;

    [Header("플레이어 스탯")]
    public float maxHealth = 100f;
    private float currentHealth;
    
    [Header("애니메이션")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        currentHealth = maxHealth;
        gameObject.name = "Player";
        gameObject.tag = "Player";
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 플레이어 입력을 처리합니다
    /// </summary>
    private void HandleInput()
    {
        float moveX = 0f;
        float moveY = 0f;

        // WASD 입력
        if (Input.GetKey(KeyCode.W))
            moveY = 1f;
        if (Input.GetKey(KeyCode.S))
            moveY = -1f;
        if (Input.GetKey(KeyCode.A))
            moveX = -1f;
        if (Input.GetKey(KeyCode.D))
            moveX = 1f;

        moveDirection = new Vector2(moveX, moveY).normalized;

        // 이동 방향이 있으면 마지막 방향 업데이트
        if (moveDirection != Vector2.zero)
        {
            lastDirection = moveDirection;
            
            // 스프라이트 뒤집기 (왼쪽 보기)
            if (moveDirection.x < 0)
                spriteRenderer.flipX = true;
            else if (moveDirection.x > 0)
                spriteRenderer.flipX = false;
        }

        // 애니메이션 파라미터 설정
        if (animator != null)
        {
            animator.SetFloat("moveX", moveDirection.x);
            animator.SetFloat("moveY", moveDirection.y);
            animator.SetBool("isMoving", moveDirection.magnitude > 0);
        }
    }

    /// <summary>
    /// 플레이어를 이동시킵니다
    /// </summary>
    private void Move()
    {
        rb.velocity = moveDirection * moveSpeed;
    }

    /// <summary>
    /// 플레이어가 피해를 입습니다
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"플레이어 피해! 현재 체력: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 플레이어 체력을 회복합니다
    /// </summary>
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    /// <summary>
    /// 플레이어가 죽습니다
    /// </summary>
    private void Die()
    {
        Debug.Log("플레이어 사망!");
        GameManager.Instance.GameOver();
        Destroy(gameObject);
    }

    // Getter 메서드들
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public float GetHealthPercentage() => currentHealth / maxHealth;
    public Vector2 GetLastDirection() => lastDirection;
    public Vector2 GetPosition() => (Vector2)transform.position;
}
