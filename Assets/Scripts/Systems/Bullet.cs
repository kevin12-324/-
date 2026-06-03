using UnityEngine;

/// <summary>
/// 탄환 시스템을 관리합니다
/// 플레이어가 발사한 탄환이 적을 맞추면 데미지를 입힙니다
/// </summary>
public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float damage;
    private float lifetime;
    private float elapsedTime = 0f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // 라이프타임이 끝나면 탄환 제거
        if (elapsedTime >= lifetime)
        {
            DestroyBullet();
        }
    }

    /// <summary>
    /// 탄환을 초기화합니다
    /// </summary>
    public void Initialize(Vector2 shootDirection, float shootSpeed, float shootDamage, float shootLifetime)
    {
        direction = shootDirection.normalized;
        speed = shootSpeed;
        damage = shootDamage;
        lifetime = shootLifetime;

        // 탄환 이동
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }

        // 탄환 회전 (발사 방향으로)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 스프라이트 뒤집기
        if (spriteRenderer != null && direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    /// <summary>
    /// 탄환이 적과 충돌했을 때
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                DestroyBullet();
            }
        }
    }

    /// <summary>
    /// 탄환을 제거합니다
    /// </summary>
    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    // Getter 메서드들
    public float GetDamage() => damage;
    public Vector2 GetDirection() => direction;
}
