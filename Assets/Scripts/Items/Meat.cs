using UnityEngine;

/// <summary>
/// 고기 아이템 시스템
/// 플레이어가 접촉하면 체력 회복
/// </summary>
public class Meat : MonoBehaviour
{
    [Header("고기 설정")]
    public float healAmount = 20f; // 회복량
    public float rotationSpeed = 100f; // 회전 속도
    public float bobSpeed = 2f; // 위아래 움직임 속도
    public float bobAmount = 0.3f; // 위아래 움직임 정도

    private Vector3 startPosition;
    private SpriteRenderer spriteRenderer;
    private float elapsedTime = 0f;

    private void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.tag = "Meat";

        // 고기 색상을 조금 더 밝게 (반짝이는 효과)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // 회전 효과
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // 위아래 움직임 (부드러운 반복)
        float newY = startPosition.y + Mathf.Sin(elapsedTime * bobSpeed) * bobAmount;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    /// <summary>
    /// 플레이어와 충돌했을 때
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Heal(healAmount);
                Debug.Log($"플레이어 회복! +{healAmount} 체력");
                Destroy(gameObject);
            }
        }
    }

    // Getter 메서드
    public float GetHealAmount() => healAmount;
}
