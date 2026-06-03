using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 0.5f;
    
    private float attackTimer = 0f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }
    
    private void Update()
    {
        HandleInput();
        HandleAttack();
        UpdateHealth();
    }
    
    private void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }
    
    private void HandleInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;
    }
    
    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;
        
        if (attackTimer <= 0)
        {
            AttackNearbyEnemies();
            attackTimer = attackCooldown;
        }
    }
    
    private void AttackNearbyEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
        
        foreach (Collider2D hit in hitEnemies)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void UpdateHealth()
    {
        // UI 업데이트는 별도의 UIManager에서 처리
    }
    
    private void Die()
    {
        GameManager.instance.GameOver();
        Destroy(gameObject);
    }
    
    public float GetHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
