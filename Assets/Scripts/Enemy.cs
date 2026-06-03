using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float maxHealth = 20f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private int expValue = 10;
    
    private float attackTimer = 0f;
    private Transform player;
    private Rigidbody2D rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    
    private void Update()
    {
        if (player == null) return;
        
        attackTimer -= Time.deltaTime;
    }
    
    private void FixedUpdate()
    {
        if (player == null) return;
        
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        GameManager.instance.AddScore(scoreValue);
        Destroy(gameObject);
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && attackTimer <= 0)
        {
            Player playerScript = collision.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(attackDamage);
                attackTimer = attackCooldown;
            }
        }
    }
}
